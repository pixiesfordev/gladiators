using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using DG.Tweening;
using Gladiators.Battle;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System.Collections.Generic;
using UnityEngine;

public enum NumType {
    Dmg_Small,
    Dmg_Medium,
    Dmg_Large,

    Dmg_Bleeding,
    Dmg_Poison,
    Dmg_Burning,

    Restore_Hp,
    Restore_Vigor,
}

public enum QueueNumType {
    Dmg,
    Dmg_Bleeding,
    Dmg_Poison,
    Dmg_Burning,
    Restore_Hp,
    Restore_Vigor,
}

public enum KnockbackType {
    Slide, // 滑行
    Knockup,// 擊飛
}

public class Character : MonoBehaviour {

    [SerializeField] Transform boardRotateTrans;
    [SerializeField] Transform CharaCenterPivotTrans; // 要抓模型中心用這個Transform
    [SerializeField] Transform SideRotationParent;
    public Transform BOARD;
    public ParticleSystem MoveSmoke;
    public EffectSpeller MyEffectSpeller;
    public Transform BuffParent;

    public Vector3 CenterPos { get { return new Vector3(transform.position.x, transform.position.y + modelCenter, transform.position.z); } }
    public Vector3 TopPos { get { return new Vector3(transform.position.x, transform.position.y + modelCenter * 2, transform.position.z); } }
    float modelCenter;
    public Vector3 BotPos { get { return new Vector3(transform.position.x, transform.position.y, transform.position.z); } }


    public Animator animator;
    RightLeft Side;

    Character enemy;


    public bool IsRushing { get; private set; }

    const float KNOCKBACK_TIME = 0.6f;//擊退/飛時間

    // 狀態
    HashSet<EffectType> effectTypes = new HashSet<EffectType>();
    public bool CanMove {
        get {
            return !effectTypes.IsMobileRestriction();
        }
    }

    public void Init(int _jsonID, Vector2 _pos, Character _opponent, RightLeft _side) {
        MyEffectSpeller.Init(_opponent);
        var jsonGladiator = GameDictionary.GetJsonData<JsonGladiator>(_jsonID);
        enemy = _opponent;
        Side = _side;
        transform.localPosition = _pos;
        SetRush(false);
        MoveSmoke.Stop();
        setModel(jsonGladiator);
        // 初始化跳數字字典
        foreach (QueueNumType type in System.Enum.GetValues(typeof(QueueNumType))) {
            damageQueues[type] = new Queue<(NumType, int)>();
            isProcessing[type] = false;
        }
    }

    void setModel(JsonGladiator _json) {
        if (_json == null) {
            WriteLog.LogError($"setModel錯誤，json為null");
            return;
        }
        modelCenter = (float)_json.ModelCenter;
        CharaCenterPivotTrans.localPosition = new Vector3(0, modelCenter, 0);
        AddressablesLoader.GetPrefab($"Gladiator/{_json.Ref}/BOARD", (boardPrefab, handle) => {
            if (boardPrefab != null) {
                if (BOARD != null) {
                    Destroy(BOARD.gameObject);
                }

                GameObject newBoard = Instantiate(boardPrefab);
                BOARD = newBoard.transform;
                BOARD.SetParent(boardRotateTrans);
                BOARD.localPosition = new Vector3(0f, 0f, 0f);
                BOARD.name = "BOARD";

                float sideRotationdAngle = (Side == RightLeft.Left) ? 0 : 180;
                WriteLog.Log($"id {name} Side {Side}  angle: {sideRotationdAngle}");
                SideRotationParent.localRotation = Quaternion.Euler(0, sideRotationdAngle, 0);
            }
        });
    }


    public void PlayAni(string _aniName, bool _replay = false) {
        if (!_replay && animator.GetCurrentAnimatorStateInfo(0).IsName(_aniName)) return;
        animator.SetTrigger(_aniName);
    }

    public void SetRush(bool _on) {
        if (IsRushing == _on) return;
        IsRushing = _on;
        if (IsRushing) {
            WriteLog.LogError("Rush");
            MoveSmoke.Play();
            animator.SetFloat("moveSpeed", 1.5f);
        } else {
            WriteLog.LogError("Cancel Rush");
            MoveSmoke.Stop();
            animator.SetFloat("moveSpeed", 1f);
        }
    }



    public async UniTask MoveClientToPos(Vector3 _pos, float _duration, bool _move = false) {
        if (_move) {
            if (CanMove) {
                PlayAni("move");
                Tween tween = transform.DOLocalMove(_pos, _duration).SetEase(Ease.Linear);
                await tween.AsyncWaitForCompletion();
            }
        } else {
            Tween tween = transform.DOLocalMove(_pos, _duration).SetEase(Ease.Linear);
            await tween.AsyncWaitForCompletion();
        }
    }

    public void UpdateEffectTypes(HashSet<EffectType> _effectTypes) {
        MyEffectSpeller.PlayBuffEffect(_effectTypes);
        effectTypes = _effectTypes;
        BattleSceneUI.Instance?.SetInstantSkillLocker(BattleSceneUI.SpellLock.Effect, effectTypes.IsInstantSkillRestriction());
    }
    public void HandleEffects(List<PackEffect> _effectDatas) {
        HashSet<EffectType> _effects = new HashSet<EffectType>();
        foreach (var effectData in _effectDatas) {
            var (success, effectType) = JsonSkillEffect.ConvertStrToEffectType(effectData.EffectName);
            if (success) {
                _effects.Add(effectType);
            }
        }
        UpdateEffectTypes(_effects);
    }
    public void HandleKnockback(Vector2 _beforePos, Vector2 _afterPos, bool _isKnockwall) {
        if (die) return;
        transform.localPosition = new Vector3(_beforePos.x, 0, _beforePos.y);
        float knockbackDist = Vector2.Distance(_beforePos, _afterPos);

        // 判斷擊退類型
        var knockbackType = KnockbackType.Slide;
        if (knockbackDist > 15) knockbackType = KnockbackType.Knockup;

        // 播放碰撞動畫
        PlayAni("knockback");
        BattleManager.Instance.vCam.GetComponent<CameraShake>()?.Shake();

        Vector3 originalPos = transform.localPosition;
        Vector3 finalPos = new Vector3(_afterPos.x, originalPos.y, _afterPos.y);

        if (knockbackType == KnockbackType.Knockup) {// 擊飛
            float knockupHeight = knockbackDist / 6f;
            float gravity = 8f * knockupHeight / (KNOCKBACK_TIME * KNOCKBACK_TIME); // 8 * h / T^2
            float initialVelocityY = gravity * (KNOCKBACK_TIME / 2f); // v0 = g * (T/2)
            Vector3 horizontalVelocity = (finalPos - originalPos) / KNOCKBACK_TIME;

            UniTask.Void(async () => {
                float passTime = 0f;
                while (passTime < KNOCKBACK_TIME) {
                    passTime += Time.deltaTime;
                    float t = passTime / KNOCKBACK_TIME;

                    // 更新水平位置
                    Vector3 newPos = originalPos + horizontalVelocity * passTime;

                    // 更新垂直位置
                    float newY = originalPos.y + initialVelocityY * passTime - 0.5f * gravity * passTime * passTime;

                    // 設定新的位置
                    transform.localPosition = new Vector3(newPos.x, newY, newPos.z);
                    faceDir();
                    await UniTask.Yield();
                }
                transform.localPosition = finalPos;
                faceDir();
                // 撞牆檢查
                if (_isKnockwall) {
                    knockWall();
                }
                // 播放暈眩動畫
                PlayAni("stun");
            });
        } else if (knockbackType == KnockbackType.Slide) {// 擊退滑行
            Vector3 horizontalDisplacement = finalPos - originalPos;
            float totalDistance = horizontalDisplacement.magnitude;
            Vector3 direction = horizontalDisplacement.normalized;
            float frictionCoefficient = 2f; // 摩擦力調整數值

            UniTask.Void(async () => {
                float passTime = 0f;
                while (passTime < KNOCKBACK_TIME) {
                    passTime += Time.deltaTime;
                    float t = Mathf.Clamp01(passTime / KNOCKBACK_TIME);

                    // 使用摩擦力係數調整減速曲線
                    float easedT = 1 - Mathf.Pow(1 - t, frictionCoefficient);
                    float currentDistance = totalDistance * easedT;
                    Vector3 newPos = originalPos + direction * currentDistance;

                    // 更新位置
                    transform.localPosition = new Vector3(newPos.x, originalPos.y, newPos.z);
                    faceDir();
                    await UniTask.Yield();
                }
                transform.localPosition = finalPos;
                faceDir();
                // 撞牆檢查
                if (_isKnockwall) {
                    knockWall();
                }
                // 播放暈眩動畫
                PlayAni("stun");
            });
        }

    }

    void faceDir() {
        Vector3 dir = enemy.transform.position - transform.position;

        dir.y = 0; // 只考慮 2D 面向，忽略 y 軸
        if (dir.sqrMagnitude < 0.001f) {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        targetRotation *= (Side == RightLeft.Left) ? Quaternion.Euler(0, -90f, 0) : Quaternion.Euler(0, 90f, 0);
        transform.rotation = targetRotation;
    }




    float knockbackForce = 10f; // 擊退力量
    float knockbackDuration = 5f; // 擊退持續時間(秒)
    float rotationSpeed = 360f; // 旋轉速度，度/秒
    bool die = false;


    // 角色死亡時的擊退和旋轉效果
    public async UniTask DieKnockout() {
        if (die) return;
        die = true;
        Vector3 knockDir = transform.right * (int)Side;// 朝向後方擊退

        float startTime = Time.time;
        float rotateDir = (float)Side;

        // 開始擊退和旋轉
        while (Time.time < startTime + knockbackDuration) {
            transform.position += knockDir * knockbackForce * Time.deltaTime; // 擊退
            CharaCenterPivotTrans.Rotate(Vector3.up, rotateDir * rotationSpeed * Time.deltaTime); // 旋轉
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    void sprayCoin() {
        AddressablesLoader.GetParticle("Battle/CFXR _BOOM_", (prefab, handle) => {
            var go = Instantiate(prefab);
            go.transform.position = transform.position + Vector3.up * 6;
        });
    }

    void knockWall() {
        BattleManager.Instance.vCam.GetComponent<CameraShake>()?.Shake();
        //AddressablesLoader.GetParticle("Battle/CFXR _BOOM_", (prefab, handle) => {
        //    var go = Instantiate(prefab);
        //    go.transform.position = transform.position + Vector3.up * 6;
        //});
    }

    [SerializeField] float damageShowYPos = 6f;
    [SerializeField] float restoreShowYPos = 20f;
    [SerializeField] DamageNumber defaultDamagePrefab;
    [SerializeField] DamageNumber bleedDamagePrefab;
    [SerializeField] DamageNumber poisonDamagePrefab;
    [SerializeField] DamageNumber burningDamagePrefab;
    [SerializeField] DamageNumber recoveryHPPrefab;
    [SerializeField] DamageNumber recoveryPhysicalPrefab;
    int minIntervalMilliseconds = 300; // 同類型的跳數字最小間隔時間（毫秒）
    Dictionary<QueueNumType, Queue<(NumType, int)>> damageQueues = new Dictionary<QueueNumType, Queue<(NumType, int)>>(); // 該類型的跳數字列隊
    Dictionary<QueueNumType, bool> isProcessing = new Dictionary<QueueNumType, bool>(); // 該類型的跳數字是否在列隊中



    public void ShowBattleNumber(NumType _type, int _value) {
        QueueNumType type = QueueNumType.Dmg;
        switch (_type) {
            case NumType.Dmg_Small:
            case NumType.Dmg_Medium:
            case NumType.Dmg_Large:
                type = QueueNumType.Dmg;
                break;
            case NumType.Restore_Hp:
                type = QueueNumType.Restore_Hp;
                break;
            case NumType.Dmg_Poison:
                type = QueueNumType.Dmg_Poison;
                break;
            case NumType.Dmg_Bleeding:
                type = QueueNumType.Dmg_Bleeding;
                break;
            case NumType.Dmg_Burning:
                type = QueueNumType.Dmg_Burning;
                break;
            case NumType.Restore_Vigor:
                type = QueueNumType.Restore_Vigor;
                break;
            default:
                WriteLog.LogError($"ShowBattleNumber 有尚未定義的NumType({_type})");
                break;
        }

        // 將傷害值加入對應的隊列
        damageQueues[type].Enqueue((_type, _value));

        // 如果該類型尚未在處理中，則開始處理
        if (!isProcessing[type]) {
            processQueueAsync(type).Forget();
        }
    }

    async UniTaskVoid processQueueAsync(QueueNumType _queueType) {
        isProcessing[_queueType] = true;

        while (damageQueues[_queueType].Count > 0) {
            var (type, value) = damageQueues[_queueType].Dequeue();
            displayDamageNumber(_queueType, type, value);
            await UniTask.Delay(minIntervalMilliseconds);
        }
        isProcessing[_queueType] = false;
    }

    void displayDamageNumber(QueueNumType _queueType, NumType _type, int _value) {
        DamageNumber damagePopup = null;
        float yPos = damageShowYPos;
        switch (_type) {
            default:
            case NumType.Dmg_Small:
                damagePopup = defaultDamagePrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Dmg_Medium:
                damagePopup = defaultDamagePrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                damagePopup.SetScale(1.8f);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Dmg_Large:
                damagePopup = defaultDamagePrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                damagePopup.SetScale(2f);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Dmg_Bleeding:
                damagePopup = bleedDamagePrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                break;
            case NumType.Dmg_Poison:
                damagePopup = poisonDamagePrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                break;
            case NumType.Dmg_Burning:
                damagePopup = burningDamagePrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                break;
            case NumType.Restore_Hp:
                yPos = restoreShowYPos;
                damagePopup = recoveryHPPrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Restore_Vigor:
                yPos = restoreShowYPos;
                damagePopup = recoveryPhysicalPrefab.Spawn(this.transform.position + new Vector3(0, yPos, 1), _value);
                damagePopup.SetFollowedTarget(this.transform);
                break;
        }

        if (damagePopup != null) {
            damagePopup.transform.SetParent(this.transform);
            float sideRotationAngle = (Side == RightLeft.Left) ? 0 : 180;
            damagePopup.transform.localRotation = Quaternion.Euler(0, sideRotationAngle, 0);
        }
    }
}

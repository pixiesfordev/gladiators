using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using DG.Tweening;
using Gladiators.Battle;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities;
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

public class Character : MonoBehaviour {


    public Transform Base;
    public Transform rotate;
    public Transform Baseturntable;
    public Transform BOARD;
    public Transform CamLook_Left;
    public Transform CamLook_Right;
    public ParticleSystem MoveSmoke;
    public EffectSpeller MyEffectSpeller;
    public Transform BuffParent;
    [SerializeField] Transform CenterTrans;
    public Vector3 CenterPos { get { return new Vector3(transform.position.x, transform.position.y + modelCenter, transform.position.z); } }
    public Vector3 TopPos { get { return new Vector3(transform.position.x, transform.position.y + modelCenter * 2, transform.position.z); } }
    float modelCenter;
    public Vector3 BotPos { get { return new Vector3(transform.position.x, transform.position.y, transform.position.z); } }


    public Animator animator;

    public RightLeft FaceDir { get; private set; }// 玩家面相方向(玩家看到自己都是在左方且面相右方)


    Character enemy;
    [SerializeField] Transform SideRotationParent;

    public bool IsRushing { get; private set; }
    public bool IsKnockback { get; private set; }

    const float KNOCKUP_TIME = 0.4f;//擊飛時間

    // 狀態
    public int MeleeSkillID { get; private set; }
    List<EffectType> effectTypes = new List<EffectType>();
    public bool CanMove {
        get {
            if (IsKnockback) return false;
            return !effectTypes.IsMobileRestriction();
        }
    }

    public void Init(int _jsonID, float _pos, Character _opponent, RightLeft _faceDir, float _knockAngle) {
        MyEffectSpeller.Init(_opponent);
        var jsonGladiator = GameDictionary.GetJsonData<JsonGladiator>(_jsonID);
        enemy = _opponent;
        FaceDir = _faceDir;
        transform.localPosition = new Vector3(_pos, 0, 0);
        SetRush(false);
        MoveSmoke.Stop();
        setModel(jsonGladiator);
        SetFaceToTarget(_knockAngle);
    }

    void setModel(JsonGladiator _json) {
        if (_json == null) {
            WriteLog.LogError($"setModel錯誤，json為null");
            return;
        }
        modelCenter = (float)_json.ModelCenter;
        CenterTrans.localPosition = new Vector3(0, modelCenter, 0);
        AddressablesLoader.GetPrefab($"Gladiator/{_json.Ref}/BOARD", (boardPrefab, handle) => {
            if (boardPrefab != null) {
                if (BOARD != null) {
                    Destroy(BOARD.gameObject);
                }

                GameObject newBoard = Instantiate(boardPrefab);
                BOARD = newBoard.transform;
                BOARD.SetParent(rotate);
                BOARD.localPosition = new Vector3(0f, 0f, 0f);
                BOARD.name = "BOARD";

                float sideRotationdAngle = (FaceDir == RightLeft.Right) ? 90 : -90;
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
            MoveSmoke.Play();
            animator.SetFloat("moveSpeed", 1.5f);
        } else {
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


    public void UpdateEffectTypes(List<PackEffect> _effectDatas) {
        MyEffectSpeller.PlayBuffEffect(_effectDatas);
    }

    void SetFaceToTarget(float _knockAngle) {
        // 調整角度根據角色的面向方向(左右)
        float adjustedAngle = (FaceDir == RightLeft.Right) ? -_knockAngle : -(_knockAngle + 180f);
        transform.localRotation = Quaternion.Euler(0, adjustedAngle, 0);
    }
    public void HandleMelee(Vector3 _finalPos, List<PackEffect> _effectDatas, float _serverKnockDist, float _serverResultPos, float _knockAngl, int _skilID) {
        UpdateEffectTypes(_effectDatas);
        MeleeSkillID = _skilID;
        SetFaceToTarget(_knockAngl);
        knockback(_finalPos, _serverKnockDist, _serverResultPos, _knockAngl);
    }



    void knockback(Vector3 _finalPos, float _serverKnockPower, float _serverResultPos, float _knockAngle) {
        IsKnockback = true;
        // 播放碰撞動畫
        PlayAni("knockback");
        BattleManager.Instance.vCam.GetComponent<CameraShake>()?.Shake();



        Vector3 originalPos = transform.localPosition;
        float knockupHeight = _serverKnockPower / 6f;
        float gravity = 8f * knockupHeight / (KNOCKUP_TIME * KNOCKUP_TIME); // 8 * h / T^2
        float initialVelocityY = gravity * (KNOCKUP_TIME / 2f); // v0 = g * (T/2)

        // 計算水平位移量
        Vector3 horizontalDisplacement = new Vector3(_finalPos.x - originalPos.x, 0, _finalPos.z - originalPos.z);
        Vector3 horizontalVelocity = horizontalDisplacement / KNOCKUP_TIME;

        UniTask.Void(async () => {
            float passTime = 0f;
            while (passTime < KNOCKUP_TIME) {
                passTime += Time.deltaTime;
                float t = passTime / KNOCKUP_TIME;
                // 更新水平位置
                Vector3 newPos = originalPos + horizontalVelocity * passTime;
                // 更新垂直位置
                float newY = originalPos.y + initialVelocityY * passTime - 0.5f * gravity * passTime * passTime;
                // 設定新的位置
                transform.localPosition = new Vector3(newPos.x, newY, newPos.z);
                await UniTask.Yield();
            }

            // 確保最終的水平位置
            transform.localPosition = new Vector3(_finalPos.x, originalPos.y, _finalPos.z);
            IsKnockback = false;
            // 撞牆檢查
            if (_serverResultPos == BattleController.WALLPOS || _serverResultPos == -BattleController.WALLPOS) {
                knockWall();
            }
            // 播放暈眩動畫
            PlayAni("stun");
        });
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
    [SerializeField] DamageNumber defaultDamagePrefab;
    [SerializeField] DamageNumber bleedDamagePrefab;
    [SerializeField] DamageNumber poisonDamagePrefab;
    [SerializeField] DamageNumber burningDamagePrefab;
    [SerializeField] DamageNumber recoveryHPPrefab;
    [SerializeField] DamageNumber recoveryPhysicalPrefab;
    public void ShowBattleNumber(NumType type, int value) {

        DamageNumber damagePopup = null;

        switch (type) {
            default:
            case NumType.Dmg_Small:
                damagePopup = defaultDamagePrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos, 1), value);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Dmg_Medium:
                damagePopup = defaultDamagePrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos, 1), value);
                damagePopup.SetScale(1.8f);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Dmg_Large:
                damagePopup = defaultDamagePrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos, 1), value);
                damagePopup.SetScale(2f);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Dmg_Bleeding:
                damagePopup = bleedDamagePrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos, 1), value);
                break;
            case NumType.Dmg_Poison:
                damagePopup = poisonDamagePrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos, 1), value);
                break;
            case NumType.Dmg_Burning:
                damagePopup = burningDamagePrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos, 1), value);
                break;
            case NumType.Restore_Hp:
                damagePopup = recoveryHPPrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos + 0.5f, 1), value);
                damagePopup.SetFollowedTarget(this.transform);
                break;
            case NumType.Restore_Vigor:
                damagePopup = recoveryPhysicalPrefab.Spawn(this.transform.position + new Vector3(0, damageShowYPos + 0.5f, 1), value);
                damagePopup.SetFollowedTarget(this.transform);
                break;
        }

        damagePopup.transform.SetParent(this.transform);
        float sideRotationdAngle = (FaceDir == RightLeft.Right) ? 0 : 180;
        damagePopup.transform.localRotation = Quaternion.Euler(0, sideRotationdAngle, 0);
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gladiators.Battle;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum EffectType {
    PDmg,
    MDmg,
    RestoreHP,
    RestoreVigor,
    RegenHP,
    RegenVigor,
    Dizzy,
    Poison,
    Bleeding,
    Burning,
    Fearing,
    Vulnerable,
    Weak,
    Fatigue,
    Protection,
    MeleeSkillReflect,
    RangeSkillReflect,
    MeleeDmgReflect,
    Rush,
    Pull,
    Enraged,
    Block,
    PDefUp,
    MDefUp,
    StrUp,
    KnockbackUp,
    Purge,
    Barrier,
    Poisoning,
    ComboAttack,
    Vampire,
    CriticalUp,
    Condition_SkillVigorBelow,
    Condition_FirstAttack,
    Condition_Charge,
    Dodge_RangeAttack,
    InitUp,
    TriggerEffect_BeAttack,
    TriggerEffect_Time,
    TriggerEffect_WaitTime,
    TriggerEffect_BattleResult,
    Indomitable,
    Berserk,
    StrUpByHp,
    Chaos,
    SkillVigorUp,
    Shuffle,
    Seal,
    Fortune,
    SkillChange,
    Intuition,
    PermanentHp,
}

public class Character : MonoBehaviour {
    public Camera mainCamera;
    public Transform Base;
    public Transform Baseturntable;
    public Transform BOARD;
    public Transform CamLook_Left;
    public Transform CamLook_Right;
    public ParticleSystem MoveSmoke;


    public Animator animator;

    public GameObject skillArea;
    public GameObject skillBall;

    public RightLeft FaceDir { get; private set; }// 玩家面相方向(玩家看到自己都是在左方且面相右方)


    Character enemy;
    [SerializeField] Transform SideRotationParent;

    public bool IsRushing { get; private set; }
    public bool IsKnockback { get; private set; }
    bool InKnockDist {
        get {
            float dist = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
            return knockDist > dist;
        }
    }

    const float KNOCKUP_TIME = 0.4f;//擊飛時間
    float knockDist = 4;

    // 狀態
    public int MaxHP { get; private set; }
    public int CurHP { get; private set; }
    public float HPRatio { get { return CurHP / MaxHP; } }
    public float CurVigor { get; private set; }
    public float VigorRatio { get { return CurVigor / 10; } }
    public float CurSpd { get; private set; }
    public int MeleeSkillID { get; private set; }
    List<Skill.EffectType> EffectTypes = new List<Skill.EffectType>();
    public bool CanMove {
        get {
            if (IsKnockback) return false;
            return !EffectTypes.IsMobileRestriction();
        }
    }



    public void Init(float _pos, Character _opponent, RightLeft _faceDir, float _knockAngle) {
        enemy = _opponent;
        mainCamera = BattleManager.Instance.BattleCam;
        FaceDir = _faceDir;
        MoveClientToPos(new Vector3(_pos, 0, 0), 0).Forget();
        SetRush(false);
        setFaceToTarget(_knockAngle);
    }


    public void PlayAni(string _aniName, bool _replay = false) {
        if (!_replay && animator.GetCurrentAnimatorStateInfo(0).IsName(_aniName)) return;
        animator.SetTrigger(_aniName);
    }

    public void SetRush(bool _on) {
        IsRushing = _on;
        if (IsRushing) {
            if (CanMove) MoveSmoke.Play();
            animator.SetFloat("moveSpeed", 1.5f);
        } else {
            MoveSmoke.Stop();
            animator.SetFloat("moveSpeed", 1f);
        }
    }

    //public void Move_Loco(float _angle) {
    //    if (!CanMove) {
    //        return;
    //    }
    //    float adjustedAngle = (FaceDir == RightLeft.Right) ? _angle : _angle + 180f;
    //    float angleInRadians = adjustedAngle * Mathf.Deg2Rad;

    //    float offsetX = Mathf.Cos(angleInRadians) * CurSpd * Time.deltaTime;
    //    float offsetZ = Mathf.Sin(angleInRadians) * CurSpd * Time.deltaTime;
    //    Vector3 newPos = new Vector3(transform.localPosition.x + offsetX, 0, transform.localPosition.z + offsetZ);
    //    setClientPos(newPos);

    //    if (!InKnockDist) {
    //        PlayAni("move");
    //    } else {
    //        PlayAni("attack");
    //    }
    //}


    public async UniTask MoveClientToPos(Vector3 _pos, float _duration) {
        if (CanMove) {
            PlayAni("move");
            Tween tween = transform.DOLocalMove(_pos, _duration).SetEase(Ease.Linear);
            await tween.AsyncWaitForCompletion();
        }
    }


    public void UpdateEffectTypes(List<string> _effectTypStrs) {
        EffectTypes = Skill.ConvertStrListToEffectTypes(_effectTypStrs);
    }

    public void HandleMelee(List<string> _effectTypes, float _serverKnockPos, float _serverKnockDist, float _serverResultPos, float _knockAngl, int _skilID) {
        UpdateEffectTypes(_effectTypes);
        MeleeSkillID = _skilID;
        setFaceToTarget(_knockAngl);
        knockback(_serverKnockPos, _serverKnockDist, _serverResultPos, _knockAngl);
    }
    void setFaceToTarget(float _knockAngle) {
        // 調整角度根據角色的面向方向 (左右)
        float adjustedAngle = (FaceDir == RightLeft.Right) ? -_knockAngle : -(_knockAngle + 180f);
        transform.localRotation = Quaternion.Euler(0, adjustedAngle, 0);
    }


    void knockback(float _serverKnockPos, float _serverKnockPower, float _serverResultPos, float _knockAngle) {
        var dist = _serverKnockPos - _serverResultPos;
        WriteLog.LogError($"_serverKnockPos={_serverKnockPos} _serverResultPos={_serverResultPos}  _serverKnockPower={_serverKnockPower}");
        IsKnockback = true;

        var originalPos = transform.localPosition;
        // 計算擊退方向和距離
        float angleInRadians = _knockAngle * Mathf.Deg2Rad;
        Vector3 knockbackDir = new Vector3(Mathf.Cos(angleInRadians), 0, Mathf.Sin(angleInRadians)) * -(float)FaceDir;

        // 設定擊飛高度
        float knockupHeight = _serverKnockPower / 6;
        // 根據 KNOCKUP_TIME 計算重力
        float gravity = 2 * knockupHeight / Mathf.Pow(KNOCKUP_TIME / 2, 2);
        float initialVelocityY = gravity * (KNOCKUP_TIME / 2);
        // 水平方向速度，確保在 KNOCKUP_TIME 內完成擊退距離
        Vector3 velocity = new Vector3(knockbackDir.x * _serverKnockPower / KNOCKUP_TIME, 0, knockbackDir.z * _serverKnockPower / KNOCKUP_TIME);

        PlayAni("knockback");

        // 計算server真實的擊飛距離(最終位置-碰撞位置)
        float trueServerKnockDist = Mathf.Abs(_serverResultPos - _serverKnockPos);

        UniTask.Void(async () => {
            bool isKnockWall = false;
            float clientKnockbackDistance = 0;
            float passTime = 0f;
            float finalClientX = originalPos.x;
            float finalClientZ = originalPos.z;

            while (passTime < KNOCKUP_TIME) {
                passTime += Time.deltaTime;

                // 如果尚未撞牆，繼續計算水平移動
                if (!isKnockWall) {
                    // 計算水平位移
                    float horizontalPositionX = originalPos.x + velocity.x * passTime;
                    float horizontalPositionZ = originalPos.z + velocity.z * passTime;

                    // 計算目前client端擊退的距離
                    clientKnockbackDistance = Vector2.Distance(
                        new Vector2(horizontalPositionX, horizontalPositionZ),
                        new Vector2(originalPos.x, originalPos.z)
                    );

                    // 撞牆檢查: client端飛行距離 >= server端的擊退距離，並且server的擊退最終位置在牆壁
                    if ((_serverResultPos == BattleModelController.WALLPOS || _serverResultPos == -BattleModelController.WALLPOS) &&
                        clientKnockbackDistance >= trueServerKnockDist) {

                        knockWall(); // 觸發撞牆效果

                        // 停止水平移動
                        isKnockWall = true;
                        horizontalPositionX = originalPos.x + velocity.x * passTime;
                        horizontalPositionZ = originalPos.z + velocity.z * passTime;
                    }

                    // 紀錄最後的水平位置
                    finalClientX = horizontalPositionX;
                    finalClientZ = horizontalPositionZ;

                    // 更新角色的水平位置
                    transform.localPosition = new Vector3(horizontalPositionX, transform.localPosition.y, horizontalPositionZ);
                }

                // 計算垂直位移
                float verticalPositionY = originalPos.y + initialVelocityY * passTime - 0.5f * gravity * passTime * passTime;
                transform.localPosition = new Vector3(transform.localPosition.x, verticalPositionY, transform.localPosition.z);

                await UniTask.Yield();
            }

            IsKnockback = false;

            // 如果伺服器位置在牆壁，但client沒有偵測到撞牆，則觸發撞牆效果
            if (!isKnockWall && (_serverResultPos == BattleModelController.WALLPOS || _serverResultPos == -BattleModelController.WALLPOS)) {
                knockWall();
            }

            // 設定client位置
            MoveClientToPos(new Vector3(finalClientX, 0, finalClientZ), 0).Forget();

            PlayAni("stun");
        });
    }



    void knockWall() {
        AddressablesLoader.GetParticle("Battle/CFXR _BOOM_", (prefab, handle) => {
            var go = Instantiate(prefab);
            go.transform.position = transform.position + Vector3.up * 6;
        });
    }

}

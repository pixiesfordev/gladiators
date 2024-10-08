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

    const float KNOCKUP_TIME = 0.4f;//擊飛時間

    // 狀態
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
        FaceDir = _faceDir;
        MoveClientToPos(new Vector3(_pos, 0, 0), 0).Forget();
        SetRush(false);
        MoveSmoke.Stop();
        SetFaceToTarget(_knockAngle);
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


    public void UpdateEffectTypes(List<string> _effectTypStrs) {
        EffectTypes = Skill.ConvertStrListToEffectTypes(_effectTypStrs);
    }
    void SetFaceToTarget(float _knockAngle) {
        // 調整角度根據角色的面向方向(左右)
        float adjustedAngle = (FaceDir == RightLeft.Right) ? -_knockAngle : -(_knockAngle + 180f);
        transform.localRotation = Quaternion.Euler(0, adjustedAngle, 0);
    }
    public void HandleMelee(Vector3 _finalPos, List<string> _effectTypes, float _serverKnockDist, float _serverResultPos, float _knockAngl, int _skilID) {
        UpdateEffectTypes(_effectTypes);
        MeleeSkillID = _skilID;
        SetFaceToTarget(_knockAngl);
        knockback(_finalPos, _serverKnockDist, _serverResultPos, _knockAngl);
    }



    void knockback(Vector3 _finalPos, float _serverKnockPower, float _serverResultPos, float _knockAngle) {
        IsKnockback = true;
        // 播放碰撞動畫
        PlayAni("knockback");

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





    void knockWall() {
        AddressablesLoader.GetParticle("Battle/CFXR _BOOM_", (prefab, handle) => {
            var go = Instantiate(prefab);
            go.transform.position = transform.position + Vector3.up * 6;
        });
    }

}

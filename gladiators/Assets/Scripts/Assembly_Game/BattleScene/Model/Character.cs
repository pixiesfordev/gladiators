using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Socket.Matchgame;
using PlasticGui.WorkspaceWindow.QueryViews.Changesets;
using Scoz.Func;
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


    public Animator animator;

    public GameObject skillArea;
    public GameObject skillBall;

    public RightLeft FaceDir { get; private set; }// 玩家面相方向(玩家看到自己都是在左方且面相右方)


    Character enemy;
    [SerializeField] Transform SideRotationParent;

    public bool IsKnockback { get; private set; }
    bool InKnockDist {
        get {
            float dist = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
            return knockDist > dist;
        }
    }

    const float KNOCKUP_TIME = 0.5f;//擊飛時間
    float knockDist = 4;
    int? nowSkillID = null;
    public bool showDebug = false;


    List<Skill.EffectType> EffectTypes = new List<Skill.EffectType>();
    public bool CanMove {
        get {
            if (IsKnockback) return false;
            return !EffectTypes.IsMobileRestriction();
        }
    }
    float curSpd = 0f;
    bool isInit;


    public void Init(float _pos, Character _opponent, RightLeft _faceDir) {
        isInit = true;
        enemy = _opponent;
        mainCamera = BattleManager.Instance.BattleCam;
        FaceDir = _faceDir;
        setPos(_pos);
        BattleManager.Instance.vTargetGroup.AddMember(transform, 1, 8);
    }
    public void SetFaceToTarget() {
        Vector3 directionToTarget = enemy.transform.position - transform.position;
        SideRotationParent.localRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
    }

    void Update() {
        if (!isInit) return;
        doAction();
    }

    void doAction() {
        onMove();
    }

    public void PlayAni(string _aniName, bool _replay = false) {
        if (!_replay && animator.GetCurrentAnimatorStateInfo(0).IsName(_aniName)) return;
        animator.SetTrigger(_aniName);
    }

    void onMove() {
        if (!CanMove) return;
        float posX = (float)FaceDir * curSpd * Time.deltaTime;
        Vector3 move = new Vector3(posX, 0, 0);
        transform.Translate(move);
        if (!InKnockDist) PlayAni("move");
        else {
            PlayAni("attack");
            IsKnockback = true;
        }
    }

    void setPos(float _pos) {
        transform.localPosition = new Vector3((float)_pos, transform.localPosition.y, transform.localPosition.z);
    }

    public void SetState(PackGladiatorState _state) {
        if (CanMove) setPos((float)_state.CurPos);
        curSpd = (float)_state.CurSpd;
        EffectTypes = Skill.ConvertStrListToEffectTypes(_state.EffectTypes);
    }
    public int SkillID { get; private set; }
    public void HandleMelee(int _skilID, float _knockback) {
        SkillID = _skilID;
        Knockback(_knockback);
    }

    void Knockback(float _knockback) {
        IsKnockback = true;
        var originalPos = transform.localPosition;
        Vector3 knockbackDir = new Vector3(-(float)FaceDir, 0, 0);
        Vector3 targetPos = originalPos + knockbackDir * _knockback;
        float passTime = 0f;

        float knockupHeight = _knockback / 5; // 擊飛高度可以隨意設定, 目前設定演出是與擊退距離成正比

        // 根據擊飛高度計算重力與Y軸初速
        float gravity = 2 * knockupHeight / Mathf.Pow(KNOCKUP_TIME / 2, 2); //S=1/2*a*t平方 a=2*S/t平方
        float velocityY = gravity * (KNOCKUP_TIME / 2); // v=a*t
        Vector3 velocity = new Vector3(knockbackDir.x * (_knockback / KNOCKUP_TIME), velocityY, 0);

        UniTask.Void(async () => {
            PlayAni("knockback");

            while (passTime < KNOCKUP_TIME) {
                passTime += Time.deltaTime;
                // 更新水平方向位置
                float horizontalPositionX = originalPos.x + velocity.x * passTime;
                // 更新垂直方向位置
                float verticalPositionY = originalPos.y + velocity.y * passTime - 0.5f * gravity * passTime * passTime;
                // 更新位置
                transform.localPosition = new Vector3(horizontalPositionX, verticalPositionY, originalPos.z);
                await UniTask.Yield();
            }
            transform.localPosition = new Vector3(targetPos.x, originalPos.y, originalPos.z);
            IsKnockback = false;
            PlayAni("stun");
        });
    }



    public void OnSkill() {
        if (nowSkillID == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("idle_Animation")) {
        } else if (stateInfo.IsName("attack_spin")) {
            nowSkillID = null;
            animator.SetBool("isAttack", false);
            animator.SetBool("isAnimation", false);
        } else {
            animator.SetBool("isAttack", true);
            animator.SetBool("isAnimation", true);

        }
    }
}

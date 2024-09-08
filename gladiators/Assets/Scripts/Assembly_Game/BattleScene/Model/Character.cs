using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Socket.Matchgame;
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
    public float CurServerPos { get; private set; }//真實位置是在一維的座標系上，也就是Server位置

    const float KNOCKUP_TIME = 0.5f;//擊飛時間
    float knockDist = 4;
    int? nowSkillID = null;
    public bool showDebug = false;
    bool isInit;

    // 狀態
    public int MaxHP { get; private set; }
    public int CurHP { get; private set; }
    public float HPRatio { get { return CurHP / MaxHP; } }
    public float CurVigor { get; private set; }
    public float VigorRatio { get { return CurVigor / 10; } }
    public float CurSpd { get; private set; }
    List<Skill.EffectType> EffectTypes = new List<Skill.EffectType>();
    public bool CanMove {
        get {
            if (IsKnockback) return false;
            return !EffectTypes.IsMobileRestriction();
        }
    }



    public void Init(float _pos, Character _opponent, RightLeft _faceDir) {
        isInit = true;
        enemy = _opponent;
        mainCamera = BattleManager.Instance.BattleCam;
        FaceDir = _faceDir;
        setClientPos(new Vector3(_pos, 0, 0));
        setServerPos(_pos);
        BattleManager.Instance.vTargetGroup.AddMember(transform, 1, 8);
        SetRush(false);
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

    void onMove() {
        if (!CanMove) {
            return;
        }
        float posX = (float)FaceDir * CurSpd * Time.deltaTime;
        Vector3 move = new Vector3(posX, 0, 0);
        transform.Translate(move);
        if (!InKnockDist) PlayAni("move");
        else {
            PlayAni("attack");
            IsKnockback = true;
        }
    }

    void setClientPos(Vector3 _pos) {
        transform.localPosition = _pos;
    }

    void setServerPos(float _pos) {
        CurServerPos = _pos;
    }


    public void SetState(PackGladiatorState _state) {
        if (CanMove) setClientPos(new Vector3((float)_state.CurPos, transform.localPosition.y, transform.localPosition.z));
        setServerPos((float)_state.CurPos);
        CurSpd = (float)_state.CurSpd;
        EffectTypes = Skill.ConvertStrListToEffectTypes(_state.EffectTypes);
        MaxHP = _state.MaxHP;
        CurHP = _state.CurHp;
        CurVigor = (float)_state.CurVigor;
    }
    public int SkillID { get; private set; }
    public void HandleMelee(int _skilID, float _knockback, float _attackPos, float _resultPos) {
        SkillID = _skilID;
        Knockback(_knockback, _attackPos, _resultPos);
    }

    void Knockback(float _knockback, float _attackPos, float _resultPos) {
        IsKnockback = true;
        float serverPosDisplacement = -(float)FaceDir * _knockback;// Server座標擊退後總位移

        var originalPos = new Vector3(_attackPos, 0, 0);
        Vector3 knockbackDir = new Vector3(-(float)FaceDir, 0, 0);
        float passTime = 0f;

        float knockupHeight = _knockback / 6; // 擊飛高度可以隨意設定, 目前設定演出是與擊退距離成正比

        // 根據擊飛高度計算重力與Y軸初速
        float gravity = 2 * knockupHeight / Mathf.Pow(KNOCKUP_TIME / 2, 2); //S=1/2*a*t平方 a=2*S/t平方
        float velocityY = gravity * (KNOCKUP_TIME / 2); // v=a*t
        Vector3 velocity = new Vector3(knockbackDir.x * (_knockback / KNOCKUP_TIME), velocityY, 0);
        PlayAni("knockback");
        UniTask.Void(async () => {
            bool knockWall = false;
            float knockWallTime = 0;
            while (passTime < KNOCKUP_TIME) {
                passTime += Time.deltaTime;

                // Server位置計算
                var serverPosMove = serverPosDisplacement * (passTime / KNOCKUP_TIME);
                var tmpServerPos = _attackPos + serverPosMove;
                // 撞牆檢查
                if (knockWall == false) {
                    knockWall = knockWallCheck(ref tmpServerPos);
                    if (knockWall) {
                        knockWallTime = passTime;
                        //產生特效
                        AddressablesLoader.GetParticle("Battle/CFXR _BOOM_", (prefab, handle) => {
                            var go = Instantiate(prefab);
                            go.transform.position = transform.position + Vector3.up * 6;
                        });
                    }
                }



                // 表演用位置計算
                float horizontalPositionX = originalPos.x;
                if (!knockWall) horizontalPositionX += velocity.x * passTime;
                else horizontalPositionX += velocity.x * knockWallTime;
                float verticalPositionY = originalPos.y + velocity.y * passTime - 0.5f * gravity * passTime * passTime;
                // 更新位置
                var newPos = new Vector3(horizontalPositionX, verticalPositionY, originalPos.z);
                transform.localPosition = newPos;


                await UniTask.Yield();
            }
            IsKnockback = false;
            setServerPos(_resultPos);
            setClientPos(new Vector3(_resultPos, originalPos.y, originalPos.z));
            PlayAni("stun");
        });
    }

    bool knockWallCheck(ref float _pos) {
        if (FaceDir == RightLeft.Right && _pos <= -BattleModelController.WALLPOS) {
            _pos = -BattleModelController.WALLPOS;
            return true;
        } else if (FaceDir == RightLeft.Left && _pos >= BattleModelController.WALLPOS) {
            _pos = BattleModelController.WALLPOS;
            return true;
        }
        return false;
    }

}

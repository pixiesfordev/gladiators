using Gladiators.Battle;
using UnityEngine;

public class Character : MonoBehaviour {
    public Camera mainCamera;
    public Transform Base;
    public Transform Baseturntable;
    public Transform BOARD;
    public Transform CamLook_Left;
    public Transform CamLook_Right;

    public Collider mainCollider;
    public Rigidbody mainRigidbody;
    public Animator animator;
    public bool lookRight;

    public GameObject skillArea;
    public GameObject skillBall;

    [SerializeField] public bool isRightPlayer;
    public Character otherPlayer;

    [SerializeField] public float defaultSpeed = 1.0f;
    [SerializeField] public float runSpeed = 2.0f;
    float moveSpeed {
        get {
            if(isRun) return runSpeed;

            return defaultSpeed;
        }
    }
    [SerializeField] public float moveExitTimeThreshold = 0.85f;
    [SerializeField] float chnageCharLookTolerance = 1.0f;
    [SerializeField] float attackTolerance = 3.5f;

    [SerializeField] public bool isRun = false;
    [SerializeField] public bool canMove = false;
    [SerializeField] bool canSkill = false;
    [SerializeField] bool getAttack = false;
    [SerializeField] bool isRepel = false;
    [SerializeField] bool isRotation = false;
    [SerializeField] public bool BattleIsEnd = false;

    int? nowSkillID = null;

    public bool showDebug = false;

    void Start() {
        mainCamera = BattleManager.Instance.BattleCam;
        SetFaceToTarget();

    }

    void Update() {
        Vector3 myPosition = transform.position;
        Vector3 otherPosition = otherPlayer.transform.position;
        float distance = Vector3.Distance(otherPosition, myPosition);

        characterLookCam(distance, myPosition, otherPosition);

        if (BattleIsEnd) return;

        doAnimation();
    }

    void characterLookCam(float distance, Vector3 myPosition, Vector3 otherPosition) {
        if (getAttack) return;
        if (canSkill) return;

        if (mainCamera != null && otherPlayer.transform != null) {
            if (distance <= chnageCharLookTolerance) {

            } else {
                if (otherPosition.x < myPosition.x) {
                    lookRight = false;
                } else {
                    lookRight = true;
                }
            }
        }
    }

    void doAnimation() {
        if (getAttack) {
            GetAttack();
        } else if (canSkill) {
            OnSkill();
        } else {
            OnMove();
        }
    }

    private void charLookOtherChart() {
        if (getAttack) return;
        if (canSkill) return;

        BOARD.LookAt(mainCamera.transform.position, Vector3.up);
        if (lookRight) {
            BOARD.eulerAngles = new Vector3(-90, BOARD.eulerAngles.y, BOARD.eulerAngles.z);
        } else {
            BOARD.eulerAngles = new Vector3(-90, BOARD.eulerAngles.y, BOARD.eulerAngles.z - 180);
        }
    }

    public void setCharacter(int charID, Character _otherPlayer) {
        otherPlayer = _otherPlayer;

        //var charData = GameDictionary.GetJsonData<JsonGladiator>(charID);
        defaultSpeed = 8f;
        runSpeed = 12f;

        //if (BattleManager.Instance.isRightPlayer && isRightPlayer) {
        //    BattleManager.Instance.vCam.Follow = CamLook_Left;
        //    BattleManager.Instance.vCam.LookAt = CamLook_Left;
        //} else if (!BattleManager.Instance.isRightPlayer && !isRightPlayer) {
        //    BattleManager.Instance.vCam.Follow = CamLook_Right;
        //    BattleManager.Instance.vCam.LookAt = CamLook_Right;
        //}

        BattleManager.Instance.vTargetGroup.AddMember(transform, 1, 8);
    }

    void SetFaceToTarget() {
        Vector3 directionToTarget = otherPlayer.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
    }

    public void OnMove() {
        if (!canMove) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("move_Animation") && stateInfo.normalizedTime < moveExitTimeThreshold) {
            SetFaceToTarget();
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        } else {
            animator.SetBool("isMove", true);
        }
    }

    public void OnSkill() {
        if (getAttack) return;
        if (!canSkill) return;
        if (nowSkillID == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("idle_Animation")) {
        } else if (stateInfo.IsName("attack_spin")) {
            canSkill = false;
            nowSkillID = null;
            animator.SetBool("isAttack", false);
            animator.SetBool("isAnimation", false);
        } else {
            //var skillData = GameDictionary.GetJsonData<JsonSkill>((int)nowSkillID);
            //var skillEffectData = GameDictionary.GetJsonData<JsonSkillEffect>((int)nowSkillID);

            //if (skillData != null && skillData.Activation == "Instant") {
                animator.SetBool("isAttack", true);
                animator.SetBool("isAnimation", true);

                //create skill ball
                //var skillBall_temp = Instantiate(skillBall, skillArea.transform);
                //if (isRightPlayer) {
                //    skillBall_temp.gameObject.tag = "rightobj";
                //} else {
                //    skillBall_temp.gameObject.tag = "leftobj";
                //}
            //} else {

            //}
        }
    }

    public void doSkillAttack(int skillID) {
        nowSkillID = skillID;
        canSkill = true;
    }

    [SerializeField] public float knockbackForce = 1.0f;
    [SerializeField] public Vector3 knockbackDirection;
    [SerializeField] public float knockbackDuration = 1.0f;
    [SerializeField] public float knockbackTimer = 0f;
    [SerializeField] public float initialSpeed = 20f; // ³õËÙ¶È
    [SerializeField] public float knockbackSpeed;
    public void isGetAttack(Vector3 _knockbackDirection, float _knockbackForce = 1.0f, float _knockbackDuration = 1.0f) {
        //knockbackDirection = _knockbackDirection;
        //knockbackForce = _knockbackForce;
        //knockbackDuration = _knockbackDuration;
        getAttack = true;
    }
    public void GetAttack() {
        if (!getAttack) return;

        knockbackTimer += Time.deltaTime;

        Vector3 knockbackDirectionTemp = -transform.forward;
        float timeFraction = knockbackTimer / knockbackDuration;
        knockbackSpeed = Mathf.Lerp(initialSpeed, 0, timeFraction);
        mainRigidbody.velocity = knockbackDirectionTemp * knockbackForce * knockbackSpeed;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("idle_Animation")) {
            mainRigidbody.velocity = Vector3.zero;
            knockbackTimer = 0f;
        } else {
            animator.SetBool("isRotation", true);
            animator.SetBool("isAnimation", true);
        }

        if (knockbackTimer >= 0.8f) {
            if (stateInfo.IsName("repel_Animation")) {
                getAttack = false;
                animator.SetBool("isRepel", false);
                animator.SetBool("isRotation", false);
                animator.SetBool("isAnimation", false);
            } else {
                animator.SetBool("isRotation", false);
                animator.SetBool("isRepel", true);
            }
        }
    }

    public void BattleStart() {
        canMove = true;
        BattleIsEnd = false;
    }
    public void BattleEnd() {
        if (BattleIsEnd) return;

        BattleIsEnd = true;
    }
}

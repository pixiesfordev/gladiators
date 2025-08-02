using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gladiators.TrainCave;
using UnityEngine;

public class AttackMagicObj : AttackObj
{
    public float SpawnRadius { get; private set; } = 4f; // 生成子彈時，距離目標的半徑
    [SerializeField] SpineAnimationController SmallBossSpine;
    [SerializeField] SpineAnimationController FireUpSpine;
    [SerializeField] SpineAnimationController FireBallSpine;

    // Start is called before the first frame update
    protected override void Start() { }

    // Update is called once per frame
    protected override void Update() { }

    public override void Init()
    {
        DefendType = TrainCaveShield.ShieldType.Magic;
        SmallBossSpine.Init();
        FireUpSpine.Init();
        FireBallSpine.Init();
        base.Init();
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        var anotherAtkObj = coll.gameObject.GetComponent<AttackObj>();
        var shield = coll.gameObject.GetComponent<TrainCaveShield>();
        if (shield != null && shield.DefendType == TrainCaveShield.ShieldType.Magic)
        {
            //撞到盾牌
            TrainCaveManager.Instance.AddMagicScore();
            HitTarget = true;
        }
        else if (anotherAtkObj == null && shield == null)
        {
            //撞到玩家角色
            TrainCaveManager.Instance.PlayerHitted(this);
            HitTarget = true;
        }

        //Debug.LogErrorFormat("魔法攻擊撞到物件: {0}", coll.name);

        //有效碰撞
        if (HitTarget)
        {
            //播放打擊到物體的Spine特效
            TrainCaveUI.Instance.GenerateHitSpine(FireBallSpine.transform.position, FireBallSpine.transform.rotation);
            Destroy(gameObject);
            
            //測試用 物件碰撞後停止其速度
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.velocity = Vector2.zero;
            
        }
    }

    public override void SetSpeed(Vector2 speed)
    {
        //調整Boss的角度(固定只朝向左/右)
        //當物件角度小於等於-90度 >> 設定為0度(固定朝右)
        //當物件大於-90度 >> 設定為180度(固定朝左)
        Quaternion smallBossTargetAngle = transform.eulerAngles.z >= 270f ?
            Quaternion.Euler(Vector3.zero) :
            Quaternion.Euler(Vector3.back * 180f);
        SmallBossSpine.transform.rotation = smallBossTargetAngle;

        //Debug.LogErrorFormat("角度: {0} 修正角度: {1} 是否大於270度: {2}",
        //    transform.eulerAngles, SmallBossSpine.transform.eulerAngles, transform.eulerAngles.z >= 270f);

        SmallBossSpine.PlayAnimation("boss_b", false);
        FireUpSpine.PlayAnimation("fire_up", false);
        MoveFireBall(speed).Forget();
    }

    async UniTaskVoid MoveFireBall(Vector2 speed)
    {
        await UniTask.WaitForSeconds(2.1f);
        FireBallSpine.PlayAnimation("fire_loop", true);
        await UniTask.WaitForSeconds(0.667f);
        base.SetSpeed(speed);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gladiators.Main;
using Gladiators.TrainCave;
using Scoz.Func;
using UnityEngine;

public class AttackPhysicsObj : AttackObj
{
    public float SpawnRadius { get; private set; } = 8f; // 生成子彈時，距離目標的半徑

    [SerializeField] SpineAnimationController AtkSpine;
    [SerializeField] Transform HitSpinePos;

    int PicRandSeed = 0;
    float FlyingTimeRecord = 0f; //測試用 用來記錄飛行所需時間 方便之後調整撥放速度

    // Start is called before the first frame update
    protected override void Start() { }

    // Update is called once per frame
    protected override void Update() { }

    public override void Init()
    {
        DefendType = TrainCaveShield.ShieldType.Physics;
        AtkSpine.Init();
        PickPic();
        Collider2D.size = AttackImg.rectTransform.sizeDelta - ColiderOffset;
        AtkSpine.SetTimeScale(0.5f);
        /*
        string imgSourceName = "attack01";
        AttackImg.gameObject.SetActive(true);
        AttackImg.transform.localScale = Vector3.one + Vector3.left + Vector3.left;
        AssetGet.GetSpriteFromAtlas("TrainCaveUI", imgSourceName, (sprite) =>
        {
            if (sprite != null)
                AttackImg.sprite = sprite;
            else
            {
                AssetGet.GetSpriteFromAtlas("TrainCaveUI", "attack", (sprite) =>
                {
                    AttackImg.sprite = sprite;
                    WriteLog.LogErrorFormat("怪物攻擊圖像不存在 使用替用圖案.攻擊類別:{0}", DefendType);
                });
            }
        });
        AttackImg.SetNativeSize();
        */
    }

    void PickPic()
    {
        PicRandSeed = Random.Range(0, 2);
        string spinePrefix = PicRandSeed > 0 ? "ATTACK01_go" : "ATTACK_go";
        AtkSpine.PlayAnimation(spinePrefix, false);
        //AtkSpine.SetTimeScale(0.5f);
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        //TODO:之後改用孟璋說的比較不吃效能的方法來做 用碰撞器太吃效能
        var anotherAtkObj = coll.gameObject.GetComponent<AttackObj>();
        var shield = coll.gameObject.GetComponent<TrainCaveShield>();
        if (shield != null && shield.DefendType == TrainCaveShield.ShieldType.Physics)
        {
            //撞到盾牌
            TrainCaveManager.Instance.AddPhysicsScore();
            HitTarget = true;
        }
        else if (anotherAtkObj == null && shield == null)
        {
            //撞到玩家角色
            TrainCaveManager.Instance.PlayerHitted(this);
            HitTarget = true;
        }

        //有效碰撞
        if (HitTarget)
        {
            //播放打擊到物體的Spine特效
            Vector3 angle = transform.localEulerAngles + (Vector3.forward * 90f); //修正碰撞Spine角度
            TrainCaveUI.Instance.GenerateHitSpine(HitSpinePos.position, Quaternion.Euler(angle));
            //Debug.LogErrorFormat("花了多少時間碰撞到物體: {0}", Time.time - FlyingTimeRecord);
            //Debug.LogErrorFormat("碰撞位置: {0}", coll.ClosestPoint(transform.position));

            //物件碰撞後往回彈
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.velocity = -rb2D.velocity;

            //播放攻擊彈回演出後銷毀物件
            AttackRollBack();
        }
    }

    void AttackRollBack()
    {
        string spinePrefix = PicRandSeed > 0 ? "ATTACK01_Recycle" : "ATTACK_Recycle";
        float waitSec = PicRandSeed > 0 ? 0.65f : 0.45f;
        AtkSpine.PlayAnimation(spinePrefix, false);
        Invoke(nameof(RecycleObj), waitSec * 2);
    }

    void RecycleObj()
    {
        Destroy(gameObject);
    }

    public override void SetSpeed(Vector2 speed)
    {
        base.SetSpeed(speed);
        FlyingTimeRecord = Time.time;
    }
}

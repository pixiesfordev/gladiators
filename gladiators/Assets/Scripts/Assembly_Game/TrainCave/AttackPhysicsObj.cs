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

    int PicRandSeed = 0;
    float FlyingTimeRecord = 0f;
    Vector3 HitSpineRotateOffset = new Vector3(0f, 0f, 90f);

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
        var anotherAtkObj = coll.gameObject.GetComponent<AttackObj>();
        var shield = coll.gameObject.GetComponent<TrainCaveShield>();
        if (shield != null && shield.DefendType == TrainCaveShield.ShieldType.Physics)
            TrainCaveManager.Instance.AddPhysicsScore();
        else if (anotherAtkObj == null)
        {
            //TODO:修正Spine特效位置 >> 嘗試直接改Collder的位置 改到筆頭的尖端 記得要先備份原本的物件再來做測試
            TrainCaveManager.Instance.PlayerHitted(this);
            //播放打擊到物體的Spine特效
            TrainCaveUI.Instance.GenerateHitSpine(transform.position, transform.rotation);
            //Debug.LogErrorFormat("花了多少時間碰撞到物體: {0}", Time.time - FlyingTimeRecord);
            Debug.LogErrorFormat("碰撞位置: {0}", coll.ClosestPoint(transform.position));
            //Destroy(gameObject);

            //測試用 物件碰撞後停止其速度
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.velocity = Vector2.zero;

        }
    }

    public override void SetSpeed(Vector2 speed)
    {
        //TODO:修改攻擊方式 得用Spine撥出筆拉長的部分 所以SetSpeed只針對碰撞的透明方塊物件去做速度 但要配合筆拉長的速度
        //看看有沒有指定Spine撥放速度的Func可以用
        base.SetSpeed(speed);
        FlyingTimeRecord = Time.time;
    }
}

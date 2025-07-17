using System.Collections;
using System.Collections.Generic;
using Gladiators.Main;
using Gladiators.TrainCave;
using Scoz.Func;
using UnityEngine;

public class AttackPhysicsObj : AttackObj
{
    public float SpawnRadius { get; private set; } = 8f; // 生成子彈時，距離目標的半徑
    // Start is called before the first frame update
    protected override void Start() { }

    // Update is called once per frame
    protected override void Update() { }

    public override void Init()
    {
        DefendType = TrainCaveShield.ShieldType.Physics;
        string imgSourceName = "attack01";
        AttackImg.gameObject.SetActive(true);
        AttackImg.transform.localScale = Vector3.one + Vector3.left + Vector3.left;
        /*
        else if (DefendType == TrainCaveShield.ShieldType.Magic)
        {
            imgSourceName = "attack";
            AttackImg.transform.localScale = Vector3.one;
        }
        */
        AttackImg.gameObject.SetActive(true);
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
        base.Init();
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        var shield = coll.gameObject.GetComponent<TrainCaveShield>();
        if (shield != null && shield.DefendType == TrainCaveShield.ShieldType.Physics)
            TrainCaveManager.Instance.AddPhysicsScore();
        else
            TrainCaveManager.Instance.PlayerHitted(this);
        base.OnTriggerEnter2D(coll);
        Destroy(gameObject);
    }

    public override void SetSpeed(Vector2 speed)
    {
        //TODO:修改攻擊方式 得用Spine撥出筆拉長的部分 所以SetSpeed只針對碰撞的透明方塊物件去做速度 但要配合筆拉長的速度
        //看看有沒有指定Spine撥放速度的Func可以用
        base.SetSpeed(speed);
    }
}

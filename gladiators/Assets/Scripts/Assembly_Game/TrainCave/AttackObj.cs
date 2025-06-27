using System.Collections;
using System.Collections.Generic;
using Gladiators.Main;
using Scoz.Func;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gladiators.TrainCave {
    public class AttackObj : MonoBehaviour {
        [SerializeField] protected Image AttackImg;
        [SerializeField] protected BoxCollider2D Collider2D;

        public TrainCaveShield.ShieldType DefendType { get; protected set; } = TrainCaveShield.ShieldType.NONE;

        Vector2 ColiderOffset = new Vector2(10f, 10f);

        /*TODO:
        改攻擊演出方式 >> 
        1.魔法類為憑空淡入小Boss圖案後 噴火射向玩家角色 
        2.物理類圖案快速戳過去(已實現) 戳到任何物體後就退回去(未實現)
        */

        //TODO:細節項目
        //1.AttackObj重構 要先建一個上層Class 定義共用的基本方法跟碰撞運算演出等邏輯
        //2.建立兩個Class分別繼承物理攻擊跟魔法攻擊 改寫演出相關與產生邏輯
        //3.做兩個Prefab 一個是物理攻擊的 一個是魔法攻擊的
        //4.實現魔法攻擊細節
        // 1.建立三個Spine物件(事先放好) 分成怪物 點火 火焰
        // 2.移動火焰物件以及改角度 要朝向玩家飛行(還要放一個透明空白圖塞Colider)
        // 3.火焰碰撞後的特效演出

        // Start is called before the first frame update
        protected virtual void Start() { }

        // Update is called once per frame
        protected virtual void Update() { }

        public virtual void Init()
        { 
            Collider2D.size = AttackImg.rectTransform.sizeDelta - ColiderOffset;
        }

        protected virtual void OnTriggerEnter2D(Collider2D coll) {
            //Debug.Log ("-------开始碰撞------------");
            //Debug.Log(coll.gameObject.name);
            var shield = coll.gameObject.GetComponent<TrainCaveShield>();
            if (shield != null) {
                if (shield.DefendType == DefendType) {
                    if (DefendType == TrainCaveShield.ShieldType.Magic)
                        TrainCaveManager.Instance.AddMagicScore();
                    else if (DefendType == TrainCaveShield.ShieldType.Physics)
                        TrainCaveManager.Instance.AddPhysicsScore();
                } else {
                    TrainCaveManager.Instance.PlayerHitted(this);
                }
            } else {
                TrainCaveManager.Instance.PlayerHitted(this);
            }
            Destroy(gameObject);
            /*測試用 物件碰撞後停止其速度
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.velocity = Vector2.zero;
            */
        }

        protected virtual void OnTriggerStay2D(Collider2D coll) {
            //Debug.Log ("------正在碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        protected virtual void OnTriggerExit2D(Collider2D coll) {
            //Debug.Log ("------结束碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        public virtual void SetSpeed(Vector2 speed) {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null) {
                rb2D.velocity = speed;
            }
        }
    }
}

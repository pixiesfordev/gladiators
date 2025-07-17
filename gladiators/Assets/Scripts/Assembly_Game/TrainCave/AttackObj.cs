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
        1.魔法類為憑空淡入小Boss圖案後 噴火射向玩家角色(已實現) 
        2.物理類圖案快速戳過去 戳到任何物體後就退回去

        By 新富
        1.人物比例大概是這樣
        2.下面的血量UI 和旁邊的按鈕
        3.筆打到頓攻擊特效
        4.筆拉長的動畫
        5.頓的那個圓圈要剛好可以包圍腳色的路徑 (盾牌也是左右反轉)
        */


        //TODO:細節項目
        //V1.血量UI跟旁邊盾牌UI
        // 1.血量物件通用化(修改狩獵季節的Boss血條物件)
        // 2.盾牌UI >> 提示作用(等討論結果)
        //2.播放被攻擊到的特效
        // 1.物件產生在另外一個GameObject下(固定位置) 不過位置得是發生碰撞的位置
        // 2.撥放完後移除回收物件(用UniTask去控制 不需要中斷點)
        //3.修改物理攻擊演出方式
        // 1.播放Spine動畫 筆看起來是直接伸長(動畫名是GO)
        // 2.攻擊到角色或盾牌後要縮退(動畫名是Recycle)
        // 3.藍色根紅色的筆隨機挑選
        //4.修正魔法攻擊Boss角度 固定只朝向左/右

        // Start is called before the first frame update
        protected virtual void Start() { }

        // Update is called once per frame
        protected virtual void Update() { }

        public virtual void Init()
        { 
            Collider2D.size = AttackImg.rectTransform.sizeDelta - ColiderOffset;
        }

        protected virtual void OnTriggerEnter2D(Collider2D coll)
        {
            //Debug.Log ("-------开始碰撞------------");
            //Debug.Log(coll.gameObject.name);
            
            //播放打擊到物體的Spine特效
            TrainCaveUI.Instance.GenerateHitSpine(transform.position);
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

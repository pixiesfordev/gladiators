using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        protected Vector2 ColiderOffset = new Vector2(10f, 10f);

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
        // 1.物件產生在另外一個GameObject下(固定位置) 不過位置得是發生碰撞的位置 >> 魔法攻擊位置不夠精準 還得調整
        // 2.撥放完後移除回收物件(用UniTask去控制 不需要中斷點)
        //3.修改物理攻擊演出方式
        // 1.播放Spine動畫 筆看起來是直接伸長(動畫名是GO) >> 需要換算速度調整TimeScale
        // 2.攻擊到角色或盾牌後要縮退(動畫名是Recycle) >> 演完退出去後刪除物件
        // 3.藍色根紅色的筆隨機挑選 >> 做一個亂數
        //4.修正魔法攻擊Boss角度 固定只朝向左/右

        /* 物理攻擊演出方式構思 >> 先產生一個物件進去觀看一下延伸的長度有多長 基本上美術是說拉得很長 會在延伸到極限前撞到物體
            1.讓發射點沿著上半圓周發射(-70度到70度之間 不要完全水平 這個角度配合參數設定)
              X軸 >> x = h + r * cos()
              Y軸 >> y = k + r * sin()
            2.藉由調整SpineAnimationController的TimeScale來控制撥放速度 得有個換算公式比對值
              基本上一定是降速 因為預設只有0.6秒 太快了
            備註:好像不用想的那麼複雜 反正就是降速 碰到就撥放Recycle 所以只要足夠讓玩家反應的速度就好 也就是說SetSpeed要對應筆頭就好
        */

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
            //物件碰撞後停止其速度
        }

        protected virtual void OnTriggerStay2D(Collider2D coll) {
            //Debug.Log ("------正在碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        protected virtual void OnTriggerExit2D(Collider2D coll) {
            //Debug.Log ("------结束碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        public virtual void SetSpeed(Vector2 speed)
        {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.velocity = speed;
            }
        }
    }
}

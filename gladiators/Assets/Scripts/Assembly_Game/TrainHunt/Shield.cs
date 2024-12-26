using Scoz.Func;
using UnityEngine;

namespace Gladiators.TrainHunt {
    public class Shield : MonoBehaviour {
        [SerializeField] Transform trans_Char; // 角色座標參考
        [SerializeField] float ellipseWidth = 2f;   // 橢圓寬
        [SerializeField] float ellipseHeight = 1f;  // 橢圓高
        [SerializeField] Vector2 ellipseCenterOffset = Vector2.zero; // 橢圓中心位置偏移


        void Update() {
            // 取得滑鼠在世界座標中的位置
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 計算角色到滑鼠的向量與弧度(Radians)
            Vector2 playerPos = trans_Char.position;
            Vector2 dir = mousePos - playerPos;

            // Mathf.Atan2 取得的弧度範圍為 -π ~ π ( -180° ~ 180° )
            float radians = Mathf.Atan2(dir.y, dir.x);
            float test = radians * Mathf.Rad2Deg;
            // 將角度限制在「上半部」：0°～180° -> 對應 0 ~ π Radians
            if (radians < 0f || radians > Mathf.PI) {
                if (dir.x >= 0) radians = 0;
                else radians = Mathf.PI;
            }

            // 設定盾牌位置，使用橢圓公式: x = (width/2) * cos(θ),  y = (height/2) * sin(θ)
            float x = (ellipseWidth / 2f) * Mathf.Cos(radians);
            float y = (ellipseHeight / 2f) * Mathf.Sin(radians);

            // 盾牌中心偏移
            Vector2 offsetPos = new Vector2(x, y) + ellipseCenterOffset;

            // 終於得到盾牌最終世界位置
            Vector2 shieldPos = playerPos + offsetPos;
            transform.position = shieldPos;

            // 讓盾牌朝外旋轉(角色與滑鼠的相對方向)，將弧度轉角度
            float shieldAngle = radians * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, shieldAngle);
        }
    }

}
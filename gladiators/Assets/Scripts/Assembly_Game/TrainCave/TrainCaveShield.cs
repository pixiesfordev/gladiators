using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Codice.Client.BaseCommands;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gladiators.TrainCave {
    public class TrainCaveShield : MonoBehaviour
    {
        [SerializeField] Transform trans_Char; // 角色座標參考
        [SerializeField] float ellipseWidth = 2f;   // 橢圓寬
        [SerializeField] float ellipseHeight = 1f;  // 橢圓高
        [SerializeField] Vector2 ellipseCenterOffset = Vector2.zero; // 橢圓中心位置偏移

        [SerializeField] bool bTest = false;

        CancellationTokenSource eventCTK;

        public enum ShieldType
        {
            NONE,
            Physics,
            Magic,
        }
        public ShieldType DefendType { get; private set;} = ShieldType.NONE;
    
        void Start () {
            eventCTK = new CancellationTokenSource();
            ShieldEvent().Forget();
        }

        void Update() {
            
        }

        void OnDestroy() {
            eventCTK.Cancel();
        }

        public void InitShield(ShieldType type) {
            DefendType = type;
        }

        public void ShowShield(bool show) {
            gameObject.SetActive(show);
        }

        async UniTaskVoid ShieldEvent() {
            Vector2 mousePos;
            Vector2 playerPos;
            Vector2 dir;
            Vector2 offsetPos;
            Vector2 shieldPos;
            Vector2 tempPos;

            float radians;
            float test;
            float x;
            float y;
            float shieldAngle;

            Debug.LogFormat("玩家初始位置: {0} 物件初始位置: {1}", trans_Char.position, transform.position);

            while (true) {
                if (bTest) {
                    await UniTask.Yield();
                    continue;
                }
                // 取得滑鼠在世界座標中的位置
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // 計算角色到滑鼠的向量與弧度(Radians)
                playerPos = trans_Char.position;
                dir = mousePos - playerPos;

                // Mathf.Atan2 取得的弧度範圍為 -π ~ π ( -180° ~ 180° )
                radians = Mathf.Atan2(dir.y, dir.x);
                test = radians * Mathf.Rad2Deg;
                // 將角度限制在「上半部」：0°～180° -> 對應 0 ~ π Radians
                if (radians < 0f || radians > Mathf.PI) {
                    if (dir.x >= 0) radians = 0;
                    else radians = Mathf.PI;
                }

                // 設定盾牌位置，使用橢圓公式: x = (width/2) * cos(θ),  y = (height/2) * sin(θ)
                x = (ellipseWidth / 2f) * Mathf.Cos(radians);
                y = (ellipseHeight / 2f) * Mathf.Sin(radians);

                // 盾牌中心偏移
                tempPos = Vector2.zero;
                tempPos.x = x;
                tempPos.y = y;
                offsetPos = tempPos + ellipseCenterOffset;

                // 終於得到盾牌最終世界位置
                shieldPos = playerPos + offsetPos;

                //transform.position = shieldPos;
                transform.localPosition = shieldPos;

                // 讓盾牌朝外旋轉(角色與滑鼠的相對方向)，將弧度轉角度
                shieldAngle = radians * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, shieldAngle);
                await UniTask.Yield(eventCTK.Token);
            }
        }
    }
}

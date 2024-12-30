using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scoz.Func;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gladiators.TrainCave {
    public class TrainCaveUI : BaseUI {
        [SerializeField] Transform PlayerTrans;
        [SerializeField] SpriteRenderer PlayerPic;
        [SerializeField] MyText TimeText;
        [SerializeField] MyText MagicPoint;
        [SerializeField] MyText PhysicsPoint;

        [SerializeField] TrainCaveShield PhysicsShield;
        [SerializeField] TrainCaveShield MagicShield;

        [SerializeField] AttackObjSpawner Spawner;

        [SerializeField] GameObject GameOverObj;

        [SerializeField] Image HPCurrent;
        [SerializeField] MyText Damge;
        [SerializeField] MyText HPValText;

        [HeaderAttribute("==============TEST==============")]
        [SerializeField] MyText MousePositionVal; //測試用 監控滑鼠位置
        [Tooltip("受擊閃爍演出時間")][SerializeField] float PlayerHittedTime;


        public static TrainCaveUI Instance { get; private set; }


        Vector3 towardLeft = new Vector3(-1f, 1f, 1f);
        bool PlayerTowardLeft = true; //角色是否朝左 false為朝右 True為朝左

        int PhysicsScore = 0;
        int MagicScore = 0;
        float GameTime = 30f;

        Vector3 DamgeOriginPos;
        Vector3 DamgeEndPos;
        int MaxHP = 100;
        int CurrentHP;

        CancellationTokenSource HittedCTK;

        // Start is called before the first frame update
        void Start() {
            Init();
            PhysicsShield.InitShield(TrainCaveShield.ShieldType.Physics);
            MagicShield.InitShield(TrainCaveShield.ShieldType.Magic);
            PhysicsShield.gameObject.SetActive(false);
            MagicShield.gameObject.SetActive(false);

            DamgeOriginPos = Damge.transform.localPosition;
            DamgeEndPos = DamgeOriginPos;
            DamgeEndPos.y += 130f;

            MouseListener().Forget();
            ResetGame();
        }

        // Update is called once per frame
        void Update() {

        }

        public override void RefreshText() {

        }

        protected override void SetInstance() {
            Instance = this;
        }

        //TODO:
        /*
        1.Joystick套件(之後補 先寫PC版滑鼠控制)
         1.方向移動(角色要判斷方向跟著轉向)
         2.方向移動盾牌(盾牌要跟著搖桿動 只判斷方向) >> 孟璋已幫我完成 感謝
        2.攻擊邏輯設計 >> 孟璋已幫我完成攻擊物產生邏輯 感恩
         1.先決定是以Y軸為變量還是X軸 Y軸為變量就是X固定螢幕最左邊或最右邊 X軸為變量 就是固定從最上面
         2.決定起始位置後往角色位置移動
         3.之後有空去計算攻擊圖像的角度(要計算夾角或者網路查相關作法)
         4.攻擊物件速度要可變 攻擊間隔也隨機給一個區間
        3.碰撞判定(測試碰撞功能製作)
         1.撞到盾牌 >> 設計個圖像表示有擋到
         2.撞到角色 >> 角色閃爍
        4.計分
         1.魔法防禦分數
         2.物理防禦分數
        5.盾牌操作方式 >> 用壓住的方式操作
        */

        async UniTaskVoid GameStart() {
            TimeText.text = GameTime.ToString();
            float startTime = Time.time;
            float passTime = startTime;
            float deltaTime = 0f;
            float remainTime = 0f;
            while (deltaTime < GameTime && CurrentHP > 0) {
                passTime += Time.deltaTime;
                deltaTime = passTime - startTime;
                remainTime = (float)Math.Floor(GameTime - deltaTime);
                if (remainTime < 0f)
                    remainTime = 0f;
                //更新剩餘時間文字
                TimeText.text = remainTime.ToString();
                await UniTask.Yield();
            }
            await UniTask.Yield();
            EndGame();
        }

        /// <summary>
        /// 滑鼠監聽器
        /// </summary>
        /// <returns></returns>
        async UniTaskVoid MouseListener() {
            //TODO:
            //1.點左鍵舉物理盾牌 點右鍵舉魔法盾牌
            //2.根據滑鼠方向移動盾牌位置
            //先測試滑鼠事件
#if UNITY_EDITOR

            Vector3 curMousePos = Input.mousePosition;

            while (true) {
                MousePositionVal.text = curMousePos.ToString();
                curMousePos = Input.mousePosition;
                //判斷角色朝向 >> 測試版本先不要轉向 因為圖片看不出方向性
                /*
                if (curMousePos.x >= 0 && PlayerTowardLeft)
                    ChangePlayerDirection(false);
                else if (curMousePos.x < 0 && !PlayerTowardLeft)
                    ChangePlayerDirection(true);
                */

                //盾牌事件
                if (Input.GetMouseButtonDown(0)) {
                    ShowShield(true, MouseButton.Left);
                } else if (Input.GetMouseButtonDown(1)) {
                    ShowShield(true, MouseButton.Right);
                } else if (Input.GetMouseButtonUp(0)) {
                    ShowShield(false, MouseButton.Left);
                } else if (Input.GetMouseButtonUp(1)) {
                    ShowShield(false, MouseButton.Right);
                }
                await UniTask.Yield();
            }
#elif UNITY_IOS || UNITY_ANDROID
            //TODO:用Joystick套件控制方向
#endif
        }

        /// <summary>
        /// 設定角色朝向
        /// </summary>
        /// <param name="toLeft">True為朝向左邊</param>
        void ChangePlayerDirection(bool toLeft) {
            if (toLeft) {
                PlayerTrans.localScale = towardLeft;
            } else {
                PlayerTrans.localScale = Vector3.one;
            }
        }

        void ShowShield(bool show, MouseButton button) {
            Debug.LogFormat("顯示盾牌:{0} 按鈕:{1}", show, button);
            if (button == MouseButton.Left) {
                if (!MagicShield.gameObject.activeSelf)
                    PhysicsShield.ShowShield(show);
            } else if (button == MouseButton.Right) {
                if (!PhysicsShield.gameObject.activeSelf)
                    MagicShield.ShowShield(show);
            }
        }

        public void AddPhysicsScore() {
            PhysicsScore++;
            PhysicsPoint.text = string.Format("PHY: {0}", PhysicsScore);
        }

        public void AddMagicScore() {
            MagicScore++;
            MagicPoint.text = string.Format("MAG: {0}", MagicScore);
        }

        public void PlayerHitted(AttackObj obj) {
            //可以針對近來的Obj做不同種類的演出與傷害判定
            int reduceHP = 10;
            if (obj.DefednType == TrainCaveShield.ShieldType.Magic) {
                PlayerHittedByMagic().Forget();
            } else if (obj.DefednType == TrainCaveShield.ShieldType.Physics) {
                PlayerHittedByPhysics().Forget();
            }
            Damge.gameObject.SetActive(true);
            Damge.text = string.Format("-{0}", reduceHP);
            Damge.transform.localPosition = DamgeOriginPos;
            var tweener = Damge.transform.DOLocalMove(DamgeEndPos, PlayerHittedTime * 4);
            tweener.OnComplete(HideDamageText);
            CurrentHP -= reduceHP;
            SetHPText();
            HPCurrent.fillAmount = (float)CurrentHP / MaxHP;
        }

        void HideDamageText() {
            Damge.gameObject.SetActive(false);
        }

        void CreateHittedCTK() {
            if (HittedCTK != null) {
                HittedCTK.Cancel();
            }
            HittedCTK = new CancellationTokenSource();
        }

        async UniTask PlayerHittedByPhysics() {
            CreateHittedCTK();
            float startTime = Time.time;
            float passTime = startTime;
            Color toColor;
            Color fromColor;
            for (int i = 0; i < 4; i++) {
                if (i % 2 == 1) {
                    toColor = Color.red;
                    fromColor = Color.white;
                } else {
                    toColor = Color.white;
                    fromColor = Color.red;
                }
                while (passTime - startTime < PlayerHittedTime) {
                    passTime += Time.deltaTime;
                    PlayerPic.color = Color.Lerp(fromColor, toColor, (passTime - startTime) / PlayerHittedTime);
                    await UniTask.Yield(HittedCTK.Token);
                }
            }
        }

        async UniTask PlayerHittedByMagic() {
            CreateHittedCTK();
            float startTime = Time.time;
            float passTime = startTime;
            Color toColor;
            Color fromColor;
            for (int i = 0; i < 4; i++) {
                if (i % 2 == 1) {
                    toColor = Color.blue;
                    fromColor = Color.white;
                } else {
                    toColor = Color.white;
                    fromColor = Color.blue;
                }
                while (passTime - startTime < PlayerHittedTime) {
                    passTime += Time.deltaTime;
                    PlayerPic.color = Color.Lerp(fromColor, toColor, (passTime - startTime) / PlayerHittedTime);
                    await UniTask.Yield(HittedCTK.Token);
                }
            }
        }

        void EndGame() {
            Spawner.StopShoot();
            GameOverObj.SetActive(true);
        }

        public void ResetGame() {
            PhysicsScore = 0;
            MagicScore = 0;
            PhysicsPoint.text = string.Format("PHY: {0}", PhysicsScore);
            MagicPoint.text = string.Format("MAG: {0}", MagicScore);
            GameOverObj.SetActive(false);

            CurrentHP = MaxHP;
            SetHPText();
            HPCurrent.fillAmount = 1f;

            Damge.gameObject.SetActive(false);

            Spawner.StartShoot();
            GameStart().Forget();
        }

        void SetHPText() {
            HPValText.text = string.Format("{0}/{1}", CurrentHP, MaxHP);
        }

    }
}

using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Scoz.Func;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Codice.CM.Common.CmCallContext;
using DamageNumbersPro;

namespace Gladiators.TrainCave {
    public class TrainCaveManager : MonoBehaviour {
        public static TrainCaveManager Instance;
        [SerializeField] Camera MyCam;
        [SerializeField] Transform PlayerTrans;
        [SerializeField] TrainCaveShield PhysicsShield;
        [SerializeField] TrainCaveShield MagicShield;
        [SerializeField] SpriteRenderer PlayerPic;
        [SerializeField] bool MobileControl;
        [SerializeField] AttackObjSpawner Spawner;

        [SerializeField] DamageNumber dmgPrefab;
        [SerializeField] Vector3 dmgPopupOffset; // 跳血座標偏移
        [SerializeField] float dmgNumScal; // 跳血縮放

        [Tooltip("受擊閃爍演出時間")][SerializeField] float PlayerHittedTime;
        [Tooltip("受擊減少血量 必須為整數")][SerializeField] int HittedReduceHP;

        Vector3 towardLeft = new Vector3(-1f, 1f, 1f);
        bool PlayerTowardLeft = true; //角色是否朝左 false為朝右 True為朝左

        CancellationTokenSource HittedCTK;

        UltimateJoystick joyStick;

        int PhysicsScore = 0;
        int MagicScore = 0;
        float GameTime = 30f;

        public void Init() {
            Instance = this;
            SetCam();//設定攝影機模式
            SetInit();
            MouseListener().Forget();

#if !UNITY_EDITOR // 輸出版本要根據平台判斷操控方式
                    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) MobileControl = true;
                    else MobileControl = false;
#endif


            joyStick = UltimateJoystick.ReturnComponent("MyJoystick");
            joyStick.gameObject.SetActive(MobileControl);

            ResetGame();
        }

        void SetInit() {
            PhysicsShield.InitShield(TrainCaveShield.ShieldType.Physics);
            MagicShield.InitShield(TrainCaveShield.ShieldType.Magic);
            PhysicsShield.gameObject.SetActive(false);
            MagicShield.gameObject.SetActive(false);
        }

        void SetCam() {
            //因為戰鬥場景的攝影機有分為場景與UI, 要把場景攝影機設定為Base, UI設定為Overlay, 並在BaseCamera中加入Camera stack
            UICam.Instance.SetRendererMode(CameraRenderType.Overlay);
            addCamStack(UICam.Instance.MyCam);
        }

        /// <summary>
        /// 將指定camera加入到MyCam的CameraStack中
        /// </summary>
        void addCamStack(Camera _cam) {
            if (_cam == null) return;
            var cameraData = MyCam.GetUniversalAdditionalCameraData();
            if (cameraData == null) return;
            cameraData.cameraStack.Add(_cam);
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

        async UniTaskVoid GameStart() {
            TrainCaveUI.Instance.SetGameTime(Mathf.RoundToInt(GameTime));
            float startTime = Time.time;
            float passTime = startTime;
            float deltaTime = 0f;
            float remainTime = 0f;
            while (deltaTime < GameTime && !TrainCaveUI.Instance.HeroIsDead()) {
                passTime += Time.deltaTime;
                deltaTime = passTime - startTime;
                remainTime = (float)Math.Floor(GameTime - deltaTime);
                if (remainTime < 0f)
                    remainTime = 0f;
                //更新剩餘時間文字
                TrainCaveUI.Instance.SetGameTime(Mathf.RoundToInt(remainTime));
                await UniTask.Yield();
            }
            await UniTask.Yield();
            EndGame();
        }

        void EndGame() {
            Spawner.StopShoot();
            TrainCaveUI.Instance.ShowGameOverObj(true);
        }

        public void ResetGame() {
            TrainCaveUI.Instance.ResetGame();
            PhysicsScore = 0;
            MagicScore = 0;
            Debug.LogFormat("重新開始遊戲!");
            Spawner.StartShoot();
            GameStart().Forget();
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

        void CreateHittedCTK() {
            if (HittedCTK != null) {
                HittedCTK.Cancel();
            }
            HittedCTK = new CancellationTokenSource();
        }

        /// <summary>
        /// 滑鼠監聽器
        /// </summary>
        /// <returns></returns>
        async UniTaskVoid MouseListener() {

            Vector3 curMousePos = Input.mousePosition;

            while (true) {
                curMousePos = Input.mousePosition;
                //判斷角色朝向 >> 測試版本先不要轉向 因為圖片看不出方向性
                /*
                if (curMousePos.x >= 0 && PlayerTowardLeft)
                    ChangePlayerDirection(false);
                else if (curMousePos.x < 0 && !PlayerTowardLeft)
                    ChangePlayerDirection(true);
                */

                //盾牌事件
                if (Input.GetMouseButtonUp(0))
                    ShowShield(false, MouseButton.Left);
                if (Input.GetMouseButtonUp(1))
                    ShowShield(false, MouseButton.Right);

                if (Input.GetMouseButtonDown(0)) {
                    ShowShield(true, MouseButton.Left);
                } else if (Input.GetMouseButtonDown(1)) {
                    ShowShield(true, MouseButton.Right);
                }
                await UniTask.Yield();
            }

        }

        public void PlayerHitted(AttackObj obj) {
            //可以針對近來的Obj做不同種類的演出與傷害判定
            if (obj.DefednType == TrainCaveShield.ShieldType.Magic) {
                PlayerHittedByMagic().Forget();
            } else if (obj.DefednType == TrainCaveShield.ShieldType.Physics) {
                PlayerHittedByPhysics().Forget();
            }
            //TODO:之後角色應該會獨立成一個通用物件 這段就需要改寫
            var dmgNum = dmgPrefab.Spawn(PlayerTrans.position + dmgPopupOffset, HittedReduceHP);
            dmgNum.transform.localScale = Vector3.one * dmgNumScal;
            TrainCaveUI.Instance.OnHit(HittedReduceHP);
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
            TrainCaveUI.Instance.SetPhysicsScore(PhysicsScore);
        }

        public void AddMagicScore() {
            MagicScore++;
            TrainCaveUI.Instance.SetMagicScore(MagicScore);
        }
    }
}



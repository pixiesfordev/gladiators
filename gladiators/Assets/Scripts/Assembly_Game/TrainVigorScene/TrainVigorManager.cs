using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Scoz.Func;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace Gladiators.TrainVigor {
    public class TrainVigorManager : MonoBehaviour {
        public static TrainVigorManager Instance;
        [SerializeField] Camera MyCam;
        [SerializeField] Chara_TrainVigor Char;
        [SerializeField] Platform MyPlatform;
        [SerializeField] Spawner MySpawner;
        [SerializeField] int StartCountDownSec;
        [SerializeField] float MaxAddRegenVigor;
        [SerializeField] float CharDiePosY; // 腳色Y軸低於多少算遊戲失敗

        [SerializeField] public bool MobileControl;
        UltimateJoystick joyStick;
        bool playing = false;
        int curLeftTime;


        public void Init() {
            Instance = this;
            setCam();//設定攝影機模式
            setInit();
            setChar();

#if !UNITY_EDITOR // 輸出版本要根據平台判斷操控方式
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) MobileControl = true;
            else MobileControl = false;
#endif

            joyStick = UltimateJoystick.ReturnComponent("MyJoystick");
            joyStick.gameObject.SetActive(MobileControl);
            TrainVigorSceneUI.Instance.ShowCountingdown(false);
            Char.SetKinematic(true);
            MyPlatform.StopRotate();
        }
        void setChar() {
            Char.Init(MySpawner.VelocityRange);
        }

        void setInit() {
            MyPlatform.Init();
        }
        void setCam() {
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
        private void Update() {
            if (MobileControl) controlChar_Joystick();
            else controlChar_Mouse();
            dieCheck();
        }
        void dieCheck() {
            if (!playing) return;
            if (Char.transform.position.y < CharDiePosY) endGame();
        }
        void restartGame() {
            TrainVigorSceneUI.Instance.ShowCountingdown(false);
            StartGame();
        }
        public void StartGame() {
            playing = true;
            MyPlatform.ResetPlatform();
            Char.ResetChar();
            MyPlatform.StartRotate();
            MySpawner.StartShoot();
            Char.SetKinematic(false);
            // 開始倒數計時
            UniTask.Void(async () => {
                curLeftTime = StartCountDownSec;
                TrainVigorSceneUI.Instance.SetCountdownImg(curLeftTime);
                while (curLeftTime > 0) {
                    if (!playing) break;
                    await UniTask.Delay(1000);
                    curLeftTime--;
                    TrainVigorSceneUI.Instance.SetCountdownImg(curLeftTime);
                }
                if (playing) endGame();
            });
        }
        void endGame() {
            playing = false;
            MyPlatform.StopRotate();
            MySpawner.StopShoot();
            float addVigorGen = MaxAddRegenVigor * ((float)(StartCountDownSec - curLeftTime) / (float)StartCountDownSec);
            addVigorGen = MyMath.Round(addVigorGen, 2);
            PopupUI.ShowAttributeUI($"體力回復增加{addVigorGen}/s", restartGame);
        }
        void controlChar_Joystick() {
            if (playing == false || Char == null || joyStick == null) return;
            var xValue = joyStick.GetHorizontalAxis();
            var yValue = joyStick.GetVerticalAxis();
            Char.Move(new Vector2(xValue, yValue));
        }
        void controlChar_Mouse() {
            if (playing == false || Char == null) return;
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 mousePos = Input.mousePosition;
            Vector2 direction = mousePos - screenCenter;
            direction = direction.normalized;
            Char.Move(direction);
        }




    }
}
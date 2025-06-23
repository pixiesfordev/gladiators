using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Hunt;
using Gladiators.Main;
using Scoz.Func;
using Spine.Unity;
using System;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace Gladiators.TrainVigor {
    public class TrainVigorManager : MonoBehaviour {
        public static TrainVigorManager Instance;
        [SerializeField] Camera MyCam;
        [SerializeField] Chara_TrainVigor Char;
        [SerializeField] Platform MyPlatform;
        [SerializeField] Spawner MySpawner;
        [SerializeField] int StartCountDownSec; // 遊戲時間
        [SerializeField] int LeftSecsToLevel2; // 剩餘幾秒時進入Level2的冰球旋轉
        [SerializeField] float MaxAddRegenVigor;
        [SerializeField] float CharDiePosY; // 腳色Y軸低於多少算遊戲失敗
        [SerializeField] SkeletonAnimation[] SpineAnis_Hand; // 轉球的手(Spine檔案)

        [SerializeField] public bool MobileControl;
        UltimateJoystick joyStick;
        bool playing = false;
        int curLeftTime;


        public void Init() {
            Instance = this;
            playing = false;
            playHands(false);
            setCam();//設定攝影機模式
            setInit();
            setChar();
            MySpawner.Init();
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
        void playHands(bool _play) {
            for (int i = 0; i < SpineAnis_Hand.Length; i++) {
                if (_play) SpineAnis_Hand[i].state.TimeScale = 1f;
                else SpineAnis_Hand[i].state.TimeScale = 0f;
            }
        }
        public void StartGame() {
            MyPlatform.SetLevel(0);
            playing = true;
            playHands(true);
            MyPlatform.ResetPlatform();
            Char.ResetChar();
            MyPlatform.StartRotate();
            MySpawner.StartShoot();
            Char.SetKinematic(false);
            bool startLV2 = false;
            // 開始倒數計時
            UniTask.Void(async () => {
                curLeftTime = StartCountDownSec;
                TrainVigorSceneUI.Instance.SetCountdownImg(curLeftTime);
                while (curLeftTime > 0) {
                    if (!playing) break;
                    await UniTask.Delay(1000);
                    curLeftTime--;
                    TrainVigorSceneUI.Instance.SetCountdownImg(curLeftTime);
                    if (!startLV2 && curLeftTime < LeftSecsToLevel2) {
                        startLV2 = true;
                        MyPlatform.SetLevel(1);
                    }
                }
                if (playing) endGame();
            });
        }
        void endGame() {
            playing = false;
            playHands(false);
            MyPlatform.StopRotate();
            MySpawner.StopShoot();
            float addVigorGen = MaxAddRegenVigor * ((float)(StartCountDownSec - curLeftTime) / (float)StartCountDownSec);
            addVigorGen = MyMath.Round(addVigorGen, 2);


            List<IProps> props = new List<IProps>();
            IProps prop = new Props_Attribute() {
                Type = PropsType.Attribute,
                Name = $"體力回復增加{addVigorGen}/s",
                SpriteName = "10203",
            };
            IProps prop2 = new Props_Attribute() {
                Type = PropsType.Attribute,
                Name = $"力量增加{addVigorGen}/s",
                SpriteName = "10201",
            };
            props.Add(prop);
            props.Add(prop2);
            GainPropsUI.Instance.ShowUI(props, restartGame);
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
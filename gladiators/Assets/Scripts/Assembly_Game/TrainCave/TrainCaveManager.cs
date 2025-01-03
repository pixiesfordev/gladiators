using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Scoz.Func;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gladiators.TrainCave {
    public class TrainCaveManager : MonoBehaviour
    {
        public static TrainCaveManager Instance;
        [SerializeField] Camera MyCam;

        //TODO:把TrainCaveUI非UI的邏輯移到這裡

        public void Init() {
            Instance = this;
            SetCam();//設定攝影機模式
            SetInit();

            #if !UNITY_EDITOR // 輸出版本要根據平台判斷操控方式
                    if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) MobileControl = true;
                    else MobileControl = false;
            #endif
            //TODO:接入搖桿邏輯 & 改變SceneUI邏輯
            //joyStick = UltimateJoystick.ReturnComponent("MyJoystick");
            //joyStick.gameObject.SetActive(MobileControl);
            //TrainCaveUI.Instance.ShowCountingdown(false);
        }

        void SetInit() {
            //TODO:在這裡做初始化
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
    }
}



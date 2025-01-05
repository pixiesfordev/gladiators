using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using Scoz.Func;

namespace Gladiators.TrainHunt {
    public class TrainHuntManager : MonoBehaviour
    {
        [SerializeField] Camera MyCam;

        public static TrainHuntManager Instance;

        // Start is called before the first frame update
        void Start() {
            
        }

        public void Init() {
            Instance = this;
            setCam();//設定攝影機模式       
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

        //TODO:如果之後角色都套用3D模型 則要把邏輯寫在這裡
    }
}


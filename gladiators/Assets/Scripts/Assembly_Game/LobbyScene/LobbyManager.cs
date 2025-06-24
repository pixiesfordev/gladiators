using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace Gladiators.Main {
    public class LobbyManager : MonoBehaviour {
        [SerializeField] Camera MyCam;
        [SerializeField] TotemPole MyTotemPole;
        [SerializeField] TotemSymbolSpawner MySymbolSpawner;
        [SerializeField] public Roulette MyRoulette;

        public static LobbyManager Instance { get; private set; }

        public void Init() {
            Instance = this;
            MyRoulette.Init();
            setTestTotems();
            setCam();//設定攝影機模式
        }
        void setTestTotems() {
            var totmes = new List<Totem>() { Totem.Trial, Totem.Camp, Totem.Battle, Totem.Destiny, Totem.Trial, Totem.Battle, Totem.Boss };
            MyTotemPole.SetTotems(totmes);
            var totemSymbols = new Dictionary<int, Totem> {
                [0] = Totem.Battle,
                [1] = Totem.Destiny,
                [2] = Totem.Camp,
                [3] = Totem.Trial,
                [4] = Totem.Battle,
                [5] = Totem.Battle,
            };
            MySymbolSpawner.SetTotems(totemSymbols);
        }
        void setCam() {
            //因為場景的攝影機有分為場景與UI, 要把場景攝影機設定為Base, UI設定為Overlay, 並在BaseCamera中加入Camera stack
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

        public void PutNextTotemOnRoulette(int _idx) {
            var totme = MyTotemPole.TakeAwayNextTotem();
            MySymbolSpawner.ChangeToken(_idx, totme);
        }
    }
}
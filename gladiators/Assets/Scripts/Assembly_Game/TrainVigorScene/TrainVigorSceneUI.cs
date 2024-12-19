using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gladiators.Main {
    public class TrainVigorSceneUI : BaseUI {
        public static TrainVigorSceneUI Instance { get; private set; }
        UltimateJoystick joyStick;


        private void Start() {
            Init();
        }
        public override void Init() {
            base.Init();
            joyStick = UltimateJoystick.ReturnComponent("MyJoystick");
        }
        public override void RefreshText() {
        }
        protected override void SetInstance() {
            Instance = this;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

using System;
using Scoz.Func;

namespace Gladiators.Main {
    public class GameTimer : MonoBehaviour {
        public static GameTimer Instance;

        public void Init() {
            Instance = this;

            //每分鐘定時檢查項目
            UniTaskManager.StartRepeatTask("DoMinteThings", DoMinteThings, 60 * 1000);

        }

        /// <summary>
        /// 剛登入後，會先執一次，之後每分鐘執行1次
        /// </summary>
        void DoMinteThings() {
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scoz.Func;
using UnityEngine.UI;
using System;

namespace Scoz.Func {
    public abstract class BaseUI : MonoBehaviour {
        protected bool IsInit = false;
        public static bool operator true(BaseUI baseUI) { return baseUI != null; }
        public static bool operator false(BaseUI baseUI) { return baseUI == null; }


        public virtual void Init() {
            if (IsInit)
                return;
            IsInit = true;
            SetInstance();
            MyText.AddRefreshFunc(RefreshText);
        }
        protected virtual void OnEnable() {

        }
        protected virtual void OnDisable() {

        }
        protected virtual void OnDestroy() {
            MyText.RemoveRefreshFunc(RefreshText);
            Destroy(gameObject);
        }
        /// <summary>
        /// 把在地化文字加到這裡會隨著切換語系時自動切換
        /// </summary>
        public abstract void RefreshText();

        /// <summary>
        /// 建議UI產生時都建立Instance方便其他地方呼叫
        /// </summary>
        protected abstract void SetInstance();

        /// <summary>
        /// 開關介面用這個
        /// </summary>
        public virtual void SetActive(bool _bool) {
            gameObject.SetActive(_bool);
        }
    }
}

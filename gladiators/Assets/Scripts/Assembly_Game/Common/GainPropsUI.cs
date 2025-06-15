using Cysharp.Threading.Tasks;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gladiators.Main {
    /// <summary>
    /// 獲得道具類型
    /// </summary>
    public enum PropsType {
        Attribute, // 屬性
        Equip, // 裝備
        Resource, // 玩家資源 
    }
    public interface IProps {
        public PropsType Type { get; set; }
        public string Name { get; set; }
        public string SpriteName { get; set; }
    }

    /// <summary>
    /// 獲得(屬性加成, 裝備, 金錢)的UI，傳入獲得的項目並依造順序跳出彈窗顯示獲得的品項
    /// </summary>
    public class GainPropsUI : BaseUI {

        public static GainPropsUI Instance { get; private set; }
        const float WAIT_SECS_TO_SHOW_CLOSE_BTN = 0.3f;

        [SerializeField] Image Img_Icon;
        [SerializeField] Text Txt_Content;
        [SerializeField] Button Btn_Close;


        List<IProps> props;
        int curPropIdx;
        Action onEndAC;

        public void ShowUI(List<IProps> _props, Action _ac) {
            if (_props == null || _props.Count == 0) {
                WriteLog.LogError("傳入的_props為null");
                return;
            }
            onEndAC = _ac;
            props = _props;
            curPropIdx = 0;
            showProps(props[curPropIdx]);
        }

        void showProps(IProps _props) {
            Btn_Close.enabled = false;
            // 等待 WAIT_SECS_TO_SHOW_CLOSE_BTN 秒後才 Enable Btn_Close
            UniTask.Void(async () => {
                await UniTask.WaitForSeconds(WAIT_SECS_TO_SHOW_CLOSE_BTN);
                Btn_Close.enabled = true;
            });
            Txt_Content.text = _props.Name;
            AddressablesLoader.GetSpriteAtlas($"Props", atlas => {
                Img_Icon.sprite = atlas.GetSprite(_props.SpriteName);
                SetActive(true);
            });
        }

        public void OnNextClick() {
            curPropIdx++;
            if (curPropIdx >= props.Count) {
                onEnd();
                return;
            }
            showProps(props[curPropIdx]);
        }
        void onEnd() {
            SetActive(false);
            onEndAC?.Invoke();
        }

        public override void RefreshText() {
        }

        protected override void SetInstance() {
            Instance = this;
        }


    }
}
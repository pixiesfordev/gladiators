using Gladiators.Main;
using Loxodon.Framework.Binding.Proxy.Targets;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Gladiators.BattleSimulation {
    public class Selection_Skill : MonoBehaviour, IItem {
        public bool IsActive { get; set; }
        [SerializeField] Image Img_Icon;
        [SerializeField] Text Txt_Title;
        [SerializeField] Text Txt_Info;
        public Button Btn;

        public JsonSkill MyJsonSkill { get; private set; }

        public void SetItem(JsonSkill _json) {
            if (_json == null) {
                WriteLog.LogError($"傳入的json為null");
                return;
            }
            MyJsonSkill = _json;
            AddressablesLoader.GetSpriteAtlas("SpellIcon", atlas => {
                Img_Icon.sprite = atlas.GetSprite(MyJsonSkill.Ref);
            });
            Txt_Title.text = _json.Name;
            Txt_Info.text = _json.Description;
        }
    }
}

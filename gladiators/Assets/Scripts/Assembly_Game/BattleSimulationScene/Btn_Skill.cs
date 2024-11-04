using Gladiators.Main;
using Loxodon.Framework.Binding.Proxy.Targets;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Gladiators.BattleSimulation {
    public class Btn_Skill : MonoBehaviour, IItem {
        public bool IsActive { get; set; }
        [SerializeField] Image Img_Icon;
        [SerializeField] Image Img_Select;
        public Button Btn;

        public JsonSkill MyJsonSkill { get; private set; }

        public void SetItem(JsonSkill _jsonSkill) {
            if (_jsonSkill == null) {
                return;
            }
            MyJsonSkill = _jsonSkill;
            AddressablesLoader.GetSpriteAtlas("SpellIcon", atlas => {
                Img_Icon.sprite = atlas.GetSprite(MyJsonSkill.ID.ToString());
            });
        }
        public void SetSelected(bool _selected) {
            Img_Select.enabled = _selected;
        }
    }
}

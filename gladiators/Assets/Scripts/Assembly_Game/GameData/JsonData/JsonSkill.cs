using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scoz.Func;
using LitJson;
using System;
using System.Linq;
using SimpleJSON;
using System.Reflection;

namespace Gladiators.Main {
    public enum SkillActivation {
        Instant,//立即發動
        Melee,//肉搏
        Passive,//被動
    }
    public enum SkillType {
        Normal,//一般技能
        Divine,//神祉技能
    }
    public enum Divine {
        Turtle,//石龜
        Snake,//妖蛇
        Bear,//巨熊
        Deer,//靈鹿
    }
    public class JsonSkill : JsonBase {
        public static string DataName { get; set; }
        public string Name {
            get {
                return JsonString.GetString_static(DataName + "_" + ID, "Name");
            }
        }
        public string Ref { get; private set; }
        public SkillActivation Activation { get; private set; }
        public int Cost { get; private set; }
        public int Initiative { get; private set; }
        public int Vigor { get; private set; }
        public SkillType MySkillType { get; private set; }
        public Divine MyDivine { get; private set; }
        public List<JsonSkillEffect> Effects { get { return JsonSkillEffect.GetSkillEffectDatas(ID); } }

        protected override void SetDataFromJson(JsonData _item) {
            JsonData item = _item;
            //反射屬性
            //var myData = JsonMapper.ToObject<JsonSkill>(item.ToJson());
            //foreach (PropertyInfo propertyInfo in this.GetType().GetProperties()) {
            //    if (propertyInfo.CanRead && propertyInfo.CanWrite) {
            //        //下面這行如果報錯誤代表上方的sonMapper.ToObject<XXXXX>(item.ToJson());<---XXXXX忘了改目前Class名稱
            //        var value = propertyInfo.GetValue(myData, null);
            //        propertyInfo.SetValue(this, value, null);
            //    }
            //}

            //自定義屬性
            foreach (string key in item.Keys) {
                switch (key) {
                    case "ID":
                        ID = int.Parse(item[key].ToString());
                        break;
                    case "Activation":
                        SkillActivation activation;
                        if (MyEnum.TryParseEnum(item[key].ToString(), out activation)) Activation = activation;
                        break;
                    case "Ref":
                        Ref = item[key].ToString();
                        break;
                    case "Cost":
                        Cost = int.Parse(item[key].ToString());
                        break;
                    case "Initiative":
                        Initiative = int.Parse(item[key].ToString());
                        break;
                    case "Vigor":
                        Vigor = int.Parse(item[key].ToString());
                        break;
                    case "Type":
                        SkillType type;
                        if (MyEnum.TryParseEnum(item[key].ToString(), out type)) MySkillType = type;
                        break;
                    case "Divine":
                        Divine divine;
                        if (MyEnum.TryParseEnum(item[key].ToString(), out divine)) MyDivine = divine;
                        break;
                    default:
                        WriteLog.LogWarning(string.Format("{0}表有不明屬性:{1}", DataName, key));
                        break;
                }
            }
        }
        protected override void ResetStaticData() {
        }
    }

}

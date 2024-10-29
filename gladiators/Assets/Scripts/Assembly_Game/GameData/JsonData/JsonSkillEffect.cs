using System.Collections.Generic;
using Scoz.Func;
using LitJson;
using System;
using Unity.Entities.UniversalDelegates;


namespace Gladiators.Main {
    public enum EffectType {
        PDmg,
        MDmg,
        TrueDmg,
        RestoreHP,
        RestoreVigor,
        MeleeDmgReflect,
        Rush,
        Pull,
        Block,
        Purge,
        Shuffle,
        Fortune,
        PermanentHp,
        Intuition,
        Enraged,
        Dodge_RangeAttack,
        RegenHP,
        RegenVigor,
        Dizzy,
        Poison,
        Bleeding,
        Burning,
        Fearing,
        Vulnerable,
        Weak,
        Fatigue,
        Protection,
        MeleeSkillReflect,
        RangeSkillReflect,
        PDefUp,
        MDefUp,
        StrUp,
        KnockbackUp,
        Barrier,
        Poisoning,
        ComboAttack,
        Vampire,
        CriticalUp,
        InitUp,
        Indomitable,
        Berserk,
        StrUpByHp,
        Chaos,
        SkillVigorUp,
        StrBurst,
        TriggerEffect_BeAttack_StrUp,
        TriggerEffect_Time_Fortune,
        TriggerEffect_WaitTime_RestoreVigor,
        TriggerEffect_BattleResult_PermanentHp,
        TriggerEffect_SkillVigorBelow_ComboAttack,
        TriggerEffect_FirstAttack_Dodge,
    }
    public class JsonSkillEffect : JsonBase {
        public static string DataName { get; set; }
        public new string ID { get; private set; }
        public string Name {
            get {
                return JsonString.GetString_static(DataName + "_" + ID, "Name");
            }
        }
        public string Description {
            get {
                return JsonString.GetString_static(DataName + "_" + ID, "Description");
            }
        }
        public int SkillID { get; private set; }
        public Target MyTarget { get; private set; }
        public List<SkillEffect> EnemyEffects = new List<SkillEffect>();
        public List<SkillEffect> MySelfEffects = new List<SkillEffect>();
        static Dictionary<int, List<JsonSkillEffect>> SkillEffectDataDic = new Dictionary<int, List<JsonSkillEffect>>();
        protected override void SetDataFromJson(JsonData _item) {
            JsonData item = _item;

            //反射屬性
            //var myData= JsonMapper.ToObject<SkillEffectJsonData>(item.ToJson());
            //// 使用反射來更新屬性
            //foreach (PropertyInfo propertyInfo in this.GetType().GetProperties()) {
            //    if (propertyInfo.CanRead && propertyInfo.CanWrite) {
            //        //下面這行如果報錯誤代表上方的sonMapper.ToObject<XXXXX>(item.ToJson());<---XXXXX忘了改目前Class名稱
            //        var value = propertyInfo.GetValue(myData, null);
            //        propertyInfo.SetValue(this, value, null);
            //    }
            //}

            //自定義屬性
            EffectType tmpTEffectType = EffectType.PDmg;
            string tmpTypeValue = "";
            foreach (string key in item.Keys) {
                switch (key) {
                    case "ID":
                        ID = (string)item[key];
                        break;
                    case "SkillID":
                        SkillID = (int)item[key];
                        break;
                    case "Target":
                        MyTarget = MyEnum.ParseEnum<Target>(item[key].ToString());
                        break;
                    default:
                        try {
                            if (key.Contains("EffectType")) {
                                tmpTEffectType = MyEnum.ParseEnum<EffectType>(item[key].ToString());
                            } else if (key.Contains("EffectValue")) {
                                tmpTypeValue = item[key].ToString();
                            } else if (key.Contains("EffectProb")) {
                                SkillEffect effect = new SkillEffect(tmpTEffectType, (double)item[key], tmpTypeValue);
                                if (MyTarget == Target.Enemy) EnemyEffects.Add(effect);
                                else if (MyTarget == Target.Myself) MySelfEffects.Add(effect);
                            }
                        } catch (Exception _e) {
                            WriteLog.LogErrorFormat(DataName + "表格格式錯誤 ID:" + ID + "    Log: " + _e);
                        }
                        break;
                }
            }
            AddToSkillEffectDic(this);
        }
        protected override void ResetStaticData() {
        }
        void AddToSkillEffectDic(JsonSkillEffect _data) {
            if (SkillEffectDataDic.ContainsKey(_data.SkillID)) SkillEffectDataDic[_data.SkillID].Add(_data);
            else SkillEffectDataDic.Add(_data.SkillID, new List<JsonSkillEffect> { _data });
        }
        public static List<JsonSkillEffect> GetSkillEffectDatas(int _id) {
            if (SkillEffectDataDic.ContainsKey(_id)) return SkillEffectDataDic[_id];
            return null;
        }
    }
    public class SkillEffect {
        public EffectType EffectType { get; private set; }
        public double Prob { get; private set; }
        public string ValueStr { get; private set; }

        public SkillEffect(EffectType _type, double _prob, string _value) {
            EffectType = _type;
            Prob = _prob;
            ValueStr = _value;
        }
    }

}

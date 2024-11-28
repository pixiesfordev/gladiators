using System.Collections.Generic;
using Scoz.Func;
using LitJson;
using System;
using Unity.Entities.UniversalDelegates;
using static Gladiators.Main.JsonSkillEffect;


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

        public static (bool, EffectType) ConvertStrToEffectType(string _str) {
            if (MyEnum.TryParseEnum(_str, out EffectType type)) {
                return (true, type);
            } else {
                WriteLog.LogError($"ConvertStrToEffectType失敗: {_str}");
                return (false, EffectType.Dizzy);
            }
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
    public static class SkillExtension {
        /// <summary>
        /// BuffIcon顯示數值類型
        /// </summary>
        public enum BuffIconValType {
            Time, // 時間: 顯示數值(剩餘1秒Icon要閃爍)
            Stack, // 層數: 顯示數值
            Passive // 被動: 不須顯示Icon數值
        }
        /// <summary>
        /// 根據EffectType取得BuffIcon顯示數值類型
        /// </summary>
        public static BuffIconValType GetStackType(this EffectType _type) {
            switch (_type) {
                case EffectType.Enraged:
                case EffectType.RegenHP:
                case EffectType.RegenVigor:
                case EffectType.Dizzy:
                case EffectType.Fearing:
                case EffectType.Protection:
                case EffectType.PDefUp:
                case EffectType.MDefUp:
                case EffectType.StrUp:
                case EffectType.KnockbackUp:
                case EffectType.Barrier:
                case EffectType.Poisoning:
                case EffectType.CriticalUp:
                case EffectType.InitUp:
                case EffectType.Indomitable:
                case EffectType.Berserk:
                case EffectType.Chaos:
                    return BuffIconValType.Time;
                case EffectType.Intuition:
                case EffectType.Dodge_RangeAttack:
                case EffectType.Poison:
                case EffectType.Bleeding:
                case EffectType.Burning:
                case EffectType.MeleeSkillReflect:
                case EffectType.RangeSkillReflect:
                case EffectType.ComboAttack:
                case EffectType.Vampire:
                case EffectType.SkillVigorUp:
                case EffectType.StrBurst:
                case EffectType.TriggerEffect_BeAttack_StrUp:
                case EffectType.TriggerEffect_Time_Fortune:
                case EffectType.TriggerEffect_FirstAttack_Dodge:
                    return BuffIconValType.Stack;
                case EffectType.StrUpByHp:
                case EffectType.TriggerEffect_WaitTime_RestoreVigor:
                case EffectType.TriggerEffect_BattleResult_PermanentHp:
                case EffectType.TriggerEffect_SkillVigorBelow_ComboAttack:
                    return BuffIconValType.Passive;
                default:
                    return BuffIconValType.Passive;
            }
        }

        /// <summary>
        /// 是否為移動限制類效果
        /// </summary>
        public static bool IsMobileRestriction(this HashSet<EffectType> _effectTypes) {
            return _effectTypes.CheckEnumsExistInHashSet(EffectType.Dizzy, EffectType.Fearing, EffectType.Pull);
        }


        /// <summary>
        /// 是否為玩家操控限制類效果
        /// </summary>
        public static bool IsPlayerControlRestriction(this HashSet<EffectType> _effectTypes) {
            return _effectTypes.CheckEnumsExistInHashSet(EffectType.Berserk);
        }

        /// <summary>
        /// 是否為立即技能限制類效果
        /// </summary>
        public static bool IsInstantSkillRestriction(this HashSet<EffectType> _effectTypes) {
            return _effectTypes.CheckEnumsExistInHashSet(EffectType.Dizzy, EffectType.Fearing, EffectType.Pull);
        }

        /// <summary>
        /// 是否為擊退免疫類效果
        /// </summary>
        public static bool IsImmuneToKnockback(this HashSet<EffectType> _effectTypes) {
            return _effectTypes.CheckEnumsExistInHashSet(EffectType.Barrier);
        }
    }

}

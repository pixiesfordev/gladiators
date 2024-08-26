using Gladiators.Main;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Skill {
    public enum EffectType {
        PDmg,
        MDmg,
        RestoreHP,
        RestoreVigor,
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
        MeleeDmgReflect,
        Rush,
        Pull,
        Enraged,
        Block,
        PDefUp,
        MDefUp,
        StrUp,
        KnockbackUp,
        Purge,
        Barrier,
        Poisoning,
        ComboAttack,
        Vampire,
        CriticalUp,
        Condition_SkillVigorBelow,
        Condition_FirstAttack,
        Condition_Charge,
        Dodge_RangeAttack,
        InitUp,
        TriggerEffect_BeAttack,
        TriggerEffect_Time,
        TriggerEffect_WaitTime,
        TriggerEffect_BattleResult,
        Indomitable,
        Berserk,
        StrUpByHp,
        Chaos,
        SkillVigorUp,
        Shuffle,
        Seal,
        Fortune,
        SkillChange,
        Intuition,
        PermanentHp,
    }

    public static List<EffectType> ConvertStrListToEffectTypes(List<string> _effectTypes) {
        List<EffectType> lsit = new List<EffectType>();
        foreach (var str in _effectTypes) {
            if (MyEnum.TryParseEnum(str, out EffectType _type)) lsit.Add(_type);
        }
        return lsit;
    }

    /// <summary>
    /// 是否為移動限制類效果
    /// </summary>
    public static bool IsMobileRestriction(this List<EffectType> _effectTypes) {
        return _effectTypes.CheckEnumsExistInList(EffectType.Dizzy, EffectType.Fearing, EffectType.Pull);
    }

    /// <summary>
    /// 是否為玩家操控限制類效果
    /// </summary>
    public static bool IsPlayerControlRestriction(this List<EffectType> _effectTypes) {
        return _effectTypes.CheckEnumsExistInList(EffectType.Berserk);
    }

    /// <summary>
    /// 是否為立即技能限制類效果
    /// </summary>
    public static bool IsInstantSkillRestriction(this List<EffectType> _effectTypes) {
        return _effectTypes.CheckEnumsExistInList(EffectType.Fearing, EffectType.Fearing, EffectType.Pull);
    }

    /// <summary>
    /// 是否為擊退免疫類效果
    /// </summary>
    public static bool IsImmuneToKnockback(this List<EffectType> _effectTypes) {
        return _effectTypes.CheckEnumsExistInList(EffectType.Barrier);
    }
}

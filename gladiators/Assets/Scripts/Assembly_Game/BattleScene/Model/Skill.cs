using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Skill {

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

using System.Collections.Generic;
using System.Numerics;

namespace Gladiators.Socket.Matchgame {
    public class BATTLESTATE : SocketContent {
        //class名稱就是封包的CMD名稱
    }
    public class BATTLESTATE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackPlayerState MyPlayerState { get; private set; }
        public PackPlayerState OpponentPlayerState { get; private set; }
        public double GameTime { get; private set; }
    }

    public class PackPlayerState {
        public string DBID { get; private set; }
        public PackDivineSkill[] DivineSkills { get; private set; }
        public PackGladiatorState GladiatorState { get; private set; }
    }
    public class PackDivineSkill {
        public int JsonID { get; private set; }
        public bool Used { get; private set; }
    }
    public class PackGladiatorState {
        public int[] HandSkillIDs { get; private set; }    // (玩家自己才會收到)
        public int MaxHP { get; private set; } // 最大生命
        public int CurHp { get; private set; }      // 目前生命
        public double CurVigor { get; private set; }  // 目前體力
        public double CurSpd { get; private set; }// 目前速度
        public double CurPos { get; private set; }// 目前位置
        public bool IsRush { get; private set; }// 是否正在衝刺中
        public List<string> EffectTypes { get; private set; }  // 狀態清單
        public int ActivedMeleeJsonSkillID { get; private set; }// (玩家自己才會收到)啟用中的肉搏技能ID, 玩家啟用中的肉搏技能, 如果是0代表沒有啟用中的肉搏技能
    }
}
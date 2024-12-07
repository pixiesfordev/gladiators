using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Socket.Matchgame {

    public class GameState_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public string State { get; private set; }
    }
    public enum PackGameState {
        GAMESTATE_INITIALIZING,         // SERVER初始化中
        GAMESTATE_INITED,               // SERVER初始化完成
        GAMESTATE_WAITINGPLAYERS,      // SERVER等待雙方玩家入場
        GAMESTATE_SELECTINGDIVINESKILL, // 選擇神祉技能
        GAMESTATE_COUNTINGDOWN,        // 戰鬥倒數開始中
        GAMESTATE_FIGHTING,             // 戰鬥中
        GAMESTATE_END                   // 結束戰鬥
    }

    public class GLADIATORSTATES_TOCLIENT : SocketContent {
        public long Time { get; private set; }
        //class名稱就是封包的CMD名稱
        public PACKGLADIATORSTATE MyState { get; private set; }
        public PACKGLADIATORSTATE OpponentState { get; private set; }
    }
    public class PACKGLADIATORSTATE {
        //class名稱就是封包的CMD名稱
        public PackVector2 CurPos { get; private set; }
        public double CurSpd { get; private set; }
        public double CurVigor { get; private set; }
        public bool Rush { get; private set; }
        public List<PackEffect> EffectDatas { get; private set; }
    }
    public class PackEffect {
        public string EffectName { get; private set; }
        public double Duration { get; private set; }
    }
    public class Hp_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public string PlayerID { get; private set; }
        public int HPChange { get; private set; }
        public string EffectType { get; private set; } // 對應EffectType列舉
        public int CurHp { get; private set; }
        public int MaxHp { get; private set; }

    }
    public class KNOCKBACK_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public string PlayerID { get; private set; }
        public PackVector2 BeforePos { get; private set; }
        public PackVector2 AfterPos { get; private set; }
        public float KnockbackDist { get; private set; }
        public bool KnockWall { get; private set; }

    }
    // 自定義 Vector2 類
    public class PackVector2 {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2 ToUnityVector2() {
            return new Vector2((float)X, (float)Y);
        }

        public static PackVector2 FromUnityVector2(UnityEngine.Vector2 vec) {
            return new PackVector2 { X = vec.x, Y = vec.y };
        }
    }

}
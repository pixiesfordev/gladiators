using System;
using System.Collections;
using UnityEngine;

namespace Gladiators.Socket {
    /// <summary>
    /// Socket封包
    /// </summary>
    public class SocketCMD<T> where T : SocketContent {
        public string CMD { get; private set; }
        public int PackID { get; private set; }
        public string ErrMsg { get; private set; }
        public string ConnToken { get; private set; }
        public T Content;

        public SocketCMD() {
        }

        public SocketCMD(T _content) {
            CMD = _content.GetType().Name;
            Content = _content;
        }
        public void SetPackID(int _value) {
            PackID = _value;
        }
        public void SetConnToken(string _value) {
            ConnToken = _value;
        }
        public SocketCMD(Type _type) {
            Content = (T)Activator.CreateInstance(_type);
        }
    }

    /// <summary>
    /// Socket封包的內容
    /// </summary>
    public class SocketContent {

        public enum MatchmakerCMD_TCP {
            AUTH_TOCLIENT,//身分驗證
            CREATEROOM_TOCLIENT,//加房間or開房
        }
        public enum MatchgameCMD_TCP {
            AUTH_TOCLIENT,// (TCP)身分驗證-送Client
            SETPLAYER_TOCLIENT,// (TCP)設定玩家資料-送Client
            SETREADY_TOCLIENT,// (TCP)遊戲準備就緒-送Client
            SETDIVINESKILL_TOCLIENT,// (TCP)賄賂選擇-送Client
            STARTFIGHTING_TOCLIENT,// (TCP)戰鬥開始-送Client
            PLAYERACTION_TOCLIENT,// (TCP)玩家指令-送Client
            ENDGAME_TOCLIENT,// (TCP)遊戲結算-送Client
            PING_TOCLIENT,// 心跳-送Client(太久沒收到回傳會視為斷線)
            GAMESTATE_TOCLIENT,  // (TCP)遊戲狀態-送Client
            MELEE_TOCLIENT, // (TCP)肉搏-送Client
            BEFORE_MELEE_TOCLIENT, // (TCP)即將肉搏-送Client
            HP_TOCLIENT, // (TCP)角鬥士生命-送Client
            GLADIATORSTATES_TOCLIENT, // (TCP)角鬥士狀態-送Client

            GMACTION_TOCLIENT,// (TCP)GM指令-送Client
        }
        public enum MatchgameCMD_UDP {
        }
    }
}
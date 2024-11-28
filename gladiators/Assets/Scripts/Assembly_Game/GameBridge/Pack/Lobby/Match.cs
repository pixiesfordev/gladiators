using System.Collections;
using UnityEngine;

namespace Gladiators.Socket.Lobby {
    public class MATCH : SocketContent {
        //class名稱就是封包的CMD名稱
        public string DBMapID;
    }
    public class MATCH_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱

        public string CreaterID { get; private set; }// 創房者PlayerID 
        public string[] PlayerIDs { get; private set; }// 房間內的所有PlayerID, 索引就是玩家的座位, 一進房間後就不會更動 PlayerIDs[0]就是在座位0玩家的PlayerID
        public string DBMapID { get; private set; }// DB地圖ID
        public string DBMatchgameID { get; private set; }// 就是RoomName由Lobby產生，格視為[DBMapID]_[玩家ID]_[時間戳]
        public string IP { get; private set; }// Matchmaker派發Matchgame的IP
        public int Port { get; private set; }// Matchmaker派發Matchgame的Port
        public string PodName { get; private set; }// Matchmaker派發Matchgame的Pod名稱
    }
}
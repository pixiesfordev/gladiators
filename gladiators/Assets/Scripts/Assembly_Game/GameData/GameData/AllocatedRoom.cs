using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.BattleSimulation;
using Gladiators.Socket;
using Gladiators.Socket.Matchgame;
using LitJson;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gladiators.Main {
    /// <summary>
    /// 玩家目前所在遊戲房間的資料，CreateRoom後會從Matchmaker回傳取得資料
    /// </summary>
    public class AllocatedRoom {
        public static AllocatedRoom Instance { get; private set; }
        GameConnector connector;
        Dictionary<Tuple<string, int>, Action<string>> CMDCallback = new Dictionary<Tuple<string, int>, Action<string>>();
        /// <summary>
        /// 創房者ID
        /// </summary>
        public string CreaterID { get; private set; }
        /// <summary>
        /// DBMatchgame的ID(由Matchmaker產生，格視為[玩家ID]_[累加數字]_[日期時間])
        /// </summary>
        public string DBMatchgameID { get; private set; }
        /// <summary>
        ///  Matchmaker派發Matchgame的IP
        /// </summary>
        public string TcpIP { get; private set; }

        /// <summary>
        ///  Matchmaker派發Matchgame的Port
        /// </summary>
        public int Port { get; private set; }

        public PackPlayer MyPackPlayer { get; private set; }
        public PackPlayer OpponentPackPlayer { get; private set; }

        // Ping相關宣告
        Accumulator accumulator_Ping;
        private CancellationTokenSource pingCancellationTokenSource;
        private Dictionary<long, long> pingSendDic = new Dictionary<long, long>();
        private const int MaxLatencySamples = 10;
        private Queue<double> latencySamples = new Queue<double>();
        /// <summary>
        /// 網路延遲平均豪秒數
        /// </summary>
        public double Lantency { get; private set; }

        long firstPackServerTimestamp; // 配對開始後第一次收到封包的時間戳
        public long ClientTimeStamp { get { return (long)(Time.realtimeSinceStartup * 1000) + firstPackServerTimestamp; } } // 本地相對時間戳
        public long RenderTimestamp { get { return ClientTimeStamp - (long)Lantency - INTERPOLATION_DELAY_MILISECS; } } // 本地渲染時間戳
        void setFirstPackServerTimestamp(long _time) {
            if (firstPackServerTimestamp == 0) {
                firstPackServerTimestamp = _time - (long)(Time.realtimeSinceStartup * 1000);
            }
        }
        const long INTERPOLATION_DELAY_MILISECS = 100; // 緩衝延遲毫秒(進行插值計算時clientRender時間太接近或超前最新的封包的過程會找不到新封包而無法進行差值，所以要定義一個延遲時間)

        public enum GameState {
            GameState_NotInGame,// 不在遊戲中
            GameState_UnAuth,//已經從Matchmaker收到配對房間但還沒從Matchgame收到Auth回傳true
            GameState_WaitingPlayersData,//等待收到雙方玩家資料
            GameState_WaitingPlayersReady,//等待雙方玩家進入BattleScene
            GameState_SelectingDivineSkill,//等待選擇神祉技能中
            GameState_CountingDown,//戰鬥倒數中
            GameState_Fighting,//戰鬥中
            GameState_End,//戰鬥結束
        }
        public GameState CurGameState { get; private set; } = GameState.GameState_NotInGame;
        public void SetGameState(GameState _value) {
            CurGameState = _value;
            WriteLog.Log("遊戲狀態切換為:" + _value);
        }
        public static void Init() {
            Instance = new AllocatedRoom();
        }

        /// <summary>
        /// 設定被Matchmaker分配到的房間資料，CreateRoom後會從Matchmaker回傳取得此資料
        /// </summary>
        public void SetRoom(GameConnector _connector, string _createID, string _dbMatchgameID, string _ip, int _port) {
            connector = _connector;
            connector.RegisterOnPacketReceived(onReceiveMsg);
            CreaterID = _createID;
            DBMatchgameID = _dbMatchgameID;
            TcpIP = _ip;
            Port = _port;
            firstPackServerTimestamp = 0;
            WriteLog.LogColorFormat("設定被Matchmaker分配到的房間資料: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));
        }
        void onReceiveMsg(string _msg) {
            try {
                SocketCMD<SocketContent> data = JsonMapper.ToObject<SocketCMD<SocketContent>>(_msg);
                Tuple<string, int> cmdID = new Tuple<string, int>(data.CMD, data.PackID);
                SocketContent.MatchgameCMD_TCP cmdType;
                if (!MyEnum.TryParseEnum(data.CMD, out cmdType)) {
                    WriteLog.LogErrorFormat("收到錯誤的命令類型: {0}", cmdType);
                    return;
                } else {
                    // 不輸出的Conn Log加到清單中
                    List<string> dontShowLogCMDs = new List<string>();
                    dontShowLogCMDs.Add(SocketContent.MatchgameCMD_TCP.PING_TOCLIENT.ToString());
                    dontShowLogCMDs.Add(SocketContent.MatchgameCMD_TCP.GLADIATORSTATES_TOCLIENT.ToString());
                    if (!dontShowLogCMDs.Contains(data.CMD)) WriteLog.LogColor($"(TCP)接收: {_msg}", WriteLog.LogType.Connection);
                    if (CMDCallback.TryGetValue(cmdID, out Action<string> _cb)) {
                        CMDCallback.Remove(cmdID);
                        _cb?.Invoke(_msg);
                    }
                    switch (cmdType) {
                        case SocketContent.MatchgameCMD_TCP.AUTH_TOCLIENT:
                            var authPacket = LitJson.JsonMapper.ToObject<SocketCMD<AUTH_TOCLIENT>>(_msg);
                            HandleAuth(authPacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.SETPLAYER_TOCLIENT:
                            var setPlayerPacket = LitJson.JsonMapper.ToObject<SocketCMD<SETPLAYER_TOCLIENT>>(_msg);
                            HandleSetPlayer(setPlayerPacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.SETREADY_TOCLIENT:
                            var readyPacket = LitJson.JsonMapper.ToObject<SocketCMD<SETREADY_TOCLIENT>>(_msg);
                            HandleReady(readyPacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.SETDIVINESKILL_TOCLIENT:
                            var bribePacket = LitJson.JsonMapper.ToObject<SocketCMD<SETDIVINESKILL_TOCLIENT>>(_msg);
                            HandleSetDivineSkill(bribePacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.PLAYERACTION_TOCLIENT:
                            //收到封包要先進行PLAYERACTION_TOCLIENT反序列化之後再根據ActionType類型來使用泛型解析成正確的ActionContent
                            var actionBasePacket = JsonMapper.ToObject<SocketCMD<PLAYERACTION_TOCLIENT<JsonData>>>(_msg);
                            var actionContentJson = actionBasePacket.Content.ActionContent.ToJson();
                            HandlerPlayerAction(actionBasePacket.Content.ActionType, actionBasePacket.Content.PlayerDBID, actionContentJson);
                            break;
                        case SocketContent.MatchgameCMD_TCP.PING_TOCLIENT:
                            var pingPacket = LitJson.JsonMapper.ToObject<SocketCMD<PING_TOCLIENT>>(_msg);
                            HandlerPing(pingPacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.GAMESTATE_TOCLIENT:
                            var statePacket = LitJson.JsonMapper.ToObject<SocketCMD<GameState_TOCLIENT>>(_msg);
                            HandlerGameState(statePacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.MELEE_TOCLIENT:
                            var meleePacket = LitJson.JsonMapper.ToObject<SocketCMD<MELEE_TOCLIENT>>(_msg);
                            HandlerMelee(meleePacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.BEFORE_MELEE_TOCLIENT:
                            var beforeMeleePacket = LitJson.JsonMapper.ToObject<SocketCMD<BEFORE_MELEE_TOCLIENT>>(_msg);
                            HandlerBeforeMelee(beforeMeleePacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.GLADIATORSTATES_TOCLIENT:
                            var gStatePacket = LitJson.JsonMapper.ToObject<SocketCMD<GLADIATORSTATES_TOCLIENT>>(_msg);
                            HandlerGladiatorStates(gStatePacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.HP_TOCLIENT:
                            var hpPacket = LitJson.JsonMapper.ToObject<SocketCMD<Hp_TOCLIENT>>(_msg);
                            HandlerHp(hpPacket);
                            break;
                        case SocketContent.MatchgameCMD_TCP.GMACTION_TOCLIENT:
                            //收到封包要先進行GMACTION_TOCLIENT反序列化之後再根據ActionType類型來使用泛型解析成正確的ActionContent
                            var gmActionBasePacket = JsonMapper.ToObject<SocketCMD<GMACTION_TOCLIENT<JsonData>>>(_msg);
                            var gmActionContentJson = gmActionBasePacket.Content.ActionContent.ToJson();
                            HandlerGMAction(gmActionBasePacket.Content.ActionType, gmActionBasePacket.Content.PlayerDBID, gmActionContentJson);
                            break;
                        default:
                            WriteLog.LogErrorFormat("收到尚未定義的命令類型: {0}", cmdType);
                            break;
                    }
                }
            } catch (Exception _e) {
                WriteLog.LogError("Parse收到的封包時出錯 : " + _e.ToString());
            }
        }


        /// <summary>
        /// 清空配對房間(AllocatedRoom)資訊
        /// </summary>
        public void clearRoom() {
            SetGameState(GameState.GameState_NotInGame);
            CreaterID = null;
            DBMatchgameID = null;
            Port = 0;
            stopPingLoop();
            connector.Disconnect();
            WriteLog.LogColorFormat("清空配對房間(AllocatedRoom)資訊: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));
        }
        void sendPing() {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var cmd = new SocketCMD<PING>(new PING());
            connector.Send(cmd);
            pingSendDic[cmd.PackID] = currentTime;
        }
        /// <summary>
        /// 送AUTH給Server
        /// </summary>
        public void Auth() {
            var dbPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            var cmd = new SocketCMD<AUTH>(new AUTH(dbPlayer.ConnToken));
            connector.Send(cmd);
        }
        /// <summary>
        /// 送玩家資料給Server
        /// </summary>
        public void SetPlayer(string _dbGladiatorID) {
            var cmd = new SocketCMD<SETPLAYER>(new SETPLAYER(_dbGladiatorID));
            connector.Send(cmd);
        }
        /// <summary>
        /// 通知Server此玩家已經進入BattleScene
        /// </summary>
        public void SetReady() {
            var cmd = new SocketCMD<SETREADY>(new SETREADY());
            connector.Send(cmd);
        }

        /// <summary>
        /// 通知Server此玩家已經進入BattleScene
        /// </summary>
        public void SetDivineSkills(int[] _jsonSKillIDs) {
            var cmd = new SocketCMD<SETDIVINESKILL>(new SETDIVINESKILL(_jsonSKillIDs));
            connector.Send(cmd);
        }
        /// <summary>
        /// 通知Server此玩家已經進入Run
        /// </summary>
        public void SetRun(bool isRun) {
            var cmd = new SocketCMD<PLAYERACTION>(new PLAYERACTION("Action_Rush", new PackAction_Rush(isRun)));
            connector.Send(cmd);
        }

        /// <summary>
        /// 設定技能
        /// </summary>
        public void ActiveSkill(int _skillID, bool _on) {
            var cmd = new SocketCMD<PLAYERACTION>(new PLAYERACTION(PLAYERACTION.PackActionType.ACTION_SKILL.ToString(), new PackAction_Skill(_on, _skillID)));
            connector.Send(cmd);
        }
        /// <summary>
        /// (GM)設定角鬥士
        /// </summary>
        public void GMSetGladiator(int _gladiatorID, int[] _skillIDs) {
            var cmd = new SocketCMD<GMACTION>(new GMACTION(GMACTION.GMActionType.GMACTION_SETGLADIATOR.ToString(), new PackGMAction_SetGladiator(_gladiatorID, _skillIDs)));
            connector.Send(cmd);
        }
        public void LeaveRoom() {
            clearRoom();
            BattleManager.Instance.BattleEnd(afterKO);
        }

        void afterKO() {
            PopupUI.InitSceneTransitionProgress(0);
            PopupUI.CallSceneTransition(MyScene.BattleSimulationScene);
            SetGameState(GameState.GameState_NotInGame);
        }

        /// <summary>
        /// 收到Ping封包回傳
        /// </summary>
        void HandlerPing(SocketCMD<PING_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            long pingReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long pingSendTime = pingSendDic[_packet.PackID];
            pingSendDic.Remove(_packet.PackID);
            // 計算Ping送返時間
            long rtt = pingReceiveTime - pingSendTime;
            double oneWayLatency = rtt / 2.0;
            updateLatency(oneWayLatency);
        }
        /// <summary>
        /// Matchgame驗證完成時執行
        /// </summary>
        public void HandleAuth(SocketCMD<AUTH_TOCLIENT> _packet) {
            if (_packet.Content == null || !_packet.Content.IsAuth) {
                WriteLog.LogError("Auth錯誤 遊戲無法開始");
                return;
            }
            SetGameState(GameState.GameState_WaitingPlayersData);
            //SetPlayer("660926d4d0b8e0936ddc6afe");
            SimulationUI.Instance.SendSimulationSetting();
        }
        /// <summary>
        /// 收到雙方玩家資料後, 將目前狀態設定為GotEnemy並通知BattleScene送Ready
        /// </summary>
        public void HandleSetPlayer(SocketCMD<SETPLAYER_TOCLIENT> _packet) {
            MyPackPlayer = _packet.Content.MyPackPlayer;
            OpponentPackPlayer = _packet.Content.OpponentPackPlayer;
            //收到雙方玩家資料
            if (MyPackPlayer != null && OpponentPackPlayer != null && !string.IsNullOrEmpty(MyPackPlayer.DBID) && !string.IsNullOrEmpty(OpponentPackPlayer.DBID)) {
                SetGameState(GameState.GameState_WaitingPlayersReady);
                if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString())
                    PopupUI.CallSceneTransition(MyScene.BattleScene);
                BattleSceneUI.InitPlayerData(MyPackPlayer.MyPackGladiator, OpponentPackPlayer.MyPackGladiator, MyPackPlayer.MyPackGladiator.HandSkillIDs);
            }

            // 開始PingLoop
            accumulator_Ping = new Accumulator();
            pingLoop().Forget();
            setFirstPackServerTimestamp(_packet.Content.Time);
        }
        /// <summary>
        /// 收到準備完成, 收到雙方準備完成 就會進入神祉技能選擇階段
        /// </summary>
        public void HandleReady(SocketCMD<SETREADY_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            var allReady = _packet.Content.PlayerReadies.All(a => a);
            if (allReady) {
                SetGameState(GameState.GameState_SelectingDivineSkill);
                BattleManager.Instance.StartSelectDivineSkill();
            }
        }
        /// <summary>
        /// 收到神祉技能選擇封包, 如果雙方的資料都收到就開始遊戲
        /// </summary>
        public void HandleSetDivineSkill(SocketCMD<SETDIVINESKILL_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            if (_packet.Content == null) return;
            //更新介面神祉技能卡牌
            BattleSceneUI.Instance?.SetDivineSkillData(_packet.Content.JsonSkillIDs);
        }
        /// <summary>
        /// 收到戰鬥資訊封包, 存儲封包資料
        /// </summary>
        public void HandlerGladiatorStates(SocketCMD<GLADIATORSTATES_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            if (BattleManager.Instance != null) BattleController.Instance.UpdateGladiatorsState(_packet.PackID, _packet.Content.Time, _packet.Content.MyState, _packet.Content.OpponentState);
        }
        /// <summary>
        /// 收到角鬥士血量更新
        /// </summary>
        public void HandlerHp(SocketCMD<Hp_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            BattleSceneUI.Instance.UpdateGladiatorHP(_packet.Content.PlayerID, _packet.Content.HPChange);
            BattleController.Instance.ShowBattleNumber(_packet.Content.PlayerID, _packet.Content.HPChange, _packet.Content.EffectType);
        }
        /// <summary>
        /// 收到Player ACTION回傳
        /// </summary>
        void HandlerPlayerAction(string _actionType, string _playerID, string _jsonStr) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            PLAYERACTION.PackActionType actionType;
            if (MyEnum.TryParseEnum(_actionType, out actionType)) {
                switch (actionType) {
                    case PLAYERACTION.PackActionType.ACTIVE_MELEE_SKILL: // 收到肉搏技能啟用
                        var activeMeleeSkill = JsonMapper.ToObject<PackAction_ActiveMeleeSkill_ToClient>(_jsonStr);
                        break;
                    case PLAYERACTION.PackActionType.INSTANT_SKILL: // 收到即時技能發動
                        var instantSkillPack = JsonMapper.ToObject<PackAction_InstantSkill_ToClient>(_jsonStr);
                        if (MyPackPlayer.DBID == _playerID) BattleSceneUI.Instance.CastInstantSkill(instantSkillPack.NewSkilID);
                        BattleController.Instance.PlayInstantSkill(_playerID, instantSkillPack.SkillID);
                        break;
                    default:
                        Debug.LogError($"收到未實作處理的 PackActionType: {_actionType}");
                        break;
                }
            }
        }
        /// <summary>
        /// 收到GM ACTION回傳
        /// </summary>
        void HandlerGMAction(string _actionType, string _playerID, string _jsonStr) {
            GMACTION.GMActionType actionType;
            if (MyEnum.TryParseEnum(_actionType, out actionType)) {
                switch (actionType) {
                    case GMACTION.GMActionType.GMACTION_SETGLADIATOR:
                    //var setGladiator = JsonMapper.ToObject<PackGMAction_SetGladiator_ToClient>(_jsonStr);
                    //string skillStr = "";
                    //for (int i = 0; i < setGladiator.SkillIDs.Length; i++) {
                    //    if (i != 0) skillStr += ", ";
                    //    skillStr += setGladiator.SkillIDs[i];
                    //}
                    //WriteLog.LogError($"GMAction回傳設定角鬥士ID: {setGladiator.GladiatorID} 技能: {skillStr}");
                    //break;
                    default:
                        Debug.LogError($"收到未實作處理的 PackActionType: {_actionType}");
                        break;
                }
            }
        }
        /// <summary>
        /// 收到GameState更新
        /// </summary>
        void HandlerGameState(SocketCMD<GameState_TOCLIENT> _packet) {
            PackGameState gameState;
            if (MyEnum.TryParseEnum(_packet.Content.State, out gameState)) {
                WriteLog.LogColor($"SERVER狀態: {gameState}", WriteLog.LogType.Connection);
                switch (gameState) {
                    case PackGameState.GAMESTATE_COUNTINGDOWN:
                        SetGameState(GameState.GameState_CountingDown);
                        //關閉神祇技能選擇介面(做完演出後才去執行後續動作)
                        DivineSelectUI.Instance?.CloseUI(() => {
                            BattleSceneUI.Instance.StartBattle();
                        });
                        break;
                    case PackGameState.GAMESTATE_FIGHTING:
                        SetGameState(GameState.GameState_Fighting);
                        BattleManager.Instance.StartGame();
                        break;
                    case PackGameState.GAMESTATE_END:
                        SetGameState(GameState.GameState_End);
                        LeaveRoom();
                        break;
                }
            }
        }
        /// <summary>
        /// 收到肉搏封包
        /// </summary>
        void HandlerMelee(SocketCMD<MELEE_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            BattleManager.Instance.Melee(_packet.Content);
            if (_packet.Content.NewSkilID != 0) // NewSkilID是0代表沒有施放肉搏技能 所以也沒有抽新的技能
                BattleSceneUI.Instance.CastMeleeSkill(_packet.Content.NewSkilID);
        }
        /// <summary>
        /// 收到肉搏演出封包
        /// </summary>
        void HandlerBeforeMelee(SocketCMD<BEFORE_MELEE_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString() || BattleManager.Instance == null) return;
            BattleController.Instance.PlayMeleeSkill(_packet.Content);
        }



        async UniTaskVoid pingLoop() {
            pingCancellationTokenSource = new CancellationTokenSource();
            var token = pingCancellationTokenSource.Token;
            try {
                while (!token.IsCancellationRequested) {
                    sendPing();
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                }
            } catch (OperationCanceledException) {
                WriteLog.Log("PingLoop cancelled.");
            } catch (Exception ex) {
                WriteLog.LogError($"PingLoop error: {ex.Message}");
            }
        }
        public void stopPingLoop() {
            pingCancellationTokenSource?.Cancel();
        }
        /// <summary>
        /// 更新網路延遲估算
        /// </summary>
        void updateLatency(double newOneWayLatency) {
            if (latencySamples.Count >= MaxLatencySamples) {
                latencySamples.Dequeue();
            }
            latencySamples.Enqueue(newOneWayLatency);
            double sum = 0.0;
            foreach (var latency in latencySamples) {
                sum += latency;
            }
            Lantency = sum / latencySamples.Count;
            WriteLog.LogColor($"近{MaxLatencySamples}筆Ping計算出的網路延遲為: {MyMath.Round((float)Lantency, 2)} ms", WriteLog.LogType.Connection);
        }




    }
}

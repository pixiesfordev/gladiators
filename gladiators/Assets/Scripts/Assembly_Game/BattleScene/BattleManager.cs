using Gladiators.Main;
using Gladiators.Socket;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;
using Unity.Entities.UniversalDelegates;
using Loxodon.Framework.Binding;
using Cinemachine;
using Cysharp.Threading.Tasks.Triggers;
using System.Linq;
using Unity.VisualScripting;

namespace Gladiators.Battle {
    public class BattleManager : MonoBehaviour {
        public static BattleManager Instance;
        [SerializeField] public CinemachineVirtualCamera vCam;
        [SerializeField] public CinemachineTargetGroup vTargetGroup;
        [SerializeField] Camera MyCam;
        public Camera BattleCam => MyCam;

        [SerializeField] int BattleDefaultTime = 60;//設定戰鬥時間

        [SerializeField] bool BattleIsEnd = false;//控制戰鬥是否結束 先序列化出來供測試用
        int BattleLeftTime;

        JsonSkill SelectedMeleeSkill;

        [HeaderAttribute("===場景物件控制===")]
        [SerializeField] BattleModelController battleModelController;
        [SerializeField] public bool isRightPlayer = false;

        [HeaderAttribute("==============TEST==============")]
        //測試參數區塊
        [Tooltip("重置戰鬥")][SerializeField] bool bResetBattle = false;
        [Tooltip("前端演示測試 打勾表示只使用純前端邏輯模擬")][SerializeField] bool bFrontEndTest = true;

        void Update() {
            if (bResetBattle) {
                ResetBattle();
                bResetBattle = false;
                battleModelController.BattleStart();
            }
        }

        public async UniTask Init() {
            Instance = this;
            SetCam();//設定攝影機模式
            await CheckGameState();
        }
        async UniTask CheckGameState() {
            switch (AllocatedRoom.Instance.CurGameState) {
                case AllocatedRoom.GameState.NotInGame:
                    break;
                case AllocatedRoom.GameState.UnAuth://需要等待Matchgame Server回傳Auth成功
                    Debug.Log("需要等待Matchgame Server回傳Auth成功");
                    PopupUI.ShowLoading(JsonString.GetUIString("Loading"));
                    break;
                case AllocatedRoom.GameState.GotPlayer://如果已經收到雙方玩家資料就送Ready
                    Debug.Log("如果已經收到雙方玩家資料就送Ready");
                    GameConnector.Instance.SetReady();
                    break;
            }
            await UniTask.Yield();
        }
        public void GotOpponent() {
            GameConnector.Instance.SetReady();
        }
        public void GoBribe() { //開始遊戲用
            WriteLog.LogError("開始賄賂");
            //叫出神址選擇介面(舊稱賄賂) 這裡顯示介面讓玩家選擇
            if (DivineSelectUI.Instance != null)
                DivineSelectUI.Instance.SetActive(true);
        }
        public void StartGame(PackPlayerState _playerStates) {
            WriteLog.LogError("開始遊戲");
            if (_playerStates == null) { WriteLog.LogFormat("找不到玩家自己的資料!"); return;}
            //更新介面神祉技能卡牌
            BattleSceneUI.Instance?.SetDivineSkillData(_playerStates.BribeSkills);
            //關閉神祇技能選擇介面(做完演出後才去執行後續動作)
            DivineSelectUI.Instance?.CloseUI(() => { 
            ResetBattle();
            });
        }

        void SetCam() {
            //因為戰鬥場景的攝影機有分為場景與UI, 要把場景攝影機設定為Base, UI設定為Overlay, 並在BaseCamera中加入Camera stack
            UICam.Instance.SetRendererMode(CameraRenderType.Overlay);
            AddCamStack(UICam.Instance.MyCam);
        }
        /// <summary>
        /// 將指定camera加入到MyCam的CameraStack中
        /// </summary>
        void AddCamStack(Camera _cam) {
            if (_cam == null) return;
            var cameraData = MyCam.GetUniversalAdditionalCameraData();
            if (cameraData == null) return;
            cameraData.cameraStack.Add(_cam);
        }

        //場地及人物生成
        public async UniTask CreateTerrainAndChar(PackPlayer[] _packPlayers) {
            battleModelController.CreateTerrain(0);
            battleModelController.CreateCharacter(0, 0, _packPlayers);

            await battleModelController.WaitCharacterCreate();
            ResetBattle();
        }

        //重啟戰鬥
        void ResetBattle() {
            //相關參數在此重設 設定完才去更新UI
            BattleIsEnd = false;
            BattleLeftTime = BattleDefaultTime;
            ResetBattleSceneUI();
            ResetBattleModelController();
            //TODO:這裡可能需要加入一個延遲等待開場演出
            CountDownBattleTime().Forget();
        }

        //重設戰鬥UI
        void ResetBattleSceneUI() {
            if (BattleSceneUI.Instance == null)
                return;
            BattleSceneUI.Instance.SetTimeText(BattleLeftTime);
        }

        void ResetBattleModelController() {
            if (battleModelController == null) return;

            //battleModelController.BattleReset(); //正常不會有這行為，因為沒有重新戰鬥
        }

        void testStartGame() {
            Debug.Log("本地測試開始");

            battleModelController.BattleStart();
        }

        #region 技能施放
        /// <summary>
        /// 施放直接觸發技能
        /// </summary>
        /// <param name="_skill">技能Data</param>
        public void CastInstantSKill(JsonSkill _skill) {
            //TODO:在這裡做演出
            if (bFrontEndTest) {
                //走前端流程 直接演出
            } else {
                //走後端流程送包
            }
            Debug.LogFormat("施放直接觸發技能! 技能ID: {0}", _skill.ID);
        }

        /// <summary>
        /// 設定碰撞觸發技能
        /// </summary>
        /// <param name="_skill">技能Data</param>
        /// <param name="_selected">是否選中技能</param>
        public void SetMeleeSkill(JsonSkill _skill, bool _selected) {
            //會先把技能存在在此 等到真正碰撞後才會施放
            if (_selected) {
                SelectedMeleeSkill = _skill;
                Debug.LogFormat("設定碰撞觸發技能! 技能ID: {0}", _skill.ID);
            } else {
                SelectedMeleeSkill = null;
                Debug.LogFormat("取消碰撞觸發技能! 技能ID: {0}", _skill.ID);
            }
        }

        /// <summary>
        /// 施放碰撞觸發技能
        /// </summary>
        public void CastMeleeSkill() {
            //TODO:實際發生碰撞請呼叫此方法來進行判定
            if (bFrontEndTest) {
                //走前端流程 直接演出
                if (SelectedMeleeSkill == null) {
                    //沒選技能 則純粹挨打
                    Debug.Log("沒選擇碰撞觸發技能!");
                } else {
                    //有選技能 施放技能
                    Debug.LogFormat("施放碰撞技能. ID: {0}", SelectedMeleeSkill.ID);
                }
            } else {
                //走後端流程送包
            }
        }
        #endregion

        //TODO:
        //體力條計算(自動恢復 使用技能消耗 衝刺消耗)
        //對撞計算(傷害計算>>血量 擊退距離>>人物模型位置改變 撞牆判定>>是否被打到邊界 要額外扣血演出 這個建議不要跟傷害一起演出 而是先有撞牆演出後再扣血)
        //血量計算(傷害量/恢復量傳給UI演出)
        //戰鬥結算
        //buff設定

        //衝次
        public void GoRun(bool isRun) {
            GameConnector.Instance.SetRun(isRun);
        }

        //戰鬥剩餘秒數計算
        async UniTaskVoid CountDownBattleTime() {
        ReCount:
            //Debug.Log("測試倒數秒數.現在秒數: " + BattleLeftTime);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            //有可能會發生剩下一秒的時候分出勝負 所以一秒數完還是要再次確認是否已經分出勝負 沒有才繼續數秒
            if (!BattleIsEnd) {
                BattleLeftTime -= 1;
                BattleSceneUI.Instance.SetTimeText(BattleLeftTime);
                if (BattleLeftTime <= 0)
                    BattleEnd();
                else
                    goto ReCount;
            }
            //Debug.Log("結束時間倒數計算!");
        }

        //戰鬥結束
        void BattleEnd() {
            //進入結算環節 避免出bug 如果已經開始結算就不要重複執行
            if (BattleIsEnd) return;
            BattleIsEnd = true;
            battleModelController.BattleEnd();
            Debug.Log("戰鬥結束");
        }

        bool isfirstTime = false;
        public void StartBattleTimer() {
            if (!isfirstTime) {
                isfirstTime = true;
                battleRunStates = new List<BattleRunState>();

                ServerBattleTimer().Forget();
            }
        }

        float BattleTimer = 0.00f;
        async UniTaskVoid ServerBattleTimer() {
            BattleTimer = 0.00f;

        BattleTimber:
            var tempTest = battleRunStates.FirstOrDefault();
            var tempAction = battleActionStates.FirstOrDefault();
            if (tempTest != null && BattleTimer >= tempTest.GameTime && tempTest.Start) {
                tempTest.End = true;
                battleRunStates.Remove(tempTest);
                tempTest = battleRunStates.FirstOrDefault();
            }
            if (tempTest != null && !tempTest.Start) {
                tempTest.Start = true;
                doBattleState(tempTest.PlayerStates, tempTest.GameTime, tempTest.BattleStateType);
            }
            if (tempAction != null && !tempAction.Start) {
                tempAction.Start = true;
                doBattleState(tempAction.PlayerStates, tempAction.GameTime, tempAction.BattleStateType);
            }
            await UniTask.Delay(TimeSpan.FromMilliseconds(10));
            BattleTimer += 0.01f;
            goto BattleTimber;
        }
        public enum BattleStateType : byte {
            Default = 0,
            Move = 1,
            MoveAttack = 2,
            Knockback = 3,
            SkillAttack = 4,
        }
        List<BattleRunState> battleRunStates = new List<BattleRunState>();
        List<BattleRunState> battleActionStates = new List<BattleRunState>();
        public void setBattleState(PackPlayerState[] _playerStates, double gameTime, BattleStateType battleStateType) {
            battleRunStates.Add(new BattleRunState(gameTime, _playerStates, battleStateType));
        }
        public void setBattleState_byAction(PackPlayerState[] _playerStates, double gameTime, BattleStateType battleStateType) {
            battleActionStates.Add(new BattleRunState(gameTime, _playerStates, battleStateType));
        }
        public void doBattleState(PackPlayerState[] _playerStates, double gameTime, BattleStateType battleStateType) {
            switch (battleStateType) {
                case BattleStateType.Move:  //玩家移動
                    battleModelController.Movement(_playerStates[0], _playerStates[1]);
                    break;
                case BattleStateType.MoveAttack:    //玩家移動並近身攻擊
                    battleModelController.Movement(_playerStates[0], _playerStates[1]);
                    break;
                case BattleStateType.Knockback:
                    battleModelController.GetAttack(_playerStates[0], _playerStates[1]);
                    break;

                case BattleStateType.Default:
                default:
                    break;
            }
        }
    }
}

public class BattleRunState {
    //class名稱就是封包的CMD名稱
    public double GameTime { get; private set; }
    public PackPlayerState[] PlayerStates { get; private set; }
    public bool Start { get; set; } = false;
    public bool End { get; set; } = false;
    public Gladiators.Battle.BattleManager.BattleStateType BattleStateType { get; private set; }

    public BattleRunState(double gameTime, PackPlayerState[] _playerStates, Gladiators.Battle.BattleManager.BattleStateType battleStateType) {
        GameTime = gameTime;
        PlayerStates = _playerStates;
        Start = false;
        End = false;
        BattleStateType = battleStateType;
    }
}
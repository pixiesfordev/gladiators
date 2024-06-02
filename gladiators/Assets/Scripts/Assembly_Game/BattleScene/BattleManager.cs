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

namespace Gladiators.Battle {
    public class BattleManager : MonoBehaviour {
        public static BattleManager Instance;
        [SerializeField] Camera MyCam;
        public Camera BattleCam => MyCam;

        [SerializeField] int BattleDefaultTime = 60;//設定戰鬥時間

        [SerializeField] bool BattleIsEnd = false;//控制戰鬥是否結束 先序列化出來供測試用
        int BattleLeftTime;


        [HeaderAttribute("==============TEST==============")]
        //測試參數區塊
        [Tooltip("重置戰鬥")][SerializeField] bool bResetBattle = false;

        void Update() {
            if (bResetBattle) {
                ResetBattle();
                bResetBattle = false;
            }
        }

        public void Init() {
            Instance = this;
            SetCam();//設定攝影機模式
            ResetBattle();
            CheckGameState();
        }
        void CheckGameState() {
            switch (AllocatedRoom.Instance.CurGameState) {
                case AllocatedRoom.GameState.NotInGame://本地測試
                    break;
                case AllocatedRoom.GameState.UnAuth://需要等待Matchgame Server回傳Auth成功
                    PopupUI.ShowLoading(JsonString.GetUIString("Loading"));
                    break;
                case AllocatedRoom.GameState.GotPlayer://如果已經收到雙方玩家資料就送Ready
                    GameConnector.Instance.SetReady();
                    break;
            }
        }
        public void GotOpponent() {
            GameConnector.Instance.SetReady();
        }
        public void GoBribe() {
            WriteLog.LogError("開始賄賂");
        }
        public void StartGame() {
            WriteLog.LogError("開始遊戲");
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

        //重啟戰鬥
        void ResetBattle() {
            //相關參數在此重設 設定完才去更新UI
            BattleIsEnd = false;
            BattleLeftTime = BattleDefaultTime;
            ResetBattleSceneUI();
            //TODO:這裡可能需要加入一個延遲等待開場演出
            CountDownBattleTime().Forget();
        }

        //重設戰鬥UI
        void ResetBattleSceneUI() {
            if (BattleSceneUI.Instance == null)
                return;
            BattleSceneUI.Instance.SetTimeText(BattleLeftTime);
        }

        //TODO:
        //技能施放
        //體力條計算
        //對撞計算
        //扣血計算
        //戰鬥結算
        //buff設定

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
            Debug.Log("戰鬥結束");
        }

    }
}
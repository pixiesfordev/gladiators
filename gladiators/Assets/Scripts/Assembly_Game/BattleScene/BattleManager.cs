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

        public float GameTime { get; private set; }//遊戲時間
        public int LeftGameTime { get { return Mathf.RoundToInt((float)BattleDefaultTime - GameTime); } }//遊戲剩餘時間

        //JsonSkill SelectedMeleeSkill;

        [HeaderAttribute("===場景物件控制===")]
        [SerializeField] BattleController battleModelController;
        [SerializeField] public bool isRightPlayer = false;

        [HeaderAttribute("==============TEST==============")]
        //測試參數區塊
        [Tooltip("重置戰鬥")][SerializeField] bool bResetBattle = false;
        //[Tooltip("前端演示測試 打勾表示只使用純前端邏輯模擬")][SerializeField] bool bFrontEndTest = true;


        public async UniTask Init() {
            Instance = this;
            SetCam();//設定攝影機模式
            await CheckGameState();
        }
        public async UniTask CheckGameState() {
            switch (AllocatedRoom.Instance.CurGameState) {
                case AllocatedRoom.GameState.GameState_NotInGame:
                    WriteLog.LogError($"錯誤的GameState:{AllocatedRoom.Instance.CurGameState}");
                    break;
                case AllocatedRoom.GameState.GameState_UnAuth://需要等待Matchgame Server回傳Auth成功
                    WriteLog.LogError($"錯誤的GameState:{AllocatedRoom.Instance.CurGameState}");
                    break;
                case AllocatedRoom.GameState.GameState_WaitingPlayersData://需要等待收到雙方SetPlayer回傳
                    WriteLog.LogError($"錯誤的GameState:{AllocatedRoom.Instance.CurGameState}");
                    break;
                case AllocatedRoom.GameState.GameState_WaitingPlayersReady:
                    BattleManager.Instance.InitBattleModelController().Forget();
                    ResetBattle();
                    AllocatedRoom.Instance.SetReady();
                    break;
            }
            await UniTask.Yield();
        }
        /// <summary>
        ///  雙方選完神祉技能 可以開始戰鬥
        /// </summary>
        public void StartSelectDivineSkill() {
            WriteLog.Log($"開始選擇神祉技能");
            //叫出神址選擇介面這裡顯示介面讓玩家選擇
            if (DivineSelectUI.Instance != null)
                DivineSelectUI.Instance.SetActive(true);
        }
        public void StartGame() {
            ResetBattle();
            battleModelController.BattleStart();
        }

        public void SetVCamTargetRot(float _angle) {
            vTargetGroup.transform.localRotation = Quaternion.Euler(0, -90 + _angle, 0);
            WriteLog.Log("vTargetGroup.transform.localRotation=" + vTargetGroup.transform.localRotation);
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
        public async UniTask InitBattleModelController() {
            battleModelController.Init();
            battleModelController.CreateCharacter(AllocatedRoom.Instance.MyPackPlayer, AllocatedRoom.Instance.OpponentPackPlayer);
            await battleModelController.WaitCharacterCreate();
        }

        //重啟戰鬥
        void ResetBattle() {
            GameTime = 0;
            //相關參數在此重設 設定完才去更新UI
            BattleIsEnd = false;
        }


        #region 技能施放
        /// <summary>
        /// 施放直接觸發技能
        /// </summary>
        /// <param name="_skill">技能Data</param>
        public void CastInstantSKill(JsonSkill _skill) {
            //TODO:在這裡做演出 >> 現在直接走後端流程
            /*舊版純前端演出預留邏輯 這裡原本是假設沒接後端的封包 前端自己保存選上技能 前端發生碰撞就施放技能的邏輯
              因為邏輯流程上已經不同 目前已經要直接走後端流程 所以註解掉 保留只是當作參考用 by 瑞榮2024.9.18
            if (bFrontEndTest) {
                //走前端流程 直接演出
            } else {
                //走後端流程送包
            }
            */
            Debug.LogFormat("施放直接觸發技能! 技能ID: {0}", _skill.ID);
        }

        /// <summary>
        /// 設定碰撞觸發技能
        /// </summary>
        /// <param name="_skill">技能Data</param>
        /// <param name="_selected">是否選中技能</param>
        public void SetMeleeSkill(JsonSkill _skill, bool _selected) {
            //會先把技能存在在此 等到真正碰撞後才會施放
            /*舊版純前端演出預留邏輯 這裡原本是假設沒接後端的封包 前端自己保存選上技能 前端發生碰撞就施放技能的邏輯
              因為邏輯流程上已經不同 目前已經要直接走後端流程 所以註解掉 保留只是當作參考用 by 瑞榮2024.9.18
            if (_selected) {
                SelectedMeleeSkill = _skill;
                Debug.LogFormat("設定碰撞觸發技能! 技能ID: {0}", _skill.ID);
            } else {
                SelectedMeleeSkill = null;
                Debug.LogFormat("取消碰撞觸發技能! 技能ID: {0}", _skill.ID);
            }
            */
        }

        /// <summary>
        /// 施放碰撞觸發技能
        /// </summary>
        public void CastMeleeSkill() {
            //TODO:實際發生碰撞請呼叫此方法來進行判定 >> 現在直接走後端流程
            /*舊版純前端演出預留邏輯 這裡原本是假設沒接後端的封包 前端自己保存選上技能 前端發生碰撞就施放技能的邏輯
              因為邏輯流程上已經不同 目前已經要直接走後端流程 所以註解掉 保留只是當作參考用 by 瑞榮2024.9.18
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
            */
        }
        #endregion

        //TODO:
        //體力條計算(自動恢復 使用技能消耗 衝刺消耗)
        //對撞計算(傷害計算>>血量 擊退距離>>人物模型位置改變 撞牆判定>>是否被打到邊界 要額外扣血演出 這個建議不要跟傷害一起演出 而是先有撞牆演出後再扣血)
        //血量計算(傷害量/恢復量傳給UI演出)
        //戰鬥結算
        //buff設定

        //衝刺
        public void GoRun(bool isRun) {
            AllocatedRoom.Instance.SetRun(isRun);
        }

        public void UpdateTimer(float _gameTime) {
            if (BattleSceneUI.Instance == null) return;
            GameTime = _gameTime;
            BattleSceneUI.Instance.SetTimeText(LeftGameTime);
        }

        public void Melee(MELEE_TOCLIENT _melee) {
            battleModelController.Melee(_melee.MyAttack, _melee.OpponentAttack);
        }

        //戰鬥結束
        void BattleEnd() {
            //進入結算環節 避免出bug 如果已經開始結算就不要重複執行
            if (BattleIsEnd) return;
            BattleIsEnd = true;
            battleModelController.BattleEnd();
            Debug.Log("戰鬥結束");
        }
    }
}
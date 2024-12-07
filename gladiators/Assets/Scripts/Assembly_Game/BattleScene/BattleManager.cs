using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cysharp.Threading.Tasks;
using Cinemachine;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

namespace Gladiators.Battle {
    public class BattleManager : MonoBehaviour {
        public static BattleManager Instance;
        [SerializeField] public CinemachineVirtualCamera vCam;
        [SerializeField] public CinemachineTargetGroup vTargetGroup;
        [SerializeField] Transform camPosTarget;
        [SerializeField] Camera MyCam;
        MinMax camFOV = new MinMax(20, 60); // FOV最小與最大值
        MinMax camPosTargetY = new MinMax(2, 7);// camPosTarget的Y座標最小與最大值
        const float MAX_DIST = 40; // 雙方腳色最大距離
        public Transform WorldEffectParent; // 放特效的位置
        public Camera BattleCam => MyCam;

        [SerializeField] int BattleDefaultTime = 60;//設定戰鬥時間

        public float GameTime { get; private set; }//遊戲時間
        public int LeftGameTime { get { return Mathf.RoundToInt((float)BattleDefaultTime - GameTime); } }//遊戲剩餘時間

        //JsonSkill SelectedMeleeSkill;

        [HeaderAttribute("===場景物件控制===")]
        [SerializeField] public BattleController battleModelController;
        [SerializeField] public bool isRightPlayer = false;

        [HeaderAttribute("==============TEST==============")]
        //測試參數區塊
        [Tooltip("重置戰鬥")][SerializeField] bool bResetBattle = false;
        [Tooltip("戰鬥開始拉近鏡頭演出曲線")][SerializeField] AnimationCurve StartBattleZoomCurve;
        [Tooltip("戰鬥開始拉近鏡頭起始距離")][SerializeField] float StartBattleZoomFrom = 80f;
        [Tooltip("戰鬥開始拉近鏡頭演出時間")][SerializeField] float StartBattleZoomDuration = 3.15f;

        [Tooltip("戰鬥結束慢動作演出曲線")][SerializeField] AnimationCurve BattleEndSlowDownCurve;
        [Tooltip("戰鬥結束慢動作停止後等待秒數 用來預置模擬之後可能要做的事情")][SerializeField] float BattleEndSlowDownEndWaitTime = 2f;
        [Tooltip("戰鬥結束慢動作演出時間")][SerializeField] float BattleEndSlowDownDuration = 5f;
        //[Tooltip("前端演示測試 打勾表示只使用純前端邏輯模擬")][SerializeField] bool bFrontEndTest = true;

        public async UniTask Init() {
            Instance = this;
            SetCam();//設定攝影機模式
            SetStage();
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
                    AllocatedRoom.Instance.SetReady();
                    break;
            }
            await UniTask.Yield();
        }

        void SetStage() {
            string sceneName = "Stage1";
            //讀取子場景
            AddressablesLoader.GetAdditiveScene($"{sceneName}/{sceneName}", (scene, handle) => {
                PostProcessingManager.Instance.SetVolumeProfile("VolumeProfile_Stage1");
            });
        }

        /// <summary>
        ///  雙方選完神祉技能 可以開始戰鬥
        /// </summary>
        public void StartSelectDivineSkill() {
            //叫出神址選擇介面這裡顯示介面讓玩家選擇
            if (DivineSelectUI.Instance != null)
                DivineSelectUI.Instance.SetActive(true);
        }

        /// <summary>
        /// 播放遊戲開始動畫
        /// </summary>
        public void StartGameAnimation() {
            DoStartGameAnimation().Forget();
        }

        async UniTask DoStartGameAnimation() {
            float passTime = 0f;
            float curveVal = 0f;
            float oldFOV = vCam.m_Lens.FieldOfView;
            float deltaFOV = StartBattleZoomFrom - oldFOV;
            vCam.m_Lens.FieldOfView = StartBattleZoomFrom;
            //WriteLog.LogErrorFormat("拉近鏡頭參數! 起始值: {0} 差異值: {1} 目前值: {2}", StartBattleZoomFrom, deltaFOV, vCam.m_Lens.FieldOfView);
            while (passTime < StartBattleZoomDuration) {
                passTime += Time.deltaTime;
                curveVal = StartBattleZoomCurve.Evaluate(passTime / StartBattleZoomDuration);
                vCam.m_Lens.FieldOfView = oldFOV + (1 - curveVal) * deltaFOV;
                await UniTask.Yield();
                //WriteLog.LogErrorFormat("拉近鏡頭! 目前距離: {0}", vCam.m_Lens.FieldOfView);
            }
            //避免任何其他因素導致沒正常回歸原本的預設值 這裡再設定一次預設值
            vCam.m_Lens.FieldOfView = oldFOV;
        }


        public void StartGame() {
            battleModelController.BattleStart();
        }

        public void UpdateVCamTargetRot() {
            if (battleModelController.LeftChar == null || battleModelController.RightChar == null) {
                WriteLog.LogError($"character為null leftChar: {battleModelController.LeftChar}   rightChar: {battleModelController.RightChar}");
                return;
            }

            // 獲取兩個角色的世界座標
            Vector3 leftCharPos = battleModelController.LeftChar.transform.position;
            Vector3 rightCharPos = battleModelController.RightChar.transform.position;

            // 計算角色之間的方向
            Vector3 direction = leftCharPos - rightCharPos;

            // 確保方向的 y 分量不影響旋轉
            direction.y = 0;

            // 檢查是否有有效的方向向量
            if (direction.sqrMagnitude < Mathf.Epsilon) {
                return;
            }

            // 計算水平旋轉角度
            float targetYAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // 更新攝影機的 Y 軸旋轉
            vTargetGroup.transform.localRotation = Quaternion.Euler(0, targetYAngle, 0);
        }

        /// <summary>
        /// 根據雙方距離改變攝影機鏡頭
        /// </summary>
        public void SetCamValues(float _charsDist) {
            float ratio = (_charsDist / MAX_DIST);
            // 更改FOV
            float fov = ratio * (camFOV.Y - camFOV.X) + camFOV.X;
            changeFovAsync(fov, 0.5f).Forget();
            // 更改targetGroup的Y值
            float targetGroupY = ratio * (camPosTargetY.Y - camPosTargetY.X) + camPosTargetY.X;
            camPosTarget.localPosition = new Vector3(camPosTarget.localPosition.x, targetGroupY, camPosTarget.localPosition.z);
        }

        async UniTask changeFovAsync(float _targetFov, float _lerpDuration) {
            float startFov = vCam.m_Lens.FieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < _lerpDuration) {
                vCam.m_Lens.FieldOfView = Mathf.Lerp(startFov, _targetFov, elapsedTime / _lerpDuration);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            vCam.m_Lens.FieldOfView = _targetFov;
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
        //戰鬥結算

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
            battleModelController.Melee();
        }

        /// <summary>
        /// 戰鬥結束時呼叫
        /// </summary>
        public void BattleEnd(Action afterKo) {
            //TODO:戰鬥結束演出 >> 最後一下打下去(開始變慢+背景全白+KO動畫) >> 處決演出 >> 結算畫面
            //0.打最後一下的瞬間全白並開始緩速並播放KO動畫
            //1.背景全白 模型跟UI在白色背景前面
            //開始減速
            BattleEndSlowDown().Forget();
            //播放UI的KO動畫
            BattleSceneUI.Instance.PlayKO(afterKo);
        }

        async UniTask BattleEndSlowDown() {
            float passTime = 0f;
            float curveVal = 0f;
            //WriteLog.LogError("開始減速KO演出");
            while (passTime < BattleEndSlowDownDuration) {
                passTime += Time.unscaledDeltaTime;
                curveVal = BattleEndSlowDownCurve.Evaluate(passTime / BattleEndSlowDownDuration);
                Time.timeScale = 1 - curveVal;
                await UniTask.Yield();
                //WriteLog.LogErrorFormat("減速KO演出! 現在速度: {0}", Time.timeScale);
            }
            //WriteLog.LogErrorFormat("結束減速KO演出 等待秒數: {0}", BattleEndSlowDownEndWaitTime);
            Time.timeScale = 1f;
            await UniTask.WaitForSeconds(BattleEndSlowDownEndWaitTime);
            //WriteLog.LogError("結束減速KO演出"); 
        }
    }
}
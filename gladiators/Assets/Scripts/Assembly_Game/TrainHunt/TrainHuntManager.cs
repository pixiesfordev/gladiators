using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using Scoz.Func;
using Cysharp.Threading.Tasks;
using System;
using DamageNumbersPro;
using UnityEngine.Rendering;

namespace Gladiators.TrainHunt {
    public class TrainHuntManager : MonoBehaviour
    {
        [SerializeField] Camera MyCam;
        TrainHuntHero MyHero;
        TrainHuntBoss MyBoss;
        [SerializeField] DamageNumber dmgPrefab;
        [SerializeField] Vector3 dmgPopupOffset; // 跳血座標偏移
        [SerializeField] float dmgNumScal; // 跳血縮放

        [HeaderAttribute("==============怪物位置區==============")]
        [Tooltip("怪物起始位置")][SerializeField] Vector3 BossStartPos;
        [Tooltip("怪物結束位置")][SerializeField] Vector3 BossEndPos;
        [Tooltip("怪物起始位置角度")][SerializeField] Vector3 BossStartAngle;
        [Tooltip("怪物結束位置角度")][SerializeField] Vector3 BossEndAngle;
        [Tooltip("圓周半徑")][SerializeField] float RadiusSet = 1013f;
        [Tooltip("怪物血量")][SerializeField] int BossMaxHP;
        [HeaderAttribute("==============打擊退後參數區==============")]
        [Tooltip("灰色區每禎擊退角度量")][SerializeField] float BarGrayBossBackAngle = 0.03f;
        [Tooltip("黃色區每禎擊退角度量")][SerializeField] float BarYellowBossBackAngle = 0.06f;
        [Tooltip("橘色區每禎擊退角度量")][SerializeField] float BarOrangeBossBackAngle = 0.12f;
        [Tooltip("紅色區每禎擊退角度量")][SerializeField] float BarRedBossBackAngle = 0.18f;
        [Tooltip("擊退持續時間")][SerializeField] float BossHitBackDuration = 0.15f;
        [HeaderAttribute("==============打擊條區==============")]
        [Tooltip("黃色條數值最小值")][SerializeField] float BarSetYellowMinRange = 0.5f;
        [Tooltip("黃色條數值最大值")][SerializeField] float BarSetYellowMaxRange = 0.7f;
        [Tooltip("橘色條數值最小值")][SerializeField] float BarSetOrangeMinRange = 0.3f;
        [Tooltip("橘色條數值最大值")][SerializeField] float BarSetOrangeMaxRange = 0.45f;
        [Tooltip("紅色條數值最小值")][SerializeField] float BarSetRedMinRange = 0.1f;
        [Tooltip("紅色條數值最大值")][SerializeField] float BarSetRedMaxRange = 0.25f;
        [Tooltip("游標移動曲線")][SerializeField] AnimationCurve BarPointerCurve;
        [Tooltip("游標移動最少所需時間(最快速) 至少大於0")][SerializeField] float BarPointerMinDur;
        [Tooltip("游標移動最多所需時間(最慢速)")][SerializeField] float BarPointerMaxDur;
        [Tooltip("數值條重新挑選")][SerializeField] bool BPickBar = false;

        public static TrainHuntManager Instance;

        readonly float GameTime = 30f; //小遊戲時間
        float BarYellowRange = 0f; //打擊條黃色區域值
        float BarOrangeRange = 0f; //打擊條橘色區域值
        float BarRedRange = 0f; //打擊條紅色區域值
        float BarPointerDuration = 1f; //打擊條游標移動時間
        bool stop = false;
        int HitHPRed = 20;
        int HitHPOrange = 15;
        int HitHPYellow = 10;
        int HitHPGray = 5;

        float bossMoveAnglePerFrame = 0f; //Boss每禎移動的角度
        bool bossMoveSpeedUp = false; //Boss是否加快移動速度
        bool bossHitted = false; //Boss是否正在被攻擊

        // Start is called before the first frame update
        void Start()
        {
            MyBoss = TrainHuntSceneUI.Instance.MyBoss;
            MyHero = TrainHuntSceneUI.Instance.MyHero;
            //賦予Boss血量資料(測試用 之後應該由外部呼叫給值)
            SetBossCharInfo(BossMaxHP, 7);
            GameStart();
        }

        public void Init()
        {
            Instance = this;
            setCam();//設定攝影機模式       
        }

        void setCam()
        {
            //因為戰鬥場景的攝影機有分為場景與UI, 要把場景攝影機設定為Base, UI設定為Overlay, 並在BaseCamera中加入Camera stack
            UICam.Instance.SetRendererMode(CameraRenderType.Overlay);
            addCamStack(UICam.Instance.MyCam);
        }

        /// <summary>
        /// 將指定camera加入到MyCam的CameraStack中
        /// </summary>
        void addCamStack(Camera _cam)
        {
            if (_cam == null) return;
            var cameraData = MyCam.GetUniversalAdditionalCameraData();
            if (cameraData == null) return;
            cameraData.cameraStack.Add(_cam);
        }

        /// <summary>
        /// 遊戲開始
        /// </summary>
        public void GameStart()
        {
            //重設Boss相關旗標
            bossHitted = false;
            bossMoveSpeedUp = false;
            //決定打擊條相關參數與賦值給UI
            PickBarValue();
            //Boss開始朝玩家移動
            BossStartMove().Forget();
        }

        void EndGame()
        {
            TrainHuntSceneUI.Instance.EndGame();
        }

        public void SetBossCharInfo(int maxHP, int bossID)
        {
            TrainHuntSceneUI.Instance.SetBossCharInfo(maxHP, bossID);
        }

        /// <summary>
        /// 挑選打擊條長度的值
        /// </summary>
        public void PickBarValue()
        {
            PickBarValueFunc().Forget();
        }

        /// <summary>
        /// 挑選打擊條長度的值
        /// </summary>
        async UniTaskVoid PickBarValueFunc()
        {
            await UniTask.Yield();
            BarYellowRange = UnityEngine.Random.Range(BarSetYellowMinRange, BarSetYellowMaxRange);
            BarOrangeRange = UnityEngine.Random.Range(BarSetOrangeMinRange, BarSetOrangeMaxRange);
            BarRedRange = UnityEngine.Random.Range(BarSetRedMinRange, BarSetRedMaxRange);
            BarPointerDuration = UnityEngine.Random.Range(BarPointerMinDur, BarPointerMaxDur);
            //把值賦給UI做設定
            TrainHuntSceneUI.Instance.SetBarUI(BarYellowRange, BarOrangeRange, BarRedRange);
            TrainHuntSceneUI.Instance.SetBarMoveSpeed(BarPointerDuration);
            Debug.LogFormat("打擊條黃色區域:{0} 橘色區域: {1} 紅色區域:{2} 移動所需時間:{3}",
                BarYellowRange, BarOrangeRange, BarRedRange, BarPointerDuration);
            await UniTask.Yield();
            TrainHuntSceneUI.Instance.BarMove();
        }

        async UniTaskVoid BossStartMove()
        {
            //需要根據移動距離調整Boss的旋轉角度
            //配合PickBarValue延遲兩偵
            await UniTask.Yield();
            await UniTask.Yield();
            float startTime = Time.time;
            float passTime = startTime;
            float deltaTime = 0f;
            //Vector3 curBossPos = BossStartPos;
            Vector3 curAngle = BossStartAngle;
            //Debug.LogFormat("開始移動Boss! 開始時間:{0} 經過時間:{1} 目前位置:{2}", startTime, passTime, curBossPos);
            //TODO:之後有空可以研究改用圓周公式的方式 目前是用內圈根外圈的概念去轉動
            //x-h = rcos();
            //y-k = rsin();
            //TODO:改變移動邏輯 改成算每禎數移動量 這個移動量可以隨時改變
            //計算Boss每禎基礎移動量
            CountBaseBossMovePerFrameAngle();
            while (deltaTime < GameTime)
            {
                passTime += Time.deltaTime;
                deltaTime = passTime - startTime;
                //如果剩下10秒且Boss沒有被攻擊中 就主動讓Boss加速
                if (!bossHitted && GameTime - deltaTime <= 10f)
                {
                    bossMoveSpeedUp = true;
                    CountBaseBossMovePerFrameAngle();
                }
                //更新剩餘時間 & 怪物位置
                //curAngle.z = Mathf.Lerp(BossStartAngle.z, BossEndAngle.z, deltaTime / GameTime);
                curAngle.z += bossMoveAnglePerFrame;
                TrainHuntSceneUI.Instance.SetPointerPos(deltaTime / GameTime);
                MyBoss.transform.parent.localRotation = Quaternion.Euler(curAngle);

                /*
                //棄用的未完成的圓周公式算法 保留以做參考
                //curBossPos.x = Mathf.Lerp(BossStartPos.x, BossEndPos.x, deltaTime / GameTime);
                //curBossPos.y = Mathf.Lerp(BossStartPos.y, BossEndPos.y, deltaTime / GameTime);
                //curBossPos.x = RadiusSet * Mathf.Cos(curAngle.z * Mathf.Deg2Rad);
                //curBossPos.y = RadiusSet * Mathf.Sin(curAngle.z * Mathf.Deg2Rad);
                //Debug.LogWarningFormat("theta: {0} x: {1} y: {2} cos: {3} sin: {4}", 
                //curAngle.z, curBossPos.x, curBossPos.y, 
                //Mathf.Cos(curAngle.z * Mathf.Deg2Rad),
                //Mathf.Sin(curAngle.z * Mathf.Deg2Rad));
                MyBoss.transform.localPosition = curBossPos;
                */
                //TODO:改秒數為基礎 此外加上速度參數化
                await UniTask.Yield();
            }
            await UniTask.Yield();
            EndGame();
        }

        /// <summary>
        /// 算出每禎的Boss基礎移動角度
        /// </summary>
        void CountBaseBossMovePerFrameAngle()
        {
            //基礎用每秒120禎來定 公式:總移動角度 / (每秒禎數 * 遊戲時間)
            float totalFrameCount = 120f * GameTime;
            bossMoveAnglePerFrame = -(BossStartAngle.z - BossEndAngle.z) / totalFrameCount;
            if (bossMoveSpeedUp) bossMoveAnglePerFrame *= 2;
            //Debug.LogErrorFormat("Boss每禎移動角度: {0}", bossMoveAnglePerFrame);
        }

        /// <summary>
        /// 玩家角色發動攻擊
        /// </summary>
        public void PlayerAttack(TrainHuntSceneUI.HitArea area)
        {
            TrainHuntSceneUI.Instance.PlayerAttack(GetHitHP(area), GetWeaponPrefix(area),
                CheckPossAniExist(area), GetBossHittedPrefix(area), GetBossHittedBackPerFrameAngle(area));
        }

        /// <summary>
        /// 取得打擊區域傷害數值
        /// </summary>
        /// <returns>對應區域傷害值</returns>
        int GetHitHP(TrainHuntSceneUI.HitArea area)
        {
            return area switch
            {
                TrainHuntSceneUI.HitArea.Red => HitHPRed,
                TrainHuntSceneUI.HitArea.Orange => HitHPOrange,
                TrainHuntSceneUI.HitArea.Yellow => HitHPYellow,
                _ => HitHPGray,
            };
        }

        string GetWeaponPrefix(TrainHuntSceneUI.HitArea area)
        {
            return area switch
            {
                TrainHuntSceneUI.HitArea.Red => "01",
                TrainHuntSceneUI.HitArea.Orange => "02",
                TrainHuntSceneUI.HitArea.Yellow => "03",
                _ => "04",
            };
        }

        bool CheckPossAniExist(TrainHuntSceneUI.HitArea area)
        {
            return area switch
            {
                TrainHuntSceneUI.HitArea.Red => true,
                TrainHuntSceneUI.HitArea.Orange => true,
                TrainHuntSceneUI.HitArea.Yellow => true,
                _ => false,
            };
        }

        string GetBossHittedPrefix(TrainHuntSceneUI.HitArea area)
        {
            //TODO:等新富補演出後修改名稱
            return area switch
            {
                TrainHuntSceneUI.HitArea.Red => "harm03",
                TrainHuntSceneUI.HitArea.Orange => "harm03",
                TrainHuntSceneUI.HitArea.Yellow => "harm02",
                _ => "harm01",
            };
        }

        float GetBossHittedBackPerFrameAngle(TrainHuntSceneUI.HitArea area)
        {
            return area switch
            {
                TrainHuntSceneUI.HitArea.Red => BarRedBossBackAngle,
                TrainHuntSceneUI.HitArea.Orange => BarOrangeBossBackAngle,
                TrainHuntSceneUI.HitArea.Yellow => BarYellowBossBackAngle,
                _ => BarGrayBossBackAngle,
            };
        }

        public void JumpHitHP(int reduceHP)
        {
            var dmgNum = dmgPrefab.Spawn(MyBoss.transform.position + dmgPopupOffset, reduceHP);
            dmgNum.transform.localScale = Vector3.one * dmgNumScal;
            //改變Layer 不然會看不到
            dmgNum.gameObject.layer = LayerMask.NameToLayer("UI");
            var childs = dmgNum.GetComponentsInChildren<Transform>();
            foreach (var tran in childs)
                tran.gameObject.layer = LayerMask.NameToLayer("UI");
            var sg = dmgNum.GetComponent<SortingGroup>();
            if (sg != null) sg.sortingLayerName = "UI";
        }

        public void BossHittedAni(Vector3 hittedSpinePos, string hittedSpineAniName)
        {
            MyBoss.Hitted(hittedSpinePos, hittedSpineAniName);
        }

        public void BossHittedBack(float bossBackAngle)
        {
            //Boss後退
            bossMoveAnglePerFrame = -bossBackAngle;
            //設置旗標表示Boss正在被攻擊 避免加速運算出bug
            bossHitted = true;
        }

        /// <summary>
        /// Boss從打擊中恢復(會等待設定的秒數)
        /// </summary>
        /// <returns></returns>
        public async UniTask BossRecoveryFromHit()
        {
            await UniTask.WaitForSeconds(BossHitBackDuration);
            //Boss結束擊退演出 要設置回沒被攻擊狀態
            bossHitted = false;
            //Boss恢復原本向前移動
            CountBaseBossMovePerFrameAngle();
            MyBoss.HittedOver();
            MyBoss.Move();
        }

        public void SetPlayer(string heroID)
        {
            //TODO:根據傳入ID設定英雄圖像
            MyHero.SetHero(heroID);
        }

        /// <summary>
        /// 算出Boss已經移動的角度
        /// </summary>
        /// <returns>Boss已移動的角度</returns>
        public float GetBossMovedAngle()
        {
            //Debug.LogErrorFormat("算移動Z角度: 起始角度: {0} 目前角度: {1}", BossStartAngle.z, MyBoss.transform.parent.localRotation.eulerAngles.z);
            return Math.Abs(BossStartAngle.z - MyBoss.transform.parent.localRotation.eulerAngles.z);
        }
    }
}


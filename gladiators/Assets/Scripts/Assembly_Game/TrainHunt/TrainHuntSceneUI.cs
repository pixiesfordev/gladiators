using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using dnlib.DotNet;
using Scoz.Func;
using UnityEngine;
using UnityEngine.UI;
using Gladiators.Battle;
using UnityEditor.EditorTools;
using UnityEngine.AddressableAssets;
using DamageNumbersPro;

namespace Gladiators.TrainHunt {

    public class TrainHuntSceneUI : BaseUI {
        [SerializeField] RectTransform BarGray;
        [SerializeField] RectTransform BarYellow;
        [SerializeField] RectTransform BarOrange;
        [SerializeField] RectTransform BarRed;
        [SerializeField] RectTransform BarPointer;
        [SerializeField] RectTransform BarBG;
        [SerializeField] Transform MonsterPos;
        [SerializeField] Image MonsterIcon;
        [SerializeField] Image MonsterHitted;
        [SerializeField] RectTransform Attack;
        [SerializeField] GameObject GameOverObj;
        [SerializeField] GameObject PlayerTalkBG;
        [SerializeField] BattleGladiatorInfo BossCharInfo;

        [SerializeField] Vector3 dmgPopupOffset; // 跳血座標偏移
        [SerializeField] float dmgNumScal; // 跳血縮放

        [SerializeField] TrainHuntTimeObj TimeObj; //時間顯示物件

        [SerializeField] TrainHuntBG BGObj; //背景物件
        public TrainHuntBoss MyBoss; //Boss物件

        [HeaderAttribute("==============TEST==============")]
        [HeaderAttribute("==============游標區==============")]
        [Tooltip("黃色條數值最小值")] float BarSetYellowMinRange = 0.5f;
        [Tooltip("黃色條數值最大值")] float BarSetYellowMaxRange = 0.7f;
        [Tooltip("紅色條數值最小值")] float BarSetOrangeMinRange = 0.3f;
        [Tooltip("紅色條數值最大值")] float BarSetOrangeMaxRange = 0.45f;
        [Tooltip("紅色條數值最小值")] float BarSetRedMinRange = 0.1f;
        [Tooltip("紅色條數值最大值")] float BarSetRedMaxRange = 0.25f;
        [Tooltip("游標移動曲線")][SerializeField] AnimationCurve BarPointerCurve;
        [Tooltip("游標移動最少所需時間(最快速) 至少大於0")][SerializeField] float BarPointerMinDur;
        [Tooltip("游標移動最多所需時間(最慢速)")][SerializeField] float BarPointerMaxDur;
        [Tooltip("數值條重新挑選")][SerializeField] bool BPickBar = false;

        [HeaderAttribute("==============AddressableAssets==============")]
        [SerializeField] AssetReference TrainHuntSceneAsset;

        [HeaderAttribute("==============怪物位置區==============")]
        [Tooltip("怪物起始位置")][SerializeField] Vector3 MonsterStartPos;
        [Tooltip("怪物結束位置")][SerializeField] Vector3 MonsterEndPos;
        [Tooltip("怪物血量")][SerializeField] int MonsterMaxHP;

        [Tooltip("怪物受擊閃爍演出時間")] [SerializeField] float MonsterHittedTime;

        [Tooltip("攻擊物件移動時間")] [SerializeField] float AttackMoveDuration;

        [Tooltip("重置遊戲")][SerializeField] bool BReset = false;

        public static TrainHuntSceneUI Instance { get; private set; }

        float BarWidth = 0f; //打擊條長度
        float BarYellowRange = 0f; //打擊條黃色區域值
        float BarOrangeRange = 0f; //打擊條橘色區域值
        float BarRedRange = 0f; //打擊條紅色區域值
        float BarPointerDuration = 1f; //打擊條游標移動時間
        Vector2 BarYellowOriginSize;
        Vector2 BarOrangeOriginSize;
        Vector2 BarRedOriginSize;

        float GameTime = 30f; //小遊戲時間

        bool stop = false;

        int HitHPRed = 20;
        int HitHPOrange = 15;
        int HitHPYellow = 10;
        int HitHPGray = 5;

        Vector3 AttackOriginPos;

        Color HideColor = new Color(1f, 1f, 1f, 0f);

        public enum HitArea : short
        {
            Gray = 0,
            Yellow = 1,
            Orange = 2,
            Red = 3,
        }

        //TODO:
        //v1.上下蓋兩條黑色背景長條(因為示意圖看起來是直接用黑色條去遮背景)
        //v2.上方日夜條
        //v3.套其他背景
        //4.搬移角色與Boss的邏輯到TrainHuntManager
        //5.搬移遊戲邏輯到TrainHuntManager

        // Start is called before the first frame update
        void Start()
        {
            //介面相關值先初始化
            BarWidth = BarBG.sizeDelta.x;
            Vector2 oldSize = BarGray.sizeDelta;
            BarGray.sizeDelta = new Vector2(BarWidth, oldSize.y);
            BarYellowOriginSize = BarYellow.sizeDelta;
            BarOrangeOriginSize = BarOrange.sizeDelta;
            BarRedOriginSize = BarRed.sizeDelta;

            //初始化Manager
            Init();

            //TODO:移除掉 目前新富還沒決定好攻擊要怎麼放 先保留
            Attack.gameObject.SetActive(false);

            GameOverObj.SetActive(false);
            BGObj.StartRotate();
        }

        // Update is called once per frame
        void Update()
        {
            if (BReset) {
                BReset = false;
                ResetGame();
            }
        }

        public override void Init() {
            base.Init();
            SpawnSceneManager();
        }

        void SpawnSceneManager() {
            AddressablesLoader.GetPrefabByRef(TrainHuntSceneAsset, (battleManagerPrefab, handle) => {
                GameObject go = Instantiate(battleManagerPrefab);
                var manager = go.GetComponent<TrainHuntManager>();
                manager.Init();
            });
        }

        public override void RefreshText() {

        }

        protected override void SetInstance() {
            Instance = this;
        }

        public void SetBarUI(float yellowRange, float orangeRange, float redRange) {
            BarYellow.sizeDelta = new Vector2(BarWidth * yellowRange, BarYellowOriginSize.y);
            BarOrange.sizeDelta = new Vector2(BarWidth * orangeRange, BarOrangeOriginSize.y);
            BarRed.sizeDelta = new Vector2(BarWidth * redRange, BarRedOriginSize.y);
        }

        public void SetBarMoveSpeed(float duration) {
            BarPointerDuration = duration;
        }

        public void BarMove() {
            BarStartMove().Forget();
        }

        async UniTaskVoid BarStartMove() {
            Vector3 pointerPos = BarPointer.localPosition;
            float pointerXPos = (BarWidth - BarPointer.sizeDelta.x / 2) / 2;
            float passTime = 0f;
            pointerPos.x = pointerXPos;
            Vector3 startPos = pointerPos;
            Vector3 endPos = -pointerPos;
            bool pointerDir = true; //游標方向 true為往下 false為往上
            Debug.LogFormat("游標起始位置: {0} 結束位置: {1}", startPos, endPos);
            stop = false;
            while (!stop) {
                passTime = pointerDir ? passTime + Time.deltaTime : passTime - Time.deltaTime;
                pointerPos.x = Mathf.Lerp(startPos.x, endPos.x, BarPointerCurve.Evaluate(passTime / BarPointerDuration));
                BarPointer.localPosition = pointerPos;
                await UniTask.Yield();
                if (passTime >= BarPointerDuration)
                    pointerDir = false;
                else if (passTime <= 0f)
                    pointerDir = true;
            }
        }

        public void SetBossCharInfo(int maxHP, int heroID) {
            BossCharInfo.Init(maxHP, maxHP, heroID);
        }

        public void SetPointerPos(float rate) {
            TimeObj.SetPointerPos(rate);
        }

        /// <summary>
        /// 點擊發動攻擊
        /// </summary>
        public void ClickAttack() { 
            if (stop)
                return;
            //長條停止演出
            stop = true;
            //計算攻擊數值 & 演出攻擊動畫
            TrainHuntManager.Instance.PlayerAttack(getHitArea());
        }

        /// <summary>
        /// 判斷打擊區域落在紅/黃/白區域
        /// </summary>
        /// <returns>對應區域</returns>
        HitArea getHitArea() {
            float PointerRangeVal = Math.Abs(BarPointer.localPosition.x / (BarWidth / 2));
            Debug.LogFormat("打擊區域值:{0} 黃區:{1} 紅區:{2} 游標所在位置:{3}", PointerRangeVal, BarYellowRange, BarRedRange, 
                BarPointer.localPosition.y);
            if (PointerRangeVal < BarRedRange)
                return HitArea.Red;
            else if (PointerRangeVal < BarOrangeRange)
                return HitArea.Orange;
            else if (PointerRangeVal < BarYellowRange)
                return HitArea.Yellow;
            return HitArea.Gray;
        }

        //TODO:等新富確定攻擊演出後 這裡要修改拔除
        public void PlayerAttack(int reduceHP) {
            PlayAttack(reduceHP).Forget();
        }

        async UniTaskVoid PlayAttack(int reduceHP) {
            Debug.LogFormat("打擊演出! 打擊HP:{0}", reduceHP);
            //1.角色攻擊演出
            await MoveAttack();
            //2.怪物受擊演出(跳血量傷害與Boss受擊動畫)
            ReduceBossHP(reduceHP);
            TrainHuntManager.Instance.JumpHitHP(reduceHP);
            await TrainHuntManager.Instance.BossHitted();
            //3.攻擊演出後重新挑選長條 挑選完後長條重新開始跑
            TrainHuntManager.Instance.PickBarValue();
        }

        public void ReduceBossHP(int reduceHP) {
            BossCharInfo.AddHP(-reduceHP);
        }

        async UniTask MoveAttack() {
            Attack.transform.localPosition = AttackOriginPos;
            Attack.gameObject.SetActive(true);
            Vector3 targetPos = MonsterPos.localPosition;
            targetPos.x = targetPos.x - Attack.sizeDelta.x / 2 - MonsterIcon.GetComponent<RectTransform>().sizeDelta.x / 2;
            Attack.DOLocalMove(targetPos, AttackMoveDuration);
            await UniTask.WaitForSeconds(AttackMoveDuration);
            Attack.gameObject.SetActive(false);
        }

        public void ClickReset() {
            ResetGame();
        }

        public void EndGame() {
            GameOverObj.SetActive(true);
        }

        void ResetGame() {
            GameOverObj.SetActive(false);
            BossCharInfo.Init(MonsterMaxHP, MonsterMaxHP, 7);
            BossCharInfo.ResetHPBarToFull();
            Attack.gameObject.SetActive(false);
            TrainHuntManager.Instance.GameStart();
            BGObj.BGFarStartMove();
        }
    }

}

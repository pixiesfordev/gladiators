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
        [SerializeField] Transform MonsterPos;
        [SerializeField] Image MonsterIcon;
        [SerializeField] Image MonsterHitted;
        [SerializeField] MyText TimeText;
        [SerializeField] RectTransform Attack;
        [SerializeField] GameObject GameOverObj;
        [SerializeField] GameObject PlayerTalkBG;
        [SerializeField] BattleGladiatorInfo MonsterCharInfo;

        [SerializeField] DamageNumber dmgPrefab;
        [SerializeField] Vector3 dmgPopupOffset; // 跳血座標偏移
        [SerializeField] float dmgNumScal; // 跳血縮放

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

        float BarHeight = 0f; //打擊條長度
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

        // Start is called before the first frame update
        void Start()
        {
            Init();
            BarHeight = Screen.height - 200f;
            Vector2 oldSize = BarGray.sizeDelta;
            BarGray.sizeDelta = new Vector2(oldSize.x, BarHeight);
            BarYellowOriginSize = BarYellow.sizeDelta;
            BarOrangeOriginSize = BarOrange.sizeDelta;
            BarRedOriginSize = BarRed.sizeDelta;

            MonsterCharInfo.Init(MonsterMaxHP, MonsterMaxHP, 7);

            AttackOriginPos = Attack.transform.localPosition;
            Attack.gameObject.SetActive(false);

            GameOverObj.SetActive(false);
            PickBarValue().Forget();
            MonsterStartMove().Forget();
            PlayerTalkBGShine().Forget();
        }

        // Update is called once per frame
        void Update()
        {
            if (BPickBar) {
                BPickBar = false;
                PickBarValue().Forget();
            }
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

        /// <summary>
        /// 挑選打擊條長度的值
        /// </summary>
        async UniTaskVoid PickBarValue() {
            stop = true;
            await UniTask.Yield();
            BarYellowRange = UnityEngine.Random.Range(BarSetYellowMinRange, BarSetYellowMaxRange);
            BarOrangeRange = UnityEngine.Random.Range(BarSetOrangeMinRange, BarSetOrangeMaxRange);
            BarRedRange = UnityEngine.Random.Range(BarSetRedMinRange, BarSetRedMaxRange);
            BarYellow.sizeDelta = new Vector2(BarYellowOriginSize.x, BarHeight * BarYellowRange);
            BarOrange.sizeDelta = new Vector2(BarOrangeOriginSize.x, BarHeight * BarOrangeRange);
            BarRed.sizeDelta = new Vector2(BarRedOriginSize.x, BarHeight * BarRedRange);
            BarPointerDuration = UnityEngine.Random.Range(BarPointerMinDur, BarPointerMaxDur);
            Debug.LogFormat("打擊條黃色區域:{0} 橘色區域: {1} 紅色區域:{2} 移動所需時間:{3}", 
                BarYellowRange, BarOrangeRange, BarRedRange, BarPointerDuration);
            await UniTask.Yield();
            stop = false;
            BarStartMove().Forget();
        }

        async UniTaskVoid MonsterStartMove() {
            TimeText.text = GameTime.ToString();
            //配合PickBarValue延遲兩偵
            await UniTask.Yield();
            await UniTask.Yield();
            float startTime = Time.time;
            float passTime = startTime;
            float deltaTime = 0f;
            float remainTime = 0f;
            Vector3 curMonsterPos = MonsterStartPos;
            Debug.LogFormat("開始移動怪物! 開始時間:{0} 經過時間:{1} 目前位置:{2}", startTime, passTime, curMonsterPos);
            while (deltaTime < GameTime) {
                passTime += Time.deltaTime;
                deltaTime = passTime - startTime;
                remainTime = (float)Math.Floor(GameTime - deltaTime);
                if (remainTime < 0f)
                    remainTime = 0f;
                //更新剩餘時間文字
                TimeText.text = remainTime.ToString();
                //更新怪物位置
                curMonsterPos.x = Mathf.Lerp(MonsterStartPos.x, MonsterEndPos.x, deltaTime / GameTime);
                MonsterPos.localPosition = curMonsterPos;
                await UniTask.Yield();
            }
            await UniTask.Yield();
            EndGame();
        }

        async UniTaskVoid BarStartMove() {
            Vector3 pointerPos = BarPointer.localPosition;
            float pointerYPos = (BarHeight - BarPointer.sizeDelta.y / 2) / 2;
            float passTime = 0f;
            pointerPos.y = pointerYPos;
            Vector3 startPos = pointerPos;
            Vector3 endPos = -pointerPos;
            bool pointerDir = true; //游標方向 true為往下 false為往上
            Debug.LogFormat("游標起始位置: {0} 結束位置: {1}", startPos, endPos);
            while (!stop) {
                passTime = pointerDir ? passTime + Time.deltaTime : passTime - Time.deltaTime;
                pointerPos.y = Mathf.Lerp(startPos.y, endPos.y, BarPointerCurve.Evaluate(passTime / BarPointerDuration));
                BarPointer.localPosition = pointerPos;
                await UniTask.Yield();
                if (passTime >= BarPointerDuration)
                    pointerDir = false;
                else if (passTime <= 0f)
                    pointerDir = true;
            }
        }

        /// <summary>
        /// 點擊發動攻擊
        /// </summary>
        public void ClickAttack() { 
            if (stop)
                return;
            //長條停止演出
            stop = true;
            //演出攻擊動畫
            PlayAttack(GetHitHP()).Forget();
        }

        public void ClickReset() {
            ResetGame();
        }

        /// <summary>
        /// 判斷打擊區域落在紅/黃/白區域
        /// </summary>
        /// <returns>對應區域傷害值</returns>
        int GetHitHP() {
            float PointerRangeVal = Math.Abs(BarPointer.localPosition.y / (BarHeight / 2));
            Debug.LogFormat("打擊區域值:{0} 黃區:{1} 紅區:{2} 游標所在位置:{3}", PointerRangeVal, BarYellowRange, BarRedRange, 
                BarPointer.localPosition.y);
            if (PointerRangeVal < BarRedRange)
                return HitHPRed;
            else if (PointerRangeVal < BarOrangeRange)
                return HitHPOrange;
            else if (PointerRangeVal < BarYellowRange)
                return HitHPYellow;
            return HitHPGray;
        }

        async UniTaskVoid PlayAttack(int reduceHP) {
            Debug.LogFormat("打擊演出! 打擊HP:{0}", reduceHP);
            //1.角色攻擊演出
            await MoveAttack();
            //2.怪物受擊演出(跳血量傷害與閃爍兩次)
            ReduceMonsterHP(reduceHP);
            MonsterHitted.DOColor(Color.red, MonsterHittedTime);
            await UniTask.WaitForSeconds(MonsterHittedTime);
            MonsterHitted.DOColor(HideColor, MonsterHittedTime);
            await UniTask.WaitForSeconds(MonsterHittedTime);
            MonsterHitted.DOColor(Color.red, MonsterHittedTime);
            await UniTask.WaitForSeconds(MonsterHittedTime);
            MonsterHitted.DOColor(HideColor, MonsterHittedTime);
            await UniTask.WaitForSeconds(MonsterHittedTime);
            //3.攻擊演出後重新挑選長條 挑選完後長條重新開始跑
            PickBarValue().Forget();
        }

        public void ReduceMonsterHP(int reduceHP) {
            var dmgNum = dmgPrefab.Spawn(MonsterPos.position + dmgPopupOffset, reduceHP);
            dmgNum.transform.localScale = Vector3.one * dmgNumScal;
            MonsterCharInfo.AddHP(-reduceHP);
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

        void EndGame() {
            GameOverObj.SetActive(true);
        }

        void ResetGame() {
            GameOverObj.SetActive(false);
            MonsterCharInfo.Init(MonsterMaxHP, MonsterMaxHP, 7);
            MonsterCharInfo.ResetHPBarToFull();
            Attack.gameObject.SetActive(false);
            PickBarValue().Forget();
            MonsterStartMove().Forget();
        }

        async UniTaskVoid PlayerTalkBGShine() {
            bool show = false;
            while (true) {
                show = !show;
                PlayerTalkBG.SetActive(show);
                await UniTask.WaitForSeconds(1f);
            }
        }
    }

}

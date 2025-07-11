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
using Unity.VisualScripting;

namespace Gladiators.TrainHunt {

    public class TrainHuntSceneUI : BaseUI {
        [SerializeField] RectTransform BarGray;
        [SerializeField] RectTransform BarYellow;
        [SerializeField] RectTransform BarOrange;
        [SerializeField] RectTransform BarRed;
        [SerializeField] RectTransform BarPointer;
        [SerializeField] RectTransform BarBG;
        [SerializeField] RectTransform WeaponsRT;
        [SerializeField] RectTransform PossWeaponsRT;
        [SerializeField] TrainHuntHeroWeapon WeaponPrefab;
        [SerializeField] GameObject GameOverObj;
        [SerializeField] BattleGladiatorInfo BossCharInfo;

        [SerializeField] TrainTimeObj TimeObj; //時間顯示物件

        [SerializeField] TrainHuntBG BGObj; //背景物件
        public TrainHuntBoss MyBoss; //Boss物件
        public TrainHuntHero MyHero; //Hero物件

        [HeaderAttribute("==============TEST==============")]
        [Tooltip("重置遊戲")][SerializeField] bool BReset = false;
        [HeaderAttribute("==============游標區==============")]
        [Tooltip("游標移動曲線")][SerializeField] AnimationCurve BarPointerCurve;
        [HeaderAttribute("==============AddressableAssets==============")]
        [SerializeField] AssetReference TrainHuntSceneAsset;

        [HeaderAttribute("==============攻擊參數區==============")]
        [Tooltip("攻擊物件移動時間")][SerializeField] float AttackMoveDuration;
        [Tooltip("武器擊中演出秒數")][SerializeField] float AttackHitAniDuration;

        public static TrainHuntSceneUI Instance { get; private set; }

        float BarWidth = 0f; //打擊條長度
        float BarYellowRange = 0f; //打擊條黃色區域值
        float BarOrangeRange = 0f; //打擊條橘色區域值
        float BarRedRange = 0f; //打擊條紅色區域值
        float BarPointerDuration = 1f; //打擊條游標移動時間
        Vector2 BarYellowOriginSize;
        Vector2 BarOrangeOriginSize;
        Vector2 BarRedOriginSize;

        bool stop = false;

        List<TrainHuntHeroWeapon> WeaponObjs = new List<TrainHuntHeroWeapon>();

        public enum HitArea : short
        {
            Gray = 0,
            Yellow = 1,
            Orange = 2,
            Red = 3,
        }

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

            //HeroWeapon.Init();

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
            BarYellowRange = yellowRange;
            BarOrangeRange = orangeRange;
            BarRedRange = redRange;
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

        public void SetBossCharInfo(int maxHP, int bossID)
        {
            BossCharInfo.Init(maxHP, maxHP, bossID);
            MyBoss.InitHP(maxHP, maxHP);
        }

        public void HideBossCharInfo()
        {
            BossCharInfo.gameObject.SetActive(false); //先隱藏Boss頭像資訊
        }

        public void SetPointerPos(float rate)
        {
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

        public void PlayerAttack(int reduceHP, string weaponPrefix, bool HavePossAni, string hittedPrefix,
            float bossBackAngle) {
            PlayAttack(reduceHP, weaponPrefix, HavePossAni, hittedPrefix, bossBackAngle).Forget();
        }

        async UniTaskVoid PlayAttack(int reduceHP, string weaponPrefix, bool HavePossAni, string hittedPrefix,
            float bossBackAngle) {
            Debug.LogFormat("打擊演出! 打擊HP:{0}", reduceHP);
            TrainHuntHeroWeaponParameter para = new TrainHuntHeroWeaponParameter();
            //1.角色攻擊演出
            await MoveAttack(weaponPrefix, HavePossAni, para.HeroWeaponOffset, para.BossHittedSpinePos,
                hittedPrefix, bossBackAngle);
            //2.怪物受擊演出(跳血量傷害與Boss受擊動畫)
            ReduceBossHP(reduceHP);
            TrainHuntManager.Instance.JumpHitHP(reduceHP);
            await TrainHuntManager.Instance.BossRecoveryFromHit();
            //3.攻擊演出後重新挑選長條 挑選完後長條重新開始跑
            TrainHuntManager.Instance.PickBarValue();
        }

        public void ReduceBossHP(int reduceHP)
        {
            //BossCharInfo.AddHP(-reduceHP);
            MyBoss.ReduceHP(reduceHP);
        }

        async UniTask MoveAttack(string weaponPrefix, bool HavePossAni, Vector3 heroWeaponOffset, 
            Vector3 hittedSpinePos, string hittedSpineAniName, float bossBackAngle)
        {
            //用程式碼產生Spine
            TrainHuntHeroWeapon obj = Instantiate(WeaponPrefab, Vector3.zero, Quaternion.identity, WeaponsRT);
            obj.Init("Weapon" + WeaponObjs.Count);
            //添加進集合 遊戲重新開始或關閉時清空內部物件釋放資源
            WeaponObjs.Add(obj);
            //TODO:計算隨機插中Boss任意位置(需要測試看要怎麼做才能確保都能沿著Boss身體去做出偏移量)
            //飛行
            //註1.起始角度與BossDirection的角度為反向關係 BossDirection每+一度 起始角度就要-1度
            //註2.最終角度固定為-50度(來自於TrainHuntManager的BossStartAngle 除上0.85是因為有縮放)
            Vector3 startAngle = new Vector3(0f, 0f, 0f - TrainHuntManager.Instance.GetBossMovedAngle());
            Vector3 endAngle = new Vector3(0f, 0f, -50f / 0.85f);
            obj.transform.localRotation = Quaternion.Euler(startAngle);
            obj.Move(weaponPrefix, heroWeaponOffset, AttackMoveDuration);
            obj.transform.DOLocalRotate(endAngle, AttackMoveDuration);
            //Debug.LogErrorFormat("起始角度: {0} 最終角度: {1} 武器起始角度: {2}",
            //    startAngle, endAngle, obj.transform.localRotation);
            await UniTask.WaitForSeconds(AttackMoveDuration - AttackHitAniDuration);
            //插中
            obj.Hit(weaponPrefix);
            //播放Boss被擊中動畫(這個一定要在SetParent的禎之前 不然武器位置會因為Spine的跳動而亂飄)
            TrainHuntManager.Instance.BossHittedAni(hittedSpinePos, hittedSpineAniName);
            TrainHuntManager.Instance.BossHittedBack(bossBackAngle);
            //播放被擊中Spine
            await UniTask.WaitForSeconds(AttackHitAniDuration);
            //判斷是否有插住動畫
            if (HavePossAni)
                obj.Poss(weaponPrefix);
            else
                obj.Hide();
            //改變武器的Parent(插在Boss身上)
            obj.transform.SetParent(PossWeaponsRT, true);
        }

        public void ClickReset() {
            ResetGame();
        }

        public void EndGame() {
            GameOverObj.SetActive(true);
        }

        void ResetGame()
        {
            GameOverObj.SetActive(false);
            BossCharInfo.ResetHPBarToFull();
            MyBoss.ResetHP();
            TrainHuntManager.Instance.GameStart();
            BGObj.BGFarStartMove();
            //回收武器Spine物件
            foreach (var obj in WeaponObjs)
                Destroy(obj.gameObject);
            WeaponObjs.Clear();
        }
    }

}

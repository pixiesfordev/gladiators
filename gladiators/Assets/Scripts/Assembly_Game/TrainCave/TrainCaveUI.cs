using Scoz.Func;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Gladiators.Battle;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Gladiators.TrainCave {
    public class TrainCaveUI : BaseUI {

        [SerializeField] MyText MagicPoint;
        [SerializeField] MyText PhysicsPoint;

        [SerializeField] GameObject GameOverObj;

        [SerializeField] TrainCaveShield PhysicsShield;
        [SerializeField] TrainCaveShield MagicShield;

        [SerializeField] BattleGladiatorInfo CharInfo;
        [SerializeField] SpriteRenderer HeroRenderer;
        [SerializeField] TrainTimeObj TimeObj;

        public Transform AttackObjTrans;

        [Tooltip("受擊閃爍演出時間")][SerializeField] float PlayerHittedTime;

        [HeaderAttribute("==============AddressableAssets==============")]
        [SerializeField] AssetReference TrainCaveSceneAsset;

        public static TrainCaveUI Instance { get; private set; }

        CancellationTokenSource HittedCTK;

        // Start is called before the first frame update
        void Start() {
            Init();
            ResetGame();
        }

        public override void Init() {
            base.Init();
            InitShield();
            SpawnSceneManager();
        }

        void InitShield() {
            PhysicsShield.InitShield(TrainCaveShield.ShieldType.Physics);
            MagicShield.InitShield(TrainCaveShield.ShieldType.Magic);
            PhysicsShield.gameObject.SetActive(false);
            MagicShield.gameObject.SetActive(false);
        }

        void SpawnSceneManager() {
            AddressablesLoader.GetPrefabByRef(TrainCaveSceneAsset, (battleManagerPrefab, handle) => {
                GameObject go = Instantiate(battleManagerPrefab);
                var manager = go.GetComponent<TrainCaveManager>();
                manager.Init();
            });
        }

        public override void RefreshText() {

        }

        protected override void SetInstance() {
            Instance = this;
        }

        //TODO:
        /*
        1.Joystick套件(之後補 先寫PC版滑鼠控制)
         1.方向移動(角色要判斷方向跟著轉向)
         2.方向移動盾牌(盾牌要跟著搖桿動 只判斷方向) >> 孟璋已幫我完成 感謝
        2.攻擊邏輯設計 >> 孟璋已幫我完成攻擊物產生邏輯 感恩
         1.先決定是以Y軸為變量還是X軸 Y軸為變量就是X固定螢幕最左邊或最右邊 X軸為變量 就是固定從最上面
         2.決定起始位置後往角色位置移動
         3.之後有空去計算攻擊圖像的角度(要計算夾角或者網路查相關作法)
         4.攻擊物件速度要可變 攻擊間隔也隨機給一個區間
        3.碰撞判定(測試碰撞功能製作)
         1.撞到盾牌 >> 設計個圖像表示有擋到
         2.撞到角色 >> 角色閃爍
        4.計分
         1.魔法防禦分數
         2.物理防禦分數
        5.盾牌操作方式 >> 用壓住的方式操作
        */
        
        /* 2025.3 TODOLIST:
        1.套入介面圖 >> 目前大部分都已經套完 但攻擊按鈕還沒套 因為建議改成非按鈕形式 否則會誤導玩家
        v2.時間(封裝成通用物件)
        3.血條
        4.鍵盤按鍵功能/滑鼠功能
         1.物理盾牌
         2.魔法盾牌 
        */

        public void SetPhysicsScore(int _score) {
            PhysicsPoint.text = string.Format("PHY: {0}", _score);
        }
        public void SetMagicScore(int _score) {
            MagicPoint.text = string.Format("MAG: {0}", _score);
        }

        public void ShowGameOverObj(bool _active) {
            GameOverObj.SetActive(_active);
        }

        public void OnResetClick() {
            TrainCaveManager.Instance.ResetGame();
            CharInfo.ResetHPBarToFull();
        }

        public void ResetGame() {
            PhysicsPoint.text = string.Format("PHY: {0}", 0);
            MagicPoint.text = string.Format("MAG: {0}", 0);
            GameOverObj.SetActive(false);
            CharInfo.Init(1000, 1000, 7);
        }

        public void OnHit(int _dmg) {
            CharInfo.AddHP(_dmg);
        }

        public bool HeroIsDead() {
            return CharInfo.HeroIsDead();
        }

        public void ShowShield(TrainCaveShield.ShieldType _type, bool show) {
            switch (_type) {
                case TrainCaveShield.ShieldType.Physics:
                    if (!MagicShield.gameObject.activeSelf)
                        PhysicsShield.ShowShield(show);
                    break;
                case TrainCaveShield.ShieldType.Magic:
                    if (!PhysicsShield.gameObject.activeSelf)
                        MagicShield.ShowShield(show);
                    break;
            }
        }

        public void PlayerHittedAni(TrainCaveShield.ShieldType type) {
            PlayerHitted(type).Forget();
        }

        async UniTask PlayerHitted(TrainCaveShield.ShieldType type) {
            CreateHittedCTK();
            float startTime = Time.time;
            float passTime = startTime;
            Color changeColor;
            if (type == TrainCaveShield.ShieldType.Magic) {
                changeColor = Color.blue;
            } else if (type == TrainCaveShield.ShieldType.Physics) {
                changeColor = Color.red;
            } else {
                changeColor = Color.green;
            }
            Color toColor;
            Color fromColor;
            for (int i = 0; i < 4; i++) {
                if (i % 2 == 1) {
                    toColor = changeColor;
                    fromColor = Color.white;
                } else {
                    toColor = Color.white;
                    fromColor = changeColor;
                }
                while (passTime - startTime < PlayerHittedTime) {
                    passTime += Time.deltaTime;
                    HeroRenderer.color = Color.Lerp(fromColor, toColor, (passTime - startTime) / PlayerHittedTime);
                    await UniTask.Yield(HittedCTK.Token);
                }
            }
        }

        void CreateHittedCTK() {
            if (HittedCTK != null) {
                HittedCTK.Cancel();
            }
            HittedCTK = new CancellationTokenSource();
        }

        public void SetPointerPos(float rate) {
            TimeObj.SetPointerPos(rate);
        }

    }
}

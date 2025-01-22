using Scoz.Func;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Gladiators.Battle;

namespace Gladiators.TrainCave {
    public class TrainCaveUI : BaseUI {

        [SerializeField] MyText TimeText;
        [SerializeField] MyText MagicPoint;
        [SerializeField] MyText PhysicsPoint;

        [SerializeField] GameObject GameOverObj;

        [SerializeField] BattleGladiatorInfo CharInfo;

        [HeaderAttribute("==============AddressableAssets==============")]
        [SerializeField] AssetReference TrainCaveSceneAsset;



        public static TrainCaveUI Instance { get; private set; }


        // Start is called before the first frame update
        void Start() {
            Init();
            ResetGame();
        }

        public override void Init() {
            base.Init();
            SpawnSceneManager();
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

        public void SetGameTime(int _time) {
            TimeText.text = _time.ToString();
        }

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

    }
}

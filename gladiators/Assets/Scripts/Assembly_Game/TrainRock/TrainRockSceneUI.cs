using Gladiators.Battle;
using Scoz.Func;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Gladiators.TrainRock {
    public class TrainRockSceneUI : BaseUI {
        [HeaderAttribute("==============AddressableAssets==============")]
        [SerializeField] AssetReference TrainRockSceneAsset;
        [SerializeField] Image[] Img_Coutingdown;
        [SerializeField] BattleGladiatorInfo CharInfo;
        public static TrainRockSceneUI Instance { get; private set; }


        private void Start() {
            Init();
        }
        public override void Init() {
            base.Init();
            CharInfo.Init(1000, 800, 7);
            spawnSceneManager();
        }
        public override void RefreshText() {
        }
        protected override void SetInstance() {
            Instance = this;
        }
        void spawnSceneManager() {
            AddressablesLoader.GetPrefabByRef(TrainRockSceneAsset, (battleManagerPrefab, handle) => {
                GameObject go = Instantiate(battleManagerPrefab);
                var manager = go.GetComponent<TrainRockManager>();
                manager.Init();
            });
        }

        public void ShowCountingdown(bool _show) {
            for (int i = 0; i < Img_Coutingdown.Length; i++) Img_Coutingdown[i].gameObject.SetActive(_show);
        }

        public void SetCountdownImg(int _num) {

            if (_num < 0) return;
            string numStr = _num.ToString();
            int length = numStr.Length;
            if (length > Img_Coutingdown.Length) {
                Debug.LogError("Img_Coutingdown 長度不足，要加一下prefab");
                return;
            }

            AddressablesLoader.GetSpriteAtlas("Number", atlas => {
                for (int i = 0; i < Img_Coutingdown.Length; i++) {
                    if (i < length) {
                        char digitChar = numStr[i];
                        Img_Coutingdown[i].sprite = atlas.GetSprite(digitChar.ToString());
                        Img_Coutingdown[i].gameObject.SetActive(true);
                    } else {
                        Img_Coutingdown[i].gameObject.SetActive(false);
                    }
                }
            });
        }

        /// <summary>
        /// Animator Event呼叫
        /// </summary>
        public void OnStartCountingDownEnd() {
            //TrainRockManager.Instance.StartGame();
        }

        public void AddHP(int _value) {
            CharInfo.AddHP(_value);
        }

        public void AddMaxHP(int _value) {
            CharInfo.AddMaxHP(_value);
        }

    }
}
using Cysharp.Threading.Tasks;
using Gladiators.Main;
using Gladiators.Socket;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
namespace Gladiators.BattleSimulation {
    public class SimulationUI : BaseUI {

        public static SimulationUI Instance { get; private set; }

        // Gladiator
        [SerializeField] Image Img_Gladiator;
        [SerializeField] Image Img_Talent;
        [SerializeField] Text Txt_TalentTitle;
        [SerializeField] Text Txt_TalentInfo;

        List<JsonGladiator> allGladiators;
        int curGladiatorIdx = 0;
        public static JsonGladiator CurGladiator { get; private set; }

        // Skill
        [SerializeField] SkillSelectionUI MySkillSelectionUI;
        [SerializeField] Btn_Skill[] Btn_Skills;
        [SerializeField] Text Txt_SkillTitle;
        [SerializeField] Text Txt_SkillInfo;
        int[] curSkillIDs = new int[5] { 1001, 1002, 1003, 1004, 1005 };
        int curSkillIdx = 0;

        public override void RefreshText() {
        }

        protected override void SetInstance() {
            Instance = this;
        }

        private void Start() {
            Init();
            SetCam();
        }
        void SetCam() {
            UICam.Instance.SetRendererMode(CameraRenderType.Base);
        }

        public override void Init() {
            base.Init();
            // 初始化角鬥士 與 天賦
            allGladiators = GameDictionary.GetJsonDic<JsonGladiator>().Values.ToList();
            setCurGladiator(allGladiators[curGladiatorIdx]);

            // 初始化技能按鈕
            for (int i = 0; i < Btn_Skills.Length; i++) {
                var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(curSkillIDs[i]);
                Btn_Skills[i].SetItem(jsonSkill);
                int idx = i;
                Btn_Skills[idx].Btn.onClick.AddListener(() => {
                    selectSkill(idx);
                });
            }
            selectSkill(0);
            MySkillSelectionUI.Init();
            MySkillSelectionUI.SetActive(false);
        }
        void setCurGladiator(JsonGladiator _json) {
            CurGladiator = _json;
            AddressablesLoader.GetSprite($"Gladiator/{CurGladiator.ID}", (sprite, handle) => {
                Img_Gladiator.sprite = sprite;
                Img_Gladiator.SetNativeSize();
            });
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(CurGladiator.ID);
            if (jsonSkill == null) return;
            AddressablesLoader.GetSpriteAtlas("SpellIcon", atlas => {
                Img_Talent.sprite = atlas.GetSprite(jsonSkill.Ref);
            });
            Txt_TalentTitle.text = jsonSkill.Name;
            Txt_TalentTitle.text = jsonSkill.Description;
        }
        public void NextGladiator() {
            curGladiatorIdx++;
            if (curGladiatorIdx >= allGladiators.Count) curGladiatorIdx = 0;
            setCurGladiator(allGladiators[curGladiatorIdx]);
        }
        public void PreviousGladiator() {
            curGladiatorIdx--;
            if (curGladiatorIdx < 0) curGladiatorIdx = allGladiators.Count - 1;
            setCurGladiator(allGladiators[curGladiatorIdx]);
        }
        void selectSkill(int _idx) {
            curSkillIdx = _idx;
            for (int i = 0; i < Btn_Skills.Length; i++) {
                Btn_Skills[i].SetSelected(_idx == i);
            }
            Txt_SkillTitle.text = Btn_Skills[_idx].MyJsonSkill.Name;
            Txt_SkillInfo.text = Btn_Skills[_idx].MyJsonSkill.Description;
        }
        public void UpdateSkill(JsonSkill _jsonSkill) {
            Btn_Skills[curSkillIdx].SetItem(_jsonSkill);
            curSkillIDs[curSkillIdx] = _jsonSkill.ID;
            Txt_SkillTitle.text = _jsonSkill.Name;
            Txt_SkillInfo.text = _jsonSkill.Description;
        }
        public void OnChangeSkillClick() {
            var jsonSkills = GameDictionary.GetJsonDic<JsonSkill>().Values.ToList();
            jsonSkills = jsonSkills.FindAll(a => a.MySkillType == SkillType.Normal);
            MySkillSelectionUI.ShowUI(jsonSkills, curSkillIDs);
        }
        public void OnVsBot() {
            var gameState = GamePlayer.Instance.GetDBData<DBGameState>();
            string serverName = "Matchgame_TestVer";
            GameConnector.NewConnector(serverName, gameState.MatchgameTestverTcpIp, gameState.MatchgameTestverPort, () => {
                var connector = GameConnector.GetConnector(serverName);
                if (connector != null) {
                    AllocatedRoom.Instance.SetRoom(connector, "testCreater", gameState.MatchgameTestverRoomName, gameState.MatchgameTestverTcpIp, gameState.MatchgameTestverPort);
                    AllocatedRoom.Instance.Auth();
                }
            }, AllocatedRoom.Instance.LeaveRoom).Forget();
        }
        public void OnVsPlayer() {
            AllocatedLobby.Instance.Match("TestMap");
        }
        public void SendSimulationSetting() {
            List<int> skills = new List<int>(curSkillIDs);
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(CurGladiator.ID);
            skills.Add(jsonSkill.ID);
            AllocatedRoom.Instance.GMSetGladiator(CurGladiator.ID, skills.ToArray());
        }


    }
}

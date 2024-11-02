using UnityEngine;
using Gladiators.Socket;
using Gladiators.Main;
using System;
using Gladiators.Battle;

namespace Scoz.Func {
    public partial class TestTool : MonoBehaviour {

        [SerializeField] GameObject ToolGO;

        public static Animator MyAni;

        int[] skillIDs;
        int skillOnID = 0;

        public void UpdateSkills(int[] _skillIDs, int _skillOnID) {
            skillIDs = _skillIDs;
            skillOnID = _skillOnID;
            string log = "手牌: ";
            for (int i = 0; i < _skillIDs.Length; i++) {
                log += _skillIDs[i].ToString();
                if (i != _skillIDs.Length - 1) log += ", ";
            }
            log += "  啟用中的技能ID: " + _skillOnID;
            WriteLog.LogError(log);
        }
        void clickSkill(int _idx) {
            if (_idx < 0 || _idx > 2) return;
            WriteLog.LogError("點技能" + skillIDs[_idx]);
            AllocatedRoom.Instance.ActiveSkill(skillIDs[_idx], (skillOnID != skillIDs[_idx]));
            if (skillOnID != skillIDs[_idx]) {
                WriteLog.LogError("啟用技能" + skillIDs[_idx]);
            } else {
                WriteLog.LogError("關閉技能" + skillIDs[_idx]);
            }
        }

        int key = 0;
        // Update is called once per frame
        void KeyDetector() {


            if (Input.GetKeyDown(KeyCode.Q)) {
                Action connFunc = null;
                PopupUI.ShowLoading(JsonString.GetUIString("Loading"));
                connFunc = () => GameConnector.Instance.ConnectToMatchgameTestVer(() => {
                    PopupUI.HideLoading();
                }, () => {
                    WriteLog.LogError("連線遊戲房失敗");
                }, () => {
                    if (AllocatedRoom.Instance.CurGameState == AllocatedRoom.GameState.GameState_Fighting) {
                        WriteLog.LogError("需要斷線重連");
                        connFunc();
                    }
                });
                connFunc();

            } else if (Input.GetKeyDown(KeyCode.W)) {
                clickSkill(0);
            } else if (Input.GetKeyDown(KeyCode.E)) {
                clickSkill(1);
            } else if (Input.GetKeyDown(KeyCode.R)) {
                clickSkill(2);
            } else if (Input.GetKeyDown(KeyCode.P)) {
            } else if (Input.GetKeyDown(KeyCode.O)) {
            } else if (Input.GetKeyDown(KeyCode.I)) {
            } else if (Input.GetKeyDown(KeyCode.Z)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Damage_Small, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.X)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Damage_Bleed, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.C)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Damage_Poison, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.V)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Damage_Burning, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.B)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Recovery_HP, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.N)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Recovery_Physical, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.L)) {
                //var data = GameDictionary.GetJsonData<JsonGladiator>(1);
                //WriteLog.WriteObj(data);
                //var data2 = GameDictionary.GetJsonData<JsonSkill>(1);
                //WriteLog.WriteObj(data2);
                //var data3 = GameDictionary.GetJsonData<JsonSkillEffect>("1");
                //WriteLog.WriteObj(data3);
            }
        }


        public void OnModifyHP(int _value) {
        }
        public void OnModifySanP(int _value) {
        }
        public void ClearLocoData() {
            PlayerPrefs.DeleteAll();
        }

    }
}

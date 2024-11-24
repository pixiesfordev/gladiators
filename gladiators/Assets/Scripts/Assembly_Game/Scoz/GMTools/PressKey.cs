using UnityEngine;
using Gladiators.Socket;
using Gladiators.Main;
using System;
using Gladiators.Battle;

namespace Scoz.Func {
    public partial class TestTool : MonoBehaviour {

        [SerializeField] GameObject ToolGO;

        public static Animator MyAni;

        // Update is called once per frame
        void KeyDetector() {


            if (Input.GetKeyDown(KeyCode.Q)) {


            } else if (Input.GetKeyDown(KeyCode.W)) {
            } else if (Input.GetKeyDown(KeyCode.E)) {
            } else if (Input.GetKeyDown(KeyCode.R)) {
            } else if (Input.GetKeyDown(KeyCode.P)) {
            } else if (Input.GetKeyDown(KeyCode.O)) {
            } else if (Input.GetKeyDown(KeyCode.I)) {
            } else if (Input.GetKeyDown(KeyCode.Z)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Dmg_Small, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.X)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Dmg_Bleeding, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.C)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Dmg_Poison, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.V)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Dmg_Burning, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.B)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Restore_Hp, UnityEngine.Random.Range(1, 100));
            } else if (Input.GetKeyDown(KeyCode.N)) {
                BattleManager.Instance.battleModelController.leftChar.ShowBattleNumber(NumType.Restore_Vigor, UnityEngine.Random.Range(1, 100));
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

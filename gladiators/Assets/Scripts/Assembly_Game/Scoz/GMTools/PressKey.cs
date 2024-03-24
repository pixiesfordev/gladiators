using UnityEngine;
using Service.Realms;
using System.Linq;
using Gladiators.Socket;
using Gladiators.Main;

namespace Scoz.Func {
    public partial class TestTool : MonoBehaviour {

        [SerializeField] GameObject ToolGO;

        public static Animator MyAni;
        int key = 0;
        // Update is called once per frame
        void KeyDetector() {


            if (Input.GetKeyDown(KeyCode.Q)) {
                var data = GameDictionary.GetJsonData<GladiatorJsonData>(1);
                WriteLog.WriteObj(data);
                var data2 = GameDictionary.GetJsonData<SkillJsonData>(1);
                WriteLog.WriteObj(data2);
                var data3 = GameDictionary.GetJsonData<SkillEffectJsonData>("1");
                WriteLog.WriteObj(data3);

            } else if (Input.GetKeyDown(KeyCode.W)) {

            } else if (Input.GetKeyDown(KeyCode.E)) {
            } else if (Input.GetKeyDown(KeyCode.R)) {

            } else if (Input.GetKeyDown(KeyCode.P)) {
            } else if (Input.GetKeyDown(KeyCode.O)) {

            } else if (Input.GetKeyDown(KeyCode.I)) {

            } else if (Input.GetKeyDown(KeyCode.L)) {
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

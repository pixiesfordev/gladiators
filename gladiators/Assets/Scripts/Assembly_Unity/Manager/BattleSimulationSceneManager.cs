using UnityEngine;

namespace Gladiators.Main {
    public class BattleSimulationSceneManager : MonoBehaviour {
        void Start() {
            BaseManager.CreateNewInstance();
        }
    }
}
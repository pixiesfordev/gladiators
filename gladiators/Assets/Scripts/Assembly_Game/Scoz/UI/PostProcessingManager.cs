using Cysharp.Threading.Tasks;
using Gladiators.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Scoz.Func {

    public class PostProcessingManager : MonoBehaviour {

        public static PostProcessingManager Instance;
        private Volume volume;

        public void Init() {
            Instance = this;
            volume = GetComponent<Volume>();
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            DontDestroyOnLoad(gameObject);

            //初始化時先執行一次
            OnLevelFinishedLoading(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        void OnLevelFinishedLoading(Scene _scene, LoadSceneMode _mode) {
        }

        public void SetVolumeProfile(string _path) {
            AddressablesLoader.GetVolumeProflie(_path, profile => {
                if (profile == null) {
                    WriteLog.LogError("SetVolumeProfile 錯誤");
                    return;
                }
                volume.profile = profile;
            });
        }
    }
}
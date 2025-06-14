using System.Collections;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using System;
using UnityEditor.AddressableAssets;
using Scoz.Func;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines.Editor;

namespace Scoz.Editor {
    public class BuildCommand {

        private static string[] BuildScenes = { "Assets/Scenes/MainScene.unity" };
        private static string ANDROID_MANIFEST_PATH = "Assets/Plugins/Android/AndroidManifest.xml";
        static object owner = new System.Object();

        private static void BuildAssetBundleAsync(EnvVersion _version, Action callback) {

            SwitchVersion.RunSwitchVersion(_version, "");
            Debug.Log("Start Build Bundle EnvVersion: " + _version);
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult buildResult);
            if (!string.IsNullOrEmpty(buildResult.Error)) {
                Debug.LogError("Build Bundle Error.");
                callback?.Invoke();
            }
            callback?.Invoke();
        }

        private static void UpdateAssetBundleAsync(EnvVersion _version, Action callback) {
            SwitchVersion.RunSwitchVersion(_version, "");
            Debug.Log("Start Build Bundle");
            var path = ContentUpdateScript.GetContentStateDataPath(false);
            if (!string.IsNullOrEmpty(path)) {
                Debug.Log("Update Bundle at path : " + path);
                ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, path);
                callback?.Invoke();
            } else {
                Debug.LogError("ContentUpdateScript path is null.");
                callback?.Invoke();
            }

        }

        public static void SetPlayerSettingsVersion() {
            string[] args = System.Environment.GetCommandLineArgs();
            string version = "";
            string versionCode = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log("ARG " + i + ": " + args[i]);
                switch (args[i]) {
                    case "-buildVersion":
                        version = args[i + 1];
                        break;
                    case "-buildVersionCode":
                        versionCode = args[i + 1];
                        break;
                }
            }
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = int.Parse(versionCode);
            Close();
        }

        public static void BuildBundleWithArg() {
            EditorSceneManager.OpenScene($"Assets/Scenes/" + MyScene.StartScene.ToString() + ".unity");
            string[] args = System.Environment.GetCommandLineArgs();
            EnvVersion envVersion = EnvVersion.Dev;
            string version = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log("ARG " + i + ": " + args[i]);
                switch (args[i]) {
                    case "-enviorment":
                        if (!MyEnum.TryParseEnum(args[i + 1], out envVersion)) {
                            Debug.LogError("傳入的版本參數錯誤");
                            Close();
                            return;
                        }
                        break;
                    case "-buildVersion":
                        version = args[i + 1];
                        break;
                }
            }
            if (string.IsNullOrEmpty(version)) {
                Debug.LogError("version argument is not set.");
                Close();
                return;
            }
            PlayerSettings.bundleVersion = version;
            BuildAssetBundleAsync(envVersion, Close);
        }

        public static void UpdateBundleWithArg() {
            EditorSceneManager.OpenScene($"Assets/Scenes/" + MyScene.StartScene.ToString() + ".unity");
            string[] args = System.Environment.GetCommandLineArgs();
            EnvVersion envVersion = EnvVersion.Dev;
            string version = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log("ARG " + i + ": " + args[i]);
                switch (args[i]) {
                    case "-enviorment":
                        if (!MyEnum.TryParseEnum(args[i + 1], out envVersion)) {
                            Debug.LogError("傳入的版本參數錯誤");
                            Close();
                            return;
                        }
                        break;
                    case "-buildVersion":
                        version = args[i + 1];
                        break;
                }
            }
            if (string.IsNullOrEmpty(version)) {
                Debug.LogError("version argument is not set.");
                Close();
                return;
            }
            PlayerSettings.bundleVersion = version;
            UpdateAssetBundleAsync(envVersion, Close);
        }
        public static void BuildAPK() {
            string[] args = System.Environment.GetCommandLineArgs();
            EnvVersion envVersion = EnvVersion.Dev;
            string version = "";
            string versionCode = "";
            string keyaliasPass = "";
            string keystorePass = "";
            string outputFileName = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log("ARG " + i + ": " + args[i]);
                switch (args[i]) {
                    case "-enviorment":
                        if (!MyEnum.TryParseEnum(args[i + 1], out envVersion)) {
                            Debug.LogError("傳入的版本參數錯誤");
                            Close();
                            return;
                        }
                        break;
                    case "-buildVersion":
                        version = args[i + 1];
                        break;
                    case "-buildVersionCode":
                        versionCode = args[i + 1];
                        break;
                    case "-keyaliasPass":
                        keyaliasPass = args[i + 1];
                        break;
                    case "-keystorePass":
                        keystorePass = args[i + 1];
                        break;
                    case "-outputFileName":
                        outputFileName = args[i + 1];
                        break;
                }
            }
            Debug.LogFormat("輸出APK位置: {0}", outputFileName);
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = int.Parse(versionCode);
            PlayerSettings.keyaliasPass = keyaliasPass;
            PlayerSettings.keystorePass = keystorePass;
            EditorCoroutineUtility.StartCoroutine(BuildAPKAsync(envVersion, outputFileName, Close), owner);
        }

        public static void BuildAAB() {
            string[] args = System.Environment.GetCommandLineArgs();
            EnvVersion envVersion = EnvVersion.Dev;
            string version = "";
            string versionCode = "";
            string keyaliasPass = "";
            string keystorePass = "";
            string outputFileName = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log("ARG " + i + ": " + args[i]);
                switch (args[i]) {
                    case "-enviorment":
                        if (!MyEnum.TryParseEnum(args[i + 1], out envVersion)) {
                            Debug.LogError("傳入的版本參數錯誤");
                            Close();
                            return;
                        }
                        break;
                    case "-buildVersion":
                        version = args[i + 1];
                        break;
                    case "-buildVersionCode":
                        versionCode = args[i + 1];
                        break;
                    case "-keyaliasPass":
                        keyaliasPass = args[i + 1];
                        break;
                    case "-keystorePass":
                        keystorePass = args[i + 1];
                        break;
                    case "-outputFileName":
                        outputFileName = args[i + 1];
                        break;
                }
            }
            Debug.LogFormat("輸出AAB位置: {0}", outputFileName);
            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = int.Parse(versionCode);
            PlayerSettings.keyaliasPass = keyaliasPass;
            PlayerSettings.keystorePass = keystorePass;
            EditorCoroutineUtility.StartCoroutine(BuildAabAsync(envVersion, outputFileName, Close), owner);
        }

        private static void Close() {
            EditorApplication.Exit(0);
        }


        private static IEnumerator BuildAPKAsync(EnvVersion envVersion, string outputFileName, Action callback) {
            //設定
            EditorUserBuildSettings.development = true;
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            PlayerSettings.Android.useAPKExpansionFiles = false;
            ModifyAndroidManifest.ModifyDebuggable(ANDROID_MANIFEST_PATH, true);
            //切換環境
            SwitchVersion.RunSwitchVersion(envVersion, "");
            BuildPipeline.BuildPlayer(BuildScenes, outputFileName, BuildTarget.Android, BuildOptions.None);
            callback?.Invoke();
            yield return null;
        }

        private static IEnumerator BuildAabAsync(EnvVersion _envVersion, string outputFileName, Action callback) {
            //設定
            EditorUserBuildSettings.development = false;
            EditorUserBuildSettings.buildAppBundle = true;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            PlayerSettings.Android.useAPKExpansionFiles = true;
            ModifyAndroidManifest.ModifyDebuggable(ANDROID_MANIFEST_PATH, false);


            SwitchVersion.RunSwitchVersion(_envVersion, "");
            BuildPipeline.BuildPlayer(BuildScenes, outputFileName, BuildTarget.Android, BuildOptions.None);
            callback?.Invoke();
            yield return null;


        }

        public static void BuildXcode() {
            string[] args = System.Environment.GetCommandLineArgs();
            EnvVersion envVersion = EnvVersion.Dev;
            string version = "";
            string versionCode = "";
            string outputFileName = "";
            for (int i = 0; i < args.Length; i++) {
                Debug.Log("ARG " + i + ": " + args[i]);
                switch (args[i]) {
                    case "-enviorment":
                        if (!MyEnum.TryParseEnum(args[i + 1], out envVersion)) {
                            Debug.LogError("傳入的版本參數錯誤");
                            Close();
                            return;
                        }
                        break;
                    case "-buildVersion":
                        version = args[i + 1];
                        break;
                    case "-buildVersionCode":
                        versionCode = args[i + 1];
                        break;
                    case "-outputFileName":
                        outputFileName = args[i + 1];
                        break;
                }
            }
            Debug.LogFormat("輸出Xcode Project位置: {0}", outputFileName);
            PlayerSettings.bundleVersion = version;
            PlayerSettings.iOS.buildNumber = versionCode;

            try {
                EditorCoroutineUtility.StartCoroutine(BuildXcodeAsync(envVersion, outputFileName, Close), owner);
            } catch (Exception _e) {

                Debug.LogError("BuildXcode發生錯誤: " + _e);
            }
        }
        private static IEnumerator BuildXcodeAsync(EnvVersion evnVersion, string outputFileName, Action callback) {
            SwitchVersion.RunSwitchVersion(evnVersion, "");
            Debug.Log("Start BuildXcodeAsync");
            BuildPipeline.BuildPlayer(BuildScenes, outputFileName, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
            callback?.Invoke();
            yield return null;
        }
    }
}
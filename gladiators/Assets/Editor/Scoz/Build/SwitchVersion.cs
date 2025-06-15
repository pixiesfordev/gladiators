using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using Scoz.Func;

namespace Scoz.Editor {
    public class SwitchVersion {

        // Dev版本的Defines
        static readonly List<string> DevDefines = new List<string> { "DEBUG_LOG" };
        static object owner = new System.Object();

        //static Dictionary<EnvVersion, int> FACEBOOK_APP_INDEX_DIC = new Dictionary<EnvVersion, int>() {
        //    { EnvVersion.Dev, 0},
        //    { EnvVersion.Test, 1},
        //    { EnvVersion.Release, 2},
        //};


        [MenuItem("Scoz/SwitchVersion/1. Dev")]
        public static void SwitchToDev() {
            bool isYes = EditorUtility.DisplayDialog("切換環境版本", "切換版本至 " + EnvVersion.Dev.ToString(), "切!", "不好😔");
            if (isYes) {
                RunSwitchVersion(EnvVersion.Dev, "");
            }
        }
        [MenuItem("Scoz/SwitchVersion/2. Test")]
        public static void SwitchToTest() {
            bool isYes = EditorUtility.DisplayDialog("切換版本", "切換版本至 " + EnvVersion.Test.ToString(), "切!", "不好😔");
            if (isYes) {
                RunSwitchVersion(EnvVersion.Test, "");
            }
        }
        [MenuItem("Scoz/SwitchVersion/3. Release")]
        public static void SwitchToRelease() {
            bool isYes = EditorUtility.DisplayDialog("切換版本", "切換版本至 " + EnvVersion.Release.ToString(), "切!", "不好😔");
            if (isYes) {
                isYes = EditorUtility.DisplayDialog("最後警告", "真的要切到版本!!!!!!!!!!!!!!!!! " + EnvVersion.Release.ToString(), "不要怕!", "不好😱");
                if (isYes) {
                    RunSwitchVersion(EnvVersion.Release, "");
                }
            }
        }

        public static void RunSwitchVersion(EnvVersion _version, string _process) {

            //修改Addressable設定
            var success = SetRemoteLoadPath(_version);
            if (!success) {
                WriteLog.LogError("SetRemoteLoadPath錯誤");
                return;
            }

            //修改package名稱
            if (Setting_Config.PACKAGE_NAMES.TryGetValue(_version, out string packageName)) {
                // 設定Package Name
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, packageName);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, packageName);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, packageName);
                // 設定Product Name
                PlayerSettings.productName = Setting_Config.PACKAGE_NAME;
                // 設定公司名稱
                PlayerSettings.companyName = Setting_Config.COMPANY_NAME;
            }

            //重新讀取更新後的google-services.json避免沒有刷新問題
            AssetDatabase.Refresh();

            ChangeDefineAsync(_version, _process);
        }

        public static bool SetRemoteLoadPathBeforeBuildBundle() {
#if Dev
            var success = SwitchVersion.SetRemoteLoadPath(EnvVersion.Dev);
            if (!success) return false;
#elif Test
            var success = SwitchVersion.SetRemoteLoadPath(EnvVersion.Test);
            if (!success) return false;
#elif Release
            var success = SwitchVersion.SetRemoteLoadPath(EnvVersion.Release);
            if (!success) return false;
#else
            Debug.LogError("目前尚未選擇版本，請先執行SwitchVersion");
            return false;
#endif
            return true;
        }

        public static bool SetRemoteLoadPath(EnvVersion _env) {

            //修改Addressable設定
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            settings.ContentStateBuildPath = string.Format(Setting_Config.ADDRESABLE_BIN_PATH, currentBuildTarget, _env, Scoz.Func.VersionSetting.AppLargeVersion);
            if (Setting_Config.ADDRESABALE_PROFILES.TryGetValue(_env, out string profileName)) {
                string prfileID = settings.profileSettings.GetProfileId(profileName);
                if (!string.IsNullOrEmpty(prfileID)) {
                    //WriteLog.Log("Profile ID : " + prfileID);
                    settings.activeProfileId = prfileID;//設定目前使用的Addressable Profile
                    //依據版本設定遠端載入的Bundle包位置
                    string remoteLoadPath = @$"https://storage.googleapis.com/{Setting_Config.GCS_BUNDLE_PATHS[_env]}/{Scoz.Func.VersionSetting.AppLargeVersion}/[BuildTarget]";

                    settings.profileSettings.SetValue(prfileID, "RemoteLoadPath", remoteLoadPath);
                } else {
                    WriteLog.LogError("Addressable prfile setting error.");
                    return false;
                }
            } else {
                WriteLog.LogError("Addressable prfile setting error.");
                return false;
            }
            return true;
        }

        static void ChangeDefineAsync(EnvVersion _envVersion, string _process) {
            try {
                bool anyChange = false; // 是否有任何 Define 變更
                BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[4] {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WebGL
                };

                for (int j = 0; j < buildTargetGroups.Length; j++) {
                    BuildTargetGroup group = buildTargetGroups[j];
                    string originDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
                    List<string> defines = originDefine.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    // 根據版本處理 Defines
                    if (_envVersion != EnvVersion.Dev) {
                        defines.RemoveAll(a => DevDefines.Contains(a));
                    } else {
                        defines.AddRange(DevDefines);
                        defines = defines.Distinct().ToList();
                    }

                    bool anyVersionDefine = false;
                    // 處理版本 Defines
                    for (int i = 0; i < defines.Count; i++) {
                        if (MyEnum.IsTypeOfEnum<EnvVersion>(defines[i])) {
                            defines[i] = _envVersion.ToString(); // 替換成目標版本
                            anyVersionDefine = true;
                        }
                    }
                    string newDefine = string.Join(";", defines);
                    if (!anyVersionDefine) {
                        newDefine = newDefine + ";" + _envVersion.ToString();
                    }

                    // 如果現有的 define 與新的不相同就更新
                    if (!originDefine.Equals(newDefine, StringComparison.Ordinal)) {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, newDefine);
                        anyChange = true;
                    }
                }
                SceneView.RepaintAll();
                if (!anyChange) {
                    ProcessContinuation.RunProcess(_process);
                }
            } catch (Exception _ex) {
                WriteLog.LogError(_ex);
            }
        }

    }
}

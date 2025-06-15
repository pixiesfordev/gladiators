using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using Scoz.Func;
using UnityEngine;

namespace Scoz.Editor {
    public class UploadBundle {
        const string DIALOG_MESSAGE = "上傳資源包到Google Storage，請確認以下：\n\n1. 已安裝GoogleCloud工具，並完成初始化\n2. 已加入環境變數\n3. 已登入有權限的帳號\n\n環境: {0}\nBundle包版號: {1}\n";
        [MenuItem("Scoz/UploadBundle/1. Dev")]
        public static void UploadBundleToDev() {
            bool isYes = EditorUtility.DisplayDialog("上傳資源包", string.Format(DIALOG_MESSAGE, "Dev", VersionSetting.AppLargeVersion), "好!", "住手!😔");
            if (isYes)
                UploadGoogleCloud(EnvVersion.Dev);
        }
        [MenuItem("Scoz/UploadBundle/2. Test")]
        public static void UploadBundleToTest() {
            bool isYes = EditorUtility.DisplayDialog("上傳資源包", string.Format(DIALOG_MESSAGE, "Test", VersionSetting.AppLargeVersion), "好!", "住手!😔");
            if (isYes)
                UploadGoogleCloud(EnvVersion.Test);
        }
        [MenuItem("Scoz/UploadBundle/3. Release")]
        public static void UploadBundleToRelease() {
            bool isYes = EditorUtility.DisplayDialog("上傳資源包", string.Format(DIALOG_MESSAGE, "Release", VersionSetting.AppLargeVersion), "好!", "住手!😔");
            if (isYes) {
                isYes = EditorUtility.DisplayDialog("這是Release版本, 我勸你多想想!", string.Format(DIALOG_MESSAGE, "Release", VersionSetting.AppLargeVersion), "怕三小!", "住手!😔");
                if (isYes) UploadGoogleCloud(EnvVersion.Release);
            }

        }

        public static void UploadGoogleCloud(EnvVersion _envVersion) {
            string googleProjectID = "";
            if (Setting_Config.GOOGLE_PROJECTS.TryGetValue(_envVersion, out string id)) {
                googleProjectID = id;
            } else {
                WriteLog.LogError("找不到GPC專案ID：" + _envVersion + " version.");
                return;
            }

            string storagePath = "";
            if (Setting_Config.GCS_BUNDLE_PATHS.TryGetValue(_envVersion, out string path)) {
                storagePath = path;
            } else {
                WriteLog.LogError("找不到GPC專案ID：" + _envVersion + " version.");
                return;
            }

            var logStrs = new List<string>();

            // start the child process
            Process process = new Process();

            WriteLog.LogFormat("專案ID: {0}  StoragePath: {1}  BundleVersion: {2}",
                googleProjectID, storagePath, VersionSetting.AppLargeVersion);

#if UNITY_EDITOR_WIN
            string fileName = "UploadBundle.bat";
            // 以系統管理員(runas)執行
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Verb = "runas";

            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.FileName = fileName;
            WriteLog.Log($"googleProjectID: {googleProjectID} storagePath: {storagePath} VersionSetting.AppLargeVersion: {VersionSetting.AppLargeVersion}");
            process.StartInfo.Arguments = string.Format("{0} {1} {2}", googleProjectID, storagePath, VersionSetting.AppLargeVersion);
            process.StartInfo.WorkingDirectory = "./";
#elif UNITY_EDITOR_OSX
    string fileName = "UploadBundle.sh";
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.CreateNoWindow = true;
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardError = true;
    process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
    process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
    process.StartInfo.FileName = fileName;
    process.StartInfo.Arguments = Application.dataPath + string.Format("/../{0} {1} {2} {3}",
        fileName, googleProjectID, storagePath, VersionSetting.AppLargeVersion);
    process.StartInfo.WorkingDirectory = "./";
#endif

            int exitCode = -1;
            WriteLog.Log("命令檔位置：" + process.StartInfo.WorkingDirectory + process.StartInfo.FileName);

            try {
                // 只在 Android/OSX 能重導輸出時有用，Windows runas 模式下無法使用
#if !UNITY_EDITOR_WIN
        process.OutputDataReceived += (sender, args) => {
            if (!string.IsNullOrEmpty(args.Data))
                logStrs.Add(args.Data);
        };
        process.ErrorDataReceived += (sender, args) => {
            if (!string.IsNullOrEmpty(args.Data))
                logStrs.Add(args.Data);
        };
#endif

                process.Start();

#if !UNITY_EDITOR_WIN
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
#endif

                process.WaitForExit(); // 確保 Unity 等待外部命令檔執行完成

                // 顯示到 Unity 的 Console
                foreach (var str in logStrs) {
                    WriteLog.Log(str);
                }

            } catch (Exception e) {
                WriteLog.LogError("發生錯誤：" + e.ToString());
            } finally {
                exitCode = process.ExitCode;
                process.Dispose();
                process = null;
            }

            if (exitCode != 0) {
                WriteLog.LogError("執行失敗 ExitCode：" + exitCode);
                EditorUtility.DisplayDialog("執行" + fileName, "執行中斷，請查看Console Log", "嗚嗚嗚", "");
            } else {
                WriteLog.Log("執行成功 ExitCode：" + exitCode);
                EditorUtility.DisplayDialog("執行" + fileName, "執行成功，請查看Console Log確保無任何錯誤", "確認", "");
            }
        }

    }
}
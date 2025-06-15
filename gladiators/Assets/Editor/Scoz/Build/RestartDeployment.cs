using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scoz.Func;

namespace Scoz.Editor {
    public class Restart_Deployment {
        const string DIALOG_MESSAGE = "即將對 Deployment 執行 rolling restart\n\n"
            + "GameName: {0}\n"
            + "devtest版: {1}\n\n"
            + "請確認是否繼續？";

        [MenuItem("Scoz/RestartDeployment/1. Devtest")]
        public static void RestartCardSwapDevTest() {
            if (!EditorUtility.DisplayDialog(
                    "重啟 Deployment",
                    string.Format(DIALOG_MESSAGE, Setting_Config.PROJECT_DEPLOYMENT_NAME, true),
                    "確定", "取消"))
                return;

            RunBat(Setting_Config.PROJECT_DEPLOYMENT_NAME, false);
        }
        [MenuItem("Scoz/RestartDeployment/2. Dev")]
        public static void RestartCardSwapDev() {
            if (!EditorUtility.DisplayDialog(
                    "重啟 Deployment",
                    string.Format(DIALOG_MESSAGE, Setting_Config.PROJECT_DEPLOYMENT_NAME, false),
                    "確定", "取消"))
                return;

            RunBat(Setting_Config.PROJECT_DEPLOYMENT_NAME, true);
        }

        static void RunBat(string gameName, bool useProdSuffix) {
            // 1. 構造參數
            var suffixFlag = useProdSuffix ? "true" : "false";
            var args = $"{gameName} {suffixFlag}";

            // 2. bat 檔路徑
            var batPath = Path.Combine(Application.dataPath, "..", "RestartDeployment.bat");
            var psi = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/c \"\"{batPath}\" {args}\"",
                WorkingDirectory = Path.GetDirectoryName(batPath),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                StandardErrorEncoding = System.Text.Encoding.UTF8
            };

            WriteLog.LogFormat("執行：{0} {1}", psi.FileName, psi.Arguments);

            using (var proc = new Process { StartInfo = psi }) {
                proc.OutputDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) WriteLog.Log(e.Data); };
                proc.ErrorDataReceived += (s, e) => { if (!string.IsNullOrEmpty(e.Data)) WriteLog.Log(e.Data); };

                try {
                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();
                } catch (Exception ex) {
                    WriteLog.LogError("執行 RestartDeployment.bat 時發生錯誤：" + ex);
                    EditorUtility.DisplayDialog("重啟失敗", "無法啟動 RestartDeployment.bat，請檢查路徑與權限。", "OK");
                    return;
                }

                if (proc.ExitCode == 0) {
                    WriteLog.Log("RestartDeployment.bat 執行成功 (ExitCode=0)");
                    EditorUtility.DisplayDialog("重啟完成", $"Deployment {gameName} 已重啟。", "OK");
                } else {
                    WriteLog.LogError($"RestartDeployment.bat 執行失敗 (ExitCode={proc.ExitCode})");
                    EditorUtility.DisplayDialog("重啟失敗", $"ExitCode={proc.ExitCode}，詳情請看 Console Log。", "OK");
                }
            }
        }
    }
}

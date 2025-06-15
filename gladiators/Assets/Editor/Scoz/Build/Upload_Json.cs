using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Scoz.Func;

namespace Scoz.Editor {
    public class Upload_Json {
        const string DIALOG_MESSAGE = "上傳JSON到Google Storage並更新JSON，請確認以下：\n\n"
            + "1. 已安裝GoogleCloud工具，並完成初始化\n"
            + "2. 已加入環境變數\n"
            + "3. 已登入有權限的帳號\n\n"
            + "環境: {0}\nJson包版號: {1}\n";

        [MenuItem("Scoz/UploadJson/1. Dev")]
        public static void UploadJsonToDev() {
            if (EditorUtility.DisplayDialog(
                "上傳Json",
                string.Format(DIALOG_MESSAGE, EnvVersion.Dev, VersionSetting.AppLargeVersion),
                "好!", "取消")) {
                UploadJsonToGCS(EnvVersion.Dev);
            }
        }

        [MenuItem("Scoz/UploadJson/2. Test")]
        public static void UploadJsonToTest() {
            if (EditorUtility.DisplayDialog(
                "上傳Json",
                string.Format(DIALOG_MESSAGE, EnvVersion.Test, VersionSetting.AppLargeVersion),
                "好!", "取消")) {
                UploadJsonToGCS(EnvVersion.Test);
            }
        }

        static void UploadJsonToGCS(EnvVersion env) {
            // 0. 檔名清單
            var list = Setting_Config.ServerJsons;

            // 1. 專案、路徑
            if (!Setting_Config.GOOGLE_PROJECTS.TryGetValue(env, out var projectId)) {
                WriteLog.LogError("找不到 GCP 專案 ID：" + env);
                return;
            }
            if (!Setting_Config.GCS_JSON_PATHS.TryGetValue(env, out var storagePath)) {
                WriteLog.LogError("找不到 GCS 路徑：" + env);
                return;
            }

            // 2. 組參數
            var args = new List<string> { projectId, storagePath };
            if (list != null && list.Count > 0) args.AddRange(list);
            var arguments = string.Join(" ", args);

            // 3. 根據平台選擇呼叫 .bat 還是 .sh
            string command, cmdArgs, workDir;
            if (Application.platform == RuntimePlatform.WindowsEditor) {
                // Windows: 用 cmd.exe 執行 .bat
                command = "cmd.exe";
                string bat = Path.Combine(Application.dataPath, "..", "UploadJson.bat");
                workDir = Path.GetDirectoryName(bat);
                cmdArgs = $"/c \"\"{bat}\" {arguments}\"";
            } else {
                // macOS/Linux: 用 bash 執行 .sh
                command = "/bin/bash";
                string sh = Path.Combine(Application.dataPath, "..", "UploadJson.sh");
                workDir = Path.GetDirectoryName(sh);
                cmdArgs = $"\"{sh}\" {arguments}";
            }

            var psi = new ProcessStartInfo {
                FileName = command,
                Arguments = cmdArgs,
                WorkingDirectory = workDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                StandardErrorEncoding = System.Text.Encoding.UTF8
            };

            WriteLog.LogFormat("執行：{0} {1}", psi.FileName, psi.Arguments);

            using (var proc = new Process { StartInfo = psi }) {
                proc.OutputDataReceived += (s, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        WriteLog.Log(e.Data);
                };
                proc.ErrorDataReceived += (s, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        WriteLog.Log(e.Data);   // ← 不要用 LogError，改成 Log
                };

                try {
                    proc.Start();
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit();
                } catch (Exception ex) {
                    WriteLog.LogError($"執行上傳腳本時發生錯誤：{ex}");
                    EditorUtility.DisplayDialog("上傳失敗", "無法啟動外部腳本，請檢查路徑與權限。", "OK");
                    return;
                }

                if (proc.ExitCode == 0) {
                    WriteLog.Log("上傳腳本執行成功 (ExitCode=0)");
                    EditorUtility.DisplayDialog("上傳完成", "所有 JSON 已成功上傳並設定完畢。", "OK");
                } else {
                    WriteLog.LogError($"上傳腳本執行失敗 (ExitCode={proc.ExitCode})");
                    EditorUtility.DisplayDialog("上傳失敗", $"ExitCode={proc.ExitCode}，詳情請看 Console Log。", "OK");
                }
            }
        }
    }
}

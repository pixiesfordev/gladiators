using UnityEditor;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEngine;
using Scoz.Func;
using System;
using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Installer;

namespace Scoz.Editor {
    public class CloudBuild : BuildScriptPackedMode {

        public static void RunBuild() {
            // 這裡可查看有哪些UCB內建的環境變數可用: https://docs.unity.com/ugs/en-us/manual/devops/manual/build-automation/advanced-build-configuration/environment-variables
            WriteLog.Log("========== Run CloudBuild ==========");
            DateTime startTime = DateTime.Now;
            // ========== 設定環境變數 ==========
            WriteLog.Log("========== 1. 檢查環境變數 ==========");
            string projectName = Environment.GetEnvironmentVariable("project_name");
            string ucbNumber = Environment.GetEnvironmentVariable("UCB_BUILD_NUMBER");
            string envVersion = Environment.GetEnvironmentVariable("ENV_VERSION");
            string gameVersion = Environment.GetEnvironmentVariable("GAME_VERSION");
            string generateAllStr = Environment.GetEnvironmentVariable("GENERATE_ALL");
            string buildType = Environment.GetEnvironmentVariable("BUILD_TYPE");
            string callbackURL = Environment.GetEnvironmentVariable("CALLBACK_URL");

            Debug.Log($"ProjectName: {projectName}");
            Debug.Log($"BuildNumber: {ucbNumber}");
            Debug.Log($"EnvVersion: {envVersion}");
            Debug.Log($"GameVersion: {gameVersion}");
            Debug.Log($"GenerateAll: {generateAllStr}");
            Debug.Log($"BuildType: {buildType}");
            Debug.Log($"CallbackURL: {callbackURL}");

            //generateAllStr = "true";
            //envVersion = "Dev";
            //gameVersion = "1.5.1";

            if (!bool.TryParse(generateAllStr, out bool generateAll)) {
                string log = $"版本參數設定錯誤 GenerateAll: {generateAllStr}";
                WriteLog.LogError(log);
                Environment.Exit(1); // 觸發錯誤退出
                return;
            }
            if(buildType!="NewBuild" && buildType != "UpdateAPreviousBuild") {
                string log = $"版本參數設定錯誤 NewBuild: {buildType}";
                WriteLog.Log(log);
                Environment.Exit(1); // 觸發錯誤退出
                return;
            }
            if (string.IsNullOrEmpty(gameVersion)) {
                string log = $"版本參數設定錯誤 GameVersion: {gameVersion}";
                WriteLog.LogError(log);
                Environment.Exit(1); // 觸發錯誤退出
                return;
            }
            if (string.IsNullOrEmpty(callbackURL)) {
                string log = $"版本參數設定錯誤 CallbackURL: {callbackURL}";
                WriteLog.LogError(log);
                Environment.Exit(1);
                return;
            }
            WriteLog.Log($"環境版本: {envVersion}  版號: {gameVersion} GenerateAll: {generateAllStr} BuildType: {buildType}");
            EnvVersion env = EnvVersion.Dev;
            if (!MyEnum.TryParseEnum(envVersion, out EnvVersion _envVersion)) {
                string log = $"版本參數設定錯誤 EnvVersion: {envVersion}";
                WriteLog.LogError(log);
                Environment.Exit(1);
                return;
            } else {
                env = _envVersion;
            }
            var passTime = (DateTime.Now - startTime);
            WriteLog.Log($"========== 檢查環境變數完成! 花費 {passTime.TotalSeconds} 秒 ==========");

            // ========== 設定 環境版本 &  版號 ==========
            WriteLog.Log("========== 2. 設定 環境版本 &  版號 ==========");
            PlayerSettings.bundleVersion = gameVersion; // 設定遊戲版本
            string nextProcessStr = "ScozBuildScript.CloudBuild_NewBuild2";
            string paramStr = $"{envVersion},{gameVersion},{generateAllStr},{buildType}";
            ProcessContinuation.SetProcess(nextProcessStr, paramStr);
            SwitchVersion.RunSwitchVersion(env, nextProcessStr); // 切環境版本
        }
        public static void CloudBuild_NewBuild2(string _env, string _gameVersion, string _generateAllStr, string _buildType) {
            bool generateAll = false;
            if (!bool.TryParse(_generateAllStr, out generateAll)) {
                WriteLog.LogError($"版本參數設定錯誤 GenerateAll: {_generateAllStr}");
                Environment.Exit(1);
                return;
            }
            EnvVersion env = EnvVersion.Dev;
            if (!MyEnum.TryParseEnum(_env, out EnvVersion _envVersion)) {
                WriteLog.LogError($"版本參數設定錯誤 EnvVersion: {_env}");
                Environment.Exit(1); // 觸發錯誤退出
                return;
            } else {
                env = _envVersion;
            }

            WriteLog.Log("========== 設定 環境版本 &  版號 完成! ==========");


            // ========== HybridCRL ==========
            WriteLog.Log("========== 3. 開始設定 HybridCLR ==========");
            DateTime startTime = DateTime.Now;
            var controller = new InstallerController();
            bool hasInstall = controller.HasInstalledHybridCLR();
            // 還沒安裝或版本不對就進行安裝
            if (!hasInstall || controller.PackageVersion != controller.InstalledLibil2cppVersion) {
                WriteLog.Log($"目前 HybridCLR 版本是 ${controller.InstalledLibil2cppVersion} 進行安裝 HybridCLR {controller.PackageVersion}");
                controller.InstallDefaultHybridCLR();
                WriteLog.Log($"HybridCLR {controller.PackageVersion} 安裝完成!");
            } else {
                WriteLog.Log($"HybridCLR 已是最新版 {controller.PackageVersion}");
            }
            // 有需要就跑 GenerateAll
            if (generateAll) {
                WriteLog.Log($"執行 HybridCLR GenerateAll");
                PrebuildCommand.GenerateAll();
                WriteLog.Log($"HybridCLR GenerateAll 更新完成");
            }
            var passTime = (DateTime.Now - startTime);
            WriteLog.Log($"========== 設定 HybridCLR 完成! 花費 {passTime.TotalSeconds} 秒 ==========");

            // ========== 包 Bundle ==========
            WriteLog.Log("========== 4. 開始包 Bundle ==========");
            startTime = DateTime.Now;
            if (_buildType=="NewBuild") {
                WriteLog.Log("進行 NewBuild");
                ManualBuild.NewBuild();
                passTime = (DateTime.Now - startTime);
                WriteLog.Log($"========== 包 Bundle 完成! 花費 {passTime.TotalSeconds} 秒 ==========");
            } else if(_buildType == "UpdateAPreviousBuild") {
                WriteLog.Log("進行 Update A Previous Build");
                ManualBuild.UpdateAPreviousBuild();
                passTime = (DateTime.Now - startTime);
                WriteLog.Log($"========== 包 Bundle 完成! 花費 {passTime.TotalSeconds} 秒 ==========");
                callStopUCB();
            } else {
                WriteLog.LogError($"_buildType參數錯誤: {_buildType}");
                Environment.Exit(1); // 觸發錯誤退出
                return;
            }

        }

        /// <summary>
        /// Cloud Build 結束後的後呼叫
        /// </summary>
        public static void PostProcess() {

        }

        static void callStopUCB() {
            string ucbNumber = Environment.GetEnvironmentVariable("UCB_BUILD_NUMBER");
            string orgId = Environment.GetEnvironmentVariable("UCB_ORG_ID");
            string projectId = Environment.GetEnvironmentVariable("UCB_PROJECT_ID");
            string buildTargetId = Environment.GetEnvironmentVariable("UCB_BUILD_TARGET_ID");
            string apiToken = Environment.GetEnvironmentVariable("UCB_API_TOKEN");

            // 記錄原始環境變數值以診斷
            WriteLog.Log("原始 ucbNumber: " + (ucbNumber ?? "null"));
            WriteLog.Log("原始 orgId: " + (orgId ?? "null"));
            WriteLog.Log("原始 projectId: " + (projectId ?? "null"));
            WriteLog.Log("原始 buildTargetId: " + (buildTargetId ?? "null"));
            WriteLog.Log("原始 apiToken: " + (apiToken ?? "null"));

            // 檢查必要參數
            if (string.IsNullOrEmpty(ucbNumber) || string.IsNullOrEmpty(orgId) ||
                string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(buildTargetId) ||
                string.IsNullOrEmpty(apiToken)) {
                WriteLog.LogError("缺少停止建置所需的環境變數 (UCB_BUILD_NUMBER, UCB_ORG_ID, UCB_PROJECT_ID, UCB_BUILD_TARGET_ID, UCB_API_TOKEN)");
                Environment.Exit(1);
                return;
            }

            // 處理轉義序列並確保 URL 安全
            string safeOrgId = Uri.EscapeDataString(orgId.Replace("\\", "\\\\"));
            string safeProjectId = Uri.EscapeDataString(projectId.Replace("\\", "\\\\"));
            string safeBuildTargetId = Uri.EscapeDataString(buildTargetId.Replace("\\", "\\\\"));
            string safeUcbNumber = Uri.EscapeDataString(ucbNumber.Replace("\\", "\\\\"));
            string safeApiToken = Uri.EscapeDataString(apiToken.Replace("\\", "\\\\"));

            WriteLog.LogError("嘗試停止 UCB Build " + safeUcbNumber + "...");

            // 透過 API 停止建置
            try {
                using (var client = new System.Net.Http.HttpClient()) {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", safeApiToken);
                    var task = client.DeleteAsync("https://build-api.cloud.unity3d.com/api/v1/orgs/" + safeOrgId + "/projects/" + safeProjectId + "/buildtargets/" + safeBuildTargetId + "/builds/" + safeUcbNumber);
                    task.Wait(10000); // 等待最多 10 秒
                    if (task.Result.IsSuccessStatusCode) {
                        WriteLog.Log("成功透過 API 停止 UCB Build");
                        WriteLog.Log("UCB Number: " + safeUcbNumber);
                    } else {
                        WriteLog.LogError("UCB API 停止失敗 (Build " + safeUcbNumber + ")，狀態碼: " + task.Result.StatusCode);
                        Environment.Exit(1); // 觸發錯誤退出
                    }
                }
            } catch (Exception e) {
                // 確保 e.Message 安全
                string safeMessage = Uri.EscapeDataString(e.Message.Replace("\\", "\\\\"));
                WriteLog.LogError("UCB API 停止失敗 (Build " + safeUcbNumber + "): " + safeMessage);
                Environment.Exit(1); // 觸發錯誤退出
            }
        }

    }
}
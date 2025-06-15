using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using Scoz.Func;
using UnityEngine;

namespace Scoz.Editor {
    public static class Setting_Config {
        public const string PROJECT_SERVER_NAME = "card_swap";
        public const string PROJECT_DEPLOYMENT_NAME = "card-swap";
        public const string Bundle_NAME = "card_swap_bundle";
        public const string PACKAGE_NAME = "cardswap";
        public const string COMPANY_NAME = "aura";
        public const string ADDRESABLE_BIN_PATH = "Assets/AddressableAssetsData/{0}/{1}/{2}/";//平台/版本/遊戲版本 例如 WebGL/Dev/1.1
        public static List<string> ServerJsons = new List<string>() { "Bet", "HandType" };
        public static Dictionary<EnvVersion, string> GOOGLE_PROJECTS = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, "csc5023-games-dev"},
            { EnvVersion.Test, "csc5023-minigames-test"},
            { EnvVersion.Release, "mini-games-release"},
        };
        public static Dictionary<EnvVersion, string> GCS_WEBGL_PATHS_DEVTEST = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, $"minigames-devtest/{PROJECT_SERVER_NAME}"},
            { EnvVersion.Test, $"minigames-devtest/{PROJECT_SERVER_NAME}"},
            { EnvVersion.Release, $"minigames-devtest/{PROJECT_SERVER_NAME}"},
        };
        public static Dictionary<EnvVersion, string> GCS_WEBGL_PATHS = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, $"minigames-private-dev/{PROJECT_SERVER_NAME}/webgl"},
            { EnvVersion.Test, $"minigames-private-test/{PROJECT_SERVER_NAME}/webgl"},
            { EnvVersion.Release, $"minigames-private-release/{PROJECT_SERVER_NAME}/webgl"},
        };
        public static Dictionary<EnvVersion, string> GCS_BUNDLE_PATHS = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, $"minigames-public-dev/{Bundle_NAME}"},
            { EnvVersion.Test, $"minigames-public-test/{Bundle_NAME}"},
            { EnvVersion.Release, $"minigames-public-release/{Bundle_NAME}"},
        };
        public static Dictionary<EnvVersion, string> GCS_JSON_PATHS = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, $"minigames-private-dev/gamejsons/{PROJECT_SERVER_NAME}"},
            { EnvVersion.Test, $"minigames-private-test/gamejsons/{PROJECT_SERVER_NAME}"},
            { EnvVersion.Release, $"minigames-private-release/gamejsons/{PROJECT_SERVER_NAME}"},
        };
        public static Dictionary<EnvVersion, string> ADDRESABALE_PROFILES = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, "GoogleCloud-Dev"},
            { EnvVersion.Test, "GoogleCloud-Test"},
            { EnvVersion.Release, "GoogleCloud-Release"},
        };
        public static Dictionary<EnvVersion, string> KEYSTORE_ALIAS = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, "123456"},
            { EnvVersion.Test, "123456"},
            { EnvVersion.Release, "123456"},
        };

        public static Dictionary<EnvVersion, string> PACKAGE_NAMES = new Dictionary<EnvVersion, string>() {
            { EnvVersion.Dev, $"com.{COMPANY_NAME}.{PACKAGE_NAME}.dev"},
            { EnvVersion.Test, $"com.{COMPANY_NAME}.{PACKAGE_NAME}.test"},
            { EnvVersion.Release, $"com.{COMPANY_NAME}.{PACKAGE_NAME}"},
        };
    }
}
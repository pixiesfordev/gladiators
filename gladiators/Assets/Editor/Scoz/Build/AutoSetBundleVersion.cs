using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
/// <summary>
/// 自動+Bundle版號的工具，NewBuild時會重置回1，之後UpdatePreviousBuild會自動加版號
/// </summary>
public static class AutoSetBundleVersion {
    const string FILE_PATH = "Assets/Scripts/Assembly_Game/Manager/BundleVersion.cs";
    const int FIRST_VERSION = 1; // 第一版版號

    public static void SetBundleVersionToFirstVersion() {
        if (File.Exists(FILE_PATH)) {
            string txt = File.ReadAllText(FILE_PATH);
            if (txt.Contains($"const int VERSION = {FIRST_VERSION};")) return;
        }
        writeBundleVersion(FIRST_VERSION);
    }

    // 自動增版號
    public static void IncrementBundleVersion() {
        int _newVer = FIRST_VERSION;
        if (File.Exists(FILE_PATH)) {
            string _txt = File.ReadAllText(FILE_PATH);
            Match _m = Regex.Match(_txt, @"const\s+int\s+VERSION\s*=\s*(\d+)");
            if (_m.Success && int.TryParse(_m.Groups[1].Value, out int _curVer))
                _newVer = _curVer + 1;
        }
        writeBundleVersion(_newVer);
    }

    static void writeBundleVersion(int _ver) {
        StringBuilder _sb = new StringBuilder();
        _sb.AppendLine("public static class BundleVersion {");
        _sb.AppendLine($"    public const int VERSION = {_ver};");
        _sb.AppendLine("}");

        Directory.CreateDirectory(Path.GetDirectoryName(FILE_PATH));
        File.WriteAllText(FILE_PATH, _sb.ToString());
        AssetDatabase.Refresh();
    }
}

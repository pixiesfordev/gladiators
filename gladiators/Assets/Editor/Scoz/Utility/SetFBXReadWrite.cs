using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class SetFBXReadWrite : ScriptableObject {

    [MenuItem("Assets/Scoz/FBX/Enable FBX ReadWrite")]
    static void EnableReadWrite() {
        SetReadWrite(true);
    }

    [MenuItem("Assets/Scoz/FBX/Disable FBX ReadWrite")]
    static void DisableReadWrite() {
        SetReadWrite(false);
    }

    static void SetReadWrite(bool flag) {
        string[] selectedAssetGUIDs = Selection.assetGUIDs;

        foreach (string guid in selectedAssetGUIDs) {
            string folderPath = AssetDatabase.GUIDToAssetPath(guid);

            if (AssetDatabase.IsValidFolder(folderPath)) {
                ProcessFolder(folderPath, flag);
            } else {
                Debug.LogError("路徑不存在：" + folderPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static void ProcessFolder(string folderPath, bool flag) {
        string folderFullPath = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), folderPath);

        if (Directory.Exists(folderFullPath)) {
            string[] fileEntries = Directory.GetFiles(folderFullPath, "*.fbx", SearchOption.AllDirectories);
            foreach (string fileName in fileEntries) {
                string assetPath = "Assets" + fileName.Substring(Application.dataPath.Length);
                ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (importer != null) {
                    importer.isReadable = flag;
                    importer.SaveAndReimport();
                }
            }
        } else {
            Debug.LogError("路徑不存在：" + folderFullPath);
        }
    }
}

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Utils {
    public static class GetGuidTool {
        [MenuItem("Assets/Copy GUID to Clipboard", false, 20)]
        private static void CopyGUID() {
            if (Selection.activeObject == null) return;
        
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var guid = AssetDatabase.AssetPathToGUID(path);
        
            GUIUtility.systemCopyBuffer = guid; // Copia negli appunti
            Debug.Log($"Copied GUID: {guid}");
        }
    }
}

#endif
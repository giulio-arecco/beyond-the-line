using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils.Ink {
    #if UNITY_EDITOR
    public class InkRecompiler {
        [MenuItem("Tools/Ink/Force Recompile All Ink Files")]
        public static void ForceRecompileAllInkFilesSynchronized() {
            var projectPath = Application.dataPath;
            var assetsPath = "Assets"; 

            // Find all ink files 
            var inkFilePaths = Directory.GetFiles(projectPath, "*.ink", SearchOption.AllDirectories);

            var compiledCount = 0;

            AssetDatabase.StartAssetEditing(); 

            try {
                // Iterate and force compilation by reimporting the ink files
                foreach (var fullPath in inkFilePaths) {
                    var unityAssetPath = fullPath.Replace(projectPath, assetsPath).Replace('\\', '/');
                
                    AssetDatabase.ImportAsset(unityAssetPath, ImportAssetOptions.ForceUpdate);
                
                    // This forces Unity to finish the import pipeline for the current file before continuing to execute the script.
                    // In practice, it acts as a synchronization barrier.
                    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

                    compiledCount++;
                }
            }
            finally {
                AssetDatabase.StopAssetEditing();
            }
        
            // Save the imported assets
            AssetDatabase.SaveAssets();

            if (compiledCount > 0) {
            
                Debug.Log($"Ink recompiling complete. Processed {compiledCount} .ink files.");
            }
            else {
                Debug.LogWarning("No .ink file found in project.");
            }
        }
    }
    #endif
}
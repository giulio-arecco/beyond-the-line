using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json; 

public static class StatsExporter {
    public static readonly string[] StatsToSave = {
        "LISTENED_TO_RADIO",
        "KNOWN_MILITARY_ROAD_STATUS",
        "KNOWN_MOUNTAINPASS_STATUS",
        
        "KNOWN_MOUNTAINPASS",
        "COMPLETED_MOUNTAINPASS",
        "COMPLETED_BORDER",
        
        "SET_CAMP_COUNT",
        "ELIAS_OPTIONAL_DIALOGUE_DONE",
        "LIRA_OPTIONAL_DIALOGUE_DONE",
        
        "READ_NOTEBOOK"
    };
    
    public static void SaveGameStats(Dictionary<string, object> inkStats) {
        var baseDirectory = GetBaseDirectory();
        var targetFolder = Path.Combine(baseDirectory, "SessionStats");

        if (!Directory.Exists(targetFolder)) {
            Directory.CreateDirectory(targetFolder);
            Debug.Log($"[StatsExporter] Created directory: {targetFolder}");
        }

        var jsonOutput = JsonConvert.SerializeObject(inkStats, Formatting.Indented);

        var filePath = GetUniqueFilePath(targetFolder, "session_stats", "json");

        try {
            File.WriteAllText(filePath, jsonOutput);
            Debug.Log($"[StatsExporter] Stats written successfully to: {filePath}");
        }
        catch (System.Exception e) {
            Debug.LogError($"[StatsExporter] Error writing stats to file: {e.Message}");
        }
    }

    private static string GetBaseDirectory() {
#if UNITY_EDITOR
        // Save the data in the Assets directory parent
        return Directory.GetParent(Application.dataPath).FullName; 
#else
        return System.AppDomain.CurrentDomain.BaseDirectory;
#endif
    }

    private static string GetUniqueFilePath(string folder, string baseName, string extension) {
        var index = 0;
        string fullPath;

        do {
            var fileName = $"{baseName}_{index}.{extension}";
            fullPath = Path.Combine(folder, fileName);
            index++;
        } 
        while (File.Exists(fullPath));

        return fullPath;
    }
}
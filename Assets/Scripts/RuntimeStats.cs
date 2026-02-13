using System;
using System.Collections.Generic;

public static class RuntimeStats {
    private static readonly int[] IntStats = new int[(int)IntRuntimeStat.Count];
    private static readonly float[] FloatStats = new float[(int)FloatRuntimeStat.Count];
    private static readonly bool[] BoolStats = new bool[(int)BoolRuntimeStat.Count];
    private static readonly string[] StringStats = new string[(int)StringRuntimeStat.Count];
    
    public static void SetStat(IntRuntimeStat stat, int value) => IntStats[(int)stat] = value;
    public static int GetStat(IntRuntimeStat stat) => IntStats[(int)stat];
    public static void IncreaseStat(IntRuntimeStat stat, int amount) => IntStats[(int)stat] += amount;
    
    public static void SetStat(FloatRuntimeStat stat, float value) => FloatStats[(int)stat] = value;
    public static float GetStat(FloatRuntimeStat stat) => FloatStats[(int)stat];
    public static void IncreaseStat(FloatRuntimeStat stat, float amount) => FloatStats[(int)stat] += amount;
    
    public static void SetStat(BoolRuntimeStat stat, bool value) => BoolStats[(int)stat] = value;
    public static bool GetStat(BoolRuntimeStat stat) => BoolStats[(int)stat];
    public static void ToggleStat(BoolRuntimeStat stat) => BoolStats[(int)stat] = !BoolStats[(int)stat];
    
    public static void SetStat(StringRuntimeStat stat, string value) => StringStats[(int)stat] = value;
    public static string GetStat(StringRuntimeStat stat) => StringStats[(int)stat];
    
    public static void ResetAll() {
        Array.Clear(IntStats, 0, IntStats.Length);
        Array.Clear(FloatStats, 0, FloatStats.Length);
        Array.Clear(BoolStats, 0, BoolStats.Length);
        Array.Clear(StringStats, 0, StringStats.Length);
    }
    
    public static Dictionary<string, object> GetAllStatsAsDictionary() {
        var exportDict = new Dictionary<string, object>();
        
        for (var i = 0; i < (int)IntRuntimeStat.Count; i++) 
            exportDict[((IntRuntimeStat)i).ToString()] = IntStats[i];

        for (var i = 0; i < (int)FloatRuntimeStat.Count; i++) 
            exportDict[((FloatRuntimeStat)i).ToString()] = FloatStats[i];

        for (var i = 0; i < (int)BoolRuntimeStat.Count; i++) 
            exportDict[((BoolRuntimeStat)i).ToString()] = BoolStats[i];

        for (var i = 0; i < (int)StringRuntimeStat.Count; i++) 
            exportDict[((StringRuntimeStat)i).ToString()] = StringStats[i];

        return exportDict;
    }
}
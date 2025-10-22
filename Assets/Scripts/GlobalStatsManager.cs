using System;
using UnityEngine;

public class IntStat {
    private int _value;
    public int Value {
        get => _value;
        set {
            var newValue = Mathf.Clamp(value, 0, 100);
            
            if (_value == newValue) return;
            _value = newValue;
            OnValueChanged?.Invoke(_value);
        }
    }
    
    public event Action<int> OnValueChanged;
}

public class GlobalStats {
    public IntStat Health { get; private set; } = new();
    public IntStat Fatigue { get; private set; } = new();
    public IntStat Hunger { get; private set; } = new();
    public IntStat GroupCohesion { get; private set; } = new();
    public IntStat Notoriety { get; private set; } = new();
    public IntStat Intimidation { get; private set; } = new();
}

public class GlobalStatsManager: Singleton<GlobalStatsManager> {
    public GlobalStats GlobalStats { get; private set; } = new();

    private void Start() {
        GlobalStats.Health.Value = 100;
        GlobalStats.GroupCohesion.Value = 50;
    }
}

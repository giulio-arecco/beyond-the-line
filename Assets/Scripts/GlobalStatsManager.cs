using System;
using System.Reflection;
using Inventory.Interfaces;
using Storage.Storables;
using UnityEngine;
using Utils.SerializeInterface;
using static Utils.TypeUtils;

public class IntStat {
    private readonly int minValue, maxValue;
    
    private int _value;
    public int Value {
        get => _value;
        set {
            var newValue = Mathf.Clamp(value, minValue, maxValue);
            
            if (_value == newValue) return;
            _value = newValue;
            OnValueChanged?.Invoke(_value);
        }
    }

    public IntStat() {
        minValue = 0;
        maxValue = 100;
    }

    public IntStat(int minValue, int maxValue) {
        this.minValue = minValue;
        this.maxValue = maxValue;
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

    public void IncreaseStat(string statName, object statValue) {
        switch (statValue) {
            case int intValue:
                IncreaseIntStat(statName, intValue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statValue), statValue, $"Unsupported stat type: {statValue?.GetType().Name}");
        }
    }
    
    public void DecreaseStat(string statName, object statValue) {
        switch (statValue) {
            case int intValue:
                DecreaseIntStat(statName, intValue);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statValue), statValue, $"Unsupported stat type: {statValue?.GetType().Name}");
        }
    }

    public void IncreaseIntStat(string statName, int valueToAdd) {
        var type = GetType();
        
        var prop = type.GetProperty(statName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null) {
            Debug.LogError($"Property '{statName}' not found in {type.Name}");
            return;
        }
        
        if (prop.PropertyType == typeof(IntStat) && prop.CanRead) {
            var propValue = (IntStat)prop.GetValue(this);
            propValue.Value += valueToAdd;
        }
        else {
            Debug.LogError($"The property '{statName}' is either not an integer or not readable");
        }
    }

    public void DecreaseIntStat(string statName, int valueToSubtract) {
        var type = GetType();
        
        var prop = type.GetProperty(statName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null) {
            Debug.LogError($"Property '{statName}' not found in {type.Name}");
            return;
        }
        
        if (prop.PropertyType == typeof(IntStat) && prop.CanRead) {
            var propValue = (IntStat)prop.GetValue(this);
            propValue.Value -= valueToSubtract;
        }
        else {
            Debug.LogError($"The property '{statName}' is either not an integer or not readable");
        }
    }
}

public class GlobalStatsManager: Singleton<GlobalStatsManager> {
    [SerializeField] InterfaceReference<IStorage<Item>> inventory;
    
    public GlobalStats GlobalStats { get; private set; }

    protected override void Awake() {
        base.Awake();
        GlobalStats = new GlobalStats();
    }

    private void Start() {
        inventory.Value.OnAdd += Storage_OnAdd;
        inventory.Value.OnRemove += Storage_OnRemove;
        
        GlobalStats.Health.Value = 100;
    }

    private void OnDestroy() {
        if (inventory.Value != null) {
            inventory.Value.OnAdd -= Storage_OnAdd;
            inventory.Value.OnRemove -= Storage_OnRemove;
        }
    }


    private void Storage_OnAdd(Storable element) {
        var item = ConvertTo<Item>(element);
        var modifiers = item.TypedInfo.StatsModifiers;
        
        if (modifiers == null || modifiers.Length == 0) return;

        foreach (var statMod in modifiers) {
            GlobalStats.IncreaseIntStat(statMod.statName, statMod.statValue);
        }
    }
    
    private void Storage_OnRemove(Storable element) {
        var item = ConvertTo<Item>(element);
        var modifiers = item.TypedInfo.StatsModifiers;
        
        if (modifiers == null || modifiers.Length == 0) return;

        foreach (var statMod in modifiers) {
            GlobalStats.DecreaseIntStat(statMod.statName, statMod.statValue);
        }
    }
}

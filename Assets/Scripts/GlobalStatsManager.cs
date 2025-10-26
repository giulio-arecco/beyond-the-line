using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ink.UnityIntegration;
using Inventory.Interfaces;
using Storage.Storables;
using UnityEngine;
using Utils.SerializeInterface;
using static Utils.TypeUtils;

public class IntStat {
    public readonly int MinValue, MaxValue;
    
    private int _value;
    public int Value {
        get => _value;
        set {
            var newValue = Mathf.Clamp(value, MinValue, MaxValue);
            
            if (_value == newValue) return;
            _value = newValue;
            OnValueChanged?.Invoke(_value);
        }
    }

    public IntStat() {
        MinValue = 0;
        MaxValue = 100;
    }

    public IntStat(int minValue, int maxValue) {
        this.MinValue = minValue;
        this.MaxValue = maxValue;
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

    private void ChangeStatValue(string statName, object delta, bool isIncrease) {
        var type = GetType();
        
        var prop = type.GetProperty(statName, BindingFlags.Public | BindingFlags.Instance);
        
        if (prop == null) {
            Debug.LogError($"Property '{statName}' not found in {type.Name}");
            return;
        }
        
        if (!prop.CanRead) {
            Debug.LogError($"The property '{statName}' is not readable");
            return;
        }

        var castedStat = prop.GetValue(this) switch {
            IntStat intStat => intStat,
            _ => throw new InvalidOperationException($"Unsupported stat type: {prop.PropertyType.Name}")
        };

        var castedValue = delta switch {
            int intValue => intValue,
            _ => throw new ArgumentOutOfRangeException(nameof(delta), delta,
                $"Unsupported value type: {delta.GetType().Name}")
        };

        if (castedStat.Value.GetType() == castedValue.GetType()) {
            castedStat.Value += isIncrease? castedValue : -castedValue;
        }
        else {
            throw new InvalidOperationException($"The stat {statName} and the value {delta} are of different types");
        }
    }
    
    public object GetStatValue(string statName) {
        var type = GetType();
        
        var prop = type.GetProperty(statName, BindingFlags.Public | BindingFlags.Instance);
        if (prop == null) {
            Debug.LogError($"Property '{statName}' not found in {type.Name}");
            return null;
        }

        return prop.GetValue(this) switch {
            IntStat intStat => intStat.Value,
            _ => throw new InvalidOperationException($"Unsupported stat type: {prop.PropertyType.Name}")
        };
    }

    public void IncreaseStatValue(string statName, object valueToAdd) {
        ChangeStatValue(statName, valueToAdd, true);
    }

    public void DecreaseStatValue(string statName, object valueToSubtract) {
        ChangeStatValue(statName, valueToSubtract, false);
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
            GlobalStats.IncreaseStatValue(statMod.statName, statMod.statValue);
        }
    }
    
    private void Storage_OnRemove(Storable element) {
        var item = ConvertTo<Item>(element);
        var modifiers = item.TypedInfo.StatsModifiers;
        
        if (modifiers == null || modifiers.Length == 0) return;

        foreach (var statMod in modifiers) {
            GlobalStats.DecreaseStatValue(statMod.statName, statMod.statValue);
        }
    }
}

using System;
using TMPro;
using UnityEngine;

namespace UI {
    public class UIStatsHandler : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI healthValueText;
        [SerializeField] private TextMeshProUGUI fatigueValueText;
        [SerializeField] private TextMeshProUGUI hungerValueText;
        [SerializeField] private TextMeshProUGUI groupCohesionValueText;
        [SerializeField] private TextMeshProUGUI notorietyValueText;
        [SerializeField] private TextMeshProUGUI intimidationValueText;
        
        private void Start() {
            var globalStats = GlobalStatsManager.Instance.GlobalStats;
            
            globalStats.Health.OnValueChanged += GlobalStats_OnHealthChanged;
            globalStats.Fatigue.OnValueChanged += GlobalStats_OnFatigueChanged;
            globalStats.Hunger.OnValueChanged += GlobalStats_OnHungerChanged;
            globalStats.GroupCohesion.OnValueChanged += GlobalStats_OnGroupCohesionChanged;
            globalStats.Notoriety.OnValueChanged += GlobalStats_OnNotorietyChanged;
            globalStats.Intimidation.OnValueChanged += GlobalStats_OnIntimidationChanged;
            
            var maxHealth = globalStats.Health.MaxValue;
            healthValueText.text = $"{globalStats.Health.Value.ToString()} / {maxHealth.ToString()}";
            
            var maxFatigue = globalStats.Fatigue.MaxValue;
            fatigueValueText.text = $"{globalStats.Fatigue.Value.ToString()} / {maxFatigue.ToString()}";
            
            var maxHunger = globalStats.Hunger.MaxValue;
            hungerValueText.text = $"{globalStats.Hunger.Value.ToString()} / {maxHunger.ToString()}";
            
            var maxGroupCohesion = globalStats.GroupCohesion.MaxValue;
            groupCohesionValueText.text = $"{globalStats.GroupCohesion.Value.ToString()} / {maxGroupCohesion.ToString()}";
            
            var maxNotoriety = globalStats.Notoriety.MaxValue;
            notorietyValueText.text = $"{globalStats.Notoriety.Value.ToString()} / {maxNotoriety.ToString()}";
            
            var maxIntimidation = globalStats.Intimidation.MaxValue;
            intimidationValueText.text = $"{globalStats.Intimidation.Value.ToString()} / {maxIntimidation.ToString()}";
        }

        private void OnDestroy() {
            if (GlobalStatsManager.TryGetInstance(out var globalStatsManager)) {
                var globalStats = globalStatsManager.GlobalStats;
                
                globalStats.Health.OnValueChanged -= GlobalStats_OnHealthChanged;
                globalStats.Fatigue.OnValueChanged -= GlobalStats_OnFatigueChanged;
                globalStats.Hunger.OnValueChanged -= GlobalStats_OnHungerChanged;
                globalStats.GroupCohesion.OnValueChanged -= GlobalStats_OnGroupCohesionChanged;
                globalStats.Notoriety.OnValueChanged -= GlobalStats_OnNotorietyChanged;
                globalStats.Intimidation.OnValueChanged -= GlobalStats_OnIntimidationChanged;
            }
        }

        private void GlobalStats_OnHealthChanged(int health) {
            var maxHealth = GlobalStatsManager.Instance.GlobalStats.Health.MaxValue;
            healthValueText.text = $"{health.ToString()} / {maxHealth.ToString()}";
        }
        
        private void GlobalStats_OnFatigueChanged(int fatigue) {
            var maxFatigue = GlobalStatsManager.Instance.GlobalStats.Fatigue.MaxValue;
            fatigueValueText.text = $"{fatigue.ToString()} / {maxFatigue.ToString()}";
        }
        
        private void GlobalStats_OnHungerChanged(int hunger) {
            var maxHunger = GlobalStatsManager.Instance.GlobalStats.Hunger.MaxValue;
            hungerValueText.text = $"{hunger.ToString()} / {maxHunger.ToString()}";
        }
        
        private void GlobalStats_OnGroupCohesionChanged(int groupCohesion) {
            var maxGroupCohesion = GlobalStatsManager.Instance.GlobalStats.GroupCohesion.MaxValue;
            groupCohesionValueText.text = $"{groupCohesion.ToString()} / {maxGroupCohesion.ToString()}";
        }
        
        private void GlobalStats_OnNotorietyChanged(int notoriety) {
            var maxNotoriety = GlobalStatsManager.Instance.GlobalStats.Notoriety.MaxValue;
            notorietyValueText.text = $"{notoriety.ToString()} / {maxNotoriety.ToString()}";
        }
        
        private void GlobalStats_OnIntimidationChanged(int intimidation) {
            var maxIntimidation = GlobalStatsManager.Instance.GlobalStats.Intimidation.MaxValue;
            intimidationValueText.text = $"{intimidation.ToString()} / {maxIntimidation.ToString()}";
        }
    }
}

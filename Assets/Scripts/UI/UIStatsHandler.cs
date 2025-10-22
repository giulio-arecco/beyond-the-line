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
            
            healthValueText.text = globalStats.Health.Value.ToString();
            fatigueValueText.text = globalStats.Fatigue.Value.ToString();
            hungerValueText.text = globalStats.Hunger.Value.ToString();
            groupCohesionValueText.text = globalStats.GroupCohesion.Value.ToString();
            notorietyValueText.text = globalStats.Notoriety.Value.ToString();
            intimidationValueText.text = globalStats.Intimidation.Value.ToString();
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
            healthValueText.text = health.ToString();
        }
        
        private void GlobalStats_OnFatigueChanged(int fatigue) {
            fatigueValueText.text = fatigue.ToString();
        }
        
        private void GlobalStats_OnHungerChanged(int hunger) {
            hungerValueText.text = hunger.ToString();
        }
        
        private void GlobalStats_OnGroupCohesionChanged(int groupCohesion) {
            groupCohesionValueText.text = groupCohesion.ToString();
        }
        
        private void GlobalStats_OnNotorietyChanged(int notoriety) {
            notorietyValueText.text = notoriety.ToString();
        }
        
        private void GlobalStats_OnIntimidationChanged(int intimidation) {
            intimidationValueText.text = intimidation.ToString();
        }
    }
}

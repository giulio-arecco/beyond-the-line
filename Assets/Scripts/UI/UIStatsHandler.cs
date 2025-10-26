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
        [SerializeField] private Gradient statColorGradient;
        
        private void Start() {
            var globalStats = GlobalStatsManager.Instance.GlobalStats;
            
            globalStats.Health.OnValueChanged += GlobalStats_OnHealthChanged;
            globalStats.Fatigue.OnValueChanged += GlobalStats_OnFatigueChanged;
            globalStats.Hunger.OnValueChanged += GlobalStats_OnHungerChanged;
            globalStats.GroupCohesion.OnValueChanged += GlobalStats_OnGroupCohesionChanged;
            globalStats.Notoriety.OnValueChanged += GlobalStats_OnNotorietyChanged;
            globalStats.Intimidation.OnValueChanged += GlobalStats_OnIntimidationChanged;

            UpdateStatTextAndColor(healthValueText, globalStats.Health.Value, globalStats.Health.MaxValue);
            UpdateStatTextAndColor(fatigueValueText, globalStats.Fatigue.Value, globalStats.Fatigue.MaxValue, invertLerp: true);
            UpdateStatTextAndColor(hungerValueText, globalStats.Hunger.Value, globalStats.Hunger.MaxValue, invertLerp: true);
            UpdateStatTextAndColor(groupCohesionValueText, globalStats.GroupCohesion.Value, globalStats.GroupCohesion.MaxValue);
            UpdateStatText(notorietyValueText, globalStats.Notoriety.Value, globalStats.Notoriety.MaxValue);
            UpdateStatText(intimidationValueText, globalStats.Intimidation.Value, globalStats.Intimidation.MaxValue);
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
        
        private void UpdateStatText(TextMeshProUGUI text, int value, int maxValue) {
            text.text = $"{value} / {maxValue}";
        }

        private void UpdateStatTextAndColor(TextMeshProUGUI text, int value, int maxValue, bool invertLerp = false) {
            text.text = $"{value} / {maxValue}";
            
            var gradientValue = Mathf.Clamp01((float)value / maxValue);
            if (invertLerp) gradientValue = 1 - gradientValue;
            
            text.color = statColorGradient.Evaluate(gradientValue);
        }

        private void GlobalStats_OnHealthChanged(int health) =>
            UpdateStatTextAndColor(healthValueText, health, GlobalStatsManager.Instance.GlobalStats.Health.MaxValue);

        private void GlobalStats_OnFatigueChanged(int fatigue) =>
            UpdateStatTextAndColor(fatigueValueText, fatigue, GlobalStatsManager.Instance.GlobalStats.Fatigue.MaxValue);

        private void GlobalStats_OnHungerChanged(int hunger) =>
            UpdateStatTextAndColor(hungerValueText, hunger, GlobalStatsManager.Instance.GlobalStats.Hunger.MaxValue);

        private void GlobalStats_OnGroupCohesionChanged(int groupCohesion) =>
            UpdateStatTextAndColor(groupCohesionValueText, groupCohesion, GlobalStatsManager.Instance.GlobalStats.GroupCohesion.MaxValue);

        private void GlobalStats_OnNotorietyChanged(int notoriety) =>
            UpdateStatText(notorietyValueText, notoriety, GlobalStatsManager.Instance.GlobalStats.Notoriety.MaxValue);

        private void GlobalStats_OnIntimidationChanged(int intimidation) =>
            UpdateStatText(intimidationValueText, intimidation, GlobalStatsManager.Instance.GlobalStats.Intimidation.MaxValue);
    }
}

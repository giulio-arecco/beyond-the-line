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

            UpdateStatDisplayedValueAndColor(healthValueText, globalStats.Health.Value, globalStats.Health.MaxValue);
            UpdateStatDisplayedValueAndColor(fatigueValueText, globalStats.Fatigue.Value, globalStats.Fatigue.MaxValue, invertLerp: true);
            UpdateStatDisplayedValueAndColor(hungerValueText, globalStats.Hunger.Value, globalStats.Hunger.MaxValue, invertLerp: true);
            SetStatText(groupCohesionValueText, "-");
            UpdateStatDisplayedValue(notorietyValueText, globalStats.Notoriety.Value, globalStats.Notoriety.MaxValue);
            UpdateStatDisplayedValue(intimidationValueText, globalStats.Intimidation.Value, globalStats.Intimidation.MaxValue);
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

        private void SetStatText(TextMeshProUGUI textField, string text) {
            textField.text = text;
        }
        
        private void UpdateStatDisplayedValue(TextMeshProUGUI textField, float value, float maxValue) {
            textField.text = $"{value} / {maxValue}";
        }

        private void UpdateStatDisplayedValueAndColor(TextMeshProUGUI textField, float value, float maxValue, bool invertLerp = false) {
            textField.text = $"{value} / {maxValue}";
            
            var gradientValue = Mathf.Clamp01(value / maxValue);
            if (invertLerp) gradientValue = 1 - gradientValue;
            
            textField.color = statColorGradient.Evaluate(gradientValue);
        }

        private void GlobalStats_OnHealthChanged(int health) =>
            UpdateStatDisplayedValueAndColor(healthValueText, health, GlobalStatsManager.Instance.GlobalStats.Health.MaxValue);

        private void GlobalStats_OnFatigueChanged(int fatigue) =>
            UpdateStatDisplayedValueAndColor(fatigueValueText, fatigue, GlobalStatsManager.Instance.GlobalStats.Fatigue.MaxValue);

        private void GlobalStats_OnHungerChanged(int hunger) =>
            UpdateStatDisplayedValueAndColor(hungerValueText, hunger, GlobalStatsManager.Instance.GlobalStats.Hunger.MaxValue);

        private void GlobalStats_OnGroupCohesionChanged(int groupCohesion) =>
            UpdateStatDisplayedValueAndColor(groupCohesionValueText, groupCohesion, GlobalStatsManager.Instance.GlobalStats.GroupCohesion.MaxValue);

        private void GlobalStats_OnNotorietyChanged(int notoriety) =>
            UpdateStatDisplayedValue(notorietyValueText, notoriety, GlobalStatsManager.Instance.GlobalStats.Notoriety.MaxValue);

        private void GlobalStats_OnIntimidationChanged(int intimidation) =>
            UpdateStatDisplayedValue(intimidationValueText, intimidation, GlobalStatsManager.Instance.GlobalStats.Intimidation.MaxValue);
    }
}

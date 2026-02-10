using UnityEngine;

namespace UI.GlobalStats {
    public class UIStatsHandler : MonoBehaviour {
        [Header("Stats Text")]
        [SerializeField] private UIStat healthEntry;
        [SerializeField] private UIStat fatigueEntry;
        // [SerializeField] private TextMeshProUGUI hungerValueText;
        // [SerializeField] private TextMeshProUGUI groupCohesionValueText;
        // [SerializeField] private TextMeshProUGUI notorietyValueText;
        // [SerializeField] private TextMeshProUGUI intimidationValueText;

        [Header("Settings")] 
        [SerializeField] private bool animateStatChange = true;
        [SerializeField] private Gradient statColorGradient;
        
        private void Start() {
            var globalStats = GlobalStatsManager.Instance.GlobalStats;
            
            globalStats.Health.OnValueChanged += GlobalStats_OnHealthChanged;
            globalStats.Fatigue.OnValueChanged += GlobalStats_OnFatigueChanged;
            // globalStats.Hunger.OnValueChanged += GlobalStats_OnHungerChanged;
            // globalStats.Cohesion.OnValueChanged += GlobalStats_OnGroupCohesionChanged;
            // globalStats.Notoriety.OnValueChanged += GlobalStats_OnNotorietyChanged;
            // globalStats.Intimidation.OnValueChanged += GlobalStats_OnIntimidationChanged;

            InitStat(healthEntry, globalStats.Health.Value, globalStats.Health.MaxValue);
            InitStat(fatigueEntry, globalStats.Fatigue.Value, globalStats.Fatigue.MaxValue, invertLerp: true);
            // UpdateStatUI(hungerValueText, globalStats.Hunger.Value, globalStats.Hunger.MaxValue, invertLerp: true);
            // SetStatText(groupCohesionValueText, "-");
            // UpdateStatDisplayedValue(notorietyValueText, globalStats.Notoriety.Value, globalStats.Notoriety.MaxValue);
            // UpdateStatDisplayedValue(intimidationValueText, globalStats.Intimidation.Value, globalStats.Intimidation.MaxValue);
        }

        private void OnDestroy() {
            if (GlobalStatsManager.TryGetInstance(out var globalStatsManager)) {
                var globalStats = globalStatsManager.GlobalStats;
                
                globalStats.Health.OnValueChanged -= GlobalStats_OnHealthChanged;
                globalStats.Fatigue.OnValueChanged -= GlobalStats_OnFatigueChanged;
                // globalStats.Hunger.OnValueChanged -= GlobalStats_OnHungerChanged;
                // globalStats.Cohesion.OnValueChanged -= GlobalStats_OnGroupCohesionChanged;
                // globalStats.Notoriety.OnValueChanged -= GlobalStats_OnNotorietyChanged;
                // globalStats.Intimidation.OnValueChanged -= GlobalStats_OnIntimidationChanged;
            }
        }
        
        private Color GetGradientColor(float value, float maxValue, bool invertLerp) {
            var gradientValue = Mathf.Clamp01(value / maxValue);
            if (invertLerp) gradientValue = 1 - gradientValue;
            return statColorGradient.Evaluate(gradientValue);
        }

        private void InitStat(UIStat entry, int val, int max, bool invertLerp = false) {
            var color = GetGradientColor(val, max, invertLerp);
            entry.UpdateText(val, max, color);
        }

        private void GlobalStats_OnHealthChanged(int oldHealth, int newHealth) {
            var max = GlobalStatsManager.Instance.GlobalStats.Health.MaxValue;
            var targetColor = GetGradientColor(newHealth, max, false); 
            
            if (animateStatChange) healthEntry.UpdateTextAnimated(oldHealth, newHealth, max, targetColor, newHealth > oldHealth);
            else fatigueEntry.UpdateText(newHealth, max, targetColor);
        }
        
        private void GlobalStats_OnFatigueChanged(int oldFatigue, int newFatigue) {
            var max = GlobalStatsManager.Instance.GlobalStats.Fatigue.MaxValue;
            var targetColor = GetGradientColor(newFatigue, max, true);
            
            if (animateStatChange) fatigueEntry.UpdateTextAnimated(oldFatigue, newFatigue, max, targetColor, newFatigue < oldFatigue);
            else fatigueEntry.UpdateText(newFatigue, max, targetColor);
        }

        // private void GlobalStats_OnHungerChanged(int hunger) =>
        //     UpdateStatUI(hungerValueText, hunger, GlobalStatsManager.Instance.GlobalStats.Hunger.MaxValue);
        //
        // private void GlobalStats_OnGroupCohesionChanged(int groupCohesion) =>
        //     UpdateStatUI(groupCohesionValueText, groupCohesion, GlobalStatsManager.Instance.GlobalStats.Cohesion.MaxValue);
        //
        // private void GlobalStats_OnNotorietyChanged(int notoriety) =>
        //     UpdateStatDisplayedValue(notorietyValueText, notoriety, GlobalStatsManager.Instance.GlobalStats.Notoriety.MaxValue);
        //
        // private void GlobalStats_OnIntimidationChanged(int intimidation) =>
        //     UpdateStatDisplayedValue(intimidationValueText, intimidation, GlobalStatsManager.Instance.GlobalStats.Intimidation.MaxValue);
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.GlobalStats {
    public class UIStat : MonoBehaviour {
        [Header("Text References")]
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI valueText;

        [Header("Animation Settings")] 
        [SerializeField] private float animDuration = 0.5f;
        [SerializeField] private Vector3 punchScale = new(0.3f, 0.3f, 0);
        [FormerlySerializedAs("increaseColor")] [SerializeField] private Color goodFlashColor = Color.green;
        [FormerlySerializedAs("decreaseColor")] [SerializeField] private Color badFlashColor = Color.red;

        private Tween _currentTween;

        public void UpdateText(int newVal, int maxVal, Color newColor) {
            valueText.text = $"{newVal} / {maxVal}";
            valueText.color = newColor;
        }
        
        public void UpdateTextAnimated(int oldVal, int newVal, int maxVal, bool isPositiveEvent) {
            if (oldVal == newVal) return;

            var flashColor = isPositiveEvent ? goodFlashColor : badFlashColor;
            var labelColor = labelText.color;

            if (_currentTween != null && _currentTween.IsActive()) _currentTween.Kill(true);
            
            // State reset
            transform.localScale = Vector3.one;

            var seq = DOTween.Sequence();

            // Punch Scale
            seq.Join(transform.DOPunchScale(punchScale, animDuration, 10, 1));

            // Rolling Number
            seq.Join(DOTween.To(() => oldVal, x => { valueText.text = $"{x} / {maxVal}"; }, newVal, animDuration));

            // Color Flash
            seq.Join(labelText.DOColor(flashColor, 0.1f)); 
            
            // Back to original color
            seq.Append(labelText.DOColor(labelColor, animDuration - 0.1f));

            seq.Play();
            _currentTween = seq;
        }
    }
}
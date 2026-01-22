using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Button))]
    public class UIButtonTextVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
        [Header("Settings")]
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private Color pressedColor;
        
        private Color _normalColor;
        
        private void Start() {
            _normalColor = targetText.color;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            targetText.color = highlightedColor;
        }

        public void OnPointerExit(PointerEventData eventData) {
            targetText.color = _normalColor;
        }

        public void OnPointerDown(PointerEventData eventData) {
            targetText.color = pressedColor;
        }

        public void OnPointerUp(PointerEventData eventData) {
            targetText.color = highlightedColor;
        }
    }
}
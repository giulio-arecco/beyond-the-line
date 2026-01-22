using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class UIButtonSpriteSwapper : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, 
        IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler 
    {
        [Header("Target")]
        [SerializeField] private Image targetImage;

        [Header("Sprites")]
        [SerializeField] private Sprite highlightedSprite;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite pressedSprite;

        private Sprite _normalSprite;
        private bool _isHovered;
        private bool _isPressed;
        private bool _isSelected;

        private void Start() {
            _normalSprite = targetImage.sprite;
        }

        private void OnDisable() {
            _isHovered = false;
            _isPressed = false;
            _isSelected = false;
            targetImage.sprite = _normalSprite;
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            _isHovered = true;
            if (highlightedSprite) UpdateVisuals();
        }

        public void OnPointerExit(PointerEventData eventData) {
            _isHovered = false;
            if (highlightedSprite) UpdateVisuals();
        }

        public void OnPointerDown(PointerEventData eventData) {
            _isPressed = true;
            if (pressedSprite) UpdateVisuals();
        }

        public void OnPointerUp(PointerEventData eventData) {
            _isPressed = false;
            if (pressedSprite) UpdateVisuals();
        }

        public void OnSelect(BaseEventData eventData) {
            _isSelected = true;
            if (selectedSprite) UpdateVisuals();
        }

        public void OnDeselect(BaseEventData eventData) {
            _isSelected = false;
            if (selectedSprite) UpdateVisuals();
        }
    
        private void UpdateVisuals() {
            if (_isSelected) {
                targetImage.sprite = selectedSprite;
            }
            else if (_isPressed) {
                targetImage.sprite = pressedSprite;
            }
            else if (_isHovered) {
                targetImage.sprite = highlightedSprite;
            }
            else {
                targetImage.sprite = _normalSprite;
            }
        }
    }
}
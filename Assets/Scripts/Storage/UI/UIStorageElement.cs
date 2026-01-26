using Storage.Storables;
using UI;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Storage.UI {
    public class UIStorageElement : MonoBehaviour {
        public Storable Storable { get; private set; }
        
        private Image _image;
        private AspectRatioFitter _fitter;
        private Outline _outline;
        private UIButtonNavigationEvents _buttonNavigationEvents;
        
        private void Awake() {
            _image = GetComponent<Image>();
            _fitter = GetComponent<AspectRatioFitter>();
            _outline = GetComponent<Outline>();
        }
        
        private void OnDestroy() {
            _buttonNavigationEvents?.onSelectEnter.RemoveListener(EnableOutline);
            _buttonNavigationEvents?.onSelectExit.RemoveListener(DisableOutline);
            _buttonNavigationEvents = null;
        }
        
        private void EnableOutline() => _outline.enabled = true;
        private void DisableOutline() => _outline.enabled = false;
        
        public void InitAndAddToSlot(Storable newElement, UIStorageSlot slot) {
            Storable = newElement;
        
            var sprite = Storable.Info.sprite;
            
            transform.SetParent(slot.transform);
            transform.localPosition = slot.ElementSpriteAnchorDeltaPixels;
            transform.SetSiblingIndex(slot.ChildButton.transform.GetSiblingIndex());
            
            _image.sprite = sprite;
            _image.rectTransform.localScale = slot.ElementSpriteLocalScale;
            
            _outline.enabled = false;
            if (slot.ChildButton.TryGetComponent(out _buttonNavigationEvents)) {
                _buttonNavigationEvents.onSelectEnter.AddListener(EnableOutline);
                _buttonNavigationEvents.onSelectExit.AddListener(DisableOutline);
            }
            else {
                Debug.LogError("[UIStorageElement] UIButtonNavigationEvents Component not found on the provided UIStorageSlot reference.");
            }
            
            // Fit the grid cell size and aspect ratio
            _fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            _fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

            slot.ChildElement = this;
        }
    }
}

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
        private UIButtonStateController _buttonStateController;
        
        private void Awake() {
            _image = GetComponent<Image>();
            _fitter = GetComponent<AspectRatioFitter>();
            _outline = GetComponent<Outline>();
        }
        
        private void OnDestroy() {
            _buttonStateController?.onSelectEnter.RemoveListener(EnableOutline);
            _buttonStateController?.onSelectExit.RemoveListener(DisableOutline);
            _buttonStateController = null;
        }
        
        private void EnableOutline() => _outline.enabled = true;
        private void DisableOutline() => _outline.enabled = false;
        
        public void InitAndAddToSlot(Storable newElement, UIStorageSlot slot) {
            Storable = newElement;
        
            var sprite = Storable.Info.sprite;
            
            transform.SetParent(slot.transform);
            transform.localPosition = slot.ElementSpriteAnchorDeltaPixels;
            transform.SetSiblingIndex(slot.ChildButtonController.transform.GetSiblingIndex());
            
            _image.sprite = sprite;
            _image.rectTransform.localScale = slot.ElementSpriteLocalScale;
            
            _outline.enabled = false;
            _buttonStateController = slot.ChildButtonController;
            _buttonStateController.onSelectEnter.AddListener(EnableOutline);
            _buttonStateController.onSelectExit.AddListener(DisableOutline);
            
            // Fit the grid cell size and aspect ratio
            _fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            _fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

            slot.ChildElement = this;
        }
    }
}

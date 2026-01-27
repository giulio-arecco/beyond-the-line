using Storage.Storables;
using UI;
using UltEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Storage.UI {
    public class UIStorageElement : MonoBehaviour {
        public Storable Storable { get; private set; }
        
        [SerializeField] private Image image;
        [SerializeField] private Image border;
        
        private AspectRatioFitter _fitter;
        private UIButtonStateController _buttonStateController;

        private void Awake() {
            _fitter = GetComponent<AspectRatioFitter>();
        }
        
        private void OnDestroy() {
            _buttonStateController?.onSelectEnter.RemoveListener(EnableBorder);
            _buttonStateController?.onSelectExit.RemoveListener(DisableBorder);
            _buttonStateController = null;
        }
        
        private void EnableBorder() => border.gameObject.SetActive(true);
        private void DisableBorder() => border.gameObject.SetActive(false);

        public void InitAndAddToSlot(Storable newElement, UIStorageSlot slot) {
            Storable = newElement;

            var sprite = Storable.Info.sprite;

            transform.SetParent(slot.transform);
            transform.localPosition = slot.ElementAnchorOffsetPixels;
            transform.SetSiblingIndex(slot.ChildButtonController.transform.GetSiblingIndex());

            image.sprite = sprite;
            image.rectTransform.localScale = slot.ElementSpriteLocalScale;
            
            border.rectTransform.localScale = slot.ElementBorderLocalScale;
            border.gameObject.SetActive(false);
            _buttonStateController = slot.ChildButtonController;
            _buttonStateController.onSelectEnter.AddListener(EnableBorder);
            _buttonStateController.onSelectExit.AddListener(DisableBorder);
            
            // Fit the grid cell size and aspect ratio
            _fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            _fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

            slot.ChildElement = this;
        }
    }
}

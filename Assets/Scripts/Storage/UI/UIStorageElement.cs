using System;
using Storage.Storables;
using Storage.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI {
    public class UIStorageElement : MonoBehaviour {
        public Storable Storable { get; private set; }
        
        private Image _image;
        private AspectRatioFitter _fitter;

        private void Awake() {
            _image = GetComponent<Image>();
            _fitter = GetComponent<AspectRatioFitter>();
        }

        public void InitAndAddToSlot(Storable newElement, UIStorageSlot slot) {
            Storable = newElement;
        
            var sprite = Storable.Info.sprite;
            
            transform.SetParent(slot.transform);
            transform.SetSiblingIndex(slot.ChildButton.transform.GetSiblingIndex());
        
            _image.sprite = sprite;
            _image.rectTransform.localScale = slot.ElementSpriteLocalScale;
            
            // Fit the grid cell size and aspect ratio
            _fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            _fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

            slot.ChildElement = this;
        }
    }
}

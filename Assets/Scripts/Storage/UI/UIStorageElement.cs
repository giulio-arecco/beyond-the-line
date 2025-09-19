using Storage.Storables;
using Storage.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI {
    public class UIStorageElement : MonoBehaviour {
        public Storable Storable { get; private set; }

        public void InitAndAddToSlot(Storable newElement, UIStorageSlot slot) {
            Storable = newElement;
        
            var image = gameObject.AddComponent<Image>();
            var fitter = gameObject.AddComponent<AspectRatioFitter>();
            var sprite = Storable.Info.sprite;
            
            transform.SetParent(slot.transform);
            transform.SetSiblingIndex(slot.ChildButton.transform.GetSiblingIndex());
        
            image.sprite = sprite;
            image.rectTransform.localScale = slot.ElementSpriteLocalScale;
            
            // Fit the grid cell size and aspect ratio
            fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

            slot.ChildElement = this;
        }
    }
}

using Storage.Storables;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI {
    public class UIStorageElement : MonoBehaviour {
        public Storable Element { get; private set; }

        public void InitAndAddToSlot(Storable newElement, UIStorageSlot slot) {
            Element = newElement;
        
            var image = gameObject.AddComponent<Image>();
            var fitter = gameObject.AddComponent<AspectRatioFitter>();
            var sprite = Element.Info.sprite;
            transform.SetParent(slot.transform);
        
            image.sprite = sprite;
            image.rectTransform.localScale = slot.ElementSpriteLocalScale;
            
            // Fit the grid cell size and aspect ratio
            fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

            slot.ChildElement = this;
        }
    }
}

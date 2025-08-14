using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour {
    public Item Item { get; private set; }

    public void InitAndAddToSlot(Item newItem, UIInventorySlot slot) {
        Item = newItem;
        
        var image = gameObject.AddComponent<Image>();
        var fitter = gameObject.AddComponent<AspectRatioFitter>();
        var sprite = Item.itemData.sprite;
        transform.SetParent(slot.transform);
        
        image.sprite = sprite;
        image.rectTransform.localScale = slot.ItemSpriteLocalScale;
            
        // Fit the grid cell size and aspect ratio
        fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        fitter.aspectRatio = sprite.rect.width / sprite.rect.height;

        slot.ChildItem = this;
    }
}

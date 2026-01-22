using Inventory.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Storage.UI {
    public class UIStorageSlot : MonoBehaviour {
        [SerializeField] private Vector2 elementSpriteLocalScale = new(0.9f, 0.9f);
        [SerializeField] private Vector2 elementSpriteAnchorDeltaPixels;
        [field: SerializeField] public Button ChildButton { get; private set; }
    
        public Vector2 ElementSpriteLocalScale { get => elementSpriteLocalScale; private set => elementSpriteLocalScale = value; }
        public Vector2 ElementSpriteAnchorDeltaPixels { get => elementSpriteAnchorDeltaPixels; private set => elementSpriteAnchorDeltaPixels = value; }
        public UIStorageElement ChildElement { get; set; }
    }
}

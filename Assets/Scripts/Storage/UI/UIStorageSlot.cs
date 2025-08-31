using UnityEngine;

namespace Inventory.UI {
    public class UIStorageSlot : MonoBehaviour {
        [SerializeField] private Vector2 elementSpriteLocalScale = new(0.9f, 0.9f);
    
        public Vector2 ElementSpriteLocalScale { get => elementSpriteLocalScale; private set => elementSpriteLocalScale = value; }
        public UIStorageElement ChildElement { get; set; }
    }
}

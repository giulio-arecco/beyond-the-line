using Inventory.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Storage.UI {
    public class UIStorageSlot : MonoBehaviour {
        [SerializeField] private Vector2 elementSpriteLocalScale = new(0.9f, 0.9f);
        [field: SerializeField] public Button ChildButton { get; private set; }
    
        public Vector2 ElementSpriteLocalScale { get => elementSpriteLocalScale; private set => elementSpriteLocalScale = value; }
        public UIStorageElement ChildElement { get; set; }
    }
}

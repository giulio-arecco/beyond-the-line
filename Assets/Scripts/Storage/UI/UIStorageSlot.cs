using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Storage.UI {
    public class UIStorageSlot : MonoBehaviour {
        [field: SerializeField] public UIButtonStateController ChildButtonController { get; private set; }

        [field: SerializeField] public Vector2 ElementSpriteLocalScale { get; private set; } = new(0.9f, 0.9f);
        [field: SerializeField] public Vector2 ElementSpriteAnchorDeltaPixels { get; private set; }
        public UIStorageElement ChildElement { get; set; }
    }
}

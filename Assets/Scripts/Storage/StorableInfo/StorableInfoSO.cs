using UnityEngine;

namespace Inventory.StorableInfo {
    public abstract class StorableInfoSO : ScriptableObject {
        public Sprite sprite;
        public string id;
        public string entityName;
        public string description;
    }
}

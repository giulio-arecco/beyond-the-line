using UnityEngine;

namespace Storage.StorableInfo {
    public abstract class StorableInfoSO : ScriptableObject {
        public Sprite sprite;
        public string id;
        public string entityName;
        [TextArea] public string description;
    }
}

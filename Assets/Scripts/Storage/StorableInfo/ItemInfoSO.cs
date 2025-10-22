using UnityEngine;

namespace Storage.StorableInfo {
   
    [System.Serializable]
    public struct IntStatModifier {
        public string statName;
        public int statValue;
    }
    
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Objects/ItemInfo")]
    public class ItemInfoSO : StorableInfoSO {
        [field: SerializeField] public IntStatModifier[] StatsModifiers { get; private set; }
    }
}

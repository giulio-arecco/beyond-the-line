using System;
using UnityEngine;
using Utils.Extensions;

namespace Storage.StorableInfo {
   
    [Serializable]
    public struct IntStatModifier {
        public string displayName;
        public string statName;
        public int statValue;
    }
    
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Objects/ItemInfo")]
    public class ItemInfoSO : StorableInfoSO {
        [field: SerializeField] public IntStatModifier[] StatsModifiers { get; private set; }
        
        private void OnEnable() {
            if (!StatsModifiers.IsNullOrEmpty()) 
                Array.Sort(StatsModifiers, (x, y) => string.Compare(x.statName, y.statName, StringComparison.OrdinalIgnoreCase));
        }
    }
}

using Storage.Storables;
using UnityEngine;

namespace Storage.StorableInfo {
    [CreateAssetMenu(fileName = "CompanionInfo", menuName = "Scriptable Objects/Storage/StorableInfo/CompanionInfo", order = 1)]
    public class CompanionInfoSO : StorableInfoSO {
        [field: SerializeField] public Item[] Items { get; private set; }
    }
}

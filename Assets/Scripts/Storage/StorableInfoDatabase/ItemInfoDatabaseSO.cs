using Storage.StorableInfo;
using UnityEngine;

namespace Storage.StorableInfoDatabase {
    [CreateAssetMenu(fileName = "ItemInfoDatabase", menuName = "Scriptable Objects/Storage/StorableInfoDatabase/ItemInfoDatabase", order = 0)]
    public class ItemInfoDatabaseSO : StorableInfoDatabaseSO<ItemInfoSO> {}
}
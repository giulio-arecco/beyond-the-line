using Inventory.StorableInfo;
using UnityEngine;

namespace Storage.StorableInfoDatabase {
    [CreateAssetMenu(fileName = "ItemInfoDatabase", menuName = "Scriptable Objects/StorableInfoDatabase/ItemInfoDatabase")]
    public class ItemInfoDatabaseSO : StorableInfoDatabaseSO<ItemInfoSO> {}
}
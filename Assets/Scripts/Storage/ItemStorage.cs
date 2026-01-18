using Storage.StorableInfo;
using Storage.Storables;
using UnityEngine;

namespace Storage {
    public class ItemStorage : StorageBase<Item> {
        [SerializeField] private ItemInfoSO[] startingItems;

        private void Awake() {
            foreach (var itemInfoSO in startingItems) {
                var item = new Item(itemInfoSO);
                Add(item);
            }
        }
    }
}

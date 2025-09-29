using System;
using Storage.StorableInfo;

namespace Storage.Storables {
    [Serializable]
    public class Item : Storable<ItemInfoSO> {
        public Item(ItemInfoSO info) {
            TypedInfo = info;
        }
    }
}

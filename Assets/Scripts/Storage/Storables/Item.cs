using Storage.StorableInfo;

namespace Storage.Storables {
    public class Item : Storable {
        public Item(ItemInfoSO info) {
            Info = info;
        }
    }
}

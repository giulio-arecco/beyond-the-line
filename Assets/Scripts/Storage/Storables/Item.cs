using Inventory.StorableInfo;

namespace Inventory.Storables {
    public class Item : Storable {
        public Item(ItemInfoSO info) {
            this.info = info;
        }
    }
}

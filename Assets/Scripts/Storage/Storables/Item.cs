using Inventory.StorableInfo;

namespace Inventory.Storables {
    public class Item : Storable {
        public Item(StorableInfoSO info) {
            this.info = info;
        }
    }
}

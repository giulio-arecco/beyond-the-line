using Inventory.StorableInfo;

namespace Inventory.Storables {
    public class Companion : Storable {
        public int Health { get; private set; } = 100;
        public int Hunger { get; private set; } = 0;

        public Companion(StorableInfoSO info) {
            this.info = info;
        }
    }
}

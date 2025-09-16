using Inventory.Interfaces;
using Storage.StorableInfo;

namespace Storage.Storables {
    public class Companion : Storable<CompanionInfoSO> {
        public int Health { get; private set; } = 100;
        public int Hunger { get; private set; } = 0;

        public Companion(CompanionInfoSO info) {
            TypedInfo = info;
        }

        public void CopyItemsTo(IStorage<Item> storage) {
            foreach (var item in TypedInfo.Items) {
                storage.Add(item);
            }
        }
    }
}

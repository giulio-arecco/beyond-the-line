using Storage.StorableInfo;

namespace Storage.Storables {
    public class Companion : Storable {
        public int Health { get; private set; } = 100;
        public int Hunger { get; private set; } = 0;

        public Companion(CompanionInfoSO info) {
            Info = info;
        }
    }
}

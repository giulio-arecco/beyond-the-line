using Storage.StorableInfo;
using Storage.Storables;
using UnityEngine;

namespace Storage {
    public class CompanionStorage : StorageBase<Companion> {
        [SerializeField] private CompanionInfoSO[] startingCompanions;

        private void Awake() {
            foreach (var companionInfoSO in startingCompanions) {
                var item = new Companion(companionInfoSO);
                Add(item);
            }
        }
    }
}


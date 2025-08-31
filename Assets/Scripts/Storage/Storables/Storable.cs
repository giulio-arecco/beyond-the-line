using System;
using Inventory.StorableInfo;
using UnityEngine;

namespace Inventory.Storables {
    [Serializable]
    public abstract class Storable {
        [SerializeField] protected StorableInfoSO info;
        public StorableInfoSO Info => info;
    }
}
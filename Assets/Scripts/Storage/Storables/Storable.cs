using System;
using Storage.StorableInfo;
using UnityEngine;

namespace Storage.Storables {
    [Serializable]
    public abstract class Storable {
        [field: SerializeField] public StorableInfoSO Info { get; protected set; }
    }
}
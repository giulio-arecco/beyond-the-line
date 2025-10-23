using System;
using Storage.StorableInfo;
using UnityEngine;

namespace Storage.Storables {
    [Serializable]
    public abstract class Storable: IComparable<Storable> {
        public abstract StorableInfoSO Info { get; }

        public int CompareTo(Storable other) {
            return string.Compare(Info.id, other.Info.id, StringComparison.OrdinalIgnoreCase);
        }
    }
    
    [Serializable]
    public abstract class Storable<TInfo> : Storable where TInfo : StorableInfoSO {
        [field: SerializeField] public TInfo TypedInfo { get; protected set; }
        
        public override StorableInfoSO Info => TypedInfo;
    }
}
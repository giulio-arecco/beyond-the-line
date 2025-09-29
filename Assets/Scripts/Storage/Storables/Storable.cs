using System;
using Storage.StorableInfo;
using UnityEngine;

namespace Storage.Storables {
    [Serializable]
    public abstract class Storable {
        public abstract StorableInfoSO Info { get; }
    }
    
    [Serializable]
    public abstract class Storable<TInfo> : Storable where TInfo : StorableInfoSO {
        [field: SerializeField] public TInfo TypedInfo { get; protected set; }
        
        public override StorableInfoSO Info => TypedInfo;
    }
}
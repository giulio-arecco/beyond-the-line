using System;
using System.Collections.Generic;
using Inventory.Storables;

namespace Inventory.Interfaces {
    public interface IStorage {
        IReadOnlyList<Storable> Elements { get; }

        public event Action<Storable> OnAdd;
        public event Action<string> OnRemove;
    
        public void Add(Storable element);
        public void Remove(string id);
        public bool Has(string id);
        public Storable GetElement(string id);
        public Storable[] GetElements();
    }

    public interface IStorage<out T> : IStorage where T : Storable {
        public IReadOnlyList<T> TypedElements { get; }
        public T GetTypedElement(string id);
        public T[] GetTypedElements();
    }
}
using System;
using System.Collections.Generic;
using Storage.Storables;

namespace Inventory.Interfaces {
    public interface IStorage {
        public event Action<Storable> OnAdd;
        public event Action<Storable> OnRemove;
    
        public void Add(Storable element);
        public void Remove(string id);
        public void RemoveMany(string id, int count);
        public bool Has(string id);
        public bool IsEmpty();
        public Storable GetElement(string id);
        public IReadOnlyList<Storable> GetElements();
    }

    public interface IStorage<out T> : IStorage where T : Storable {
        public T GetTypedElement(string id);
        public IReadOnlyList<T> GetTypedElements();
    }
}
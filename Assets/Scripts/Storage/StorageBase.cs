using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Interfaces;
using Storage.Storables;
using UnityEngine;

namespace Storage {
    public abstract class StorageBase<T>: MonoBehaviour, IStorage<T> where T : Storable {
        private readonly List<T> _elements = new();
    
        public IReadOnlyList<Storable> Elements => _elements;
        public IReadOnlyList<T> TypedElements => _elements;

        public event Action<Storable> OnAdd;
        public event Action<Storable> OnRemove;

        public void Add(Storable element) {
            if (element.GetType() != typeof(T)) {
                throw new ArgumentException("Storable is not of type " + typeof(T));
            }
        
            var castedElement = element as T;
        
            _elements.Add(castedElement);
            _elements.Sort();
            
            OnAdd?.Invoke(castedElement);
        }

        public void Remove(string id) {
            var elementToRemove = _elements.Find(x => id == x.Info.id);

            if (elementToRemove != null) {
                _elements.Remove(elementToRemove);
                _elements.Sort();
                
                OnRemove?.Invoke(elementToRemove);
            }
        }
    
        public bool Has(string id) {
            var element = _elements.Find(x => x.Info.id == id);
            return element != null;
        }

        public Storable GetElement(string id) => _elements.Find(x => id == x.Info.id);
        public Storable[] GetElements() {
            return _elements.Cast<Storable>().ToArray();
        }
    
        public T GetTypedElement(string id) => _elements.Find(x => x.Info.id == id);
        public T[] GetTypedElements() => _elements.ToArray();
    }
}

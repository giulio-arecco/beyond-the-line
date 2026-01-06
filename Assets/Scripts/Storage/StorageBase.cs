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
            else {
                Debug.LogWarning($"No storable with id '{id}' was found to remove from {this}");
            }
        }

        public void RemoveMany(string id, int count) {
            if (count <= 0) return;
            
            var itemsRemoved = 0;

            for (var i = _elements.Count - 1; i >= 0; i--) {
                if (itemsRemoved >= count) break;

                if (_elements[i].Info.id == id) {
                    var elementToRemove = _elements[i];
                    
                    _elements.RemoveAt(i);
                    OnRemove?.Invoke(elementToRemove);
                    
                    itemsRemoved++;
                }
            }

            if (itemsRemoved > 0) {
                if (itemsRemoved < count) Debug.LogWarning($"Requested to remove {count} storables with id '{id}', but only {itemsRemoved} were found and removed from {this}.");
                _elements.Sort();
            }
            else {
                Debug.LogWarning($"No storables with id '{id}' were found to remove from {this}.");
            }
        }
    
        public bool Has(string id) {
            var element = _elements.Find(x => x.Info.id == id);
            return element != null;
        }
        
        public bool IsEmpty() => _elements.Count == 0;
        public Storable GetElement(string id) => _elements.Find(x => id == x.Info.id);
        public Storable[] GetElements() => _elements.Cast<Storable>().ToArray();
        public T GetTypedElement(string id) => _elements.Find(x => x.Info.id == id);
        public T[] GetTypedElements() => _elements.ToArray();
    }
}

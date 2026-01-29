using System;
using System.Collections.Generic;
using Inventory.Interfaces;
using Storage.Storables;
using UnityEngine;

namespace Storage {
    public abstract class StorageBase<T>: MonoBehaviour, IStorage<T> where T : Storable {
        private readonly List<T> _elements = new();
        private bool _isDirty;
        public event Action<Storable> OnAdd;
        public event Action<Storable> OnRemove;

        private void EnsureSorted() {
            if (!_isDirty) return;
            
            _elements.Sort();
            _isDirty = false;
        }

        public void Add(Storable element) {
            if (element == null) throw new ArgumentNullException(nameof(element));
            
            if (element is not T castedElement) {
                throw new ArgumentException($"Element of type {element.GetType()} is not assignable to {typeof(T)}");
            }
        
            _elements.Add(castedElement);
            _isDirty = true;
            
            OnAdd?.Invoke(castedElement);
        }

        public void Remove(string id) {
            var index = _elements.FindIndex(x => id == x.Info.id);

            if (index != -1) {
                var elementToRemove = _elements[index];
                _elements.RemoveAt(index);
                
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
                if (itemsRemoved < count) Debug.LogWarning($"Requested to remove {count} elements with id '{id}', but only {itemsRemoved} were found and removed from {this}.");
            }
            else {
                Debug.LogWarning($"No elements with id '{id}' were found to remove from {this}.");
            }
        }
    
        public bool Has(string id) => _elements.Exists(x => x.Info.id == id);
        public bool IsEmpty() => _elements.Count == 0;
        public Storable GetElement(string id) => _elements.Find(x => id == x.Info.id);
        public T GetTypedElement(string id) => _elements.Find(x => x.Info.id == id);
        
        public IReadOnlyList<Storable> GetSortedElements() {
            EnsureSorted();
            return _elements;
        }
        
        public IReadOnlyList<T> GetSortedTypedElements() {
            EnsureSorted();
            return _elements;
        }
    }
}

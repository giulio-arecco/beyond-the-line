using System;
using Inventory.Interfaces;
using Inventory.Storables;
using UnityEngine;
using Utils.SerializeInterface;

namespace Inventory.UI {
    public class UIStorage : MonoBehaviour {
        [SerializeField] private InterfaceReference<IStorage> storage;
        [SerializeField] private UIStorageSlot[] storageSlots;
    
        private int _nextAvailableSlot;

        private void OnEnable() {
            RefreshUI();
            storage.Value.OnAdd += Storage_OnAdd;
            storage.Value.OnRemove += Storage_OnRemove;
        }
    
        private void OnDisable() {
            ClearUI();
            storage.Value.OnAdd -= Storage_OnAdd;
            storage.Value.OnRemove -= Storage_OnRemove;
        }

        private void RefreshUI() {
            if (_nextAvailableSlot > 0) {
                Debug.LogError("Next available slot should be 0 when refreshing UI");
            }
        
            var elements = storage.Value.GetElements();
            if (elements == null) return;

            foreach (var element in elements) {
                NewStorageElement(element);
            }
        }

        private void ClearUI() {
            foreach (var slot in storageSlots) {
                if (slot.ChildElement != null) {
                    Destroy(slot.ChildElement.gameObject);
                    slot.ChildElement = null;
                }
            }
        
            _nextAvailableSlot = 0;
        }
    
        private void NewStorageElement(Storable element) {
            if (_nextAvailableSlot >= storageSlots.Length) {
                Debug.LogWarning("Storage UI is full. The element won't be added to the storage UI");
                return;
            }
        
            var slot = storageSlots[_nextAvailableSlot++];

            var elementGo = new GameObject(element.Info.id);
            var elementComponent = elementGo.AddComponent<UIStorageElement>();
            elementComponent.InitAndAddToSlot(element, slot);
        }

        private void ShiftStorageElements() {
            var j = 0;
            for (var i = 0; i < storageSlots.Length; i++) {
                if (storageSlots[i].ChildElement != null) {
                    _nextAvailableSlot = i + 1;
                    continue;
                }
            
                // Find the next non-empty slot after i
                j = Math.Max(j, i + 1);
                while (j < storageSlots.Length && storageSlots[j].ChildElement == null) {
                    j++;
                }

                if (j >= storageSlots.Length) {
                    _nextAvailableSlot = i;
                    break;
                }

                // Move the element
                storageSlots[j].ChildElement.transform.SetParent(storageSlots[i].transform);
                storageSlots[i].ChildElement = storageSlots[j].ChildElement;
                storageSlots[j].ChildElement = null;
                _nextAvailableSlot = i + 1;
            }
        }

        private void Storage_OnAdd(Storable element) {
            NewStorageElement(element);
        }
    
        private void Storage_OnRemove(string id) {
            var hasRemoved = false;
            foreach (var slot in storageSlots) {
                if (slot.ChildElement && slot.ChildElement.Element.Info.id == id) {
                    hasRemoved = true;
                    Destroy(slot.ChildElement.gameObject);
                    slot.ChildElement = null;
                }
            }

            if (hasRemoved) {
                ShiftStorageElements();
            }
        }
    }
}

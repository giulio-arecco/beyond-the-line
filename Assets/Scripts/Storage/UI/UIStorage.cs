using System;
using Inventory.Interfaces;
using Storage.Interfaces;
using Storage.Storables;
using Storage.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Utils.SerializeInterface;

namespace Inventory.UI {
    public class UIStorage : MonoBehaviour {
        [SerializeField] private InterfaceReference<IStorage> storage;
        [SerializeField] private InterfaceReference<IStorableTextWriter> storageTextWriter;
        [SerializeField] private UIStorageSlot[] storageSlots;
        [SerializeField] private UIStorageElement storageElementPrefab;
    
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
            if (EventSystem.current)
                EventSystem.current.SetSelectedGameObject(null);
            else
                Debug.LogWarning("No active EventSystem");

            storageTextWriter.Value?.ClearAllText();

            foreach (var slot in storageSlots) {
                if (slot.ChildElement != null) {
                    slot.ChildButton.onClick.RemoveAllListeners();
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

            var elementComponent = Instantiate(storageElementPrefab);
            elementComponent.InitAndAddToSlot(element, slot);
            
            if (storageTextWriter.Value != null) {
                slot.ChildButton.onClick.AddListener(() => storageTextWriter.Value.SetNameText(elementComponent.Storable));
                slot.ChildButton.onClick.AddListener(() => storageTextWriter.Value.SetDescriptionText(elementComponent.Storable));
                slot.ChildButton.onClick.AddListener(() => storageTextWriter.Value.SetOtherText(elementComponent.Storable));
                slot.ChildButton.enabled = true;
            }
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
    
        private void Storage_OnRemove(Storable element) {
            var hasRemoved = false;
            foreach (var slot in storageSlots) {
                if (slot.ChildElement && slot.ChildElement.Storable.Info.id == element.Info.id) {
                    hasRemoved = true;
                    Destroy(slot.ChildElement.gameObject);
                    slot.ChildElement = null;
                    
                    if (EventSystem.current.currentSelectedGameObject == slot.ChildButton.gameObject) 
                        EventSystem.current.SetSelectedGameObject(null);
                    
                    storageTextWriter.Value?.ClearAllText();
                    slot.ChildButton.onClick.RemoveAllListeners();
                    slot.ChildButton.enabled = false;
                }
            }

            if (hasRemoved) {
                ShiftStorageElements();
            }
        }
    }
}

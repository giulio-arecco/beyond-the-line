using System;
using UnityEngine;

public class UIInventory : MonoBehaviour {
    [SerializeField] private Inventory inventory;
    [SerializeField] private UIInventorySlot[] inventorySlots;
    private int _nextAvailableSlot;

    private void OnEnable() {
        RefreshUI();
        inventory.OnItemAdded += Inventory_OnItemAdded;
        inventory.OnItemRemoved += Inventory_OnItemRemoved;
    }
    
    private void OnDisable() {
        ClearUI();
        inventory.OnItemAdded -= Inventory_OnItemAdded;
        inventory.OnItemRemoved -= Inventory_OnItemRemoved;
    }

    private void RefreshUI() {
        if (_nextAvailableSlot > 0) {
            Debug.LogError("Next available slot should be 0 when refreshing UI");
        }
        
        var items = inventory.GetItems();
        if (items == null) return;

        foreach (var item in items) {
            NewInventoryItem(item);
        }
    }

    private void ClearUI() {
        foreach (var slot in inventorySlots) {
            if (slot.ChildItem != null) {
                Destroy(slot.ChildItem.gameObject);
                slot.ChildItem = null;
            }
        }
        
        _nextAvailableSlot = 0;
    }
    
    private void NewInventoryItem(Item item) {
        if (_nextAvailableSlot >= inventorySlots.Length) {
            Debug.LogWarning("Inventory UI is full. The item won't be added to the inventory UI");
            return;
        }
        
        var slot = inventorySlots[_nextAvailableSlot++];

        var itemGo = new GameObject(item.itemData.id);
        var itemComponent = itemGo.AddComponent<UIInventoryItem>();
        itemComponent.InitAndAddToSlot(item, slot);
    }

    private void ShiftInventoryItems() {
        var j = 0;
        for (var i = 0; i < inventorySlots.Length; i++) {
            if (inventorySlots[i].ChildItem != null) {
                _nextAvailableSlot = i + 1;
                continue;
            }
            
            // Find the next non-empty slot after i
            j = Math.Max(j, i + 1);
            while (j < inventorySlots.Length && inventorySlots[j].ChildItem == null) {
                j++;
            }

            if (j >= inventorySlots.Length) {
                _nextAvailableSlot = i;
                break;
            }

            // Move the item
            inventorySlots[j].ChildItem.transform.SetParent(inventorySlots[i].transform);
            inventorySlots[i].ChildItem = inventorySlots[j].ChildItem;
            inventorySlots[j].ChildItem = null;
            _nextAvailableSlot = i + 1;
        }
    }

    private void Inventory_OnItemAdded(Item item) {
        NewInventoryItem(item);
    }
    
    private void Inventory_OnItemRemoved(string itemId) {
        var hasRemoved = false;
        foreach (var slot in inventorySlots) {
            if (slot.ChildItem && slot.ChildItem.Item.itemData.id == itemId) {
                hasRemoved = true;
                Destroy(slot.ChildItem.gameObject);
                slot.ChildItem = null;
            }
        }

        if (hasRemoved) {
            ShiftInventoryItems();
        }
    }
}

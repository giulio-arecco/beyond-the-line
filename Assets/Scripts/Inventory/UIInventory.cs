using System;
using UnityEngine;

public class UIInventory : MonoBehaviour {
    [SerializeField] private UIInventorySlot[] inventorySlots;
    private int _nextAvailableSlot;
    
    private void Start() {
        InitItemsUI();
        Player.Instance.Inventory.OnItemAdded += Inventory_OnItemAdded;
        Player.Instance.Inventory.OnItemRemoved += Inventory_OnItemRemoved;
    }

    private void OnDisable() {
        // TODO investigate null ref here 
        Player.TryGetInstance().Inventory.OnItemAdded -= Inventory_OnItemAdded;
        Player.TryGetInstance().Inventory.OnItemRemoved -= Inventory_OnItemRemoved;
    }

    private void InitItemsUI() {
        if (_nextAvailableSlot > 0)
            Debug.LogWarning("Initializing the inventory UI when it's not empty. " +
                             "This method should only be called once to add the inventory items to the inventory UI");
        
        var items = Player.Instance.Inventory.GetItems();
        if (items == null) return;

        foreach (var item in items) {
            NewInventoryItem(item);
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
    
    private void NewInventoryItem(Item item) {
        if (_nextAvailableSlot >= inventorySlots.Length) {
            Debug.LogError("Inventory UI is full. The item won't be added to the inventory UI");
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
}

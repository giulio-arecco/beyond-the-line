using Ink.Runtime;
using Inventory.Interfaces;
using Inventory.Storables;
using UnityEngine;

public class StoryFunctionsBinder {
    private readonly IStorage<Item>  _playerInventory;
    private readonly ItemInfoDatabaseSO _itemInfoDatabase;

    public StoryFunctionsBinder(IStorage<Item> playerInventory, ItemInfoDatabaseSO itemInfoDatabase) {
        _playerInventory = playerInventory;
        _itemInfoDatabase = itemInfoDatabase;
    }

    public void BindGlobalFunctions(Story story) {
        story.BindExternalFunction("HasItem", (string itemId) => HasItem(itemId));
        Debug.Log("Successfully bound the HasItem function to the Ink Story");
        story.BindExternalFunction("AddItemToInventory", (string itemId) => AddItemToInventory(itemId));
        Debug.Log("Successfully bound the AddItemToInventory function to the Ink Story");
    }

    public void UnbindGlobalFunctions(Story story) {
        story.UnbindExternalFunction("HasItem");
        Debug.Log("Successfully unbound the HasItem function to the Ink Story");
        story.UnbindExternalFunction("AddItemToInventory");
        Debug.Log("Successfully unbound the AddItemToInventory function to the Ink Story");
    }

    private bool HasItem(string itemId) {
        return _playerInventory.Has(itemId);
    }

    private void AddItemToInventory(string itemId) {
        var itemData = _itemInfoDatabase.GetItemById(itemId);
        var item = new Item(itemData);
        _playerInventory.Add(item);
    }
}

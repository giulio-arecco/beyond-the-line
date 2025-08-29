using Ink.Runtime;
using UnityEngine;

public class StoryFunctionsBinder {
    private readonly Inventory _playerInventory;
    private readonly ItemDataDatabaseSO _itemDataDatabase;

    public StoryFunctionsBinder(Inventory playerInventory, ItemDataDatabaseSO itemDataDatabase) {
        _playerInventory = playerInventory;
        _itemDataDatabase = itemDataDatabase;
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
        return _playerInventory.HasItem(itemId);
    }

    private void AddItemToInventory(string itemId) {
        var itemData = _itemDataDatabase.GetItemById(itemId);
        var item = new Item(itemData);
        _playerInventory.AddItem(item);
    }
}

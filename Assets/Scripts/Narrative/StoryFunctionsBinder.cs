using Ink.Runtime;
using UnityEngine;

public class StoryFunctionsBinder {
    private readonly Inventory _playerInventory;

    public StoryFunctionsBinder(Inventory playerInventory) {
        _playerInventory = playerInventory;
    }

    public void BindGlobalFunctions(Story story) {
        story.BindExternalFunction("HasItem", (string itemId) => HasItem(itemId));
        Debug.Log("Successfully bound the HasItem function to the Ink Story");
    }

    public void UnbindGlobalFunctions(Story story) {
        story.UnbindExternalFunction("HasItem");
        Debug.Log("Successfully unbound the HasItem function to the Ink Story");
    }

    private bool HasItem(string itemId) {
        return _playerInventory.HasItem(itemId);
    }
}

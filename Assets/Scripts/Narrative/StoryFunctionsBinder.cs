using Ink.Runtime;
using UnityEngine;

public class StoryFunctionsBinder {
    private readonly Inventory _playerInventory;

    public StoryFunctionsBinder(Inventory playerInventory) {
        _playerInventory = playerInventory;
    }

    public void BindGlobalFunctions(Story story) {
        story.BindExternalFunction("HasItem", (string itemId) => HasItem(itemId));
        Debug.Log("Function HasItem bound correctly to the Ink Story");
    }

    public void UnbindGlobalFunctions(Story story) {
        story.UnbindExternalFunction("HasItem");
        Debug.Log("Function HasItem unbound correctly to the Ink Story");
    }

    private bool HasItem(string itemId) {
        return _playerInventory.HasItem(itemId);
    }
}

using System;
using Ink.Runtime;
using Inventory.Interfaces;
using Storage.StorableInfoDatabase;
using Storage.Storables;
using UnityEngine;

public class StoryFunctionsBinder {
    private readonly IStorage<Item>  _playerInventory;
    private readonly IStorage<Companion>  _playerCompanions;
    private readonly ItemInfoDatabaseSO _itemInfoDatabase;
    private readonly CompanionInfoDatabaseSO _companionInfoDatabase;

    public StoryFunctionsBinder(IStorage<Item> playerInventory, IStorage<Companion> playerCompanions, ItemInfoDatabaseSO itemInfoDatabase, CompanionInfoDatabaseSO companionInfoDatabase) {
        _playerInventory = playerInventory;
        _playerCompanions = playerCompanions;
        _itemInfoDatabase = itemInfoDatabase;
        _companionInfoDatabase = companionInfoDatabase;
    }

    public void BindGlobalFunctions(Story story) {
        story.BindExternalFunction("HasItem", (string itemId) => HasItem(itemId));
        Debug.Log("Successfully bound the HasItem function to the Ink Story");
        story.BindExternalFunction("HasCompanion", (string companionId) => HasCompanion(companionId));
        Debug.Log("Successfully bound the HasCompanion function to the Ink Story");
        story.BindExternalFunction("AddItemToInventory", (string itemId) => AddItemToInventory(itemId));
        Debug.Log("Successfully bound the AddItemToInventory function to the Ink Story");
        story.BindExternalFunction("AddCompanionToParty", (string companionId) => AddCompanionToParty(companionId));
        Debug.Log("Successfully bound the AddCompanionToParty function to the Ink Story");
    }

    public void UnbindGlobalFunctions(Story story) {
        story.UnbindExternalFunction("HasItem");
        Debug.Log("Successfully unbound the HasItem function to the Ink Story");
        story.UnbindExternalFunction("HasCompanion");
        Debug.Log("Successfully unbound the HasCompanion function to the Ink Story");
        story.UnbindExternalFunction("AddItemToInventory");
        Debug.Log("Successfully unbound the AddItemToInventory function to the Ink Story");
        story.UnbindExternalFunction("AddCompanionToParty");
        Debug.Log("Successfully unbound the AddCompanionToParty function to the Ink Story");
    }

    private bool HasItem(string itemId) {
        return _playerInventory.Has(itemId);
    }
    
    private bool HasCompanion(string companionId) {
        return _playerCompanions.Has(companionId);
    }

    private void AddItemToInventory(string itemId) {
        var itemInfo = _itemInfoDatabase.GetItemById(itemId);
        var item = new Item(itemInfo);
        _playerInventory.Add(item);
    }
    
    private void AddCompanionToParty(string companionId) {
        var companionInfo = _companionInfoDatabase.GetItemById(companionId);
        var companion = new Companion(companionInfo);
        _playerCompanions.Add(companion);
    }
}

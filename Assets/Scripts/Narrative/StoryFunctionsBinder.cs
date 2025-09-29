using System;
using Ink.Runtime;
using Inventory.Interfaces;
using Storage.StorableInfoDatabase;
using Storage.Storables;
using UnityEngine;
using static Utils.TypeUtils;

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
        story.BindExternalFunction("HasCompanion", (string companionId) => HasCompanion(companionId));
        story.BindExternalFunction("AddItemToInventory", (string itemId) => AddItemToInventory(itemId));
        story.BindExternalFunction("AddCompanionToParty", (string companionId) => AddCompanionToParty(companionId));
        story.BindExternalFunction("SetCompanionStat", (string companionId, string statName, object statValue) => SetCompanionStat(companionId, statName, statValue));
        story.BindExternalFunction("GetCompanionStat", (string companionId, string statName) => GetCompanionStat(companionId, statName));
        Debug.Log("Successfully bound external global functions to the Ink Story");
    }

    public void UnbindGlobalFunctions(Story story) {
        story.UnbindExternalFunction("HasItem");
        story.UnbindExternalFunction("HasCompanion");
        story.UnbindExternalFunction("AddItemToInventory");
        story.UnbindExternalFunction("AddCompanionToParty");
        story.UnbindExternalFunction("SetCompanionStat");
        story.UnbindExternalFunction("GetCompanionStat");
        Debug.Log("Successfully unbound external global functions from the Ink Story");
    }

    private bool HasItem(string itemId) {
        return _playerInventory.Has(itemId);
    }
    
    private bool HasCompanion(string companionId) {
        return _playerCompanions.Has(companionId);
    }

    private void AddItemToInventory(string itemId) {
        var itemInfo = _itemInfoDatabase.GetEntryById(itemId);
        var item = new Item(itemInfo);
        _playerInventory.Add(item);
    }
    
    private void AddCompanionToParty(string companionId) {
        var companionInfo = _companionInfoDatabase.GetEntryById(companionId);
        var companion = new Companion(companionInfo);
        
        companion.CopyItemsTo(_playerInventory);
        _playerCompanions.Add(companion);
    }
    
    private object GetCompanionStat(string companionId, string statName) {
        var companion = _playerCompanions.GetTypedElement(companionId);
        if (companion == null) {
            Debug.LogError($"Companion {companionId} not found among current companions");
            return null;
        }

        return statName switch {
            "Health" => companion.Health,
            "Hunger" => companion.Hunger,
            _ => throw new ArgumentException($"{statName} stat not found")
        };
    }
    
    private void SetCompanionStat(string companionId, string statName, object statValue) {
        var companion = _playerCompanions.GetTypedElement(companionId);
        if (companion == null) {
            Debug.LogError($"Companion {companionId} not found among current companions");
            return;
        }

        switch (statName) {
            case "Health":
                companion.Health = ConvertTo<int>(statValue);
                Debug.Log($"{companion.Info.name}'s health set to {companion.Health}");
                break;
            case "Hunger":
                companion.Hunger = ConvertTo<int>(statValue);
                Debug.Log($"{companion.Info.name}'s hunger set to {companion.Hunger}");
                break;
            default:
                throw new ArgumentException($"{statName} stat not found");
        }
    }
}



using System;
using Audio;
using Enums;
using Ink.Runtime;
using Inventory.Interfaces;
using Storage.StorableInfoDatabase;
using Storage.Storables;
using UnityEngine;
using static Utils.TypeUtils;

namespace Narrative {
    public class StoryFunctionsBinder {
        private readonly IStorage<Item>  _playerInventory;
        private readonly IStorage<Companion>  _playerCompanions;
        private readonly ItemInfoDatabaseSO _itemInfoDatabase;
        private readonly CompanionInfoDatabaseSO _companionInfoDatabase;
        private readonly MusicLibrarySO _musicLibrary;

        public StoryFunctionsBinder(IStorage<Item> playerInventory, IStorage<Companion> playerCompanions, ItemInfoDatabaseSO itemInfoDatabase, CompanionInfoDatabaseSO companionInfoDatabase, MusicLibrarySO musicLibrary) {
            _playerInventory = playerInventory;
            _playerCompanions = playerCompanions;
            _itemInfoDatabase = itemInfoDatabase;
            _companionInfoDatabase = companionInfoDatabase;
            _musicLibrary = musicLibrary;
        }

        public void BindGlobalFunctions(Story story) {
            story.BindExternalFunction("HasItem", (string itemId) => HasItem(itemId));
            story.BindExternalFunction("HasCompanion", (string companionId) => HasCompanion(companionId));
            story.BindExternalFunction("AddItemToInventory", (string itemId) => AddItemToInventory(itemId));
            story.BindExternalFunction("RemoveItemFromInventory", (string itemId, int count) => RemoveItemFromInventory(itemId, count));
            story.BindExternalFunction("AddCompanionToParty", (string companionId) => AddCompanionToParty(companionId));
            story.BindExternalFunction("RemoveCompanionFromParty", (string companionId) => RemoveCompanionFromParty(companionId));
            story.BindExternalFunction("SetCompanionStat", (string companionId, string statName, object statValue) => SetCompanionStat(companionId, statName, statValue));
            story.BindExternalFunction("GetCompanionStat", (string companionId, string statName) => GetCompanionStat(companionId, statName));
            story.BindExternalFunction("GetGlobalStat", (string statName) => GetGlobalStat(statName));
            story.BindExternalFunction("IncreaseGlobalStat", (string statName, object statValue) => IncreaseGlobalStat(statName, statValue));
            story.BindExternalFunction("DecreaseGlobalStat", (string statName, object statValue) => DecreaseGlobalStat(statName, statValue));
            story.BindExternalFunction("PlayMusic", (string trackId, int transitionType) => PlayMusic(trackId, (AudioTransitionType) transitionType));
            story.BindExternalFunction("StopMusic", StopMusic);
            Debug.Log("Successfully bound external global functions to the Ink Story");
        }

        public void UnbindGlobalFunctions(Story story) {
            story.UnbindExternalFunction("HasItem");
            story.UnbindExternalFunction("HasCompanion");
            story.UnbindExternalFunction("AddItemToInventory");
            story.UnbindExternalFunction("RemoveItemFromInventory");
            story.UnbindExternalFunction("AddCompanionToParty");
            story.UnbindExternalFunction("RemoveCompanionFromParty");
            story.UnbindExternalFunction("SetCompanionStat");
            story.UnbindExternalFunction("GetCompanionStat");
            story.UnbindExternalFunction("GetGlobalStat");
            story.UnbindExternalFunction("IncreaseGlobalStat");
            story.UnbindExternalFunction("DecreaseGlobalStat");
            story.UnbindExternalFunction("PlayMusic");
            story.UnbindExternalFunction("StopMusic");
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

        private void RemoveItemFromInventory(string itemId, int count) {
            _playerInventory.RemoveMany(itemId, count);
        }
    
        private void AddCompanionToParty(string companionId) {
            var companionInfo = _companionInfoDatabase.GetEntryById(companionId);
            var companion = new Companion(companionInfo);
        
            companion.CopyItemsTo(_playerInventory);
            _playerCompanions.Add(companion);
        }
        
        private void RemoveCompanionFromParty(string companionId) {
            _playerCompanions.Remove(companionId);
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

        private object GetGlobalStat(string statName) {
            return GlobalStatsManager.Instance.GlobalStats.GetStatValue(statName);
        }
        
        private void IncreaseGlobalStat(string statName, object statValue) {
            GlobalStatsManager.Instance.GlobalStats.IncreaseStatValue(statName, statValue);
        }
        
        private void DecreaseGlobalStat(string statName, object statValue) {
            GlobalStatsManager.Instance.GlobalStats.DecreaseStatValue(statName, statValue);
        }
        
        // TODO: Write getter, increase and decrease methods for each global stat to avoid reflection boilerplate

        private void PlayMusic(string trackId, AudioTransitionType transitionType) {
            var track = _musicLibrary.GetClip(trackId);
            MusicManager.Instance.PlayMusic(track, transitionType);
        }

        private void StopMusic() => MusicManager.Instance.StopMusic();
    }
}



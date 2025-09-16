using Inventory.Interfaces;
using Storage;
using UnityEngine;

public class Player : Singleton<Player> {
    private int _playerHealth = 100;

    private void Start() {
        StoryManager.Instance.SubscribeToVariableChange("playerHealth", OnPlayerHealthChange, _playerHealth);
    }
    
    private void OnDestroy() {
        if (StoryManager.TryGetInstance(out var storyManager)) {
            storyManager.UnsubscribeFromVariableChange("playerHealth", OnPlayerHealthChange);
        }
    }

    private void OnPlayerHealthChange(Ink.Runtime.Object health) {
        _playerHealth = ((Ink.Runtime.IntValue) health).value;
        Debug.Log("Current Player's health: " + _playerHealth);
    }
}

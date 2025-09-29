using Narrative;
using UnityEngine;
using static Utils.TypeUtils;

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
        _playerHealth = ConvertTo<Ink.Runtime.IntValue>(health).value;
        Debug.Log("Current Player's health: " + _playerHealth);
    }
}

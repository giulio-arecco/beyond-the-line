using UnityEngine;

public class MockPlayer : MonoBehaviour {
    private int _playerHealth = 100;

    private void Start() {
        StoryManager.Instance.SubscribeToVariableChange("playerHealth", OnPlayerHealthChange, _playerHealth);
    }

    private void OnPlayerHealthChange(Ink.Runtime.Object health) {
        _playerHealth = ((Ink.Runtime.IntValue)health).value;
        Debug.Log("Current Player's health: " + _playerHealth);
    }
}

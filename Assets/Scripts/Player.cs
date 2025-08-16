using UnityEngine;

public class Player : Singleton<Player> {
    [SerializeField] private Inventory inventory;
    [SerializeField] private Item TestItem;
    [SerializeField] private Item TestItem1;
    
    public Inventory Inventory => inventory;

    private int _playerHealth = 100;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            Inventory.RemoveItem("TestItem");
        }
        
        if (Input.GetKeyDown(KeyCode.O)) {
            Inventory.RemoveItem("TestItem1");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            Inventory.AddItem(TestItem);
        }
        
        if (Input.GetKeyDown(KeyCode.D)) {
            Inventory.AddItem(TestItem1);
        }
    }

    private void Start() {
        StoryManager.Instance.SubscribeToVariableChange("playerHealth", OnPlayerHealthChange, _playerHealth);
    }

    private void OnPlayerHealthChange(Ink.Runtime.Object health) {
        _playerHealth = ((Ink.Runtime.IntValue)health).value;
        Debug.Log("Current Player's health: " + _playerHealth);
    }
}

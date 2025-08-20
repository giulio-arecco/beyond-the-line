using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Player : Singleton<Player>, IUpdateObserver {
    [SerializeField] private Item TestItem;
    [SerializeField] private Item TestItem1;

    public int UpdatePriority { get; set; }

    private int _playerHealth = 100;
    private Inventory _inventory;

    protected override void Awake() {
        base.Awake();
        _inventory = GetComponent<Inventory>();
    }

    private void OnEnable() {
        UpdateManager.Instance.Register(this);
    }

    private void Start() {
        StoryManager.Instance.SubscribeToVariableChange("playerHealth", OnPlayerHealthChange, _playerHealth);
    }

    private void OnDisable() {
        if (UpdateManager.TryGetInstance(out var updateManager)) {
            updateManager.Unregister(this);
        }
    }
    
    private void OnDestroy() {
        if (StoryManager.TryGetInstance(out var storyManager)) {
            storyManager.UnsubscribeFromVariableChange("playerHealth", OnPlayerHealthChange);
        }
    }

    public void ObservedUpdate() {
        if (Input.GetKeyDown(KeyCode.U)) {
            _inventory.RemoveItem("TestItem");
        }
        
        if (Input.GetKeyDown(KeyCode.O)) {
            _inventory.RemoveItem("TestItem1");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            _inventory.AddItem(TestItem);
        }
        
        if (Input.GetKeyDown(KeyCode.D)) {
            _inventory.AddItem(TestItem1);
        }
    }

    private void OnPlayerHealthChange(Ink.Runtime.Object health) {
        _playerHealth = ((Ink.Runtime.IntValue) health).value;
        Debug.Log("Current Player's health: " + _playerHealth);
    }
}

using Inventory.Interfaces;
using Inventory.Storables;
using Storage;
using UnityEngine;

[RequireComponent(typeof(StorageBase<Item>))]
public class Player : Singleton<Player>, IUpdateObserver {
    [SerializeField] private Item TestItem;
    [SerializeField] private Item TestItem1;

    public int UpdatePriority { get; set; }

    private int _playerHealth = 100;
    private IStorage<Item> _inventory;

    protected override void Awake() {
        base.Awake();
        _inventory = GetComponent<StorageBase<Item>>();
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
            _inventory.Remove("TestItem");
        }
        
        if (Input.GetKeyDown(KeyCode.O)) {
            _inventory.Remove("TestItem1");
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            _inventory.Add(TestItem);
        }
        
        if (Input.GetKeyDown(KeyCode.D)) {
            _inventory.Add(TestItem1);
        }
    }

    private void OnPlayerHealthChange(Ink.Runtime.Object health) {
        _playerHealth = ((Ink.Runtime.IntValue) health).value;
        Debug.Log("Current Player's health: " + _playerHealth);
    }
}

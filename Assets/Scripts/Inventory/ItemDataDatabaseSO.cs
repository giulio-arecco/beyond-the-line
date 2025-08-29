using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataDatabase", menuName = "Scriptable Objects/ItemDataDatabase")]
public class ItemDataDatabaseSO : ScriptableObject {
    [SerializeField] private List<ItemDataSO> items;

    private Dictionary<string, ItemDataSO> _lookup;

    private void Init() {
        _lookup = new Dictionary<string, ItemDataSO>();
        foreach (var item in items) {
            _lookup[item.id] = item;
        }
    }

    public ItemDataSO GetItemById(string id) {
        if (_lookup == null) Init();
        
        if (_lookup.TryGetValue(id, out var itemData)) {
            return itemData;
        }
        
        Debug.LogError($"ItemData with id {id} not found");
        return null;
    }
}
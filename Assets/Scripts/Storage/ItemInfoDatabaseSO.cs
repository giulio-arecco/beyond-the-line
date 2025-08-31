using System.Collections.Generic;
using Inventory.StorableInfo;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataDatabase", menuName = "Scriptable Objects/ItemDataDatabase")]
public class ItemInfoDatabaseSO : ScriptableObject {
    [SerializeField] private List<ItemInfoSO> items;

    private Dictionary<string, ItemInfoSO> _lookup;

    private void Init() {
        _lookup = new Dictionary<string, ItemInfoSO>();
        foreach (var item in items) {
            _lookup[item.id] = item;
        }
    }

    public ItemInfoSO GetItemById(string id) {
        if (_lookup == null) Init();
        
        if (_lookup.TryGetValue(id, out var itemData)) {
            return itemData;
        }
        
        Debug.LogError($"ItemData with id {id} not found");
        return null;
    }
}
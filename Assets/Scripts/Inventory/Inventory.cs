using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class Item {
    public ItemData itemData;
}

public class Inventory : MonoBehaviour {
    [SerializeField] private List<Item> items = new();
    
    public event Action<Item> OnItemAdded;
    public event Action<string> OnItemRemoved;

    public void AddItem(Item item) {
        items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void RemoveItem(string id) {
        items.RemoveAll(x => id == x.itemData.id);
        OnItemRemoved?.Invoke(id);
    }
    
    public bool HasItem(string id) {
        var item = items.Find(x => x.itemData.id == id);
        return item != null;
    }

    [CanBeNull]
    public Item GetItem(string id) {
        return items.Find(x => id == x.itemData.id);
    }

    [CanBeNull]
    public Item[] GetItems() {
        return items.ToArray();
    }
}

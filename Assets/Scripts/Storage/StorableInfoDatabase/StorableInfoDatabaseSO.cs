using System.Collections.Generic;
using Inventory.StorableInfo;
using UnityEngine;

namespace Storage.StorableInfoDatabase {
    public abstract class StorableInfoDatabaseSO<T> : ScriptableObject where T : StorableInfoSO {
        [SerializeField] private List<T> items;

        private Dictionary<string, T> _lookup;

        private void Init() {
            _lookup = new Dictionary<string, T>();
            foreach (var item in items) {
                _lookup[item.id] = item;
            }
        }

        public T GetItemById(string id) {
            if (_lookup == null) Init();

            if (_lookup.TryGetValue(id, out var itemData)) {
                return itemData;
            }

            Debug.LogError($"ItemData with id {id} not found");
            return null;
        }
    }
}
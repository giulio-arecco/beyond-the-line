using System.Collections.Generic;
using Inventory.StorableInfo;
using UnityEngine;
using UnityEngine.Serialization;

namespace Storage.StorableInfoDatabase {
    public abstract class StorableInfoDatabaseSO<T> : ScriptableObject where T : StorableInfoSO {
        [SerializeField] private List<T> infoList;

        private Dictionary<string, T> _lookup;

        private void Init() {
            _lookup = new Dictionary<string, T>();
            foreach (var item in infoList) {
                _lookup[item.id] = item;
            }
        }

        public T GetItemById(string id) {
            if (_lookup == null) Init();

            if (_lookup.TryGetValue(id, out var itemData)) {
                return itemData;
            }

            Debug.LogError($"StorableInfoSO with id {id} not found");
            return null;
        }
    }
}
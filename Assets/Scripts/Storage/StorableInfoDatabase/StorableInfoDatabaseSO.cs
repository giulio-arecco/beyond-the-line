using System.Collections.Generic;
using Storage.StorableInfo;
using UnityEngine;

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

        public T GetEntryById(string id) {
            if (_lookup == null) Init();

            if (_lookup.TryGetValue(id, out var entryInfo)) {
                return entryInfo;
            }

            Debug.LogError($"StorableInfoSO with id {id} not found");
            return null;
        }
    }
}
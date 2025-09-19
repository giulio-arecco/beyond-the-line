using System;
using Inventory.Interfaces;
using Storage.StorableInfo;

namespace Storage.Storables {
    public class Companion : Storable<CompanionInfoSO> {
        public static readonly int MaxHealth = 100;
        public static readonly int MaxHunger = 100;
        
        private int _health;
        public int Health {
            get => _health;
            set {
                _health = value;
                OnHealthChanged?.Invoke(this, _health);
            }
        }

        private int _hunger;
        public int Hunger {
            get => _hunger;
            set {
                _hunger = value;
                OnHungerChanged?.Invoke(this, _hunger);
            }
        }

        public event Action<Companion, int> OnHealthChanged;
        public event Action<Companion, int> OnHungerChanged;
        
        public Companion(CompanionInfoSO info) {
            TypedInfo = info;
            _health = 100;
            _hunger = 0;
        }

        public void CopyItemsTo(IStorage<Item> storage) {
            foreach (var item in TypedInfo.Items) {
                storage.Add(item);
            }
        }
    }
}

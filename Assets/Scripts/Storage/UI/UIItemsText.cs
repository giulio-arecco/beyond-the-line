using Storage.Interfaces;
using Storage.Storables;
using TMPro;
using UnityEngine;
using static Utils.TypeUtils;

namespace Storage.UI {
    [System.Serializable]
    public struct LabelValueTextFields {
        public TextMeshProUGUI label;
        public TextMeshProUGUI value;
    }
    
    public class UIItemsText : MonoBehaviour, IStorableTextWriter {
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI descriptionField;
        [SerializeField] private LabelValueTextFields[] statModifiersFields;
        
        private void Start() {
            ClearAllText();
        }

        private void SetItemStatModifiersText(Item item) {
            ClearStatsModifiersText();
            
            var itemStatsModifiers = item.TypedInfo.StatsModifiers;
            if (itemStatsModifiers == null || itemStatsModifiers.Length == 0) return;
            
            if (statModifiersFields.Length < itemStatsModifiers.Length) {
                Debug.LogError($"Not enough text fields were provided to display all the stats modifiers for item with id '{item.Info.id}'");
            }

            for (var i = 0; i < itemStatsModifiers.Length; i++) {
                statModifiersFields[i].label.text = itemStatsModifiers[i].displayName;
                statModifiersFields[i].value.text = itemStatsModifiers[i].statValue.ToString();
            }
        }

        private void ClearStatsModifiersText() {
            foreach (var fields in statModifiersFields) {
                fields.label.text = "";
                fields.value.text = "";
            }
        }

        public void SetNameText(Storable storable) {
                nameField.text = storable.Info.entityName;
        }

        public void SetDescriptionText(Storable storable) {
                descriptionField.text = storable.Info.description;
        }

        public void SetOtherText(Storable storable) {
            if (typeof(Item).IsAssignableFrom(storable.GetType())) {
                SetItemStatModifiersText(ConvertTo<Item>(storable));
            }
            else {
                Debug.LogError("The Storable concrete type is not Companion");
            }
        }
        
        public void ClearAllText() {
            if (nameField) {
                nameField.text = "";
            }
            
            if (descriptionField) {
                descriptionField.text = "";
            }

            ClearStatsModifiersText();
        }
    }
}
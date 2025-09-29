using Inventory.UI;
using Storage.Storables;
using TMPro;
using UnityEngine;
using static Utils.TypeUtils;

namespace Storage.UI {
    public class UIStorageText : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI descriptionField;
        [SerializeField] private TextMeshProUGUI[] statsLabels;
        [SerializeField] private TextMeshProUGUI[] statsValues;

        private void Start() {
            ClearAllText();
        }

        public void SetNameText(Storable storable) {
            if (nameField) {
                nameField.text = storable.Info.name;
            }
        }

        public void SetDescriptionText(Storable storable) {
            if (descriptionField) {
                descriptionField.text = storable.Info.description;
            }
        }

        public void SetStatsText(Storable storable) {
            if (typeof(Companion).IsAssignableFrom(storable.GetType())) {
                SetCompanionStatsText(ConvertTo<Companion>(storable));
            }
        }

        public void ClearAllText() {
            if (nameField) {
                nameField.text = "";
            }
            
            if (descriptionField) {
                descriptionField.text = "";
            }

            if (statsLabels.Length > 0 && statsValues.Length > 0) {
                for (var i = 0; i < statsLabels.Length; i++) {
                    statsLabels[i].text = "";
                    statsValues[i].text = "";
                }
            }
        }

        private void SetCompanionStatsText(Companion companion) {
            statsLabels[0].text = "Health";
            statsLabels[1].text = "Hunger";
        
            statsValues[0].text = $"{companion.Health}/{Companion.MaxHealth}";
            statsValues[1].text = $"{companion.Hunger}/{Companion.MaxHunger}";
        }
    }
}

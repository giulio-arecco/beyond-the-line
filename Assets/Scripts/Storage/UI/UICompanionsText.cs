using Storage.Interfaces;
using Storage.Storables;
using TMPro;
using UnityEngine;
using static Utils.TypeUtils;

namespace Storage.UI {
    public class UICompanionsText : MonoBehaviour, IStorableTextWriter {
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI descriptionField;
        
        [Header("Stats Labels")]
        [SerializeField] private TextMeshProUGUI healthLabelField;
        [SerializeField] private TextMeshProUGUI hungerLabelField;
        
        [Header("Stats Values")]
        [SerializeField] private TextMeshProUGUI healthValueField;
        [SerializeField] private TextMeshProUGUI hungerValueField;
        
        
        private void Start() {
            ClearAllText();
        }

        private void SetCompanionStatsText(Companion companion) {
            healthLabelField.text = "Salute";
            hungerLabelField.text = "Fame";
        
            healthValueField.text = $"{companion.Health}/{Companion.MaxHealth}";
            hungerValueField.text = $"{companion.Hunger}/{Companion.MaxHunger}";
        }

        public void SetNameText(Storable storable) {
                nameField.text = storable.Info.entityName;
        }

        public void SetDescriptionText(Storable storable) {
                descriptionField.text = storable.Info.description;
        }

        public void SetOtherText(Storable storable) {
            if (storable is Companion companion) {
                SetCompanionStatsText(companion);
            }
            else {
                Debug.LogError("The Storable runtime type is not Companion");
            }
        }
        
        public void ClearAllText() {
            if (nameField) {
                nameField.text = "";
            }
            
            if (descriptionField) {
                descriptionField.text = "";
            }

            healthLabelField.text = "";
            hungerLabelField.text = "";
        
            healthValueField.text = "";
            hungerValueField.text = "";
        }
    }
}
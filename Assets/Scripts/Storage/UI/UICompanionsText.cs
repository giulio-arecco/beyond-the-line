using Storage.Interfaces;
using Storage.Storables;
using TMPro;
using UnityEngine;
using static Utils.TypeUtils;

namespace Storage.UI {
    public class UICompanionsText : MonoBehaviour, IStorableTextWriter {
        [Header("Text Display Options")]
        [SerializeField] private bool showName = true;
        [SerializeField] private bool showDescription = true;
        [SerializeField] private bool showStats = false;
        
        [Header("Name and Description")]
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
            if (!showName) return;
            nameField.text = storable.Info.entityName;
        }

        public void SetDescriptionText(Storable storable) {
            if (!showDescription) return;
            descriptionField.text = storable.Info.description;
        }

        public void SetOtherText(Storable storable) {
            if (showStats) {
                if (storable is Companion companion) {
                    SetCompanionStatsText(companion);
                }
                else {
                    Debug.LogError("The Storable runtime type is not Companion");
                }
            }
        }
        
        public void ClearAllText() {
            if (showName) {
                nameField.text = "";
            }
            
            if (showDescription) {
                descriptionField.text = "";
            }

            if (showStats) {
                healthLabelField.text = "";
                hungerLabelField.text = "";
                healthValueField.text = "";
                hungerValueField.text = "";
            }
        }
    }
}
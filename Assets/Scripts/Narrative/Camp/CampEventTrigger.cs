using System.Collections.Generic;
using Ink.Runtime;
using Inventory.Interfaces;
using Storage.Storables;
using UnityEngine;
using UnityEngine.UI;
using Utils.SerializeInterface;
using static Utils.TypeUtils;

namespace Narrative.Camp {
    public class CampEventTrigger : MonoBehaviour {
        [Header("Button and Inventory handling")] 
        [SerializeField] private InterfaceReference<IStorage<Item>> playerInventory;
        [SerializeField] private Button campButton;
        [Header("Stories")]
        [SerializeField] private TextAsset inkJsonCampBase;
        [SerializeField] private List<OptionalStory> optionalInkJsons;
        
        private void Start() {
            var canSetCampObj = StoryManager.Instance.GetRegistryVariable("CAN_SET_CAMP");
            var canSetCamp = ConvertTo<BoolValue>(canSetCampObj).value;
            campButton.interactable = canSetCamp;
            
            StoryManager.Instance.SubscribeToVariableChange("CAN_SET_CAMP", StoryVariablesRegistry_OnValueChanged);
        }

        private void OnDestroy() {
            StoryManager.Instance.UnsubscribeFromVariableChange("CAN_SET_CAMP", StoryVariablesRegistry_OnValueChanged);
        }

        public void StartCamp() {
            foreach (var optionalStory in optionalInkJsons) {
                StoryManager.Instance.EnqueueOptionalStory(optionalStory);
            }
            StoryManager.Instance.EnterStory(inkJsonCampBase);
        }

        private void StoryVariablesRegistry_OnValueChanged(Ink.Runtime.Object value) {
            var canSetCamp = ConvertTo<BoolValue>(value).value;
            campButton.interactable = canSetCamp;
        }
    }
}
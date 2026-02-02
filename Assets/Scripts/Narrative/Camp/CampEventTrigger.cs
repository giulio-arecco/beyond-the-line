using System.Collections.Generic;
using Ink.Runtime;
using Inventory.Interfaces;
using Storage.Storables;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils.SerializeInterface;
using static Utils.TypeUtils;

namespace Narrative.Camp {
    public class CampEventTrigger : MonoBehaviour {
        [Header("Button and Inventory handling")] 
        [SerializeField] private InterfaceReference<IStorage<Item>> playerInventory;
        [SerializeField] private UIButtonStateController triggerButton;
        
        [Header("Stories")]
        [SerializeField] private TextAsset inkJsonCampBase;
        [SerializeField] private List<OptionalStory> optionalInkJsons;
        
        private void Start() {
            var canSetCampObj = StoryManager.Instance.GetRegistryVariable("CAN_SET_CAMP");
            var canSetCamp = ConvertTo<BoolValue>(canSetCampObj).value;
            triggerButton.SetInteractable(canSetCamp);
            
            StoryManager.Instance.SubscribeToVariableChange("CAN_SET_CAMP", StoryVariablesRegistry_OnValueChanged);
        }

        private void OnDestroy() {
            if (StoryManager.TryGetInstance(out var storyManager)) {
                storyManager.UnsubscribeFromVariableChange("CAN_SET_CAMP", StoryVariablesRegistry_OnValueChanged);
            }
        }

        public void StartCamp() {
            foreach (var optionalStory in optionalInkJsons) {
                StoryManager.Instance.EnqueueOptionalStory(optionalStory);
            }
            StoryManager.Instance.EnterStory(inkJsonCampBase);
        }

        private void StoryVariablesRegistry_OnValueChanged(Ink.Runtime.Object value) {
            var canSetCamp = ConvertTo<BoolValue>(value).value;
            triggerButton.SetInteractable(canSetCamp);
        }
    }
}
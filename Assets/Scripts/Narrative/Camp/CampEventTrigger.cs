using System.Collections.Generic;
using Inventory.Interfaces;
using Storage.Storables;
using UnityEngine;
using UnityEngine.UI;
using Utils.SerializeInterface;

namespace Narrative.Camp {
    public class CampEventTrigger : MonoBehaviour {
        [Header("Button and Inventory handling")] 
        [SerializeField] private InterfaceReference<IStorage<Item>> playerInventory;
        [SerializeField] private Button campButton;
        [Header("Stories")]
        [SerializeField] private TextAsset inkJsonCampBase;
        [SerializeField] private List<OptionalStory> optionalInkJsons;

        public void StartCamp() {
            foreach (var optionalStory in optionalInkJsons) {
                StoryManager.Instance.EnqueueOptionalStory(optionalStory);
            }
            StoryManager.Instance.EnterStory(inkJsonCampBase);
        }
    }
}
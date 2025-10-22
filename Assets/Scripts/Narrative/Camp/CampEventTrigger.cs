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

        private void Start() {
            campButton.interactable = playerInventory.Value.Has("Ration");
            
            playerInventory.Value.OnAdd += IStorage_OnAdd;
            playerInventory.Value.OnRemove += IStorage_OnRemove;
        }

        private void OnDestroy() {
            if (playerInventory.Value != null) {
                playerInventory.Value.OnAdd -= IStorage_OnAdd;
                playerInventory.Value.OnRemove -= IStorage_OnRemove;
            }
        }

        private void IStorage_OnAdd(Storable storable) {
            if (storable.Info.id == "Ration") {
                campButton.interactable= true;
            }
        }
        
        private void IStorage_OnRemove(Storable element) {
            if (element.Info.id == "Ration" && !playerInventory.Value.Has("Ration")) {
                campButton.interactable= false;
            }
        }

        public void StartCamp() {
            playerInventory.Value.Remove("Ration");
            
            foreach (var optionalStory in optionalInkJsons) {
                StoryManager.Instance.EnqueueOptionalStory(optionalStory);
            }
            StoryManager.Instance.EnterStory(inkJsonCampBase);
        }
    }
}
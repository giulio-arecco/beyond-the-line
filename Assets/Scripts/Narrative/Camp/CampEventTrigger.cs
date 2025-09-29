using System.Collections.Generic;
using UnityEngine;

namespace Narrative.Camp {
    public class CampEventTrigger : MonoBehaviour {
        [SerializeField] private TextAsset inkJsonCampBase;
        [SerializeField] private List<OptionalStory> inkJsonList;

        public void StartCamp() {
            foreach (var optionalStory in inkJsonList) {
                StoryManager.Instance.EnqueueOptionalStory(optionalStory);
            }
            StoryManager.Instance.EnterStory(inkJsonCampBase);
        }
    }
}
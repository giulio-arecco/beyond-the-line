using Ink.Runtime;
using Narrative;
using UI;
using UnityEngine;
using static Utils.TypeUtils;

namespace Architecture {
    public class EndOfGameHandler : MonoBehaviour {
        [SerializeField] private UIPanelController endOfGamePanel;

        private void Start() {
            StoryManager.Instance.OnStoryExit += StoryManager_OnStoryExit;
        }
        
        private void OnDestroy() {
            if (StoryManager.TryGetInstance(out var storyManager)) {
                storyManager.OnStoryExit -= StoryManager_OnStoryExit;
            }
        }

        private void StoryManager_OnStoryExit() {
            var storyManager = StoryManager.Instance;
            
            var hasStoryEnded = ConvertTo<BoolValue>(storyManager.GetRegistryVariable("END_OF_STORY")).value;
            if (!hasStoryEnded) return;
            
            endOfGamePanel.SetVisibleAndInteractable(true);

            var stats = storyManager.GetRegistryVariables(StatsExporter.StatsToSave); 
            StatsExporter.SaveGameStats(stats);
        }
    }
}

using Enums;
using Ink.Runtime;
using Narrative;
using UI;
using UnityEngine;
using static Utils.TypeUtils;

namespace Architecture {
    public class EndOfGameHandler : MonoBehaviour {
        [SerializeField] private InputReaderSO inputReader;
        [SerializeField] private UIPanelController endOfGamePanel;

        private const string END_OF_STORY_VAR_NAME = "END_OF_STORY";
        private const string STATS_PREFIX = "CPS";

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
            
            inputReader.DisableInputActionMap("Gameplay");
            
            var hasStoryEnded = ConvertTo<BoolValue>(storyManager.GetRegistryVariable(END_OF_STORY_VAR_NAME)).value;
            if (!hasStoryEnded) return;
            
            UINavigator.Instance.PushUILayer(endOfGamePanel, UILayerPushOptions.RemoveAllPreviousLayers);

            var stats = storyManager.GetRegistryVariables(STATS_PREFIX); 
            StatsExporter.SaveGameStats(stats);
        }
    }
}

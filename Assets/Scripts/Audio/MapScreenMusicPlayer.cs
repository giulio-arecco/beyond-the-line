using Enums;
using Narrative;
using UI;
using UnityEngine;

namespace Audio {
    public class MapScreenMusicPlayer : MonoBehaviour {
        [SerializeField] private AudioClip mapScreenClip;
    
        private void Start() {
            MusicManager.Instance.PlayMusic(mapScreenClip, AudioTransitionType.FadeOutIn, -1f, 0.25f);
            StoryManager.Instance.OnStoryExit += StoryManager_OnStoryExit;
        }
    
        private void OnDestroy() {
            if (StoryManager.TryGetInstance(out var storyManager)) {
                storyManager.OnStoryExit -= StoryManager_OnStoryExit;
            }
        }

        private void StoryManager_OnStoryExit() {
            MusicManager.Instance.PlayMusic(mapScreenClip, AudioTransitionType.FadeOutIn, -1f, 0.25f);
        }
    }
}

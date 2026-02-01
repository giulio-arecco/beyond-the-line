using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Enums;

namespace Audio {
    public class AudioManager : PersistentSingleton<AudioManager> {
        
        [System.Serializable]
        private struct UISoundMapping {
            public UISoundType type;
            public AudioClip clip;
        }

        [Header("UI Sounds Config")]
        [SerializeField] private List<UISoundMapping> soundMappings;
        [SerializeField] private AudioSource uiSource;
        
        [Header("Settings")]
        [SerializeField, Range(0f, 0.2f)] private float pitchVariance = 0.1f;
        [SerializeField] private float minTimeBetweenClicks = 0.05f;

        private double _lastClickTime;
        private Dictionary<UISoundType, AudioClip> _uiAudioLibrary;

        protected override void Awake() {
            base.Awake();
            InitializeLibrary();
        }

        private void InitializeLibrary() {
            _uiAudioLibrary = new Dictionary<UISoundType, AudioClip>();
            foreach (var mapping in soundMappings) {
                if (!_uiAudioLibrary.ContainsKey(mapping.type)) {
                    _uiAudioLibrary.Add(mapping.type, mapping.clip);
                }
            }
        }

        public void PlayUISound(UISoundType type) {
            if (_uiAudioLibrary.TryGetValue(type, out var clip)) {
                PlayClipInternal(clip); 
            } else {
                Debug.LogError($"[AudioManager] No audio clip assigned for UISoundType: {type}");
            }
        }

        private void PlayClipInternal(AudioClip clip) {
            // We use double to make precision errors more unlikely
            var currentTime = Time.realtimeSinceStartupAsDouble;
            
            // Debounce
            if (currentTime - _lastClickTime < minTimeBetweenClicks) return;

            _lastClickTime = currentTime;

            // Pitch variation
            const float originalPitch = 1.0f; 
            uiSource.pitch = originalPitch + Random.Range(-pitchVariance, pitchVariance);

            uiSource.PlayOneShot(clip);
        }
    }
}
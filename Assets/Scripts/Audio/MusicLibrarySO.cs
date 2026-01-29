using System.Collections.Generic;
using UnityEngine;

namespace Audio {
    [CreateAssetMenu(fileName = "MusicLibrarySO", menuName = "Scriptable Objects/Audio/MusicLibrary", order = 0)]
    public class MusicLibrarySO : ScriptableObject {
        [System.Serializable]
        public struct MusicTrack {
            public string trackId;
            public AudioClip clip;
        }

        [SerializeField]
        private List<MusicTrack> tracks = new();

        // Internal cache
        private Dictionary<string, AudioClip> trackDictionary;

        private void OnEnable() {
            InitializeDictionary();
        }

        private void InitializeDictionary() {
            trackDictionary = new Dictionary<string, AudioClip>();
            
            foreach (var track in tracks) {
                if (!string.IsNullOrEmpty(track.trackId) && track.clip != null) {
                        trackDictionary.TryAdd(track.trackId, track.clip);
                }
            }
        }

        public AudioClip GetClip(string id) {
            if (trackDictionary == null || trackDictionary.Count == 0) InitializeDictionary();

            if (trackDictionary.TryGetValue(id, out AudioClip clip)) {
                return clip;
            }

            Debug.LogError($"[MusicLibrarySO] No track found with id '{id}'");
            return null;
        }
    }
}

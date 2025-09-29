using UnityEngine;

namespace Narrative {
    [System.Serializable]
    public class OptionalStory {
        [field: SerializeField] public TextAsset InkJson { get; private set; }
        [field: SerializeField] public bool IsReplayable { get; private set; }
        public bool IsPlayable { get; set; } = true;
    }
}

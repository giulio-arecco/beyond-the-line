using UnityEngine;

public class StoryEventTrigger : MonoBehaviour {
    [SerializeField] private TextAsset inkJson;

    public void TriggerStoryEvent() {
        StoryManager.Instance.EnterStoryEvent(inkJson);
    }
}

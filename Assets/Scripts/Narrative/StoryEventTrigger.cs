using UnityEngine;

public class StoryEventTrigger : MonoBehaviour {
    [SerializeField] private TextAsset inkJson;

    private void Start() {
        StoryManager.Instance.EnterStoryEvent(inkJson);
    }

    private void Update() {
        if (StoryManager.Instance.StoryIsProgressing) return;
        if (Input.GetKeyDown(KeyCode.R)) {
            StoryManager.Instance.EnterStoryEvent(inkJson);
        }
    }
}

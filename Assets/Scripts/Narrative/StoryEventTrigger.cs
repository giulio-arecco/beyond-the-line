using System.Collections.Generic;
using UnityEngine;

public class StoryEventTrigger : MonoBehaviour {
    [SerializeField] private List<TextAsset> inkJsonList;

    private Queue<TextAsset> _inkJsonQueue;

    private void Awake() {
        _inkJsonQueue = new Queue<TextAsset>(inkJsonList);
    }

    public void TriggerStoryEvent() {
        if (_inkJsonQueue.Count > 0)
            StoryManager.Instance.EnterStoryEvent(_inkJsonQueue.Dequeue());
        else
            Debug.LogError("Empty inkJson queue");
    }
}

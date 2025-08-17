using System;
using UnityEngine;

public class StoryEventTrigger : MonoBehaviour, IUpdateObserver {
    [SerializeField] private TextAsset inkJson;
    
    public int UpdatePriority { get; set; }

    private void OnEnable() {
        UpdateManager.Instance.Register(this);
    }

    private void OnDisable() {
        if (UpdateManager.TryGetInstance(out var updateManager)) {
            updateManager.Unregister(this);
        }
    }

    private void Start() {
        StoryManager.Instance.EnterStoryEvent(inkJson);
    }
    
    public void ObservedUpdate() {
        if (StoryManager.Instance.StoryIsProgressing) return;
        if (Input.GetKeyDown(KeyCode.R)) {
            StoryManager.Instance.EnterStoryEvent(inkJson);
        }
    }
}

using UI;
using UnityEngine;

namespace Metrics {
    public class UIPanelControllerShowedTracker : MonoBehaviour {
        [SerializeField] private UIPanelController uiPanelController;
        [SerializeField] private IntRuntimeStat statToTrack;

        private void Start() {
            uiPanelController.OnCanvasGroupVisible += UIPanelController_OnCanvasGroupVisible;
        }

        private void OnDestroy() {
            uiPanelController.OnCanvasGroupVisible -= UIPanelController_OnCanvasGroupVisible;
        }

        private void UIPanelController_OnCanvasGroupVisible() {
            Debug.Log($"Increased stat {statToTrack}");
            RuntimeStats.IncreaseStat(statToTrack, 1);
        }
    }
}

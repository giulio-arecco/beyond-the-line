using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UI;
using UnityEngine;

namespace NodeCanvas.CustomTasks.Conditions {
    [Category("Custom/UIPanelController")]
    [Description("Check if a CanvasGroup is visible, interactable and blocks raycasts.")]
    public class CheckUIPanelControllerVisibleAndInteractable: ConditionTask<UIPanelController> {
        protected override string info {
            get {
                if (agent == null) return "*Missing UIPanelController* is visible and interactable";
                return $"{agent.name} is visible and interactable";
            }
        }
        
        protected override bool OnCheck() {
            if (!agent) {
                Debug.LogWarning("[NodeCanvas] CheckUIPanelControllerVisibleAndInteractable: UIPanelController (agent) is null");
                return false;
            }

            return agent.IsVisibleAndInteractable();
        }
    }
}

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using Utils.Extensions;
using CanvasGroup = UnityEngine.CanvasGroup;

namespace NodeCanvas.CustomTasks.Conditions {
    [Category("Custom/CanvasGroup")]
    [Description("Check if a CanvasGroup is visible, interactable and blocks raycasts.")]
    public class CheckCanvasGroupVisibleAndInteractable: ConditionTask<CanvasGroup> {
        protected override string info {
            get {
                if (agent == null) return "*Missing CanvasGroup* is visible and interactable";
                return $"{agent.name} is visible and interactable";
            }
        }
        
        protected override bool OnCheck() {
            if (!agent) {
                Debug.LogWarning($"[NodeCanvas] CheckCanvasGroupVisibleAndInteractable: Canvas group is null");
                return false;
            }

            return agent.IsVisibleAndInteractable();
        }
    }
}

using UnityEngine;

namespace Utils.Extensions {
    public static class CanvasGroupExtensions {
        public static void SetVisibleAndInteractable(this CanvasGroup group, bool visibleAndInteractable) {
            if (group == null) return;

            if (visibleAndInteractable) {
                group.alpha = 1f; 
                group.interactable = true; 
                group.blocksRaycasts = true;
            }
            else {
                group.alpha = 0f; 
                group.interactable = false; 
                group.blocksRaycasts = false; 
            }
        }
    }
}
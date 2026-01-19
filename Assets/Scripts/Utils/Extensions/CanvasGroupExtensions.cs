using UnityEngine;

namespace Utils.Extensions {
    public static class CanvasGroupExtensions {
        public static void SetVisibleAndInteractable(this CanvasGroup canvasGroup, bool visibleAndInteractable) {
            if (canvasGroup == null) return;

            if (visibleAndInteractable) {
                canvasGroup.alpha = 1f; 
                canvasGroup.interactable = true; 
                canvasGroup.blocksRaycasts = true;
            }
            else {
                canvasGroup.alpha = 0f; 
                canvasGroup.interactable = false; 
                canvasGroup.blocksRaycasts = false; 
            }
        }

        public static bool IsVisibleAndInteractable(this CanvasGroup canvasGroup) => canvasGroup.alpha != 0 && canvasGroup.interactable && canvasGroup.blocksRaycasts;
        
    }
}
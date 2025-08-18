using UnityEngine;

public class UIInputHandler : MonoBehaviour {
    [SerializeField] private InputReader input;
    
    [Header("Canvas Elements")]
    [SerializeField] CanvasGroup inventoryCanvasGroup;

    private void OnEnable() {
        input.OpenCloseInventory += Input_OpenCloseInventory;
        input.EnableInputActions();
    }   

    private void OnDisable() {
        input.OpenCloseInventory -= Input_OpenCloseInventory;
    }

    private void Input_OpenCloseInventory() {
        if (Mathf.Approximately(inventoryCanvasGroup.alpha, 1f)) {
            inventoryCanvasGroup.alpha = 0;
            inventoryCanvasGroup.interactable = false;
            inventoryCanvasGroup.blocksRaycasts = false;
        }
        else if (Mathf.Approximately(inventoryCanvasGroup.alpha, 0f)) {
            inventoryCanvasGroup.alpha = 1;
            inventoryCanvasGroup.interactable = true;
            inventoryCanvasGroup.blocksRaycasts = true;
        }
    }
}

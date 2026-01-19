using UnityEngine;
using Enums;
using Utils.Extensions;

public class UIInputHandler : MonoBehaviour {
    [SerializeField] private InputReaderSO input;

    [Header("Settings")] 
    [SerializeField] private bool bindOpenCloseInventoryAction = true;
    [SerializeField] private bool bindOpenCloseCompanionsAction = true;
    [SerializeField] private bool bindPrevUILayerAction = true;
    
    [Header("Canvas Elements")]
    [SerializeField] private CanvasGroup inventoryPanel;
    [SerializeField] private CanvasGroup companionsPanel;

    private void OnEnable() {
        if (bindOpenCloseInventoryAction) input.OpenCloseInventory += Input_OpenCloseInventory;
        if (bindOpenCloseCompanionsAction) input.OpenCloseCompanions += Input_OpenCloseCompanions;
        if (bindPrevUILayerAction) input.ExitUIPanel += Input_ExitUIPanel;
    }   

    private void OnDisable() {
        if (bindOpenCloseInventoryAction) input.OpenCloseInventory -= Input_OpenCloseInventory;
        if (bindOpenCloseCompanionsAction) input.OpenCloseCompanions -= Input_OpenCloseCompanions;
        if (bindPrevUILayerAction) input.ExitUIPanel -= Input_ExitUIPanel;
    }

    private void Input_OpenCloseInventory() {
        if (inventoryPanel.IsVisibleAndInteractable()) {
            UINavigator.Instance.PopUILayer(inventoryPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(new[] {inventoryPanel}, UILayerPushOptions.RemoveAllPreviousLayers);
        }
    }
    
    private void Input_OpenCloseCompanions() {
        if (companionsPanel.IsVisibleAndInteractable()) {
            UINavigator.Instance.PopUILayer(companionsPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(new[] {companionsPanel}, UILayerPushOptions.RemoveAllPreviousLayers);
        }
    }
    
    private void Input_ExitUIPanel() => UINavigator.Instance.PopUILayer();
}

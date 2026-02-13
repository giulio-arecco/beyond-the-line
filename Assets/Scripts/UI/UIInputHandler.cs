using UnityEngine;
using Enums;
using UI;
using Utils.Extensions;

public class UIInputHandler : MonoBehaviour {
    [SerializeField] private InputReaderSO input;

    [Header("Settings")] 
    [SerializeField] private bool bindOpenCloseInventoryAction = true;
    [SerializeField] private bool bindOpenCloseCompanionsAction = true;
    [SerializeField] private bool bindExitUIPanelAction = true;
    
    [Header("Canvas Elements")]
    [SerializeField] private UIPanelController inventoryPanel;
    [SerializeField] private UIPanelController companionsPanel;

    private void OnEnable() {
        if (bindOpenCloseInventoryAction) input.OpenCloseInventory += Input_OpenCloseInventory;
        if (bindOpenCloseCompanionsAction) input.OpenCloseCompanions += Input_OpenCloseCompanions;
        if (bindExitUIPanelAction) input.ExitUIPanel += Input_ExitUIPanel;
    }   

    private void OnDisable() {
        if (bindOpenCloseInventoryAction) input.OpenCloseInventory -= Input_OpenCloseInventory;
        if (bindOpenCloseCompanionsAction) input.OpenCloseCompanions -= Input_OpenCloseCompanions;
        if (bindExitUIPanelAction) input.ExitUIPanel -= Input_ExitUIPanel;
    }

    private void Input_OpenCloseInventory() {
        if (inventoryPanel.IsVisibleAndInteractable()) {
            UINavigator.Instance.PopUILayer(inventoryPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(new[] {inventoryPanel}, UILayerPushOptions.RemoveAllPreviousLayers);
            RuntimeStats.IncreaseStat(IntRuntimeStat.CPS_Opened_Inventory_Count, 1);
        }
    }
    
    private void Input_OpenCloseCompanions() {
        if (companionsPanel.IsVisibleAndInteractable()) {
            UINavigator.Instance.PopUILayer(companionsPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(new[] {companionsPanel}, UILayerPushOptions.RemoveAllPreviousLayers);
            RuntimeStats.IncreaseStat(IntRuntimeStat.CPS_Opened_Companions_Count, 1);
        }
    }
    
    private void Input_ExitUIPanel() => UINavigator.Instance.PopUILayer();
}

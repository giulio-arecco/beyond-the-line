using UnityEngine;
using Enums;

public class UIInputHandler : MonoBehaviour {
    [SerializeField] private InputReaderSO input;
    
    [Header("Canvas Elements")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject companionsPanel;

    private void OnEnable() {
        input.OpenCloseInventory += Input_OpenCloseInventory;
        input.OpenCloseCompanions += Input_OpenCloseCompanions;
        input.PrevUILayer += Input_PrevUILayer;
        input.EnableInputActions();
    }   

    private void OnDisable() {
        input.OpenCloseInventory -= Input_OpenCloseInventory;
        input.OpenCloseCompanions -= Input_OpenCloseCompanions;
        input.PrevUILayer -= Input_PrevUILayer;
    }

    private void Input_OpenCloseInventory() {
        if (inventoryPanel.activeInHierarchy) {
            UINavigator.Instance.PopUILayer(inventoryPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(inventoryPanel, UILayerPushOptions.RemoveAllPreviousLayers);
        }
    }
    
    private void Input_OpenCloseCompanions() {
        if (companionsPanel.activeInHierarchy) {
            UINavigator.Instance.PopUILayer(companionsPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(companionsPanel, UILayerPushOptions.RemoveAllPreviousLayers);
        }
    }
    
    private void Input_PrevUILayer() => UINavigator.Instance.PopUILayer();
}

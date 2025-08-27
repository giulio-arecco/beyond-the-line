using UnityEngine;

public class UIInputHandler : MonoBehaviour {
    [SerializeField] private InputReader input;
    
    [Header("Canvas Elements")]
    [SerializeField] GameObject inventoryPanel;

    private void OnEnable() {
        input.OpenCloseInventory += Input_OpenCloseInventory;
        input.PrevUILayer += Input_PrevUILayer;
        input.EnableInputActions();
    }   

    private void OnDisable() {
        input.OpenCloseInventory -= Input_OpenCloseInventory;
        input.PrevUILayer -= Input_PrevUILayer;
    }

    private void Input_OpenCloseInventory() {
        if (inventoryPanel.activeInHierarchy) {
            UINavigator.Instance.PopUILayer(inventoryPanel);
        }
        else {
            UINavigator.Instance.PushUILayer(inventoryPanel, true);
        }
    }
    
    private void Input_PrevUILayer() => UINavigator.Instance.PopUILayer();
}

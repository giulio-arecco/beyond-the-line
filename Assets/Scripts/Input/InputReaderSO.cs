using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static InputSystem_Actions;

public interface IInputReader { 
    void EnableInputActions();
    void DisableInputActions();
}

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReaderSO : ScriptableObject, IInputReader, IGameplayActions, IUIActions {
    public event Action ContinueStory;
    public event Action OpenCloseInventory;
    public event Action PrevUILayer;
    
    public InputSystem_Actions inputActions;

    public bool IsContinueStoryKeyPressed => inputActions.Gameplay.ContinueStory.IsPressed();
    public bool IsOpenCloseInventoryKeyPressed => inputActions.Gameplay.OpenCloseInventory.IsPressed();
    public bool IsPrevUILayerKeyPressed => inputActions.Gameplay.PrevUILayer.IsPressed();

    public void EnableInputActions() {
        if (inputActions == null) {
            inputActions = new InputSystem_Actions();
            inputActions.Gameplay.SetCallbacks(this);
            inputActions.UI.SetCallbacks(this);
        }
        
        inputActions.Enable();
        
        // Make the InputSystemUIInputModule (in the EventSystem) reference the same input actions asset we're using here
        var eventSystem = EventSystem.current;
        if (eventSystem == null) {
            Debug.LogWarning("No EventSystem found in scene.");
            return;
        }

        var uiModule = eventSystem.GetComponent<InputSystemUIInputModule>();
        if (uiModule == null) {
            Debug.LogWarning("No InputSystemUIInputModule found in scene.");
            return;
        }

        if (uiModule.actionsAsset != inputActions.asset) {
            uiModule.actionsAsset = inputActions.asset;
            Debug.Log("Successfully assigned inputActions.asset to InputSystemUIInputModule.");
        }
    }

    public void DisableInputActions() {
        inputActions.Disable();
    }
    
    public void EnableInputAction(string actionMapName, string actionName) {
        inputActions.asset.FindActionMap(actionMapName, true).FindAction(actionName, true).Enable();
        Debug.Log("Successfully enabled the " + actionName + " action in the " + actionMapName + " action map.");
    }

    public void DisableInputAction(string actionMapName, string actionName) {
        inputActions.asset.FindActionMap(actionMapName, true).FindAction(actionName, true).Disable();
        Debug.Log("Successfully disabled the " + actionName + " action in the " + actionMapName + " action map.");
    }
    
    public void EnableInputActionMap(string actionMapName) {
        inputActions.asset.FindActionMap(actionMapName, true).Enable();
        Debug.Log("Successfully enabled the " + actionMapName + " action map.");
    }

    public void DisableInputActionMap(string actionMapName) {
        inputActions.asset.FindActionMap(actionMapName, true).Disable();
        Debug.Log("Successfully disabled the " + actionMapName + " action map.");
    }

    // --- IGameplayActions ---
    public void OnContinueStory(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Canceled) ContinueStory?.Invoke();
    }

    public void OnOpenCloseInventory(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Canceled) OpenCloseInventory?.Invoke();
    }

    public void OnPrevUILayer(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Canceled) PrevUILayer?.Invoke();
    }

    // --- IUIActions ---
    public void OnNavigate(InputAction.CallbackContext context) { }
    public void OnSubmit(InputAction.CallbackContext context) { }
    public void OnCancel(InputAction.CallbackContext context) { }
    public void OnPoint(InputAction.CallbackContext context) { }
    public void OnClick(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnMiddleClick(InputAction.CallbackContext context) { }
    public void OnScrollWheel(InputAction.CallbackContext context) { }
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
}

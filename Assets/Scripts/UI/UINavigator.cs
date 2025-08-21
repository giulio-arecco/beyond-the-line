using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UINavigator : Singleton<UINavigator> {
    [SerializeField] private GameObject[] defaultLayer;
    
    private class NavigationNode {
        public readonly GameObject UiElement;
        public bool IsActiveInLayer;

        public NavigationNode(GameObject uiElement, bool isActiveInLayer) {
            UiElement = uiElement;
            IsActiveInLayer = isActiveInLayer;
        }
    }

    private Stack<List<NavigationNode>> _uiElementsStack;

    protected override void Awake() {
        base.Awake();
        _uiElementsStack = new Stack<List<NavigationNode>>();
    }

    private void Start() {
        _uiElementsStack.Push(new List<NavigationNode>()); // root layer, not to be popped
        foreach (var element in defaultLayer) {
            _uiElementsStack.Peek().Add(new NavigationNode(element, true));
            element.SetActive(true);
        }
    }
    
    private void LogStack() {
        Debug.Log("Current UI Stack:");
        foreach (var layer in _uiElementsStack) {
            Debug.Log($"Layer: {string.Join(", ", layer.Select(n => $"{n.UiElement.name} ({n.IsActiveInLayer})"))}");
        }
    }

    public void ShowUIElement(GameObject uiElement, bool hideLayer) {
        var currentLayer = _uiElementsStack.Peek();
        
        if (hideLayer) {
            foreach (var node in currentLayer.Where(node => node.UiElement != uiElement)) {
                // only hide elements different from the one that was passed in
                node.UiElement.SetActive(false);
                node.IsActiveInLayer = false;
            }
        }

        var elementInLayer = currentLayer.Find(x => x.UiElement == uiElement);
        if (elementInLayer != null) {
            elementInLayer.IsActiveInLayer = true;
        }
        else {
            currentLayer.Add(new NavigationNode(uiElement, true));
        }
        
        uiElement.SetActive(true);
    }

    public void PushUILayer(GameObject uiElement, bool hidePreviousLayer) {
        var newLayer = new List<NavigationNode>();

        if (hidePreviousLayer && _uiElementsStack.Count > 0) {
            var previousLayer = _uiElementsStack.Peek();
            foreach (var node in previousLayer) {
                node.UiElement.SetActive(false);
                // We don't set IsActiveInLayer to false to restore the previous layer when this will be popped
            }
        }

        var newNode = new NavigationNode(uiElement, true);
        newLayer.Add(newNode);
        uiElement.SetActive(true);

        _uiElementsStack.Push(newLayer);
    }

    public void HideUIElement(GameObject uiElement) {
        var elementInLayer = _uiElementsStack.Peek().Find(x => x.UiElement == uiElement);
        if (elementInLayer != null) {
            elementInLayer.IsActiveInLayer = false;
            uiElement.SetActive(false);
        }
    }

    public void PopUILayer() {
        if (_uiElementsStack.Count == 1) return;
        
        var hiddenLayer = _uiElementsStack.Pop();
        foreach (var node in hiddenLayer) {
            node.UiElement.SetActive(false);
        }
        
        // Reactivate the elements marked as IsActiveInLayer in the (now) current layer
        var currentLayer = _uiElementsStack.Peek();
        foreach (var node in currentLayer.Where(node => node.IsActiveInLayer)) {
            node.UiElement.SetActive(true);
        }
    }
}

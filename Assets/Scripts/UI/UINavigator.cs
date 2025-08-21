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

    private Stack<List<NavigationNode>> _uiLayers;

    protected override void Awake() {
        base.Awake();
        _uiLayers = new Stack<List<NavigationNode>>();
    }

    private void Start() {
        _uiLayers.Push(new List<NavigationNode>()); // root layer, not to be popped
        foreach (var element in defaultLayer) {
            _uiLayers.Peek().Add(new NavigationNode(element, true));
            element.SetActive(true);
        }
    }
    
    private void LogStack() {
        Debug.Log("Current UI Stack:");
        foreach (var layer in _uiLayers) {
            Debug.Log($"Layer: {string.Join(", ", layer.Select(n => $"{n.UiElement.name} ({n.IsActiveInLayer})"))}");
        }
    }

    public void ShowUIElement(GameObject uiElement, bool hideLayer) {
        var currentLayer = _uiLayers.Peek();
        
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

        if (hidePreviousLayer && _uiLayers.Count > 0) {
            var previousLayer = _uiLayers.Peek();
            foreach (var node in previousLayer) {
                node.UiElement.SetActive(false);
                // We don't set IsActiveInLayer to false to restore the previous layer when this will be popped
            }
        }

        var newNode = new NavigationNode(uiElement, true);
        newLayer.Add(newNode);
        uiElement.SetActive(true);

        _uiLayers.Push(newLayer);
    }

    public void HideUIElement(GameObject uiElement) {
        var elementInLayer = _uiLayers.Peek().Find(x => x.UiElement == uiElement);
        if (elementInLayer != null) {
            elementInLayer.IsActiveInLayer = false;
            uiElement.SetActive(false);
        }
    }

    public void PopUILayer(GameObject elementInLayer = null) {
        if (_uiLayers.Count == 1) return;
        
        if (elementInLayer != null) {
            // return if the passed elementInLayer is not found in the layer to hide
            var layerToHide = _uiLayers.Peek();
            var node = layerToHide.Find(x => x.UiElement == elementInLayer);
            if (node == null) return;
        }
        
        var hiddenLayer = _uiLayers.Pop();
        foreach (var node in hiddenLayer) {
            node.UiElement.SetActive(false);
        }
        
        // Reactivate the elements marked as IsActiveInLayer in the (now) current layer
        var currentLayer = _uiLayers.Peek();
        foreach (var node in currentLayer.Where(node => node.IsActiveInLayer)) {
            node.UiElement.SetActive(true);
        }
    }
}

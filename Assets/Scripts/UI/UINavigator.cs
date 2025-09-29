using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

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

    private List<List<NavigationNode>> _uiLayers;

    protected override void Awake() {
        base.Awake();
        _uiLayers = new List<List<NavigationNode>>();
    }

    private void Start() {
        _uiLayers.Add(new List<NavigationNode>()); // root layer, not to be popped
        foreach (var element in defaultLayer) {
            _uiLayers.Last().Add(new NavigationNode(element, true));
            element.SetActive(true);
        }
    }
    
    private void LogStack() {
        Debug.Log("Current UI Stack:");
        foreach (var layer in _uiLayers) {
            Debug.Log($"Layer: {string.Join(", ", layer.Select(n => $"{n.UiElement.name} ({n.IsActiveInLayer})"))}");
        }
    }

    private void HideLayer(int index) {
        var layerToHide = _uiLayers[index];
        foreach (var node in layerToHide) {
            node.UiElement.SetActive(false);
            // We don't set IsActiveInLayer to false to restore the previous layer when this will be popped
        }
    }

    private void HideAllLayers() {
        for (var i = 0; i < _uiLayers.Count; i++) HideLayer(i);
    }

    public void ShowUIElement(GameObject uiElement, bool hideLayer) {
        var currentLayer = _uiLayers.Last();
        
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

    public void PushUILayer(GameObject uiElement, UILayerPushOptions removePreviousLayer = UILayerPushOptions.None) {
        var newLayer = new List<NavigationNode>();
        
        /* Hide and remove layers */
        HideAllLayers();
        if (_uiLayers.Count > 1) {
            switch (removePreviousLayer) {
                case UILayerPushOptions.None:
                    break;
                case UILayerPushOptions.RemovePreviousLayer:
                    _uiLayers.RemoveAt(_uiLayers.Count - 1);
                    break;
                case UILayerPushOptions.RemoveAllPreviousLayers:
                    _uiLayers.RemoveRange(1, _uiLayers.Count - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(removePreviousLayer));
            }
        }
        
        /* If the same layer has already been pushed, remove it from the layers list */
        var indexToRemove = _uiLayers.FindIndex(layer => layer.Any(node => node.UiElement == uiElement));
        if (indexToRemove > 0) {
            _uiLayers.RemoveAt(indexToRemove);
        }

        var newNode = new NavigationNode(uiElement, true);
        newLayer.Add(newNode);
        uiElement.SetActive(true);

        _uiLayers.Add(newLayer);
    }

    public void HideUIElement(GameObject uiElement) {
        var elementInLayer = _uiLayers.Last().Find(x => x.UiElement == uiElement);
        if (elementInLayer != null) {
            elementInLayer.IsActiveInLayer = false;
            uiElement.SetActive(false);
        }
    }

    public void PopUILayer(GameObject elementInLayer = null) {
        if (_uiLayers.Count == 1) return;
        
        if (elementInLayer != null) {
            // return if the passed elementInLayer is not found in the layer to hide
            var layerToHide = _uiLayers.Last();
            var node = layerToHide.Find(x => x.UiElement == elementInLayer);
            if (node == null) return;
        }

        var hiddenLayer = _uiLayers.Last();
        foreach (var node in hiddenLayer) {
            node.UiElement.SetActive(false);
        }
        _uiLayers.RemoveAt(_uiLayers.Count - 1);
        
        // Reactivate the elements marked as IsActiveInLayer in the (now) current layer
        var currentLayer = _uiLayers.Last();
        foreach (var node in currentLayer.Where(node => node.IsActiveInLayer)) {
            node.UiElement.SetActive(true);
        }
    }
}

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

    private void HideLayer(int index, GameObject[] keepActiveNodesList = null) {
        var layerToHide = _uiLayers[index];
        
        var nodesToHide = layerToHide.Where(node => 
            keepActiveNodesList == null || !keepActiveNodesList.Contains(node.UiElement)
        );

        foreach (var node in nodesToHide) {
            node.UiElement.SetActive(false);
        }
    }

    private void HideAllLayers(GameObject[] keepActiveNodesList = null) {
        for (var i = 0; i < _uiLayers.Count; i++) HideLayer(i, keepActiveNodesList);
    }

    public void ShowUIElement(GameObject uiElement, bool hideLayer = false) {
        if (_uiLayers.Count == 0) return;
        
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

    public void PushUILayer(GameObject[] uiElements, UILayerPushOptions removePreviousLayer = UILayerPushOptions.None) {
        if (uiElements == null || uiElements.Length == 0) {
            Debug.LogWarning("UINavigator: tried to push an empty layer");
            return;
        }
        
        // Hide and remove layers 
        HideAllLayers(uiElements);
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
        
        var newLayer = new List<NavigationNode>();
        foreach (var uiElement in uiElements) {
            newLayer.Add(new NavigationNode(uiElement, true));
            uiElement.SetActive(true);
        }
        
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
        
        var layerToHide = _uiLayers.Last();
        
        if (elementInLayer != null) {
            // return if the passed elementInLayer is not found in the layer to hide
            var node = layerToHide.Find(x => x.UiElement == elementInLayer);
            if (node == null) return;
        }

        _uiLayers.RemoveAt(_uiLayers.Count - 1);
        var layerToRestore = _uiLayers.Last();

        // Activate all the elements in the layer to restore (the previous layer)
        foreach (var node in layerToRestore.Where(node => node.IsActiveInLayer)) {
            node.UiElement.SetActive(true);
        }
        
        // Hide all the elements from the layer to hide, provided they are not contained in the layer to restore
        foreach (var node in layerToHide) {
            var isSharedAndActive = layerToRestore.Any(n => n.UiElement == node.UiElement && n.IsActiveInLayer);
        
            if (!isSharedAndActive) {
                node.UiElement.SetActive(false);
            }
        }
    }
}

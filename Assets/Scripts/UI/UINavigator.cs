using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Utils.Extensions;

public class UINavigator : Singleton<UINavigator> {
    [FormerlySerializedAs("baseLayer")] [SerializeField] private CanvasGroup[] protectedLayer;

    private List<List<CanvasGroup>> _layers;

    protected override void Awake() {
        base.Awake();
        _layers = new List<List<CanvasGroup>>();
    }

    private void Start() {
        _layers.Add(new List<CanvasGroup>()); // protected layer, not to be popped
        foreach (var uiElement in protectedLayer) {
            _layers.Last().Add(uiElement);
        }
    }

    private void HideLayer(int index) {
        foreach (var uiElement in _layers[index]) {
            uiElement.SetVisibleAndInteractable(false);
        }
    }

    private void HideAllLayers() {
        for (var i = 0; i < _layers.Count; i++) HideLayer(i);
    }

    public void AddUIElementOnTopLayer(CanvasGroup uiElementToAdd, bool removeElementsOnSameLayer = false) {
        if (_layers.Count == 0) return;
        
        var topLayer = _layers.Last();

        var indexesToRemove = new List<int>();
        var containedInLayer = false;
        if (removeElementsOnSameLayer) {
            for(var i = 0; i < topLayer.Count; i++) {
                if (topLayer[i] != uiElementToAdd) {
                    topLayer[i].SetVisibleAndInteractable(false);
                    indexesToRemove.Add(i);
                }
                else {
                    containedInLayer = true;
                }
            }

            foreach (var index in indexesToRemove) {
                topLayer.RemoveAt(index);
            }
        }
        
        if (!containedInLayer) {
            topLayer.Add(uiElementToAdd);
        }
        
        uiElementToAdd.SetVisibleAndInteractable(true);
    }
    
    public void RemoveUIElementFromTopLayer(CanvasGroup uiElement) {
        if (_layers.Count == 0) return;

        var index = _layers.Last().IndexOf(uiElement);
        if (index != -1) {
            uiElement.SetVisibleAndInteractable(false);
            _layers.Last().RemoveAt(index);
        }
    }
    
    public void AddUIElementOnProtectedLayer(CanvasGroup uiElementToAdd) {
        if (_layers.Count == 0) return;
        
        var firstLayer = _layers[0];
        if (!firstLayer.Contains(uiElementToAdd)) {
            firstLayer.Add(uiElementToAdd);
        }
        
        uiElementToAdd.SetVisibleAndInteractable(true);
    }
    
    public void RemoveUIElementFromProtectedLayer(CanvasGroup uiElement) {
        if (_layers.Count == 0) return;
        
        var firstLayer = _layers[0];
        var index = firstLayer.IndexOf(uiElement);
        if (index != -1) {
            uiElement.SetVisibleAndInteractable(false);
            firstLayer.RemoveAt(index);
        }
    }

    public void PushUILayer(CanvasGroup uiElement, UILayerPushOptions options = UILayerPushOptions.None) {
        PushUILayer(new[] { uiElement }, options);
    }
    
    public void PushUILayer(CanvasGroup[] uiElements, UILayerPushOptions options = UILayerPushOptions.None) {
        if (uiElements.IsNullOrEmpty()) {
            Debug.LogWarning("UINavigator: tried to push an empty layer");
            return;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
        
        // Hide and remove layers 
        HideAllLayers();
        if (_layers.Count > 1) {
            switch (options) {
                case UILayerPushOptions.None:
                    break;
                case UILayerPushOptions.RemovePreviousLayer:
                    _layers.RemoveAt(_layers.Count - 1);
                    break;
                case UILayerPushOptions.RemoveAllPreviousLayers:
                    _layers.RemoveRange(1, _layers.Count - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options));
            }
        }

        var newLayer = new List<CanvasGroup>();
        foreach (var uiElement in uiElements) {
            uiElement.SetVisibleAndInteractable(true);
            newLayer.Add(uiElement);
        }
        _layers.Add(newLayer);
    }

    public void PopUILayer() {
        PopUILayer(null);
    }

    public void PopUILayer(CanvasGroup uiElementInLayer) {
        if (_layers.Count == 1) return;
        
        EventSystem.current.SetSelectedGameObject(null);
        
        var layerToHide = _layers.Last();
        
        if (uiElementInLayer != null) {
            if (!layerToHide.Contains(uiElementInLayer)) {
                return;
            }
        }

        foreach (var uiElement in layerToHide) {
            uiElement.SetVisibleAndInteractable(false);
        }
        _layers.RemoveAt(_layers.Count - 1);
        
        // Show all the elements in the layer to restore (the previous layer)
        var layerToRestore = _layers.Last();
        foreach (var uiElement in layerToRestore) {
            uiElement.SetVisibleAndInteractable(true);
        }
    }
}

using System;
using System.Collections.Generic;
using DG.Tweening;
using Inventory.Interfaces;
using Storage.Storables;
using UI;
using UnityEngine;
using Utils.SerializeInterface;

namespace Storage {
    public class StorageAddEffects : MonoBehaviour {
        [SerializeField] private UIPanelController uiPanelController;
        [SerializeField] private InterfaceReference<IStorage> storage;
        
        [Header("Animation Components")]
        [SerializeField] private DOTweenAnimation tweener;
        [SerializeField] private List<DOTweenAnimation> syncGroup;

        private void Start() {
            var storageValue = storage.Value;

            uiPanelController.OnCanvasGroupVisible += UIPanelController_OnCanvasGroupVisible;
            storageValue.OnAdd += IStorage_OnAdd;

            if (tweener.tween == null) tweener.CreateTween();

            if (tweener.tween == null) {
                Debug.LogError($"[StorageAddEffects] The DOTweenAnimation component on  '{tweener.gameObject.name}' was not configured properly.");
                return;
            }

            if (!storageValue.IsEmpty() && !tweener.tween.IsPlaying()) tweener.DOPlay();
        }

        private void OnDestroy() {
            var storageValue = storage.Value;

            uiPanelController.OnCanvasGroupVisible -= UIPanelController_OnCanvasGroupVisible;
            storageValue.OnAdd -= IStorage_OnAdd;
            storageValue.OnRemove -= IStorage_OnAdd;
        }

        private void UIPanelController_OnCanvasGroupVisible() {
            if (tweener.tween.IsPlaying()) {
                tweener.DORewind();
            }
        }

        private void IStorage_OnAdd(Storable _) {
            if (!uiPanelController.IsVisibleAndInteractable() && !tweener.tween.IsPlaying()) {
                SyncAndPlay();
            }
        }
        
        private void SyncAndPlay() {
            var syncTime = -1f;

            if (syncGroup != null) {
                foreach (var otherAnim in syncGroup) {
                    if (otherAnim == tweener) continue;

                    if (otherAnim.tween != null && otherAnim.tween.IsPlaying()) {
                        syncTime = otherAnim.tween.Elapsed();
                        break; 
                    }
                }
            }

            if (syncTime >= 0) tweener.tween.Goto(syncTime);
            tweener.DOPlay();
        }
    }
}

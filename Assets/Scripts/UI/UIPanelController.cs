using System;
using UI.Interfaces;
using UnityEngine;
using Utils.Extensions;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanelController : MonoBehaviour {
        private CanvasGroup _canvasGroup;
        private IUIView[] _childViews;

        public event Action OnCanvasGroupVisible;

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _childViews = GetComponentsInChildren<IUIView>(true);
        }

        public void SetVisibleAndInteractable(bool visible) {
            _canvasGroup.SetVisibleAndInteractable(visible);

            if (_childViews != null) {
                foreach (var view in _childViews) {
                    if (visible) view.OnViewShow();
                    else view.OnViewHide();
                }
            }
            
            OnCanvasGroupVisible?.Invoke();
        }

        public bool IsVisibleAndInteractable() {
            return _canvasGroup.IsVisibleAndInteractable();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using UltEvents;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UI {
    [RequireComponent(typeof(FSMOwner))]
    public class UIButtonNavigationEvents : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, 
        IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler 
    {
        private const string FSM_ASSET_GUID = "3abc85d0f27a1464684a4620ad9031f6";

        [SerializeField] private bool debugLogging;
        [SerializeField] private InputReaderSO inputReader;
        
        [Header("Navigation Events")]
        public UltEvent onNormalEnter;
        public UltEvent onNormalExit;
        public UltEvent onHighlightEnter;
        public UltEvent onHighlightExit;
        public UltEvent onPressEnter;
        public UltEvent onPressExit;
        public UltEvent onSelectEnter;
        public UltEvent onSelectStayEnter;
        public UltEvent onSelectStayExit;
        public UltEvent onSelectExit;

        private bool _isHighlighted; 
        public bool IsHighlighted { get => _isHighlighted ; private set => _isHighlighted = value; }
        private bool _isPressed; 
        public bool IsPressed { get => _isPressed ; private set => _isPressed = value; }
        private bool _isSelected; 
        public bool IsSelected { get => _isSelected ; private set => _isSelected = value; }

        private const string EventsComponentVar = "NavigationEventsComponent";
        
        private FSMOwner _fsmOwner;
        private IBlackboard _blackboard;
        private Coroutine _deselectRoutine;
        
        // Memory to handle the ghost selection bug
        private int _selectionFrame = -1;
        private bool _wasSelectedBeforeClick;
        
        private void Awake() {
            if (!inputReader) Debug.LogError("[UIButtonNavigationEvents] Missing InputReaderSO Reference");
            if (!_fsmOwner) _fsmOwner = GetComponent<FSMOwner>();
        }
        
        private void Start() {
            _blackboard = _fsmOwner.graph.blackboard;
            _blackboard.SetVariableValue(EventsComponentVar, this);
            _fsmOwner.UpdateBehaviour();
        }

#if UNITY_EDITOR
        private void Reset() {
            _fsmOwner = GetComponent<FSMOwner>();
            _fsmOwner.updateMode = Graph.UpdateMode.Manual;
        
            if (_fsmOwner != null && _fsmOwner.graph == null) {
                var assetPath = AssetDatabase.GUIDToAssetPath(FSM_ASSET_GUID);
            
                if (!string.IsNullOrEmpty(assetPath)) {
                    var graph = AssetDatabase.LoadAssetAtPath<FSM>(assetPath);
                
                    if (graph != null) {
                        _fsmOwner.graph = graph;
                        _fsmOwner.blackboard = null;
                        
                        if (TryGetComponent<Blackboard>(out var existingBB)) {
                            existingBB.enabled = false;
                            Destroy(existingBB);
                        }
                        
                        Debug.Log($"[UIButtonNavigationEventsScript] FSM '{graph.name}' automatically assigned!", this);
                    } else {
                        Debug.LogError($"[UIButtonNavigationEventsScript] Unable to load FSM Asset at path: {assetPath}");
                    }
                } else {
                    Debug.LogWarning("[UIButtonNavigationEventsScript] FSM Asset GUID not valid or asset moved. Drag a FSM Asset reference in the inspector.");
                }
            }
        }
#endif
        
        public void EnterNormalState() => onNormalEnter?.Invoke();
        public void ExitNormalState() => onNormalExit?.Invoke();
        public void EnterHighlightedState() => onHighlightEnter?.Invoke();
        public void ExitHighlightedState()  => onHighlightExit?.Invoke();
        public void EnterPressedState() => onPressEnter?.Invoke();
        public void ExitPressedState() => onPressExit?.Invoke();
        public void EnterSelectedState() => onSelectEnter?.Invoke();
        public void ExitSelectedState() => onSelectExit?.Invoke();
        public void EnterSelectedStayState() => onSelectStayEnter?.Invoke();
        public void ExitSelectedStayState() => onSelectStayExit?.Invoke();
        
        public void OnPointerEnter(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnPointerEnter");
            
            UpdateState(ref _isHighlighted, true);
        }
        
        public void OnPointerExit(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnPointerExit");
            
            UpdateState(ref _isHighlighted, false);
        }
        
        public void OnPointerDown(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnPointerDown");
            
            var isSelectedNow = EventSystem.current.currentSelectedGameObject == gameObject;
            _wasSelectedBeforeClick = isSelectedNow && _selectionFrame != Time.frameCount;
            
            UpdateState(ref _isPressed, true);
        }

        public void OnPointerUp(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnPointerUp");

            UpdateState(ref _isPressed, false);
            
            // Pointer release on this game object
            if (d.pointerEnter == gameObject) {
                if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnPointerUp on this GameObject");
                
                if (EventSystem.current.currentSelectedGameObject != gameObject) {
                    // Force the EventSystem to select this GameObject now
                    EventSystem.current.SetSelectedGameObject(gameObject);
                }
                else {
                    if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] Selection Finalized");
                    
                    // Update the FSM
                    UpdateState(ref _isSelected, true);
                }
            }
            // Pointer release outside of this game object
            else {
                if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnPointerUp on GameObject: {d.pointerEnter}, current selected: {EventSystem.current.currentSelectedGameObject}");
                
                if (!_wasSelectedBeforeClick) {
                    // If the pointer was pressed on this game object, and it was not already selected, cancel the selection
                    if (EventSystem.current.currentSelectedGameObject == gameObject) {
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                    UpdateState(ref _isSelected, false);
                }
                else {
                    // Otherwise keep the old selection
                    UpdateState(ref _isSelected, true);
                }
            }
        }

        public void OnSelect(BaseEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnSelect Entered");

            _selectionFrame = Time.frameCount;
            
            // Avoid selecting while the pointer is down on this object (selection should be performed on release)
            if (inputReader.IsLmbPressed) {
                if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] Selection Skipped");
                return;
            }
            
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] Selection Finalized");
            
            UpdateState(ref _isSelected, true);
        }
        
        public void OnDeselect(BaseEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] OnDeselect Entered");
            
            // Perform the deselection only after the pointer is up
            if (inputReader.IsLmbPressed) {
                if (_deselectRoutine != null) StopCoroutine(_deselectRoutine);
                _deselectRoutine = StartCoroutine(DelayedDeselect());
            }
            else {
                if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] Deselection Finalized");
                UpdateState(ref _isSelected, false);
            }
        }
        
        private IEnumerator DelayedDeselect() {
            yield return new WaitUntil(() => !inputReader.IsLmbPressed);
            
            // Wait an extra frame to let the (possible) new selection execute its OnPointerUp method and choose whether to confirm the selection or not
            yield return null; 

            var currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == null) {
                if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] Reclaiming Selection (the new target was null)");
                
                // Reclaim the selection
                EventSystem.current.SetSelectedGameObject(gameObject);
                UpdateState(ref _isSelected, true);
            }
            else if (currentSelected != gameObject) {
                if (debugLogging) Debug.Log($"[UIButtonNavigationEventsScript - {gameObject.name}] Deselection Finalized (Target changed to {currentSelected.name})");
                
                // Confirm the deselection
                UpdateState(ref _isSelected, false);
            }
            
            _deselectRoutine = null;
        }

        private void UpdateState(ref bool stateVar, bool value) {
            stateVar = value;
            _fsmOwner.UpdateBehaviour(); 
        }
    }
}
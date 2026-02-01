using System;
using System.Collections;
using Audio;
using Enums;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using UltEvents;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(FSMOwner), typeof(Selectable))]
    public class UIButtonStateController : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, 
        IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler,
        IPointerClickHandler, ISubmitHandler
    {
        private const string FSM_ASSET_GUID = "3abc85d0f27a1464684a4620ad9031f6";
        
        [SerializeField] private InputReaderSO inputReader;

        [Header("Settings")] 
        [SerializeField] private bool playButtonSound = true;
        [SerializeField] private bool debugLogging;
        
        [Header("Audio Configuration")] 
        [SerializeField] private UISoundType pressSoundType = UISoundType.None;
        [SerializeField] private UISoundType releaseSoundType = UISoundType.None;
        [SerializeField] private UISoundType submitSoundType = UISoundType.Submit;
        
        [Header("Internal References")]
        [SerializeField] private Selectable targetSelectable;
        [SerializeField] private FSMOwner fsmOwner;

        [Header("Logic Events")] 
        public UltEvent onSubmit;
        
        [Header("Navigation Events")]
        public UltEvent onNormalEnter;
        public UltEvent onNormalExit;
        public UltEvent onHighlightEnter;
        public UltEvent onHighlightExit;
        public UltEvent onHighlightStayEnter;
        public UltEvent onHighlightStayExit;
        public UltEvent onPressEnter;
        public UltEvent onPressExit;
        public UltEvent onSelectEnter;
        public UltEvent onSelectStayEnter;
        public UltEvent onSelectStayExit;
        public UltEvent onSelectExit;
        public UltEvent onDisabledEnter;
        public UltEvent onDisabledExit;

        private bool _isHighlighted; 
        public bool IsHighlighted { get => _isHighlighted ; private set => _isHighlighted = value; }
        private bool _isPressed; 
        public bool IsPressed { get => _isPressed ; private set => _isPressed = value; }
        private bool _isSelected; 
        public bool IsSelected { get => _isSelected ; private set => _isSelected = value; }
        private bool _isDisabled;
        public bool IsDisabled { get => _isDisabled ; private set => _isDisabled = value; }

        private const string EventsComponentVar = "StateController";
        
        private IBlackboard _blackboard;
        private Coroutine _deselectRoutine;
        
        // Memory to handle the ghost selection bug
        private int _selectionFrame = -1;
        private bool _wasSelectedBeforeClick;
        
        // FIX: Variabile per ignorare la selezione automatica al "risveglio"
        private int _enableFrame = -1;
        
        private void Awake() {
            if (!targetSelectable) Debug.LogError($"[UIButtonStateController - {gameObject.name}] Missing Target Selectable");
            if (!inputReader) Debug.LogError($"[UIButtonStateController - {gameObject.name}] Missing InputReaderSO Reference");
            if (!fsmOwner) Debug.LogError($"[UIButtonStateController - {gameObject.name}] Missing FSMOwner Reference");
        }
        
        private void OnEnable() {
            CheckInteractableState();
            if (fsmOwner.isRunning) fsmOwner.UpdateBehaviour(); 
        }
        
        private void Start() {
            _blackboard = fsmOwner.graph.blackboard;
            _blackboard.SetVariableValue(EventsComponentVar, this);
            
            if (!fsmOwner.isRunning) fsmOwner.StartBehaviour(); 
            else fsmOwner.UpdateBehaviour();

            BindAudioEvents();
        }

        private void OnDisable() {
            ResetInternalState();
            UpdateState(ref _isDisabled, true);
        }

        private void OnDestroy() {
            onNormalEnter.RemoveAllListeners();
            onNormalExit.RemoveAllListeners();
            onHighlightEnter.RemoveAllListeners();
            onHighlightExit.RemoveAllListeners();
            onHighlightStayEnter.RemoveAllListeners();
            onHighlightStayExit.RemoveAllListeners();
            onPressEnter.RemoveAllListeners();
            onPressExit.RemoveAllListeners();
            onSelectEnter.RemoveAllListeners();
            onSelectExit.RemoveAllListeners();
            onSelectStayEnter.RemoveAllListeners();
            onSelectStayExit.RemoveAllListeners();
            onDisabledEnter.RemoveAllListeners();
            onDisabledExit.RemoveAllListeners();
            onSubmit.RemoveAllListeners();
        }

#if UNITY_EDITOR
        private void Reset() {
            targetSelectable = GetComponent<Selectable>();
            
            fsmOwner = GetComponent<FSMOwner>();
            fsmOwner.updateMode = Graph.UpdateMode.Manual;
        
            if (fsmOwner != null && fsmOwner.graph == null) {
                var assetPath = AssetDatabase.GUIDToAssetPath(FSM_ASSET_GUID);
            
                if (!string.IsNullOrEmpty(assetPath)) {
                    var graph = AssetDatabase.LoadAssetAtPath<FSM>(assetPath);
                
                    if (graph != null) {
                        fsmOwner.graph = graph;
                        fsmOwner.blackboard = null;
                        
                        if (TryGetComponent<Blackboard>(out var existingBB)) {
                            existingBB.enabled = false;
                            Destroy(existingBB);
                        }
                        
                        Debug.Log($"[UIButtonStateController] FSM '{graph.name}' automatically assigned!", this);
                    } else {
                        Debug.LogError($"[UIButtonStateController] Unable to load FSM Asset at path: {assetPath}");
                    }
                } else {
                    Debug.LogWarning("[UIButtonStateController] FSM Asset GUID not valid or asset moved. Drag a FSM Asset reference in the inspector.");
                }
            }
        }
#endif

        private void BindAudioEvents() {
            if (submitSoundType != UISoundType.None) {
                onSubmit.AddListener(() => AudioManager.Instance.PlayUISound(submitSoundType));
            }

            if (pressSoundType != UISoundType.None) {
                onPressEnter.AddListener(() => AudioManager.Instance.PlayUISound(pressSoundType));
            }
            
            if (releaseSoundType != UISoundType.None) {
                onPressEnter.AddListener(() => AudioManager.Instance.PlayUISound(releaseSoundType));
            }
        }
        
        private void ResetInternalState() {
            _isHighlighted = false;
            _isPressed = false;
            _isSelected = false;
            
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject) {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
        
        private void OnCanvasGroupChanged() {
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnCanvasGroupChanged");
            CheckInteractableState();
        }
        
        private void CheckInteractableState() {
            if (targetSelectable == null) return;
            
            // IsInteractable() checks both .interactable and parent CanvasGroups
            var isInteractable = targetSelectable.IsInteractable();
            
            // Se lo stato cambia da False a True (diventiamo attivi), segniamo il frame corrente.
            if (isInteractable && _isDisabled) {
                _enableFrame = Time.frameCount;
            }
            
            if (isInteractable == false) ResetInternalState();
            UpdateState(ref _isDisabled, !isInteractable);
        }

        public void SetInteractable(bool isInteractable) {
            targetSelectable.interactable = isInteractable;
            
            // Se stiamo abilitando il bottone, segniamo il frame per ignorare selezioni "ghost" immediate
            if (isInteractable) {
                _enableFrame = Time.frameCount;
            }

            if (isInteractable == false) ResetInternalState();
            UpdateState(ref _isDisabled, !isInteractable);
        }
        
        public void EnterNormalState() => onNormalEnter?.Invoke();
        public void ExitNormalState() => onNormalExit?.Invoke();
        public void EnterHighlightedState() => onHighlightEnter?.Invoke();
        public void ExitHighlightedState()  => onHighlightExit?.Invoke();
        public void EnterHighlightedStayState() => onHighlightStayEnter?.Invoke();
        public void ExitHighlightedStayState() => onHighlightStayExit?.Invoke();
        public void EnterPressedState() => onPressEnter?.Invoke();
        public void ExitPressedState() => onPressExit?.Invoke();
        public void EnterSelectedState() => onSelectEnter?.Invoke();
        public void ExitSelectedState() => onSelectExit?.Invoke();
        public void EnterSelectedStayState() => onSelectStayEnter?.Invoke();
        public void ExitSelectedStayState() => onSelectStayExit?.Invoke();
        public void EnterDisabledState() => onDisabledEnter?.Invoke();
        public void ExitDisabledState() => onDisabledExit?.Invoke();
        
        public void OnPointerEnter(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnPointerEnter");
            
            UpdateState(ref _isHighlighted, true);
        }
        
        public void OnPointerExit(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnPointerExit");
            
            UpdateState(ref _isHighlighted, false);
        }
        
        public void OnPointerDown(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnPointerDown");
            
            var isSelectedNow = EventSystem.current.currentSelectedGameObject == gameObject;
            _wasSelectedBeforeClick = isSelectedNow && _selectionFrame != Time.frameCount;
            
            UpdateState(ref _isPressed, true);
        }

        public void OnPointerUp(PointerEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnPointerUp");

            UpdateState(ref _isPressed, false);
            
            // Pointer release on this game object
            if (d.pointerEnter == gameObject) {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnPointerUp on this GameObject");
                
                if (EventSystem.current.currentSelectedGameObject != gameObject) {
                    // Force the EventSystem to select this GameObject now
                    EventSystem.current.SetSelectedGameObject(gameObject);
                }
                else {
                    if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Selection Finalized");
                    
                    // Update the FSM
                    UpdateState(ref _isSelected, true);
                }
            }
            // Pointer release outside of this game object
            else {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnPointerUp on GameObject: {d.pointerEnter}, current selected: {EventSystem.current.currentSelectedGameObject}");
                
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
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnSelect Entered");

            _selectionFrame = Time.frameCount;
            
            // FIX: Se questo evento OnSelect avviene nello stesso frame in cui
            // siamo stati riabilitati (SetInteractable true), è un residuo di Unity. Ignoralo.
            if (_enableFrame == Time.frameCount) {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Ignoring Auto-Select on Enable");
                return;
            }
            
            // Avoid selecting while the pointer is down on this object (selection should be performed on release)
            if (inputReader.IsLmbPressed) {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Selection Skipped");
                return;
            }
            
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Selection Finalized");
            
            UpdateState(ref _isSelected, true);
        }
        
        public void OnDeselect(BaseEventData d) {
            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] OnDeselect Entered");
            
            if (_isDisabled || targetSelectable.IsInteractable() == false) {
                UpdateState(ref _isSelected, false);
                return;
            }
            
            // Perform the deselection only after the pointer is up
            if (inputReader.IsLmbPressed) {
                if (_deselectRoutine != null) StopCoroutine(_deselectRoutine);
                _deselectRoutine = StartCoroutine(DelayedDeselect());
            }
            else {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Deselection Finalized");
                UpdateState(ref _isSelected, false);
            }
        }
        
        public void OnPointerClick(PointerEventData eventData) {
            if (_isDisabled) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;

            if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Clicked via Pointer");
            
            onSubmit?.Invoke();
        }

        public void OnSubmit(BaseEventData eventData) {
            if (_isDisabled) return;

            if (debugLogging) Debug.Log($"[UIButtonStateController] Clicked via Submit: {gameObject.name}");
            
            onSubmit?.Invoke();
        }
        
        private IEnumerator DelayedDeselect() {
            yield return new WaitUntil(() => !inputReader.IsLmbPressed);
            
            // Wait an extra frame to let the (possible) new selection execute its OnPointerUp method and choose whether to confirm the selection or not
            yield return null; 
            
            // If the selectable gets disabled during the coroutine, deselect it
            if (_isDisabled || targetSelectable.IsInteractable() == false) {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] DelayedDeselect Aborted (Object disabled)");
                UpdateState(ref _isSelected, false);
                _deselectRoutine = null;
                yield break;
            }

            var currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected == null) {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Reclaiming Selection (the new target was null)");
                
                // Reclaim the selection
                EventSystem.current.SetSelectedGameObject(gameObject);
                UpdateState(ref _isSelected, true);
            }
            else if (currentSelected != gameObject) {
                if (debugLogging) Debug.Log($"[UIButtonStateController - {gameObject.name}] Deselection Finalized (Target changed to {currentSelected.name})");
                
                // Confirm the deselection
                UpdateState(ref _isSelected, false);
            }
            
            _deselectRoutine = null;
        }

        private void UpdateState(ref bool stateVar, bool value) {
            stateVar = value;
            if (fsmOwner.isRunning) fsmOwner.UpdateBehaviour(); 
        }
    }
}
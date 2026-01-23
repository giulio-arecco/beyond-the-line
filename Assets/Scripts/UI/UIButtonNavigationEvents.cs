using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using UltEvents;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    [RequireComponent(typeof(FSMOwner))]
    public class UIButtonNavigationEvents : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, 
        IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler 
    {
        [SerializeField ] private string fsmAssetGuid = "3abc85d0f27a1464684a4620ad9031f6"; 
    
        [Header("Navigation Events")]
        [SerializeField] private UltEvent onNormalEnter;
        [SerializeField] private UltEvent onNormalExit;
        [SerializeField] private UltEvent onHighlightEnter;
        [SerializeField] private UltEvent onHighlightExit;
        [SerializeField] private UltEvent onPress;
        [SerializeField] private UltEvent onRelease;
        [SerializeField] private UltEvent onSelect;
        [SerializeField] private UltEvent onDeselect;

        private const string EventsComponentVar = "NavigationEventsComponent";
        private const string HoverVar = "IsHovered";
        private const string PressVar = "IsPressed";
        private const string SelectVar = "IsSelected";
        
        private FSMOwner _fsmOwner;
        private IBlackboard _blackboard;
        
        private void Awake() {
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
                var assetPath = AssetDatabase.GUIDToAssetPath(fsmAssetGuid);
            
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
                    Debug.LogWarning("[UIButtonNavigationEventsScript] FSM Asset GUID not valid or asset moved. Update the GUID in the script.");
                }
            }
        }
#endif
        
        public void TriggerEnterNormal() => onNormalEnter?.Invoke();
        public void TriggerExitNormal() => onNormalExit?.Invoke();
        public void TriggerEnterHover() => onHighlightEnter?.Invoke();
        public void TriggerExitHover()  => onHighlightExit?.Invoke();
        public void TriggerPress() => onPress?.Invoke();
        public void TriggerRelease() => onRelease?.Invoke();
        public void TriggerSelect() => onSelect?.Invoke();
        public void TriggerDeselect() => onDeselect?.Invoke();
        
        public void OnPointerEnter(PointerEventData d) => UpdateState(HoverVar, true);
        public void OnPointerExit(PointerEventData d)  => UpdateState(HoverVar, false);
        public void OnPointerDown(PointerEventData d)  => UpdateState(PressVar, true);
        public void OnPointerUp(PointerEventData d)    => UpdateState(PressVar, false);
        public void OnSelect(BaseEventData d)          => UpdateState(SelectVar, true);
        public void OnDeselect(BaseEventData d)        => UpdateState(SelectVar, false);

        private void UpdateState(string bbVarName, bool value) {
            _blackboard.SetVariableValue(bbVarName, value);
            _fsmOwner.UpdateBehaviour(); 
        }
    }
}
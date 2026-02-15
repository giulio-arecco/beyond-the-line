using System;
using System.Collections.Generic;
using Audio;
using Enums;
using Ink.Runtime;
using Inventory.Interfaces;
using Storage.StorableInfoDatabase;
using Storage.Storables;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.SerializeInterface;
using DG.Tweening;

namespace Narrative {
    public class StoryManager : Singleton<StoryManager> {
        [Header("Input Reader")]
        [SerializeField] private InputReaderSO input;

        [Header("Story UI")] 
        [SerializeField] private UIPanelController storyPanel;
        [SerializeField] private TextMeshProUGUI storyText;
    
        [Header("Choice UI")]
        [SerializeField] private GameObject[] choices;
    
        [Header("Story Global Variables")]
        [SerializeField] private TextAsset globalsInkJson;

        [Header("TypeWriter Effect Settings")] 
        [SerializeField] private float speed = 30f;
    
        [Header("External Dependencies")] 
        [SerializeField] private InterfaceReference<IStorage<Item>> playerInventory;
        [SerializeField] private InterfaceReference<IStorage<Companion>> playerCompanions;
        [SerializeField] private ItemInfoDatabaseSO itemInfoDatabase;
        [SerializeField] private CompanionInfoDatabaseSO companionInfoDatabase;
        [SerializeField] private MusicLibrarySO musicLibrary;
    
        private TextMeshProUGUI[] _choicesText;
        private Story _currentStory;
        private Queue<OptionalStory> _optionalStories;
        private StoryVariablesRegistry _storyVariablesRegistry; 
        private StoryFunctionsBinder _storyFunctionsBinder;
        private Tween _typewriterTween;
        private string _bufferedLine;
    
        public bool StoryIsProgressing { get; private set; }

        public event Action OnStoryEnter;
        public event Action OnStoryExit;

        protected override void Awake() {
            base.Awake();
            _optionalStories = new Queue<OptionalStory>();
            _storyVariablesRegistry = new StoryVariablesRegistry(globalsInkJson);
            _storyFunctionsBinder = new StoryFunctionsBinder(playerInventory.Value, playerCompanions.Value, itemInfoDatabase, companionInfoDatabase, musicLibrary);
        }

        private void OnEnable() {
            // Subscribe to input events and enable input actions
            input.ContinueStory += Input_ContinueStory;
        }

        private void OnDisable() {
            input.ContinueStory -= Input_ContinueStory;
        }

        private void Start() {
            // Get all the choices text
            _choicesText = new TextMeshProUGUI[choices.Length];
            for (var i = 0; i < choices.Length; i++) {
                _choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            }
            
            HideChoices();
        }

        private void ExitStory() {
            StoryIsProgressing = false;
            UINavigator.Instance.RemoveUIElementFromProtectedLayer(storyPanel);
            storyText.text = "";
            _bufferedLine = null;
        
            _storyVariablesRegistry.StopListening(_currentStory);
            _storyFunctionsBinder.UnbindGlobalFunctions(_currentStory);
            
            Debug.Log("Exiting story");
            
            OnStoryExit?.Invoke();
        }

        private void ShowLine(string textToType) {
            _typewriterTween?.Kill();

            // Setup
            storyText.text = textToType;
            storyText.maxVisibleCharacters = 0;

            // Compute the animation duration based on the length of the text to display to achieve constant speed
            var duration = textToType.Length / speed;

            _typewriterTween = DOTween.To(
                    () => storyText.maxVisibleCharacters, 
                    x => storyText.maxVisibleCharacters = x, textToType.Length, duration
                    )
                .SetEase(Ease.Linear)
                .OnComplete(OnLineTypingFinished);

            _typewriterTween.Play();
        }

        private void OnLineTypingFinished() {
            _typewriterTween = null;

            // Display choices if available
            if (_currentStory.currentChoices.Count > 0) {
                DisplayChoices();
                return;
            }

            // Otherwise, check if the choices are guarded by any logic (we need to continue through the ink story to find out)
            if (_currentStory.canContinue) {
                LookAheadForLogicOrText();
            }
        }
        
        private void LookAheadForLogicOrText() {
            // Continue until we find text or choices
            while (_currentStory.canContinue && _currentStory.currentChoices.Count == 0) {
                var text = _currentStory.Continue();
                
                if (!string.IsNullOrWhiteSpace(text)) {
                    // We found narrative text
                    _bufferedLine = text;
                    return; 
                }
                
                // Otherwise the line was empty (ink logic): keep looping
            }

            
            if (_currentStory.currentChoices.Count > 0) {
                // The loop ended because it found choices to display (they were guarded by some logic)
                DisplayChoices();
            }
        }

        private void SkipTypingAnimation() {
            if (_typewriterTween != null && _typewriterTween.IsActive()) {
                _typewriterTween.Complete(); 
                _typewriterTween = null;
            }
        }
        
        private string ParseCustomMarkers(string line) {
            string parsedLine;
            parsedLine = line.Replace("<nl>", "\n");
            
            return parsedLine;
        }

        private void HandleStoryFlow() {
            // Check if there is a buffered line from the previous lookahead
            if (!string.IsNullOrEmpty(_bufferedLine)) {
                var line = _bufferedLine;
                _bufferedLine = null;
                
                line = ParseCustomMarkers(line);
                ShowLine(line);
                return;
            }
            
            // If there is no buffered line, proceed normally
            while (_currentStory.canContinue) {
                var line = _currentStory.Continue();
        
                // The current line contains visible text
                if (!string.IsNullOrWhiteSpace(line)) {
                    line = ParseCustomMarkers(line);
                    ShowLine(line);
                    return;
                }
            }
    
            // Either canContinue was false (choice selection) or canContinue was true, but the line was empty (internal ink logical checks)
            // We have to check if there's any available choice before considering the story completed
            if (_currentStory.currentChoices.Count > 0) {
                DisplayChoices();
                return;
            }
    
            // The story is completed
            var optionalStory = FindPlayableOptionalStory();
            if (optionalStory != null) {
                EnterOptionalStory(optionalStory);
            }
            else {
                ExitStory();
            }
        }
    
        private OptionalStory FindPlayableOptionalStory() {
            while (_optionalStories.Count > 0) {
                var optionalStory = _optionalStories.Dequeue();
                if (!optionalStory.IsPlayable) continue;
                
                if (!optionalStory.IsReplayable) optionalStory.IsPlayable = false;

                return optionalStory;
            }

            return null;
        }
    
        private void EnterOptionalStory(OptionalStory optionalStory) {
            StoryIsProgressing = true;
        
            _storyVariablesRegistry.StopListening(_currentStory);
            _storyFunctionsBinder.UnbindGlobalFunctions(_currentStory);
            
            var story = new Story(optionalStory.InkJson.text);
            Debug.Log($"Entering optional story '{optionalStory.InkJson.name}'.");
        
            _storyVariablesRegistry.StartListening(story);
            _storyFunctionsBinder.BindGlobalFunctions(story);
            _currentStory = story;
            
            HandleStoryFlow();
        }

        private void HideChoices() {
            foreach(var choice in choices) {
                choice.SetActive(false);
            }
        }

        private void DisplayChoices() {
            EventSystem.current.SetSelectedGameObject(null);
            
            var currentChoices = _currentStory.currentChoices;
        
            // check if the UI can support the number of choices coming in
            if (currentChoices.Count > choices.Length) {
                Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
            }

            int i;
            // enable and initialize the choices up to the amount of choices for this story line
            for (i = 0; i < currentChoices.Count; i++) {
                choices[i].gameObject.SetActive(true);
                _choicesText[i].text = currentChoices[i].text;
            }
        
            // go through the remaining choices the UI supports and make sure they're hidden
            // for (; i < choices.Length; i++) {
            //     choices[i].gameObject.SetActive(false);
            // }
        
            // automatically select the first choice button
            // if (choices[0].TryGetComponent<Button>(out var choiceButton)) {
            //     choiceButton.Select();
            // }
        }
    
        private void Input_ContinueStory() {
            if (!StoryIsProgressing) return;
    
            if (_typewriterTween != null && _typewriterTween.IsActive()) {
                SkipTypingAnimation();
            }
            else if (_currentStory.currentChoices.Count == 0 || !string.IsNullOrEmpty(_bufferedLine)) {
                // Proceed if there are no available choices or if there is a buffered line to be displayed
                HandleStoryFlow();
            }
        }
    
        public void EnterStory(TextAsset inkJson) {
            _bufferedLine = null;
            _currentStory = new Story(inkJson.text);
            StoryIsProgressing = true;
        
            UINavigator.Instance.AddUIElementOnProtectedLayer(storyPanel);
            
            Debug.Log($"Entering story '{inkJson.name}'.");
        
            _storyVariablesRegistry.StartListening(_currentStory);
            _storyFunctionsBinder.BindGlobalFunctions(_currentStory);
            
            OnStoryEnter?.Invoke();
        
            HandleStoryFlow();
        }

        public void EnqueueOptionalStory(OptionalStory optionalStory) {
            _optionalStories.Enqueue(optionalStory);
        }

        public void MakeChoice(int choiceIndex) {
            Debug.Log("Chosen choice with index: " + choiceIndex);
            
            _currentStory.ChooseChoiceIndex(choiceIndex);
            HideChoices();
            
            HandleStoryFlow();
        }

        public Ink.Runtime.Object GetRegistryVariable(string variableName) {
            _storyVariablesRegistry.Variables.TryGetValue(variableName, out var registryVariable);
            
            if (registryVariable == null) {
                Debug.LogWarning($"Ink Variable '{variableName}' does not exist.");
                return null;
            }
            
            if (registryVariable.VariableValue == null) {
                Debug.LogWarning($"Ink Variable '{variableName}' is null");
                return null;
            }
            
            return registryVariable.VariableValue;
        }

        public Dictionary<string, object> GetRegistryVariables() {
            return _storyVariablesRegistry.GetCurrentValues();
        }
        
        public Dictionary<string, object> GetRegistryVariables(string prefix) {
            return _storyVariablesRegistry.GetCurrentValues(prefix);
        }
        
        public Dictionary<string, object> GetRegistryVariables(string[] variablesToFilter) {
            return _storyVariablesRegistry.GetCurrentValues(variablesToFilter);
        }
    
        public bool EvaluateConditionOnStoryVariable<T>(string variableName, T value, ComparisonType comparisonType) where T : IComparable {
            return _storyVariablesRegistry.CompareVariableTo(variableName, value, comparisonType);
        }

        public void SubscribeToVariableChange(string variableName, Action<Ink.Runtime.Object> onValueChanged, object currentValue = null) {
            if (currentValue != null) {
                var inkValue = _storyVariablesRegistry.Variables[variableName].VariableValue;
                
                // Run some sanity checks before subscribing to the variable change action
                // Extract the primitive value based on the real type of the Ink.Runtime.Object
                object unboxedInkValue = inkValue switch {
                    IntValue intVal => intVal.value,
                    FloatValue floatVal => floatVal.value,
                    StringValue strVal => strVal.value,
                    BoolValue boolVal => boolVal.value,
                    _ => throw new InvalidOperationException($"Unhandled Ink type: {inkValue.GetType().Name}")
                };

                // Check whether the passed in value and the unboxed ink value match
                if (unboxedInkValue != null && unboxedInkValue.GetType() == currentValue.GetType()) {
                    if (!unboxedInkValue.Equals(currentValue)) {
                        Debug.LogError(
                            $"The {variableName}'s global ink variable value ({unboxedInkValue}) is not equal to the provided variable" +
                            $" value ({currentValue})");
                    }
                }
                else {
                    Debug.LogError(
                        $"The {variableName}'s global ink variable type ({unboxedInkValue?.GetType()}) is not equal to the provided variable" +
                        $" value ({currentValue.GetType()})");
                }
            }

            _storyVariablesRegistry.Variables[variableName].OnValueChanged += onValueChanged;
        }

        public void UnsubscribeFromVariableChange(string variableName, Action<Ink.Runtime.Object> onValueChanged) => 
            _storyVariablesRegistry.Variables[variableName].OnValueChanged -= onValueChanged;
    }
}

using System;
using System.Collections.Generic;
using Enums;
using Ink.Runtime;
using Ink.UnityIntegration;
using Inventory.Interfaces;
using Storage.StorableInfoDatabase;
using Storage.Storables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.SerializeInterface;

namespace Narrative {
    public class StoryManager : Singleton<StoryManager> {
        [Header("Input Reader")]
        [SerializeField] private InputReaderSO input;

        [Header("Story UI")] 
        [SerializeField] private GameObject storyPanel;
        [SerializeField] private TextMeshProUGUI storyText;
    
        [Header("Choice UI")]
        [SerializeField] private GameObject[] choices;
    
        [Header("Story Global Variables")]
        [SerializeField] private InkFile globalsInkFile;
    
        [Header("External Dependencies")] 
        [SerializeField] private InterfaceReference<IStorage<Item>> playerInventory;
        [SerializeField] private InterfaceReference<IStorage<Companion>> playerCompanions;
        [SerializeField] private ItemInfoDatabaseSO itemInfoDatabase;
        [SerializeField] private CompanionInfoDatabaseSO companionInfoDatabase;
    
        private TextMeshProUGUI[] _choicesText;
        private Story _currentStory;
        private Queue<OptionalStory> _optionalStories;
        private StoryVariablesRegistry _storyVariablesRegistry; 
        private StoryFunctionsBinder _storyFunctionsBinder;
    
        public bool StoryIsProgressing { get; private set; }

        protected override void Awake() {
            base.Awake();
            _optionalStories = new Queue<OptionalStory>();
            _storyVariablesRegistry = new StoryVariablesRegistry(globalsInkFile.filePath);
            _storyFunctionsBinder = new StoryFunctionsBinder(playerInventory.Value, playerCompanions.Value, itemInfoDatabase, companionInfoDatabase);
        }

        private void OnEnable() {
            // Subscribe to input events and enable input actions
            input.ContinueStory += Input_ContinueStory;
            input.EnableInputActions();
        }

        private void OnDisable() {
            input.ContinueStory -= Input_ContinueStory;
        }

        private void Start() {
            UINavigator.Instance.HideUIElement(storyPanel);
        
            // Get all the choices text
            _choicesText = new TextMeshProUGUI[choices.Length];
            for (var i = 0; i < choices.Length; i++) {
                _choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void ExitStory() {
            StoryIsProgressing = false;
            UINavigator.Instance.PopUILayer();
            storyText.text = "";
        
            _storyVariablesRegistry.StopListening(_currentStory);
            _storyFunctionsBinder.UnbindGlobalFunctions(_currentStory);
            
            Debug.Log("Exiting story");
        
            input.EnableInputAction("Gameplay", "PrevUILayer");
            input.EnableInputAction("Gameplay", "OpenCloseInventory");
            input.EnableInputAction("Gameplay", "OpenCloseCompanions");
        }

        private void HandleStoryFlow() {
            if (_currentStory.canContinue) {
                var line = _currentStory.Continue();
                if (!string.IsNullOrWhiteSpace(line)) {
                    storyText.text = line;
                    DisplayChoices();
                    return;
                }
            }
            
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

        private void DisplayChoices() {
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
            for (; i < choices.Length; i++) {
                choices[i].gameObject.SetActive(false);
            }
        
            // automatically select the first choice button
            if (choices[0].TryGetComponent<Button>(out var choiceButton)) {
                choiceButton.Select();
            }
        }
    
        private void Input_ContinueStory() {
            if (StoryIsProgressing && _currentStory.currentChoices.Count == 0) {
                HandleStoryFlow();
            }
        }
    
        public void EnterStory(TextAsset inkJson) {
            _currentStory = new Story(inkJson.text);
            StoryIsProgressing = true;
        
            input.DisableInputAction("Gameplay", "PrevUILayer");
            input.DisableInputAction("Gameplay", "OpenCloseInventory");
            input.DisableInputAction("Gameplay", "OpenCloseCompanions");
        
            UINavigator.Instance.PushUILayer(storyPanel);
            
            Debug.Log($"Entering story '{inkJson.name}'.");
        
            _storyVariablesRegistry.StartListening(_currentStory);
            _storyFunctionsBinder.BindGlobalFunctions(_currentStory);
        
            HandleStoryFlow();
        }

        public void EnqueueOptionalStory(OptionalStory optionalStory) {
            _optionalStories.Enqueue(optionalStory);
        }

        public void MakeChoice(int choiceIndex) {
            Debug.Log("Chosen choice with index: " + choiceIndex);
            _currentStory.ChooseChoiceIndex(choiceIndex);
            HandleStoryFlow();
        }

        public Ink.Runtime.Object GetRegistryVariable(string variableName) {
            _storyVariablesRegistry.Variables.TryGetValue(variableName, out var registryVariable);
            if (registryVariable.VariableValue == null) {
                Debug.LogWarning("Ink Variable was found to be null: " + variableName);
            }
            return registryVariable.VariableValue;
        }
    
        public bool EvaluateConditionOnStoryVariable<T>(string variableName, T value, ComparisonType comparisonType) where T : IComparable {
            return _storyVariablesRegistry.CompareVariableTo(variableName, value, comparisonType);
        }

        public void SubscribeToVariableChange(string variableName, Action<Ink.Runtime.Object> onValueChanged, object currentValue) {
            var type = currentValue.GetType();
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
            if (unboxedInkValue != null && unboxedInkValue.GetType() == type) {
                if (!unboxedInkValue.Equals(currentValue)) {
                    Debug.LogError($"The {variableName}'s global ink variable value ({unboxedInkValue}) is not equal to the provided variable" +
                                   $" value ({currentValue})");
                }
            }
            else {
                Debug.LogError($"The {variableName}'s global ink variable type ({unboxedInkValue?.GetType()}) is not equal to the provided variable" +
                               $" value ({type})");
            }
        
            _storyVariablesRegistry.Variables[variableName].OnValueChanged += onValueChanged;
        }

        public void UnsubscribeFromVariableChange(string variableName, Action<Ink.Runtime.Object> onValueChanged) => 
            _storyVariablesRegistry.Variables[variableName].OnValueChanged -= onValueChanged;
    }
}

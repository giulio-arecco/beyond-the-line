using System;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEngine.UI;
using Enums;
using Inventory.Interfaces;
using Storage.StorableInfoDatabase;
using Storage.Storables;
using UnityEngine.Serialization;
using Utils;
using Utils.SerializeInterface;

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
    private StoryVariablesRegistry _storyVariablesRegistry; 
    private StoryFunctionsBinder _storyFunctionsBinder;
    
    public bool StoryIsProgressing { get; private set; }

    protected override void Awake() {
        base.Awake();
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

    public void EnterStoryEvent(TextAsset inkJson) {
        _currentStory = new Story(inkJson.text);
        StoryIsProgressing = true;
        
        input.DisableInputAction("Gameplay", "PrevUILayer");
        input.DisableInputAction("Gameplay", "OpenCloseInventory");
        UINavigator.Instance.PushUILayer(storyPanel);
        
        _storyVariablesRegistry.StartListening(_currentStory);
        _storyFunctionsBinder.BindGlobalFunctions(_currentStory);
        
        ContinueStory();
    }

    private void ExitStoryEvent() {
        StoryIsProgressing = false;
        UINavigator.Instance.PopUILayer();
        storyText.text = "";
        
        _storyVariablesRegistry.StopListening(_currentStory);
        _storyFunctionsBinder.UnbindGlobalFunctions(_currentStory);
        
        input.EnableInputAction("Gameplay", "PrevUILayer");
        input.EnableInputAction("Gameplay", "OpenCloseInventory");
    }

    private void ContinueStory() {
        if (_currentStory.canContinue) {
            // set the text for the current story line
            storyText.text = _currentStory.Continue();
            // display choices, if any, for this story line
            DisplayChoices();
        }
        else {
            ExitStoryEvent();
        }
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

    public void MakeChoice(int choiceIndex) {
        Debug.Log("Chosen choice with index: " + choiceIndex);
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
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

    private void Input_ContinueStory() {
        if (StoryIsProgressing && _currentStory.currentChoices.Count == 0) {
            ContinueStory();
        }
    }
}

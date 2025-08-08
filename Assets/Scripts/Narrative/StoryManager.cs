using TMPro;
using UnityEngine;
using Ink.Runtime;

public class StoryManager : Singleton<StoryManager> {
    [Header("Story UI")]
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private TextMeshProUGUI storyText;
    
    [Header("Story Global Variables")]
    [SerializeField] private TextAsset globalsInkJson;
    
    [Header("Choice UI")]
    [SerializeField] private GameObject[] choices;
    
    private TextMeshProUGUI[] _choicesText;
    private Story _currentStory;
    private StoryVariablesRegistry _storyVariablesRegistry; 
    
    public bool StoryIsProgressing { get; private set; }

    protected override void Awake() {
        base.Awake();
        _storyVariablesRegistry = new StoryVariablesRegistry(globalsInkJson);
    }
    
    private void Start() {
        // storyPanel.SetActive(false);
        
        // Get all the choices text
        _choicesText = new TextMeshProUGUI[choices.Length];
        for (var i = 0; i < choices.Length; i++) {
            _choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update() {
        if (!StoryIsProgressing) return;

        if (_currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space)) {
            ContinueStory();
        }
    }

    public void EnterStoryEvent(TextAsset inkJson) {
        _currentStory = new Story(inkJson.text);
        StoryIsProgressing = true;
        storyPanel.SetActive(true);
        
        _storyVariablesRegistry.StartListening(_currentStory);
        
        ContinueStory();
    }

    private void ExitStoryEvent() {
        StoryIsProgressing = false;
        storyPanel.SetActive(false);
        storyText.text = "";
        _storyVariablesRegistry.StopListening(_currentStory);
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
    }

    public void MakeChoice(int choiceIndex) {
        Debug.Log("Chosen choice with index: " + choiceIndex);
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    public Ink.Runtime.Object GetVariableState(string variableName) {
        _storyVariablesRegistry.variables.TryGetValue(variableName, out var variableValue);
        if (variableValue == null) {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
}

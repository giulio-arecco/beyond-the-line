using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using System.IO;

public class StoryVariablesRegistry {
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    public StoryVariablesRegistry(TextAsset globalsInkJson) {
        var globalVariablesStory = new Story(globalsInkJson.text);
        
        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (var name in globalVariablesStory.variablesState) {
            var value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global story variable: " + name + " = " + value);
        }
    }
    
    /// <summary>
    ///  Deprecated constructor
    /// </summary>
    public StoryVariablesRegistry(string globalsFilePath) {
        // compile the story (since the globals.ink file is considered an include file, it may not compile automatically in the editor)
        var inkFileContents = File.ReadAllText(globalsFilePath);
        var compiler = new Ink.Compiler(inkFileContents);
        var globalVariablesStory = compiler.Compile();
        
        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (var name in globalVariablesStory.variablesState) {
            var value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global story variable: " + name + " = " + value);
        }
    }
    
    public void StartListening(Story story) {
        // it's important that VariablesToStory is called before assigning the listener!
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += OnVariableChanged;
    }
    
    public void StopListening(Story story) {
        story.variablesState.variableChangedEvent -= OnVariableChanged;
    }
    
    private void OnVariableChanged(string name, Ink.Runtime.Object value) {
        // only maintain variables what were initialized fom the globals ink file
        if (variables.ContainsKey(name)) {
            variables[name] = value;
        }
    }

    private void VariablesToStory(Story story) {
        foreach (var variable in variables) {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}

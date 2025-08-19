using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using System.IO;

public class StoryVariablesRegistry {
    public class RegistryVariable {
        public Ink.Runtime.Object VariableValue;
        public Action<Ink.Runtime.Object> OnValueChanged;
        
        public RegistryVariable(Ink.Runtime.Object variableValue, Action<Ink.Runtime.Object> onValueChanged = null) {
            VariableValue = variableValue;
            OnValueChanged = onValueChanged;
        }
    }
    
    public Dictionary<string, RegistryVariable> Variables { get; private set; }
    
    public StoryVariablesRegistry(string globalsFilePath) {
        // compile the story (since the globals.ink file is considered an include file, it will not compile automatically in the editor)
        var inkFileContents = File.ReadAllText(globalsFilePath);
        var compiler = new Ink.Compiler(inkFileContents);
        var globalVariablesStory = compiler.Compile();
        
        // initialize the dictionary
        Variables = new Dictionary<string, RegistryVariable>();
        foreach (var name in globalVariablesStory.variablesState) {
            var value = globalVariablesStory.variablesState.GetVariableWithName(name);
            Variables.Add(name, new RegistryVariable(value));
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
        // only maintain variables what were initialized from the globals ink file
        if (Variables.ContainsKey(name)) {
            Variables[name].VariableValue = value;
            Variables[name].OnValueChanged?.Invoke(value);
        }
    }

    private void VariablesToStory(Story story) {
        foreach (var variable in Variables) {
            story.variablesState.SetGlobal(variable.Key, variable.Value.VariableValue);
        }
    }
}

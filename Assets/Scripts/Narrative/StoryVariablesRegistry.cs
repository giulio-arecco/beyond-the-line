using System;
using System.Collections.Generic;
using System.IO;
using Enums;
using Ink.Runtime;
using UnityEngine;

namespace Narrative {
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
    
        public StoryVariablesRegistry(TextAsset globalsInkFile) {
            var globalVariablesStory = new Story(globalsInkFile.text);
        
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

        public bool CompareVariableTo<T>(string variableName, T value, ComparisonType comparisonType) where T : IComparable { 
            var inkValue = Variables[variableName].VariableValue;
        
            var inkTypedValue = inkValue switch {
                IntValue iv    when typeof(T) == typeof(int)    => (T)(object)iv.value,
                FloatValue fv  when typeof(T) == typeof(float)  => (T)(object)fv.value,
                StringValue sv when typeof(T) == typeof(string) => (T)(object)sv.value,
                BoolValue bv   when typeof(T) == typeof(bool)   => (T)(object)bv.value,
                _ => throw new InvalidCastException(
                    $"Cannot convert Ink value of type {inkValue.GetType()} to {typeof(T)}"
                )
            };

            var result = inkTypedValue.CompareTo(value);
        
            return comparisonType switch {
                ComparisonType.Equal          => result == 0,
                ComparisonType.NotEqual       => result != 0,
                ComparisonType.Greater        => result > 0,
                ComparisonType.GreaterOrEqual => result >= 0,
                ComparisonType.Less           => result < 0,
                ComparisonType.LessOrEqual    => result <= 0,
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonType), comparisonType, null)
            };
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
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Enums;
using Narrative;
using UnityEngine;

namespace NodeCanvas.CustomTasks.Conditions {
	[Category("Custom/Ink")]
	[Description("Evaluate a condition on an Ink variable")]
	public class CheckInkVariable<T> : ConditionTask where T : IComparable {
		public BBParameter<string> VariableName;
		public BBParameter<T> Value;
		public BBParameter<ComparisonType> ComparisonType;
		
		protected override string info {
			get {
				if (Value == null) return $"Ink Variable {VariableName.value} is {ComparisonType.value} to *MissingValue*";
				return $"Ink Variable {VariableName.value} is {ComparisonType.value} to {Value.value}";
			}
		}

		protected override bool OnCheck() {
			if (StoryManager.TryGetInstance(out var storyManager)) {
				return storyManager.EvaluateConditionOnStoryVariable(VariableName.value, Value.value, ComparisonType.value);
			}
			
			Debug.LogWarning("The StoryManager instance is null");
			return false;
		}
	}
}
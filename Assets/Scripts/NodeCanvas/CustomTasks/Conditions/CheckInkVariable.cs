using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Enums;
using UnityEngine;

namespace NodeCanvas.CustomTasks.Conditions {
	[Category("Custom/Ink")]
	[Description("Evaluate a condition on an Ink variable")]
	public class CheckInkVariable<T> : ConditionTask where T : IComparable {
		public BBParameter<string> VariableName;
		public BBParameter<T> Value;
		public ComparisonType ComparisonType;

		protected override bool OnCheck() {
			if (StoryManager.TryGetInstance(out var storyManager)) {
				return storyManager.EvaluateConditionOnStoryVariable(VariableName.value, Value.value, ComparisonType);
			}
			
			Debug.LogWarning("The StoryManager instance is null");
			return false;
		}
	}
}
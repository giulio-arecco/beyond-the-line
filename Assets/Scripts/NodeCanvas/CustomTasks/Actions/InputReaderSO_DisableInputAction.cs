using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.CustomTasks.Actions {
    [Category("Custom/InputReaderSO")]
    [Description("Call EnableInputAction on the provided InputReaderSO")]
    public class InputReaderSO_DisableInputAction : ActionTask {
        [RequiredField]
        public BBParameter<InputReaderSO> inputReader;

        public BBParameter<string> ActionMapName;
        public BBParameter<string> ActionName;
        
        protected override string info => $"Disable Input Action: '{ActionMapName}' -> '{ActionName}'";

        protected override void OnExecute() {
            if (inputReader.value == null) {
                Debug.LogError($"[NodeCanvas] InputReader not assigned in Action 'InputReaderSO_DisableInputAction'");
                EndAction(false);
                return;
            }

            inputReader.value.DisableInputAction(ActionMapName.value, ActionName.value);
            EndAction(true);
        }
    }
}
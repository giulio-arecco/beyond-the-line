using UnityEngine;

namespace Architecture {
    public class SceneInitializer : MonoBehaviour {
        [SerializeField] private InputReaderSO inputReader;

        private void Awake() {
            QualitySettings.vSyncCount = 1; // VSync set to the monitor's refresh rate
            inputReader.EnableInputActions();
        }
    }
}

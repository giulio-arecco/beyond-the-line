using UnityEngine;

namespace Architecture {
    public class SceneInitializer : MonoBehaviour {
        [SerializeField] private InputReaderSO inputReader;

        private void Awake() {
            inputReader.EnableInputActions();
        }
    }
}

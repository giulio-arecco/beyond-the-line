using UnityEditor;
using UnityEngine;

namespace Architecture {
    public class ApplicationQuitter : MonoBehaviour {
        public void QuitGame() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
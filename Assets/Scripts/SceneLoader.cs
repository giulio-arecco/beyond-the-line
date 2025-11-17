using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSingleton<SceneLoader> {
    [SerializeField] private UnityEvent onSyncLoadingStarted;
    
    public void LoadScene(string sceneName) {
        onSyncLoadingStarted?.Invoke();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

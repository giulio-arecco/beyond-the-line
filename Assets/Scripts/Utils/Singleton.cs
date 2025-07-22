using UnityEngine;

/// <summary>
/// A basic singleton class.
/// Consider using PersistentSingleton or RegulatedSingleton instead of this one.
/// </summary>
/// <typeparam name="T"> The singleton component </typeparam>
public class Singleton<T> : MonoBehaviour where T : Component {
    private static bool _isShuttingDown;
    
    protected static T instance;
    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;
    
    /// <summary>
    /// Remember to always perform a null check if you are accessing the singleton instance in OnDisable, OnDestroy or
    /// any method that could be called after the instance has been destroyed, to avoid null ref exceptions.
    /// </summary>
    public static T Instance {
        get {
            if (!instance) {
                instance = FindAnyObjectByType<T>();
                if (!instance && !_isShuttingDown) {
                    if (SerializeFieldChecker.HasSerializedFields(typeof(T))) {
                        Debug.LogWarning("Auto-Generating a singleton that has some serialized fields.");
                    }
                    
                    var gameObject = new GameObject(typeof(T).Name + " Auto-Generated");
                    instance = gameObject.AddComponent<T>();
                }
            }
            
            return instance;
        }
    }
    
    /// <summary>
    /// Make sure to call base.Awake() in override if you need Awake.
    /// </summary>
    protected virtual void Awake() {
        InitializeSingleton();
    }
    
    /// <summary>
    /// Make sure to call base.OnApplicationQuit() in override if you need OnApplicationQuit.
    /// </summary>
    protected void OnApplicationQuit() {
        _isShuttingDown = true;
    }

    protected virtual void InitializeSingleton() {
        if (!Application.isPlaying) return;

        if (!instance) {
            instance = this as T;
        } else {
            if (instance != this) {
                Destroy(gameObject);
            }
        }
    }
}

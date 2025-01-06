using UnityEngine;

/// <summary>
/// Singleton that will persist through scene changes.
/// </summary>
public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    [Tooltip("If this is true, this singleton will automatically be placed on the root level of the scene hierarchy.")]
    public bool AutoUnparentOnAwake = true;

    // Tracks whether the application is shutting down.
    protected static bool IsApplicationQuitting = false;

    public static bool HasInstance => s_Instance != null;
    public static T TryGetInstance() => HasInstance ? s_Instance : null;

    protected static T s_Instance;

    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<T>();
                if (s_Instance == null && !IsApplicationQuitting)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name + " [Auto Generated]";
                    s_Instance = obj.AddComponent<T>();
                }
            }

            return s_Instance;
        }
    }

    protected virtual void Awake() => InitializeSingleton();

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying)
            return;

        if (AutoUnparentOnAwake)
            transform.SetParent(null);

        if (s_Instance == null)
        {
            s_Instance = this as T;
            DontDestroyOnLoad(transform.gameObject);
            enabled = true;
        }
        else
        {
            if (this != s_Instance)
                Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        IsApplicationQuitting = true;

        s_Instance = null;

        if(Application.isEditor)
            DestroyImmediate(gameObject);
        else
            Destroy(gameObject);
    }

}


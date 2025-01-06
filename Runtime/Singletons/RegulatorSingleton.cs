using UnityEngine;

/// <summary>
/// Persistent Regulator Singleton, will destroy any other older components of the same type it finds on awake
/// </summary>
public class RegulatorSingleton<T> : MonoBehaviour where T : Component
{
    // Tracks whether the application is shutting down.
    protected static bool IsApplicationQuitting = false;

    protected static T s_Instance;
    public static bool HasInstance => s_Instance != null;

    public float InitializationTime { get; private set; }

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
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    s_Instance = obj.AddComponent<T>();
                }
            }

            return s_Instance;
        }
    }

    protected virtual void Awake() => InitializeSingleton();

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;
        InitializationTime = Time.time;
        DontDestroyOnLoad(transform.gameObject);

        T[] oldInstances = FindObjectsOfType<T>();
        foreach (T instance in oldInstances)
        {
            if(instance.GetComponent<RegulatorSingleton<T>>().InitializationTime < InitializationTime)
                Destroy(instance.gameObject);
        }

        if (s_Instance == null)
        {
            s_Instance = this as T;
            enabled = true;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        IsApplicationQuitting = true;

        s_Instance = null;

        if (Application.isEditor)
            DestroyImmediate(gameObject);
        else
            Destroy(gameObject);
    }

}


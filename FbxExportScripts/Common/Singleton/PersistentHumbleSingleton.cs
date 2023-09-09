using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentHumbleSingleton<T> : MonoBehaviour where T : Component
{
    [Header("Persistent Humble Singleton")]
    protected static T _instance;
    public static bool HasInstance => _instance != null;
    public static T Current => _instance;


    [ReadOnly]
    public float InitializationTime;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject
                    {
                        hideFlags = HideFlags.HideAndDontSave,
                        name = typeof(T).Name + "_AutoCreated"
                    };
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        InitializationTime = Time.time;
        DontDestroyOnLoad(this.gameObject);

        T[] check = FindObjectsOfType<T>();
        foreach (T searched in check)
        {
            if (searched != this)
            {
                if (searched.GetComponent<PersistentHumbleSingleton<T>>().InitializationTime < InitializationTime)
                {
                    Destroy(searched.gameObject);
                }
            }
        }
        if (_instance == null)
        {
            _instance = this as T;
        }

    }

}

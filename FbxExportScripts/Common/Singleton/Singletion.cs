using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletion<T> : MonoBehaviour where T : Component
{
    [Header("Singleton")]
    protected static T instance;
    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;
    public static T Current => instance;


    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject obj = new GameObject
                    {
                        name = typeof(T).Name + "_AutoCeated"
                    };
                    instance = obj.AddComponent<T>();
                }            
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!Application.isPlaying)
            return;
        instance = this as T;
    }

}

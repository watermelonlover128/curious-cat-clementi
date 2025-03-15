using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for Singletons whose lifetime should only be the current scene
/// </summary>
public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{
    public static T Instance { get; private set; }
    

    protected virtual void Awake()
    {
        InitialiseSingleton();
    }

    protected virtual void OnDestroy()
    {
        // Clear Singleton instance
        if (Instance == this)
            Instance = null;
    }

    private void InitialiseSingleton()
    {
        if (Instance == null)
            Instance = (T)this;
        else
        {
            Debug.Log(typeof(T) + " Instance already assigned to " + Instance.name + ", deleting component instance in " + gameObject.name);
            Destroy(this);
        }
    }
}

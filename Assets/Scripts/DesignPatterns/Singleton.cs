using System;
using UnityEngine;

/// <summary>
/// Abstract class for Singletons whose lifetime should only be the current scene
/// </summary>
public abstract class Singleton<T> where T : Singleton<T>
{
    public static T Instance { 
        get {
            if (instance != null)
                return instance;
            return CreateInstance();
        } 
    }
    private static T instance;

    protected Singleton() {}

    private static T CreateInstance() {
        instance = (T)Activator.CreateInstance(typeof(T));
        instance.Init();
        return instance;
    }

    protected abstract void Init();
}

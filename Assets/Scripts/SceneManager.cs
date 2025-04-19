using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


class SceneManager : MonoBehaviourSingleton<SceneManager> {
    [SerializeField] private SerializedDictionary<SceneID, string> scenes;

    public delegate void SceneDelegate(SceneID id);
    public event SceneDelegate OnAdditiveSceneLoadedEvent;
    public event SceneDelegate OnSceneLoadedEvent;
    public event SceneDelegate OnSceneUnloadedEvent;
    public event SceneDelegate OnSceneStartUnloadEvent;


    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void LoadScene(SceneID sceneId) {
        string sceneName = GetSceneName(sceneId);
        LoadingScreenHandler.Instance.ToggleLoadingScreen(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadSceneAdditive(SceneID sceneId) {
        string sceneName = GetSceneName(sceneId);
        LoadingScreenHandler.Instance.ToggleLoadingScreen(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void LoadSceneAsync(SceneID sceneId, Action callback = null)
    {
        string sceneName = GetSceneName(sceneId);
        LoadingScreenHandler.Instance.ToggleLoadingScreen(true);
        StartCoroutine(WaitLoadSceneAsync(sceneName, callback));
    }

    private IEnumerator WaitLoadSceneAsync(string sceneName, Action callback) {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
            yield return null;
        callback?.Invoke();
    }

    public void LoadSceneAdditiveAsync(SceneID sceneId, Action callback = null)
    {
        string sceneName = GetSceneName(sceneId);
        LoadingScreenHandler.Instance.ToggleLoadingScreen(true);
        StartCoroutine(WaitLoadSceneAdditiveAsync(sceneName, callback));
    }

    private IEnumerator WaitLoadSceneAdditiveAsync(string sceneName, Action callback)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
            yield return null;
        callback?.Invoke();
    }

    public void UnloadSceneAsync(SceneID sceneId, Action callback = null)
    {
        string sceneName = GetSceneName(sceneId);
        this.OnSceneStartUnloadEvent?.Invoke(sceneId);
        StartCoroutine(WaitUnloadSceneAsync(sceneName, callback));
    }

    private IEnumerator WaitUnloadSceneAsync(string sceneName, Action callback)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;
        callback?.Invoke();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneID sceneId = GetSceneID(scene.name);
        if (mode == LoadSceneMode.Additive) 
            OnAdditiveSceneLoadedEvent?.Invoke(sceneId);
        else
            OnSceneLoadedEvent?.Invoke(sceneId);
        LoadingScreenHandler.Instance.ToggleLoadingScreen(false);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        OnSceneUnloadedEvent?.Invoke(GetSceneID(scene.name));
    }

    public void MoveGameObjectToScene(GameObject go, SceneID sceneId) {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go, GetScene(sceneId));
    }

    private string GetSceneName(SceneID sceneId) {
        if (!scenes.ContainsKey(sceneId)) {
            Debug.LogErrorFormat("SceneManager: Attempted to find non-existent string name for ID '%s'", sceneId);
            return "";
        }
        return scenes[sceneId];
    }

    private SceneID GetSceneID(string sceneName)
    {
        if (!scenes.ContainsValue(sceneName))
        {
            Debug.LogErrorFormat("SceneManager: Attempted to find non-existent string ID for scene name '%s'", sceneName);
            return SceneID.MAIN_MENU;
        }
        return scenes.FirstOrDefault(pair => pair.Value == sceneName).Key;
    }

    private Scene GetScene(SceneID sceneId) {
        string sceneName = GetSceneName(sceneId);
        return UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
    }

}

[System.Serializable]
public enum SceneID {
    MAIN_MENU,
    GAME_MANAGERS,
    GAME_MAIN,
    GAME_SUB_FISHSTALL

}
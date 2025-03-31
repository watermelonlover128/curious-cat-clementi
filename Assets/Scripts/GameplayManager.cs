using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    [SerializeField]
    private PlayerCamera mainCamera;
    [SerializeField]
    private GameObject playerPrefab;


    public GameObject Player { get; private set;}


    private Dictionary<SceneID, Vector3> spawnPoints = new Dictionary<SceneID, Vector3>();
    private SceneID currentSubScene;
    void Start()
    {
        this.Player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        mainCamera.SetTarget(this.Player.transform);
        SceneManager.Instance.OnAdditiveSceneLoadedEvent += OnSubGameSceneLoaded;
        SceneManager.Instance.OnSceneStartUnloadEvent += BeforeSceneUnloaded;
        SceneManager.Instance.LoadSceneAdditiveAsync(SceneID.GAME_MAIN, () => {});

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SceneManager.Instance.OnAdditiveSceneLoadedEvent -= OnSubGameSceneLoaded;
        SceneManager.Instance.OnSceneStartUnloadEvent -= BeforeSceneUnloaded;

    }

    public void EnterSubScene(SceneID sceneId) 
    {
        SceneManager.Instance.UnloadSceneAsync(currentSubScene, () => {
            SceneManager.Instance.LoadSceneAdditiveAsync(sceneId);
        });
    }

    public void EnterSubScene(SceneID sceneId, Vector3 spawnPos, bool fixedSpawnPos = true)
    {
        if (fixedSpawnPos || !this.spawnPoints.ContainsKey(sceneId))
            this.SetSpawnPoint(sceneId, spawnPos);
        SceneManager.Instance.UnloadSceneAsync(currentSubScene, () =>
        {
            SceneManager.Instance.LoadSceneAdditiveAsync(sceneId);
        });
    }

    private void OnSubGameSceneLoaded(SceneID sceneId) {
        this.currentSubScene = sceneId;
        SceneManager.Instance.MoveGameObjectToScene(this.Player, sceneId);
        this.Player.transform.localPosition = LoadPlayerEntryPoint(sceneId);
    }
    private void BeforeSceneUnloaded(SceneID sceneId)
    {
        SavePlayerExitPoint();
        SceneManager.Instance.MoveGameObjectToScene(this.Player, SceneID.GAME_MANAGERS);
    }

    private void SavePlayerExitPoint() {
        this.SetSpawnPoint(this.currentSubScene, this.Player.transform.localPosition);
    }

    private Vector3 LoadPlayerEntryPoint(SceneID sceneId) {
        if (!spawnPoints.ContainsKey(sceneId)) {
            return Vector3.zero;
        }
        return spawnPoints[sceneId];
    }

    private void SetSpawnPoint(SceneID sceneId, Vector3 pos) {
        if (!this.spawnPoints.ContainsKey(sceneId))
            this.spawnPoints.Add(sceneId, pos);
        else
            this.spawnPoints[sceneId] = pos;
    }


}

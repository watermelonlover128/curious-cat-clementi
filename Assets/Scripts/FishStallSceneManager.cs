using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishStallSceneManager : MonoBehaviour
{
    public static FishStallSceneManager instance;
    public enum GameStates {
        EnterState,
        GetFishState,
        EscapeState,
        GameOverState
    };
    public GameStates currentState { get; private set; } = GameStates.EnterState; // not sure if we truly need to check state like this, will see how

    public event Action ChaseBegin;
    public event Action OnGameOver;
    public event Action OnRespawn;

    [SerializeField] 
    private DialogueSequenceScriptableObject EnterFishStallDialogue;
    [SerializeField]
    private GameObject gameOverCanvas;
    [SerializeField]
    private GameObject exit;
    [SerializeField]
    private Transform respawnEntrance;
    [SerializeField]
    private Transform respawnEnd;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform enemiesParent;
    private List<EnemyStateManager> enemies = new List<EnemyStateManager>();

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!ObjectivesManager.Instance) {
            Debug.LogError("FishStallSceneManager: ObjectiveManager Instance not found!");
            return;
        }

        if (!DialogueManager.Instance) {
            Debug.LogError("FishStallSceneManager: DialogueManager Instance not found!");
            return;
        }

        foreach (Transform child in enemiesParent) {
            enemies.Add(child.GetComponent<EnemyStateManager>());
        }

        ObjectivesManager.Instance.OnNewObjectiveEvent += OnObjectiveBegin;
        ObjectivesManager.Instance.OnObjectiveCompletedEvent += OnObjectiveCompleted;
        DialogueManager.Instance.OnStartDialogue += OnStartDialogue;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;

        // the moment the scene begins, play an initial dialogue with sneakycat
        DialogueManager.Instance.PlayDialogueSequence(EnterFishStallDialogue);

        gameOverCanvas.SetActive(false);
    }

    // in case i need any of this
    private void OnObjectiveCompleted(Objective objective) {
        Debug.Log(objective.ID);
        switch (objective.ID) {
            case "FISH_STALL_ENTER_DIALOGUE": 
                break;
            case "OBTAIN_FISH":
                break;
            case "ESCAPE_FISHSTALL":
                break;
            default:
                break;
        }
    }

    private void OnObjectiveBegin(Objective objective) {
        Debug.Log(objective.ID);
        switch (objective.ID) {
            case "FISH_STALL_ENTER_DIALOGUE": 
                break;
            case "OBTAIN_FISH":
                break;
            case "ESCAPE_FISHSTALL":
                ChaseBegin?.Invoke();
                exit.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnStartDialogue(string dialogueID) {
        PauseManager.Instance?.Pause();
    }

    private void OnEndDialogue(string dialogueID) {
        PauseManager.Instance?.UnPause();
    }

    public void GameOver() {
        PauseManager.Instance?.Pause();
        OnGameOver?.Invoke();
        gameOverCanvas.SetActive(true);
    }

    public void ReloadScene() {
        if (ObjectivesManager.Instance.IsObjectiveOngoing("OBTAIN_FISH")) {
            player.transform.position = respawnEntrance.position;
        } else if (ObjectivesManager.Instance.IsObjectiveOngoing("ESCAPE_FISHSTALL")) {
            player.transform.position = respawnEnd.position;
        } else return;

        OnRespawn?.Invoke();
        PauseManager.Instance?.UnPause();

        gameOverCanvas.SetActive(false);
    }
}

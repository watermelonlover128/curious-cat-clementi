using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishStallSceneManager : MonoBehaviour
{
    public static FishStallSceneManager instance;

    public enum GameStates {
        EnterFishStallState,
        ObtainFishState,
        EscapeFishStallState
    }

    public GameStates currentState { get; private set; } = GameStates.EnterFishStallState;

    private bool isGameOver = false;

    // events 
    public event Action ChaseBegin;
    public event Action OnGameOver;
    public event Action OnRespawn;

    [SerializeField] 
    private DialogueSequenceScriptableObject EnterFishStallDialogue; 
    [SerializeField] 
    private DialogueSequenceScriptableObject FishmongerSeesCatDialogue; // dialogue that plays when cat is caught during the sneak sequence
    [SerializeField] 
    private DialogueSequenceScriptableObject FishmongerCapturesCatDialogue; // dialogue that plays when cat is caught during the chase

    [SerializeField]
    private GameObject gameOverCanvas; 

    [SerializeField]
    private GameObject exit; // only enables this when the escape sequence begins

    // respawn points
    [SerializeField]
    private Transform respawnEntrance;
    [SerializeField]
    private Transform respawnEnd;

    // reference to player
    [SerializeField]
    private Transform player;

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

        ObjectivesManager.Instance.OnNewObjectiveEvent += OnObjectiveBegin;
        DialogueManager.Instance.OnStartDialogue += OnStartDialogue;
        DialogueManager.Instance.OnEndDialogue += OnEndDialogue;

        // the moment the scene begins, play an initial dialogue with sneakycat
        DialogueManager.Instance.PlayDialogueSequence(EnterFishStallDialogue);

        gameOverCanvas.SetActive(false);
    }

    private void OnObjectiveBegin(Objective objective) {
        Debug.Log(objective.ID);
        switch (objective.ID) {
            case "FISH_STALL_ENTER_DIALOGUE": 
                break;
            case "OBTAIN_FISH":
                currentState = GameStates.ObtainFishState;
                break;
            case "ESCAPE_FISHSTALL":
                currentState = GameStates.EscapeFishStallState;
                ChaseBegin?.Invoke();
                exit.SetActive(true);
                break;
            default:
                break;
        }
    }

    // pausing and unpausing game when dialogue starts 
    private void OnStartDialogue(string dialogueID) {
        PauseManager.Instance?.Pause();
    }

    private void OnEndDialogue(string dialogueID) {
        PauseManager.Instance?.UnPause();
    }

    // when cat is caught, display a dialogue first before game over screen
    public void DisplayGameOverDialogue() {
        if (isGameOver) return; // if this has already been triggered, dont do it again

        if (currentState == GameStates.ObtainFishState) {
            DialogueManager.Instance.PlayDialogueSequence(FishmongerSeesCatDialogue);
        } else if (currentState == GameStates.EscapeFishStallState) {
            DialogueManager.Instance.PlayDialogueSequence(FishmongerCapturesCatDialogue);
        }

        DialogueManager.Instance.OnEndDialogue += GameOver;
        isGameOver = true;
    }

    // display game over screen, pause game 
    public void GameOver(string dialogueID) {
        PauseManager.Instance?.Pause();
        OnGameOver?.Invoke();
        gameOverCanvas.SetActive(true);

        DialogueManager.Instance.OnEndDialogue -= GameOver;
    }

    // a bit of a misnomer, we dont really get unity to reload but we do reset the positions of the characters
    public void ReloadScene() {
        // resets player to the appropriate respawn points
        // doing it like this because i didnt want to make player script subscribe to these events 
        if (currentState == GameStates.ObtainFishState) {
            player.transform.position = respawnEntrance.position;
        } 
        else if (currentState == GameStates.EscapeFishStallState) {
            player.transform.position = respawnEnd.position;
        } else return;

        OnRespawn?.Invoke();
        PauseManager.Instance?.UnPause();

        gameOverCanvas.SetActive(false);
        isGameOver = false;
    }
}

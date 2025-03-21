using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dialogue System
/// Manages the playing of dialogues in the game
/// </summary>
public class DialogueManager : MonoBehaviourSingleton<DialogueManager>
{
    [Header("UI Handler Reference")]
    [SerializeField]
    private DialogueUIHandler uiHandler;


    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
    private bool CurrentlyInDialogue => this.currentDialogueSequence != null;
    private bool currentlyWaitingChoice = false;
    private Dialogue currentDialogue;
    private DialogueSequenceScriptableObject currentDialogueSequence;


    private const string DIALOGUE_SO_DIRECTORY = "Dialogue/";

    public delegate void DialogueAction(string dialogueID);
    public event DialogueAction OnStartDialogue;
    public event DialogueAction OnEndDialogue;



    void Start() 
    {
        uiHandler.HideChoicesPanel();
        uiHandler.HideDialoguePanel();
        InputHandler.Instance.OnDialogueInput += OnDialogueInput;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (InputHandler.Instance != null)
            InputHandler.Instance.OnDialogueInput -= OnDialogueInput;

    }

    /// <summary>
    /// Called when dialogue-related input is entered
    /// </summary>
    private void OnDialogueInput() 
    {
        if (currentlyWaitingChoice || !CurrentlyInDialogue) // no action on input
            return;
       
        if (uiHandler.SkipTextAnimation()) // input results in skipping of text anim
            return;

        DisplayNextSentence(); // input results in playing next dialogue
        
    }

    /// <summary>
    /// Plays a dialogue sequence (sets it to start) based on its SO file path
    /// </summary>
    public void PlayDialogueSequence(string filename)
    {
        DialogueSequenceScriptableObject dialogueSequenceFile = (DialogueSequenceScriptableObject)Resources.Load(DIALOGUE_SO_DIRECTORY + filename);
        if (dialogueSequenceFile == null)
        {
            Debug.LogErrorFormat("DialogueManager: DialogueSequenceScriptableObject could not be found at: 'Resources/{0}'!", DIALOGUE_SO_DIRECTORY + filename);
            return;
        }
        this.PlayDialogueSequence(dialogueSequenceFile);
    }

    /// <summary>
    /// Plays a dialogue sequence (sets it to start)
    /// </summary>
    public void PlayDialogueSequence(DialogueSequenceScriptableObject dialogueSequence)
    {
        if (CurrentlyInDialogue) {
            Debug.LogWarningFormat("DialogueManager: Attempted to play another dialogue '{0}' while currently still in dialogue '{1}'!", dialogueSequence.ID, this.currentDialogueSequence.ID);
            return;
        }
        dialogueQueue.Clear();

        this.currentDialogueSequence = dialogueSequence;
        List<Dialogue> dialogues = dialogueSequence.dialogues;
        foreach (Dialogue d in dialogues) 
            this.dialogueQueue.Enqueue(d);

        uiHandler.ShowDialoguePanel();

        this.OnStartDialogue?.Invoke(this.currentDialogueSequence.ID);

        DisplayNextSentence();
    }

    /// <summary>
    /// Displays Next Dialogue in Sequence 
    /// </summary>
    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0) // end of dialogue
        {
            if (this.currentDialogueSequence.choices.Count > 0) 
                EnableChoices();
            else
                EndDialogue();
            return;
        }

        // grabs next line, sets corresponding speaker
        currentDialogue = dialogueQueue.Dequeue();
        uiHandler.SetSpeakerUI(currentDialogue.speaker);
        uiHandler.SetNewDialogueText(currentDialogue.sentence);
    }

    /// <summary>
    /// Display choices and waits for user to select one
    /// </summary>
    private void EnableChoices() {
        this.currentlyWaitingChoice = true;
        this.uiHandler.ShowChoicesPanel(this.currentDialogueSequence.choices, this.OnChoiceSelected);
    }

    /// <summary>
    /// Callback function, called when a choice has been selected
    /// </summary>
    private void OnChoiceSelected(DialogueChoice choice) {
        this.currentlyWaitingChoice = false;
        this.uiHandler.HideChoicesPanel();
        this.EndDialogue();
        if (choice.nextDialogueSequence != null)
            this.PlayDialogueSequence(choice.nextDialogueSequence);
    }

    /// <summary>
    /// Resets everything, called when a dialogue has ended
    /// </summary>
    public void EndDialogue()
    {
        uiHandler.HideDialoguePanel();
        this.OnEndDialogue?.Invoke(this.currentDialogueSequence.ID);
        this.currentDialogueSequence = null;
    }

}


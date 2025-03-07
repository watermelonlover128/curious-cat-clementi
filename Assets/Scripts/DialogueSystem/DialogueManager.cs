using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  
using System.IO;  
using System.Text;
using TMPro;

public class DialogueManager : MonoBehaviourSingleton<DialogueManager>
{

    [Header("Settings")]
    [SerializeField]
    private float textDelay = 0.05f;
    [Header("UI")]
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private TextMeshProUGUI speakerNameUI;
    [SerializeField]
    private TextMeshProUGUI dialogueTextUI;


    private SpeakerDataHandler speakerDataHandler = new SpeakerDataHandler();


    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
    private bool currentlyInDialogue = false;
    private bool currentlyTyping = false;
    private Dialogue currentDialogue;


    private const string DIALOGUE_SO_DIRECTORY = "Dialogue/";

    

    void Start() 
    {
        this.speakerDataHandler.Init();
        dialoguePanel.SetActive(false); // ensure the dialogue UI isnt visible initially

        InputHandler.Instance.OnDialogueInput += OnDialogueInput;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (InputHandler.Instance != null)
            InputHandler.Instance.OnDialogueInput -= OnDialogueInput;

    }

    private void OnDialogueInput() 
    {
        if (!currentlyInDialogue)
            return;
       
        if (currentlyTyping)
        {
            // skips typewriting animation
            StopAllCoroutines();
            dialogueTextUI.text = currentDialogue.sentence;
            currentlyTyping = false;
        }
        else
        {
            DisplayNextSentence();
        }
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
        this.PlayDialogueSequence(dialogueSequenceFile.dialogues);
    }

    /// <summary>
    /// Plays a dialogue sequence (sets it to start)
    /// </summary>
    public void PlayDialogueSequence(List<Dialogue> dialogues)
    {
        if (currentlyInDialogue) return;

        dialogueQueue.Clear();
        // reads the corresponding file for the dialogue
        foreach (Dialogue d in dialogues) 
            this.dialogueQueue.Enqueue(d);

        dialoguePanel.SetActive(true);
        currentlyInDialogue = true;

        DisplayNextSentence();
    }

    /// <summary>
    /// Displays Next Dialogue in Sequence 
    /// </summary>
    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0) // end of dialogue
        {
            EndDialogue();
            return;
        }

        // grabs next line, sets corresponding speaker
        currentDialogue = dialogueQueue.Dequeue();
        SpeakerData speaker = this.speakerDataHandler.GetSpeakerData(currentDialogue.speaker);
        speakerNameUI.text = speaker.name;

        currentlyInDialogue = true;

        // ensure previous typewriter coroutine is stopped before starting a new one
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentDialogue.sentence));
    }

    /// <summary>
    /// Plays Typing Animation
    /// </summary>
    IEnumerator TypeSentence(string sentence)
    {
        dialogueTextUI.text = "";
        currentlyTyping = true;

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueTextUI.text += letter;
            yield return new WaitForSeconds(textDelay);
        }

        currentlyTyping = false;
    }

    /// <summary>
    /// Resets everything.
    /// </summary>
    public void EndDialogue()
    {
        currentlyInDialogue = false;
        dialoguePanel.SetActive(false);

        speakerNameUI.text = "";
        dialogueTextUI.text = "";
    }

}

[System.Serializable]
public enum SpeakerID {
    PlayerCat = 0,
    Cat1,
    Cat2,
    Cat3,

}

class SpeakerDataHandler {
    private const string SPEAKER_DATA_FILENAME = "SpeakersData";
    private Dictionary<SpeakerID, SpeakerData> speakersData;

    public void Init() {
        SpeakerInfoScriptableObject speakersDataFile = (SpeakerInfoScriptableObject)Resources.Load(SPEAKER_DATA_FILENAME);
        if (speakersDataFile == null) {
            Debug.LogErrorFormat("DialogueManager:SpeakerSpritesHandler: Speaker Sprites Data Scriptable Object could not be found at: 'Resources/{0}'!", SPEAKER_DATA_FILENAME);
            return;
        }
        this.speakersData = speakersDataFile.speakersData;

    }

    public Sprite GetSpeakerSprite(SpeakerID id) {
        if (!speakersData.ContainsKey(id)) {
            Debug.LogErrorFormat("DialogueManager:SpeakerSpritesHandler: Could not find sprite data for speaker id {0}!", id);
            return null;
        }
        return speakersData[id].sprite;
    }

    public string GetSpeakerName(SpeakerID id)
    {
        if (!speakersData.ContainsKey(id))
        {
            Debug.LogErrorFormat("DialogueManager:SpeakerSpritesHandler: Could not find name data for speaker id {0}!", id);
            return null;
        }
        return speakersData[id].name;
    }

    public SpeakerData GetSpeakerData(SpeakerID id)
    {
        if (!speakersData.ContainsKey(id))
        {
            Debug.LogErrorFormat("DialogueManager:SpeakerSpritesHandler: Could not find speaker data for speaker id {0}!", id);
            return new SpeakerData();
        }
        return speakersData[id];
    }
}
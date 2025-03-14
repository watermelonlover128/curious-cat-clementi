using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Handles UI for DialogueSystem (to be called by DialogueManager)
/// </summary>
public class DialogueUIHandler : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    private float textDelay = 0.05f;
    [SerializeField]
    private bool playAnim = true;
    [Header("Dialogue UI")]
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private TextMeshProUGUI speakerNameUI;
    [SerializeField]
    private TextMeshProUGUI dialogueTextUI;
    [Header("Dialogue Choices UI")]
    [SerializeField]
    private GameObject choicesPanel;
    [SerializeField]
    private GameObject choiceUIPrefab;
    [SerializeField]
    private Transform choiceUIRect;


    private bool currentlyInTypingAnim = false;
    private string currentDialogueText;


    /// <summary>
    /// Displays Choices Panel with the given choices
    /// </summary>
    /// <param name="choices"></param> choices
    /// <param name="onSelectAction"></param> function to be called upon a choice being selected
    public void ShowChoicesPanel(List<DialogueChoice> choices, Action<DialogueChoice> onSelectAction) {
        // Clear any existing choice UI
        foreach (Transform child in this.choiceUIRect)
            Destroy(child.gameObject);
        // Fill Rect
        foreach (DialogueChoice choice in choices)
        {
            DialogueChoiceUI ui = GameObject.Instantiate(choiceUIPrefab, choiceUIRect).GetComponent<DialogueChoiceUI>();
            ui.Init(choice.label, () => onSelectAction?.Invoke(choice));
        }
        this.choicesPanel.SetActive(true);
    }

    /// <summary>
    /// Hides Choices Panel
    /// </summary>
    public void HideChoicesPanel() {
        this.choicesPanel.SetActive(false);
    }

    /// <summary>
    /// Displays the Dialogue Panel
    /// </summary>
    public void ShowDialoguePanel() {
        this.dialoguePanel.SetActive(true);
    }

    /// <summary>
    /// Hides Dialogue Panel and Resets Text UI
    /// </summary>
    public void HideDialoguePanel() {
        this.dialoguePanel.SetActive(false);
        speakerNameUI.text = "";
        dialogueTextUI.text = "";
    }

    /// <summary>
    /// Updates Speaker UI
    /// </summary>
    /// <param name="speakerId"></param>
    public void SetSpeakerUI(SpeakerID speakerId) {
        SpeakerData speaker = SpeakerDataHandler.Instance.GetSpeakerData(speakerId);
        speakerNameUI.text = speaker.name;
    }

    /// <summary>
    /// Updates Dialogue UI with new text (and atarts typing animation)
    /// </summary>
    /// <param name="text"></param>
    public void SetNewDialogueText(string text) {
        StopAllCoroutines();
        currentDialogueText = text;
        if (playAnim)
            StartCoroutine(TypeSentence(text));
    }

    /// <summary>
    /// Skips Text Animation and displays full text
    /// </summary>
    /// <returns> true if animation is skipped, false if there was no animation</returns>
    public bool SkipTextAnimation() {
        if (!currentlyInTypingAnim)
            return false;
        StopAllCoroutines();
        this.currentlyInTypingAnim = false;
        dialogueTextUI.text = currentDialogueText;
        return true;
    }

    /// <summary>
    /// Plays Typing Animation
    /// </summary>
    private IEnumerator TypeSentence(string sentence)
    {
        dialogueTextUI.text = "";
        currentlyInTypingAnim = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueTextUI.text += letter;
            yield return new WaitForSeconds(textDelay);
        }

        currentlyInTypingAnim = false;
    }
}
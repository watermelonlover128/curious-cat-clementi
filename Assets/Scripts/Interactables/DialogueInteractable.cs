using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    [SerializeField]
    private DialogueSequenceScriptableObject dialogueSequenceObject;
    public override void Interact() {
        this.TriggerDialogue(dialogueSequenceObject);
    }

    public void TriggerDialogue(DialogueSequenceScriptableObject dialogueSequenceObject)
    {
        if (!DialogueManager.Instance) {
            Debug.LogError("DialogueInteractableItem: DialogueManager Instance not found!");
            return;
        }
        this.IsInteractable = false;
        DialogueManager.Instance.PlayDialogueSequence(dialogueSequenceObject);
        DialogueManager.Instance.OnEndDialogue += OnDialogueEnded;
    }

    private void OnDialogueEnded(string id) 
    {
        if (dialogueSequenceObject.ID != id)
            return;
        DialogueManager.Instance.OnEndDialogue -= OnDialogueEnded;
        this.IsInteractable = true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractableItem : Interactable
{
    [SerializeField]
    private DialogueSequenceScriptableObject dialogueSequenceObject;
    public override void Interact() {
        TriggerDialogue(dialogueSequenceObject);
    }

    public void TriggerDialogue(DialogueSequenceScriptableObject dialogueSequenceObject)
    {
        if (!DialogueManager.Instance) {
            Debug.LogError("DialogueInteractableItem: DialogueManager Instance not found!");
            return;
        }

        DialogueManager.Instance.PlayDialogueSequence(dialogueSequenceObject);
    }
}

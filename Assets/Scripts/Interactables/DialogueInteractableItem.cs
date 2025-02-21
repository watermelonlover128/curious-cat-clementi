using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractableItem : Interactable
{
    [SerializeField]
    private string dialogueFileName;
    public override void Interact() {
        TriggerDialogue(dialogueFileName);
    }

    public void TriggerDialogue(string filename)
    {
        if (!DialogueManager.instance) return;

        DialogueManager.instance.StartDialogue(filename);
    }
}

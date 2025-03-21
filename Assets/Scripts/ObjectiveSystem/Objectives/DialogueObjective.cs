using System;
using UnityEngine;

[System.Serializable]
public class DialogueObjective : Objective
{
    public string dialogueID;

    public override void Listen()
    {
       DialogueManager.Instance.OnEndDialogue += OnDialogueEnded;   
    }

    private void OnDialogueEnded(string dialogueID) {
        if (this.dialogueID == dialogueID) {
            DialogueManager.Instance.OnEndDialogue -= OnDialogueEnded;
            this.InvokeObjectiveCompletedEvent();
        }
    }
}
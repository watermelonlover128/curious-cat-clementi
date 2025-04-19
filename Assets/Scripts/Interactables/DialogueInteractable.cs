using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    [SerializeField]
    private DialogueSequenceScriptableObject dialogueSequenceObject;
    [SerializeField]
    private SerializedDictionary<string, DialogueSequenceScriptableObject> questDialogues;

    public override void Interact() {
        this.TriggerDialogue(this.GetCurrentDialogueSequence());
    }

    public void TriggerDialogue(DialogueSequenceScriptableObject dialogueSequenceObj)
    {
        if (!DialogueManager.Instance) {
            Debug.LogError("DialogueInteractableItem: DialogueManager Instance not found!");
            return;
        }
        this.IsInteractable = false;
        DialogueManager.Instance.PlayDialogueSequence(dialogueSequenceObj);
        DialogueManager.Instance.OnEndDialogue += Listen_OnDialogueEnded;
    }

    private void Listen_OnDialogueEnded(string id) 
    {
        if (this.dialogueSequenceObject.ID != id && !this.questDialogues.Values.Any(d => d.ID == id)) {
            Debug.LogWarningFormat("DialogueInteractable {0}: Waiting for initiated dialogue to end, but dialogue '{1}' ended instead. Only one dialogue should be playing at a time!", this.gameObject.name, id);
            return;
        } 
        DialogueManager.Instance.OnEndDialogue -= Listen_OnDialogueEnded;
        this.IsInteractable = true;
    }

    private DialogueSequenceScriptableObject GetCurrentDialogueSequence() {
        foreach (KeyValuePair<string, DialogueSequenceScriptableObject> pair in questDialogues) {
            if (ObjectivesManager.Instance.IsObjectiveOngoing(pair.Key))
                return pair.Value;
        }
        return this.dialogueSequenceObject;
    }
}

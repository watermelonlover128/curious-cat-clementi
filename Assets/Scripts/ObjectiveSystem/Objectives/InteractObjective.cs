using System;
using UnityEngine;

[System.Serializable]
public class InteractObjective : Objective {
    public string interactableID;

    public override void Listen()
    {
        PlayerInteraction.OnInteractedEvent += OnPlayerInteracted;
    }

    private void OnPlayerInteracted(string interactableID) {
        if (interactableID == this.interactableID) {
            PlayerInteraction.OnInteractedEvent -= OnPlayerInteracted;
            this.InvokeObjectiveCompletedEvent();

        }
    }
}
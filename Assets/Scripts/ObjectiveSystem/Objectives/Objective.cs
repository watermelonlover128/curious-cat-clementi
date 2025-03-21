using System;
using UnityEngine;

/// <summary>
/// Represents an objective to be completed
/// </summary>
[System.Serializable]
public abstract class Objective {
    public string ID;
    public string Label;

    public delegate void ObjectiveCompleted(Objective obj);
    public event ObjectiveCompleted OnObjectiveCompletedEvent;

    private Objective nextObjective;
    public Objective Next => this.nextObjective;

    /// <summary>
    /// Invokes the objective completion event to notify observers of its completion
    /// </summary>
    protected void InvokeObjectiveCompletedEvent() {
        this.OnObjectiveCompletedEvent?.Invoke(this);
    }

    public abstract void Listen();

    public void SetNextObjective(Objective objective) {
        this.nextObjective = objective;
    }
}
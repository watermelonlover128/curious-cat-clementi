using System;
using UnityEngine;

/// <summary>
/// Represents an objective to be completed
/// </summary>
public abstract class Objective {
    public delegate void ObjectiveCompleted(Objective obj);
    public event ObjectiveCompleted OnObjectiveCompletedEvent;

    /// <summary>
    /// Invokes the objective completion event to notify observers of its completion
    /// </summary>
    protected void InvokeObjectiveCompletedEvent() {
        this.OnObjectiveCompletedEvent?.Invoke(this);
    }
}
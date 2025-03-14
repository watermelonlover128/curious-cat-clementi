using System;
using UnityEngine;

/// <summary>
/// Represents an objective to be completed
/// </summary>
public abstract class Objective : MonoBehaviour {
    public delegate void ObjectiveCompleted();
    public event ObjectiveCompleted OnObjectiveCompletedEvent;

    /// <summary>
    /// Invokes the objective completion event to notify observers of its completion
    /// </summary>
    protected void InvokeObjectiveCompletedEvent() {
        this.OnObjectiveCompletedEvent?.Invoke();
    }
}
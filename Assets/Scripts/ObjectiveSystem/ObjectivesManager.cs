using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviourSingleton<ObjectivesManager>
{
    // TODO: Remove, for testing purposes
    public ObjectiveSequenceScriptableObject testObjectiveSequence;


    public delegate void ObjectiveDelegate(Objective objective);
    public event ObjectiveDelegate OnNewObjectiveEvent;
    public event ObjectiveDelegate OnObjectiveCompletedEvent;

    void Start()
    {
        this.StartObjectiveSequence(testObjectiveSequence.objectives);   
    }

    public void StartObjectiveSequence(Objective[] objectives) {
        for (int i = 0; i < objectives.Length - 1; i++)
            objectives[i].SetNextObjective(objectives[i+1]);
        this.ListenObjective(objectives[0]);
    }

    private void ListenObjective(Objective objective) {
        objective.Listen();
        objective.OnObjectiveCompletedEvent += OnObjectiveCompleted;
        this.OnNewObjectiveEvent?.Invoke(objective);
    } 

    private void OnObjectiveCompleted(Objective objective) {
        this.OnObjectiveCompletedEvent?.Invoke(objective);
        if (objective.Next != null)
            this.ListenObjective(objective.Next);
    }
}

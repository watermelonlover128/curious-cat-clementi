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

    private List<Objective> ongoingObjectives = new List<Objective>();

    void Start()
    {
        GameplayManager.Instance.OnStartGameEvent += Listen_OnStartGame;  
    }

    void Oestroy()
    {
        GameplayManager.Instance.OnStartGameEvent -= Listen_OnStartGame;
    }

    public void StartObjectiveSequence(Objective[] objectives) {
        for (int i = 0; i < objectives.Length - 1; i++)
            objectives[i].SetNextObjective(objectives[i+1]);
        this.ListenObjective(objectives[0]);
    }

    public bool IsObjectiveOngoing(string objectiveID) {
        return ongoingObjectives.Find(obj => obj.ID == objectiveID) != null;
    }

    private void ListenObjective(Objective objective) {
        objective.Listen();
        ongoingObjectives.Add(objective);
        objective.OnObjectiveCompletedEvent += OnObjectiveCompleted;
        this.OnNewObjectiveEvent?.Invoke(objective);
    } 

    private void OnObjectiveCompleted(Objective objective) {
        this.OnObjectiveCompletedEvent?.Invoke(objective);
        this.ongoingObjectives.Remove(objective);
        if (objective.Next != null)
            this.ListenObjective(objective.Next);
    }

    private void Listen_OnStartGame() {
        this.StartObjectiveSequence(testObjectiveSequence.objectives);
    }
}

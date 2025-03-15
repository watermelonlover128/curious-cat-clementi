using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviourSingleton<ObjectivesManager>
{
    [Header("UI References")]
    [SerializeField]
    private Transform objectivesUIRect;
    [SerializeField]
    private Transform objectivesUIPrefab;



    public void AddObjective(Objective obj) {
        obj.OnObjectiveCompletedEvent += OnObjectiveCompleted;
    } 

    private void OnObjectiveCompleted(Objective obj) {
        
    }
}

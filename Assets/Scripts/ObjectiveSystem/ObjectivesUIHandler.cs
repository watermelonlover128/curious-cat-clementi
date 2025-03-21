using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesUIHandler : MonoBehaviour 
{
    [Header("UI References")]
    [SerializeField]
    private Transform objectivesUIRect;
    [SerializeField]
    private Transform objectiveUIPrefab;

    private Dictionary<Objective, ObjectiveUI> objectiveUIList = new Dictionary<Objective, ObjectiveUI>();

    void Start()
    {
        ObjectivesManager.Instance.OnNewObjectiveEvent += OnNewObjective;
        ObjectivesManager.Instance.OnObjectiveCompletedEvent += OnObjectiveCompleted;
    }

    void OnDestroy()
    {
        if (ObjectivesManager.Instance != null) {
            ObjectivesManager.Instance.OnNewObjectiveEvent -= OnNewObjective;
            ObjectivesManager.Instance.OnObjectiveCompletedEvent -= OnObjectiveCompleted;
        }
    }

    private void OnNewObjective(Objective objective) 
    {
        if (objectiveUIList.ContainsKey(objective))
        {
            Debug.LogWarning("ObjectivesUIHandler: Attempted to add duplicate objective!");
            return;
        }
        ObjectiveUI ui = Instantiate(objectiveUIPrefab, objectivesUIRect).GetComponent<ObjectiveUI>();
        ui.Init(objective.Label);
        objectiveUIList[objective] = ui; 
    }

    private void OnObjectiveCompleted(Objective objective) 
    {
        if (!objectiveUIList.ContainsKey(objective)) {
            Debug.LogWarning("ObjectivesUIHandler: Attempted to remove/complete an objective which was not previously added!");
            return;
        }
        Destroy(objectiveUIList[objective].gameObject);
        objectiveUIList.Remove(objective);
    }


}
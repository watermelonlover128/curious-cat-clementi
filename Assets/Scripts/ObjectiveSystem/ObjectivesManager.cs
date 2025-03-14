using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Transform objectivesUIRect;
    [SerializeField]
    private Transform objectivesUIPrefab;
    private List<Objective> objectives;



    public void AddObjective(Objective obj) {
        objectives.Add(obj);
    } 
}

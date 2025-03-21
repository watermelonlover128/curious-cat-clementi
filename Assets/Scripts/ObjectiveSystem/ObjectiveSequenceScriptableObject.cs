using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ObjectiveSequenceScriptableObject", menuName = "ScriptableObjects/Objectives/ObjectiveSequenceScriptableObject")]
public class ObjectiveSequenceScriptableObject : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    public Objective[] objectives;
}

[System.Serializable]
public class ObjectiveData 
{
    [SerializeReference, SubclassSelector]
    public Objective objective = null;
}
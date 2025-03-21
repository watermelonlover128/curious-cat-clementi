using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueSequenceScriptableObject", menuName = "ScriptableObjects/Dialogue/DialogueSequenceScriptableObject")]
public class DialogueSequenceScriptableObject : ScriptableObject
{
    public string ID;
    public List<Dialogue> dialogues;
    public List<DialogueChoice> choices;
}

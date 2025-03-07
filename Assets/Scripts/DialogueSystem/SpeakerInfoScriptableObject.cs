using System.Collections.Generic;
using Project.Tools.DictionaryHelp;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpeakerInfoScriptableObject", menuName = "ScriptableObjects/SpeakerInfoScriptableObject")]
public class SpeakerInfoScriptableObject : ScriptableObject
{
    public SerializableDictionary<SpeakerID, SpeakerData> speakersData;

}

[System.Serializable]
public struct SpeakerData {
    public string name;
    public Sprite sprite;
}
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpeakerInfoScriptableObject", menuName = "ScriptableObjects/SpeakerInfoScriptableObject")]
public class SpeakerInfoScriptableObject : ScriptableObject
{
    public SerializedDictionary<SpeakerID, SpeakerData> speakersData;

}

[System.Serializable]
public struct SpeakerData {
    public string name;
    public Sprite sprite;
}
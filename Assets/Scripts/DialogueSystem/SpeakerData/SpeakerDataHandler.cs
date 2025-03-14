using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SpeakerID
{
    PlayerCat = 0,
    Cat1,
    Cat2,
    Cat3,

}

/// <summary>
/// Handles speaker data for dialogues
/// TODO: If used by other classes and not just DialogueManager, make a singleton instead.
/// </summary>
class SpeakerDataHandler : Singleton<SpeakerDataHandler>
{
    private const string SPEAKER_DATA_FILENAME = "SpeakersData";
    private Dictionary<SpeakerID, SpeakerData> speakersData;

    protected override void Init()
    {
        SpeakerInfoScriptableObject speakersDataFile = (SpeakerInfoScriptableObject)Resources.Load(SPEAKER_DATA_FILENAME);
        if (speakersDataFile == null)
        {
            Debug.LogErrorFormat("DialogueManager:SpeakerDataHandler: Speaker Sprites Data Scriptable Object could not be found at: 'Resources/{0}'!", SPEAKER_DATA_FILENAME);
            return;
        }
        this.speakersData = speakersDataFile.speakersData;

    }

    public Sprite GetSpeakerSprite(SpeakerID id)
    {
        if (!speakersData.ContainsKey(id))
        {
            Debug.LogErrorFormat("DialogueManager:SpeakerDataHandler: Could not find sprite data for speaker id {0}!", id);
            return null;
        }
        return speakersData[id].sprite;
    }

    public string GetSpeakerName(SpeakerID id)
    {
        if (!speakersData.ContainsKey(id))
        {
            Debug.LogErrorFormat("DialogueManager:SpeakerDataHandler: Could not find name data for speaker id {0}!", id);
            return null;
        }
        return speakersData[id].name;
    }

    public SpeakerData GetSpeakerData(SpeakerID id)
    {
        if (!speakersData.ContainsKey(id))
        {
            Debug.LogErrorFormat("DialogueManager:SpeakerDataHandler: Could not find speaker data for speaker id {0}!", id);
            return new SpeakerData();
        }
        return speakersData[id];
    }
}
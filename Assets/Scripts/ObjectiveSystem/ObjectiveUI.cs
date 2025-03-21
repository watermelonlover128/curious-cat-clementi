using System;
using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour {
    
    [SerializeField]
    private TextMeshProUGUI labelText;

    public void Init(string label)
    {
        this.labelText.text = label;
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoiceUI : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI labelUI;
    [SerializeField]
    private Button button;

    public void Init(string label, Action onClickAction) {
        this.labelUI.text = label;
        this.button.onClick.AddListener(() => onClickAction());
    }
}
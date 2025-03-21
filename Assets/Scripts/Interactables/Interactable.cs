using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private string id;
    [SerializeField]
    private GameObject interactCanvas;

    public string ID => this.id;

    public bool IsInteractable {get; protected set;}
    
    void Start()
    {
        this.IsInteractable = true;
        this.interactCanvas.SetActive(false);   
    }
    public virtual void Interact()
    {

    }

    public void OnInteractable() 
    {
        this.interactCanvas.SetActive(true);
    }
    public void OnUninteractable()
    {
        this.interactCanvas.SetActive(false);
    }
}

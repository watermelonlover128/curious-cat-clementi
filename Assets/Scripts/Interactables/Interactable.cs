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
    
    void Start()
    {
        interactCanvas.SetActive(false);   
    }
    public virtual void Interact()
    {

    }

    public void OnInteractable() 
    {
        interactCanvas.SetActive(true);
    }
    public void OnUninteractable()
    {
        interactCanvas.SetActive(false);
    }
}

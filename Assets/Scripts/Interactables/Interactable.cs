using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject interactCanvas;

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

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

    private bool interactable;

    public bool IsInteractable {
        get {
        return this.interactable;
        }
        protected set {
            this.interactable = value;
            this.OnInteractableStatusChangedEvent?.Invoke();
        }
    }

    public delegate void InteractableStatusChanged();
    public event InteractableStatusChanged OnInteractableStatusChangedEvent;
    
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

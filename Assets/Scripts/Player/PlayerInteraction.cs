using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactablesInRange = new List<Interactable>();
    private Interactable currentInteractable = null;

    public delegate void Interacted(string interactableID);
    public static event Interacted OnInteractedEvent;

    private Interactable CurrentInteractable 
    { 
        get {
            this.SetCurrentInteractable();
            return this.currentInteractable;
        }
    }
    void Start()
    {
        InputHandler.Instance.OnPlayerInteractInput += OnInteractInput;
    }
    void OnDestroy()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnPlayerInteractInput -= OnInteractInput;
    }

    private void OnInteractInput() 
    {
        if (this.CurrentInteractable == null)
            return;
        Interactable interactable = this.CurrentInteractable;
        interactable.Interact();
        this.SetCurrentInteractable();
        PlayerInteraction.OnInteractedEvent?.Invoke(interactable.ID);
    }

    private void SetCurrentInteractable() 
    {
        if (this.currentInteractable != null)
            this.currentInteractable.OnUninteractable();
        else
            this.currentInteractable = null;
        
        List<Interactable> validInteractables = this.interactablesInRange.FindAll(i => i.IsInteractable);
        if (validInteractables.Count == 0) {
            this.currentInteractable = null;
            return;
        }
        validInteractables.Sort((a, b) =>
            Vector3.Distance(this.transform.position, a.transform.position).
                CompareTo(Vector3.Distance(this.transform.position, b.transform.position)));
        this.currentInteractable = validInteractables[0];
        this.currentInteractable.OnInteractable();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.isTrigger)
            return;

        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            this.interactablesInRange.Add(interactable);
            interactable.OnInteractableStatusChangedEvent += this.SetCurrentInteractable;
            this.SetCurrentInteractable();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            this.interactablesInRange.Remove(interactable);
            interactable.OnInteractableStatusChangedEvent -= this.SetCurrentInteractable;
            this.SetCurrentInteractable();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactablesInRange = new List<Interactable>();

    private Interactable CurrentInteractable 
    { 
        get {
            if (interactablesInRange.Count == 0)
                return null;
            interactablesInRange.Sort((a, b) =>
                Vector3.Distance(this.transform.position, a.transform.position).
                    CompareTo(Vector3.Distance(this.transform.position, b.transform.position)));
            return interactablesInRange[0];
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
        this.CurrentInteractable.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.isTrigger)
            return;

        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            this.interactablesInRange.Add(interactable);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable != null) {
            this.interactablesInRange.Remove(interactable);
        }

    }
}

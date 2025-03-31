using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviourSingleton<InputHandler> {

    public delegate void PlayerMovementInput(Vector2 dir);
    public event PlayerMovementInput OnPlayerMovementInputEvent;

    public delegate void PlayerInteractInput();
    public event PlayerInteractInput OnPlayerInteractInput;

    public delegate void DialogueInput();
    public event DialogueInput OnDialogueInput;


    void FixedUpdate()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused)
            return;
        // Check Player Movement Input
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeybindSettings.MOVEMENT_UP)) {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeybindSettings.MOVEMENT_DOWN))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeybindSettings.MOVEMENT_LEFT))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeybindSettings.MOVEMENT_RIGHT))
        {
            dir += Vector2.right;
        }
        if (dir.sqrMagnitude > 0)
            this.OnPlayerMovementInputEvent?.Invoke(dir.normalized);
    }

    void Update()
    {
        // Check Player Interact Input
        if (Input.GetKeyDown(KeybindSettings.KEY_INTERACT))
            this.OnPlayerInteractInput?.Invoke();
        // Check Skip Dialogue Input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) 
            this.OnDialogueInput?.Invoke();
        
    }


}
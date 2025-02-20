using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    private Animator animator = null;

    private Rigidbody2D rb = null;
    private Vector2 velocity = Vector2.zero;


    // TBC: Make Const Later, for now easy adjust in inspector
    [SerializeField]
    private  float FRICTION_FORCE = 40.0f;
    [SerializeField]
    private  float ACCEL_FORCE = 30.0f;
    [SerializeField]
    private float MAX_VELOCITY = 4.0f;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();   
    }

    void Start()
    {
        InputHandler.Instance.OnPlayerMovementInputEvent += OnMovementInput;
    }
    void OnDestroy()
    {
        if (InputHandler.Instance != null)
            InputHandler.Instance.OnPlayerMovementInputEvent -= OnMovementInput;
    }

    void FixedUpdate()
    {
        bool isMoving = this.velocity.sqrMagnitude > 0;
        // Update Animator
        this.animator.SetBool("IsRunning", isMoving);
        if (!isMoving)
            return;
        // Move Player By Velocity
        this.rb.position += this.velocity * Time.fixedDeltaTime;
        // Update Rotation
        if (Mathf.Abs(this.velocity.x - 0) > Mathf.Epsilon)
            this.transform.rotation = Quaternion.AngleAxis(this.velocity.x < 0 ? 180.0f : 0.0f, Vector3.up);
        // Apply Friction
        Vector2 initialDir = this.velocity;
        this.velocity -= this.velocity.normalized * FRICTION_FORCE * Time.fixedDeltaTime;
        // Stop Movement if friction caused velocity to go opposite direction
        if (Vector2.Dot(initialDir, this.velocity) < 0) {
            this.velocity = Vector2.zero;
        }
    }

    private void OnMovementInput(Vector2 dir) {
        // Apply Acceleration
        this.velocity += dir * ACCEL_FORCE * Time.fixedDeltaTime;
        // Clamp Velocity
        if (this.velocity.sqrMagnitude > MAX_VELOCITY * MAX_VELOCITY) 
            this.velocity = this.velocity.normalized * MAX_VELOCITY;
    }
}
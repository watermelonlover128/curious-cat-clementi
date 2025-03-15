using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyManager : MonoBehaviour
{
    private AIPath aiPath;

    [SerializeField]
    private Transform deathzone;

    [SerializeField]
    private aggroZone aggroZone;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.canMove = false;

        aggroZone.beginAggro += OnAggro;
    }

    // Update is called once per frame
    void Update()
    {
        if (aiPath && aiPath.canMove) {
            Vector3 moveDirection = aiPath.velocity.normalized;

            // Rotate only if moving
            if (moveDirection != Vector3.zero)
            {
                // Create a rotation that looks in the move direction (assuming 2D, rotating on Z-axis)
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

                // Apply rotation to the hitbox
                deathzone.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    void OnAggro(Collider2D collider) {
        aiPath.canMove = true;
    }
}

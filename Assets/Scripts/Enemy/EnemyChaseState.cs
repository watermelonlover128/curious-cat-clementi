using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy) {
        enemy.aiPath.canMove = true;
        enemy.deathzone.gameObject.SetActive(false);
        enemy.captureZone.SetActive(true);
    }

    public override void UpdateState(EnemyStateManager enemy) {
        Vector3 moveDirection = enemy.aiPath.velocity.normalized;

        // Rotate only if moving
        if (moveDirection != Vector3.zero)
        {
            // Create a rotation that looks in the move direction (assuming 2D, rotating on Z-axis)
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // Apply rotation to the hitbox
            enemy.deathzone.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public override void ExitState(EnemyStateManager enemy) {
        enemy.aiPath.canMove = false;
    }
}

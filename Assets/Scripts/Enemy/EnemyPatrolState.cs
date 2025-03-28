using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{ 
    private int waypointIndex = 1;
    private int dir = 1;

    public override void EnterState(EnemyStateManager enemy) {
        // make sure pathfinding is disabled, only enable relevant hitboxes
        enemy.aiPath.canMove = false;
        enemy.deathzone.gameObject.SetActive(true);
        enemy.captureZone.SetActive(false);
    }

    public override void UpdateState(EnemyStateManager enemy) {
        // move towards target waypoint
        Vector3 targetPosition = enemy.waypoints[waypointIndex].position;

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position,
            targetPosition, enemy.moveSpeed * Time.deltaTime);
        
        // rotate deathzone to face the correct direction
        Vector3 direction = (targetPosition - enemy.transform.position).normalized;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            enemy.deathzone.rotation = Quaternion.Slerp(enemy.deathzone.rotation, targetRotation, enemy.rotationSpeed * Time.deltaTime);
        }

        // increment to next waypoint when the current waypoint is reached
        if (enemy.transform.position == targetPosition)
        {
            waypointIndex += dir;
        }

        // loop back around when the path ends, either by reversing direction through the path or just looping back to the first point
        if (waypointIndex >= enemy.waypoints.Count || waypointIndex < 0) {
            if (enemy.loopReverse) {
                dir = -dir;
                waypointIndex += dir;
            } else {
                waypointIndex = 0;
            }
        }
    }

    public override void ExitState(EnemyStateManager enemy) {

    }
}

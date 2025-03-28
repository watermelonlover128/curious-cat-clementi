using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class AggroZone : MonoBehaviour
{
    [SerializeField]
    private EnemyStateManager enemy;

    // check if player in zone, helps when the chase sequence begins and the player is already in range
    private bool playerInZone = false; 
    private bool canAggro = false;
    private bool isChasing = false;

    void Start() {
        if (FishStallSceneManager.instance) {
            FishStallSceneManager.instance.ChaseBegin += EnableChase;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (canAggro && !isChasing) {
                enemy.ChangeState(enemy.chaseState);
                isChasing = true;
            }

            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerInZone = false;
        }
    }

    private void EnableChase() {
        canAggro = true;
        ForceCheckAggro();
    }

    // for cases when the player is already in range and the enemy is set to chase 
    public void ForceCheckAggro() {
        if (playerInZone && canAggro) {
            enemy.ChangeState(enemy.chaseState);
            isChasing = true;
        }
    }
}

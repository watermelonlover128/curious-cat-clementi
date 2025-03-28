using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaptureZone : MonoBehaviour
{
    // immediately game overs on trigger enter
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "player_hurtbox") {
            FishStallSceneManager.instance.GameOver();
        }
    } 
}

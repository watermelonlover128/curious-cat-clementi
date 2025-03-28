using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaptureZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            FishStallSceneManager.instance.GameOver();
        }
    } 
}

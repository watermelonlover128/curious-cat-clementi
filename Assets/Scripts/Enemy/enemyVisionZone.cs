using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// controls the vision of the fishmonger, if you're within vision for a certain amount of time, trigger a game over
public class enemyVisionZone : MonoBehaviour
{
    [SerializeField] private Image barFill;

    // can control how much time the cat needs to be in the vision zone to be caught 
    [SerializeField] private float timeInZone = 0f;
    [SerializeField] private float timeToGameOver = 1f;
    [SerializeField] private float timeDepleteMultiplier = 1.1f;

    private bool inZone = false;

    void Start() {
        FishStallSceneManager.instance.OnRespawn += Reset;
    }

    void Update()
    {
        // if paused, dont increase the timer
        if (PauseManager.Instance && PauseManager.Instance.isPaused) return; 

        // increase time if in zone, deplete if not 
        if (inZone) {
            timeInZone += Time.deltaTime;
        } else {
            timeInZone = Mathf.Max(timeInZone - Time.deltaTime * timeDepleteMultiplier, 0);
        }

        // game over when time is up
        if (timeInZone >= timeToGameOver) {
            FishStallSceneManager.instance.GameOver();
        }

        // update the time indicator
        barFill.fillAmount = Mathf.Clamp01(timeInZone / timeToGameOver);
    }


    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "player_hurtbox") {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "player_hurtbox") {
            inZone = false;
        }
    }

    // when the game restarts, reset all variables
    void Reset() {
        timeInZone = 0f;
        inZone = false;
        barFill.fillAmount = 0;
    }
}

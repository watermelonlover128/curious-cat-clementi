using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyDeathZone : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    [SerializeField] private Image barFill;

    [SerializeField] private EnemyStateManager enemy;

    [SerializeField] private float timeInZone = 0f;
    [SerializeField] private float timeToGameOver = 1.5f;
    [SerializeField] private float timeMultiplier = 1.3f;
    [SerializeField] private float timeDepleteMultiplier = 1.1f;

    private bool inZone = false;

    void Start() {
        FishStallSceneManager.instance.OnRespawn += Reset;
    }

    void Update()
    {
        if (PauseManager.Instance && PauseManager.Instance.isPaused) return; 

        if (inZone) {
            timeInZone += Time.deltaTime * timeMultiplier;
        } else {
            timeInZone = Mathf.Max(timeInZone - Time.deltaTime * timeDepleteMultiplier, 0);
        }

        if (timeInZone >= timeToGameOver) {
            FishStallSceneManager.instance.GameOver();
        }

        barFill.fillAmount = Mathf.Clamp01(timeInZone / timeToGameOver);
    }


    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            inZone = false;
        }
    }

    void Reset() {
        timeInZone = 0f;
        inZone = false;
        barFill.fillAmount = 0;
    }
}

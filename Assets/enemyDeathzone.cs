using UnityEngine;
using Pathfinding;

public class enemyDeathzone : MonoBehaviour
{
    private PolygonCollider2D polyCollider;

    private void Start() {
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("game over");
        }
    }
}

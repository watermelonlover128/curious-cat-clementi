using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class aggroZone : MonoBehaviour
{
    public event Action<Collider2D> beginAggro;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            beginAggro?.Invoke(other);
        }
    }
}

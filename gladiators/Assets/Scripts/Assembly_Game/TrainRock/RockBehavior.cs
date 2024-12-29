using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehavior : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Player") {
            TrainRockManager.Instance.doDamage();
        }
        Destroy(gameObject);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}

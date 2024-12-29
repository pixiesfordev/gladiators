using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehavior : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        // 當石頭碰到任何物件時執行清除動作
        Debug.Log($"Stone collided with: {collision.gameObject.name}");
        if (collision.gameObject.name == "Player") {
            TrainRockManager.Instance.doDamage();
        }
        Destroy(gameObject); // 銷毀石頭
    }

    private void OnBecameInvisible() {
        // 當石頭離開攝影機可視範圍時執行清除動作
        Debug.Log("Stone is no longer visible and will be destroyed.");
        Destroy(gameObject); // 銷毀石頭
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehavior : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        // ��ʯ�^�����κ�����r�����������
        Debug.Log($"Stone collided with: {collision.gameObject.name}");
        if (collision.gameObject.name == "Player") {
            TrainRockManager.Instance.doDamage();
        }
        Destroy(gameObject); // �N��ʯ�^
    }

    private void OnBecameInvisible() {
        // ��ʯ�^�x�_�zӰ�C��ҕ�����r�����������
        Debug.Log("Stone is no longer visible and will be destroyed.");
        Destroy(gameObject); // �N��ʯ�^
    }
}

using Cinemachine;
using Gladiators.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeTimer = 0.0f;

    public void Shake(float intensity = 5f, float duration = 0.25f) {
        var cbmcp = BattleManager.Instance.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cbmcp.m_AmplitudeGain = intensity;
        shakeTimer = duration;
    }

    public void Update() {
        if (shakeTimer > 0.0f) { 
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0.0f) {
                var cbmcp = BattleManager.Instance.vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cbmcp.m_AmplitudeGain = 0f;
            }
        }
    }
}

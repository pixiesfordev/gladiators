using Cysharp.Threading.Tasks;
using Scoz.Func;
using System;
using TMPro;
using UnityEngine;

namespace Gladiators.Battle {
    public class Projector : MonoBehaviour {

        Character target;
        float timeToTarget;
        Action hitAc;

        public void Init(Character _char, float _timeToTarget, Action _hitAc) {
            if (_char == null) {
                WriteLog.LogError("傳入null目標");
                return;
            }
            target = _char;
            timeToTarget = _timeToTarget;
            hitAc = _hitAc;
            // 開始移動
            move().Forget();
        }

        async UniTask move() {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < timeToTarget) {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / timeToTarget;
                Vector3 targetPosition = target.CenterPos;

                // 飛向目標
                Vector3 previousPosition = transform.position;
                transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                // 面相目標
                Vector3 direction = (transform.position - previousPosition).normalized;
                if (direction != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(direction);
                }

                await UniTask.Yield();
            }
            transform.position = target.CenterPos;
            hitAc?.Invoke();
        }

    }
}

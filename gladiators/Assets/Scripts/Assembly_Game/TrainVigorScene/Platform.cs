using Cysharp.Threading.Tasks;
using Scoz.Func;
using System;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Platform : MonoBehaviour {
        [SerializeField] MinMaxF AngleRange; // 角度範圍
        [SerializeField] MinMaxF Interval; // 每次旋轉持續時間範圍

        bool rotating = false;
        Quaternion defaultRotation;

        public void Init() {
            defaultRotation = transform.rotation;
        }
        public void ResetPlatform() {
            transform.rotation = defaultRotation;
        }

        public void StartRotate() {
            rotating = true;
            rotate().Forget(); // 開始旋轉
        }

        public void StopRotate() {
            rotating = false;
        }

        async UniTaskVoid rotate() {
            while (rotating) {
                // 隨機生成旋轉持續時間和目標角度
                float duration = UnityEngine.Random.Range(Interval.X, Interval.Y);
                float randomX = UnityEngine.Random.Range(AngleRange.X, AngleRange.Y);
                float randomY = UnityEngine.Random.Range(AngleRange.X, AngleRange.Y);
                float randomZ = UnityEngine.Random.Range(AngleRange.X, AngleRange.Y);
                Quaternion targetRotation = Quaternion.Euler(randomX, randomY, randomZ);
                // 開始平滑旋轉
                Quaternion startRotation = transform.rotation;
                float elapsed = 0f;

                while (elapsed < duration) {
                    if (!rotating) break; // 如果旋轉已停止，退出
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / duration);
                    transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                    await UniTask.Yield(); // 等待下一幀
                }

                transform.rotation = targetRotation;
            }
        }
    }
}

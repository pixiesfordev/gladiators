using Cysharp.Threading.Tasks;
using Scoz.Func;
using System.Threading;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Platform : MonoBehaviour {
        [SerializeField] MinMaxF RndSpdRange; // 隨機速度範圍
        [SerializeField] MinMaxF Interval;    // 每次旋轉持續時間範圍

        [SerializeField] Transform Ball1;
        [SerializeField] Transform Ball2;

        bool rotating = false;
        Quaternion ball1DefaultRot;
        Quaternion ball2DefaultRot;
        CancellationTokenSource cts;

        public void Init() {
            ball1DefaultRot = Ball1.rotation;
            ball2DefaultRot = Ball2.rotation;

            cts = new CancellationTokenSource();
            rotateBall(Ball1, cts.Token).Forget();
            rotateBall(Ball2, cts.Token).Forget();
        }

        public void ResetPlatform() {
            Ball1.rotation = ball1DefaultRot;
            Ball2.rotation = ball2DefaultRot;
        }

        public void StartRotate() {
            rotating = true;
        }

        public void StopRotate() {
            rotating = false;
        }

        void OnDestroy() {
            if (cts != null && !cts.IsCancellationRequested) {
                cts.Cancel();
                cts.Dispose();
            }
        }

        async UniTask rotateBall(Transform _ball, CancellationToken _ct) {
            while (!_ct.IsCancellationRequested) {
                if (rotating) {
                    float speed = RndSpdRange.GetRandInRange();
                    float duration = Interval.GetRandInRange();
                    Vector3 axis = Random.onUnitSphere;

                    float elapsed = 0f;
                    while (elapsed < duration && !_ct.IsCancellationRequested) {
                        _ball.Rotate(axis, speed * Time.deltaTime, Space.Self);
                        elapsed += Time.deltaTime;
                        await UniTask.Yield(PlayerLoopTiming.Update, _ct);
                    }
                }
                await UniTask.Yield(PlayerLoopTiming.Update, _ct);
            }
        }
    }
}

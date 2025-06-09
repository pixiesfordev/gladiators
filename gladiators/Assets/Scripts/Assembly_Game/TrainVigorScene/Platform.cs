using Cysharp.Threading.Tasks;
using Scoz.Func;
using System.Threading;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Platform : MonoBehaviour {
        [SerializeField] MinMaxF RndSpdRange; // 隨機速度範圍
        [SerializeField] MinMaxF Interval;    // 每次旋轉持續時間範圍

        bool rotating = false;
        Quaternion defaultRotation;
        CancellationTokenSource cts;

        public void Init() {
            defaultRotation = transform.rotation;
            cts = new CancellationTokenSource();
            rotateLoop(cts.Token).Forget();   // 啟動旋轉迴圈
        }

        public void ResetPlatform() {
            transform.rotation = defaultRotation;
        }

        public void StartRotate() {
            rotating = true;
        }

        public void StopRotate() {
            rotating = false;
        }

        void OnDestroy() {
            // 物件被銷毀時取消所有未完成的 UniTask
            if (cts != null && !cts.IsCancellationRequested) {
                cts.Cancel();
                cts.Dispose();
            }
        }

        async UniTask rotateLoop(CancellationToken _ct) {
            // 持續檢查 rotating 狀態及取消標記
            while (!_ct.IsCancellationRequested) {
                if (rotating) {
                    // 隨機產生本次旋轉參數
                    float speed = RndSpdRange.GetRandInRange();
                    float duration = Interval.GetRandInRange();
                    Vector3 axis = Random.onUnitSphere;

                    float elapsed = 0f;
                    // 在指定時間內持續旋轉
                    while (elapsed < duration && !_ct.IsCancellationRequested) {
                        transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
                        elapsed += Time.deltaTime;
                        await UniTask.Yield(PlayerLoopTiming.Update, _ct);
                    }
                }
                // 若未啟動旋轉或剛完成一輪，等到下一更新再檢查
                await UniTask.Yield(PlayerLoopTiming.Update, _ct);
            }
        }
    }
}

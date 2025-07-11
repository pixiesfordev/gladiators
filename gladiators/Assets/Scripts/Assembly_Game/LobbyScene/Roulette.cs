using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using Scoz.Func;

namespace Gladiators.Main {
    public class Roulette : MonoBehaviour {
        [Header("摩擦與停轉設定")]
        [Tooltip("摩擦減速度 (degrees/sec²)，越大越快停下")]
        public float friction = 300f;
        [Tooltip("當速度低於此 (deg/sec) 時，摩擦力將加倍")]
        public float quickStopThreshold = 60f;
        [Tooltip("當速度低於此 (deg/sec) 時，直接停止並進入卡位流程")]
        public float immediateStopThreshold = 10f;  // 必須小於 quickStopThreshold

        [Header("卡位設定")]
        [Tooltip("停住後等待時間（秒）")]
        public float waitBeforeSnap = 0.3f;
        [Tooltip("彈回卡位時間（秒）")]
        public float snapDuration = 0.3f;
        [Tooltip("彈回曲線")]
        public Ease snapEase = Ease.OutBack;
        [Tooltip("一個圖騰佔用角度(要能整除360)")]
        public float segmentDegree = 60f;

        // 內部狀態
        private bool _isSpinning;
        private float _angularVelocity;      // degrees/sec
        private float _accumulatedY;         // 累計角度，可超過360
        private CancellationTokenSource _cts;

        Action<int> AC_ResultIdx;

        public void Init() {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            if (360 % segmentDegree != 0) {
                WriteLog.LogError($"segmentDegree 設定錯誤{segmentDegree} 要能整除360");
            }

            _accumulatedY = transform.eulerAngles.y;
            _angularVelocity = 0f;
            _isSpinning = false;

            transform.DOKill();
        }

        public void StartSpin(float initialAngularVelocity, Action<int> _ac) {
            _cts?.Cancel();
            transform.DOKill();
            _isSpinning = false;

            _cts = new CancellationTokenSource();
            _angularVelocity = initialAngularVelocity;
            _isSpinning = true;
            AC_ResultIdx = _ac;
        }

        private async void Update() {
            if (!_isSpinning) return;
            var token = _cts.Token;

            float dt = Time.deltaTime;
            float prevVel = _angularVelocity;

            // 1. 累加角度
            _accumulatedY += prevVel * dt;

            // 2. 如果速度低於立即停止閾值，直接停止
            if (Mathf.Abs(prevVel) <= immediateStopThreshold) {
                _angularVelocity = 0f;
                _isSpinning = false;
            } else {
                // 3. 否則按 quickStopThreshold 調整摩擦
                float appliedFriction = friction;
                if (Mathf.Abs(prevVel) <= quickStopThreshold) {
                    appliedFriction *= 2f;
                }
                _angularVelocity -= Mathf.Sign(prevVel) * appliedFriction * dt;
            }

            // 4. 更新 Transform（視覺上取模 360）
            var e = transform.eulerAngles;
            e.y = _accumulatedY % 360f;
            transform.eulerAngles = e;

            // 5. 當停止（速度過零或被 immediate 停止）時，進入卡位流程
            if (!_isSpinning) {
                try {
                    await UniTask.Delay(TimeSpan.FromSeconds(waitBeforeSnap), cancellationToken: token);
                } catch (OperationCanceledException) {
                    return;
                }

                // 6. 計算最接近 segmentDegree 度數的倍數
                float snapAngle = Mathf.Round(_accumulatedY / segmentDegree) * segmentDegree;
                float delta = snapAngle - _accumulatedY;

                // 7. 彈回卡位 Tween
                var tcs = new UniTaskCompletionSource();
                var tween = transform
                    .DOLocalRotate(new Vector3(0, delta, 0), snapDuration, RotateMode.LocalAxisAdd)
                    .SetEase(snapEase);

                tween.onComplete += () => tcs.TrySetResult();
                tween.onKill += () => tcs.TrySetResult();

                try {
                    await tcs.Task.AttachExternalCancellation(token);
                    _accumulatedY = snapAngle;

                    // 8. 計算並回傳所在區段的 index（絕對對應盤面，不計圈數）
                    //    把 _accumulatedY 限制到 [0,360) 範圍
                    float norm = (_accumulatedY % 360f + 360f) % 360f;
                    //    分段：0~(segmentDegree) → idx=0，(segmentDegree)~2*segmentDegree → idx=1…
                    int resultIdx = Mathf.FloorToInt(norm / segmentDegree);
                    AC_ResultIdx?.Invoke(resultIdx);
                } catch (OperationCanceledException) {
                    // 新的 StartSpin 或 StopSpin 取消
                }
            }
        }

        public void StopSpin() {
            _cts?.Cancel();
            _isSpinning = false;
            _angularVelocity = 0f;
            transform.DOKill();
        }

        private void OnDestroy() {
            StopSpin();
            _cts?.Dispose();
        }
    }
}

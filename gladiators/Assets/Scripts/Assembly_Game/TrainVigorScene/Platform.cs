using Cysharp.Threading.Tasks;
using Scoz.Func;
using System.Threading;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Platform : MonoBehaviour {
        [SerializeField] MinMaxF[] RndSpdRange;
        [SerializeField] MinMaxF Interval;

        [SerializeField] Transform Ball1;
        [SerializeField] Transform Ball2;
        [SerializeField] Chara_TrainVigor Char;
        [SerializeField] float MaxForce = 10;

        bool rotating = false;
        Quaternion ball1DefaultRot;
        Quaternion ball2DefaultRot;
        CancellationTokenSource cts;
        int curLV = 0;
        float maxRotateSpd = 0;

        // 儲存前一幀 rotation
        Quaternion prevRot1;
        Quaternion prevRot2;



        Vector3 getAngularVelocity(Transform t, ref Quaternion prevRot) {
            Quaternion delta = t.rotation * Quaternion.Inverse(prevRot);
            delta.ToAngleAxis(out float angleDeg, out Vector3 axis);
            prevRot = t.rotation;

            // 單位：度/秒
            return axis * angleDeg / Time.fixedDeltaTime;
        }

        public void Init() {
            ball1DefaultRot = Ball1.rotation;
            ball2DefaultRot = Ball2.rotation;
            prevRot1 = Ball1.rotation;
            prevRot2 = Ball2.rotation;
            maxRotateSpd = RndSpdRange[RndSpdRange.Length - 1].Y;
            cts = new CancellationTokenSource();
            rotateBall(Ball1, cts.Token).Forget();
            rotateBall(Ball2, cts.Token).Forget();
            SetLevel(0);
        }

        public void ResetPlatform() {
            Ball1.rotation = ball1DefaultRot;
            Ball2.rotation = ball2DefaultRot;
            prevRot1 = ball1DefaultRot;
            prevRot2 = ball2DefaultRot;
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

        public void SetLevel(int _level) {
            if (_level < RndSpdRange.Length) curLV = _level;
            else {
                WriteLog.LogError($"設定_level錯誤: {_level}");
                return;
            }
        }

        async UniTask rotateBall(Transform _ball, CancellationToken _ct) {
            while (!_ct.IsCancellationRequested) {
                if (rotating) {
                    float speed = RndSpdRange[curLV].GetRandInRange();
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


        /// <summary>
        /// 測試用固定Z軸方向旋轉
        /// </summary>
        async UniTask rotateBall_Test(Transform _ball, CancellationToken _ct) {
            while (!_ct.IsCancellationRequested) {
                if (rotating) {
                    float speed = RndSpdRange[curLV].GetRandInRange();
                    float duration = Interval.GetRandInRange();
                    Vector3 axis = Vector3.forward;

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


        /// <summary>
        /// 根據目前冰球轉速與方向給予腳色持續力道
        /// 例如最大力道(MaxForce)是10 
        /// 冰球轉速是隨機的 且分等級 最高等級的最高速就是給予最大力道的時候 maxRotateSpd = RndSpdRange[RndSpdRange.Length - 1].Y;
        /// 假設目前冰球A轉速是向上轉 且速度是50 目前冰球B轉速是向下轉 且速度是25 那當下給予的力道就是 10*(50-25)/100=2.5向上
        /// </summary>
        void FixedUpdate() {
            if (!rotating || Char == null) return;

            // 計算角速度
            Vector3 angularVel1 = getAngularVelocity(Ball1, ref prevRot1);
            Vector3 angularVel2 = getAngularVelocity(Ball2, ref prevRot2);

            // 球心 → 腳色方向
            Vector3 centerToChar1 = (Char.transform.position - Ball1.position).normalized;
            Vector3 centerToChar2 = (Char.transform.position - Ball2.position).normalized;

            // 切線方向：角速度 × 接觸點方向
            Vector3 tangentDir1 = Vector3.Cross(angularVel1, centerToChar1);
            Vector3 tangentDir2 = Vector3.Cross(angularVel2, centerToChar2);

            // 僅保留 XZ 平面
            Vector3 flatTangential1 = new Vector3(tangentDir1.x, 0f, tangentDir1.z);
            Vector3 flatTangential2 = new Vector3(tangentDir2.x, 0f, tangentDir2.z);

            // 差異方向（代表哪顆球推得比較強＋方向）
            Vector3 forceDir = flatTangential1 - flatTangential2;

            // 計算力道比例
            float forceMag = Mathf.Clamp01(forceDir.magnitude / maxRotateSpd) * MaxForce;

            if (forceDir.sqrMagnitude > 0.0001f) {
                Vector3 finalForce = forceDir.normalized * forceMag;
                Char.AddForceToChar(finalForce);
            }
        }

    }
}

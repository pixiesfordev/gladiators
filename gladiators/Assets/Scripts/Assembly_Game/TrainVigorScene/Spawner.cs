using Cysharp.Threading.Tasks;
using Scoz.Func;
using System.Threading;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Spawner : MonoBehaviour {
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform centerTarget;
        [SerializeField] MinMaxF heightRange = new MinMaxF(5f, 10f);
        [SerializeField] float spawnRadius = 20f;
        [SerializeField] public MinMaxF VelocityRange = new MinMaxF(5f, 15f);
        [SerializeField] MinMaxF angleOffsetRange = new MinMaxF(0f, 30f);
        [SerializeField] MinMaxF selfRotation = new MinMaxF(30, 720f);
        [SerializeField] MinMaxF interval = new MinMaxF(1f, 4f);
        [SerializeField] bool DontShoot = true;

        bool shooting = false;
        CancellationTokenSource cts;

        public void Init() {
            cts = new CancellationTokenSource();
            shoot(cts.Token).Forget();
        }

        public void StartShoot() {
            shooting = true;
        }

        public void StopShoot() {
            shooting = false;
        }

        void OnDestroy() {
            if (cts != null && !cts.IsCancellationRequested) {
                cts.Cancel();
                cts.Dispose();
            }
        }

        /// <summary>
        /// 射出隨機投射物
        /// </summary>
        async UniTask shoot(CancellationToken _ct) {
            while (!_ct.IsCancellationRequested) {
                if (!DontShoot && shooting) {
                    // 設定位置
                    float angle = Random.Range(0f, Mathf.PI * 2f);
                    float spawnY = heightRange.GetRandInRange();
                    Vector3 spawnPos = new Vector3(
                        spawnRadius * Mathf.Cos(angle),
                        spawnY,
                        spawnRadius * Mathf.Sin(angle)
                    );

                    // 產生物件
                    GameObject obj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
                    obj.name = "Projectile";
                    Rigidbody rigid = obj.GetComponent<Rigidbody>();

                    // 隨機旋轉
                    float randomX = selfRotation.GetRandInRange();
                    float randomY = selfRotation.GetRandInRange();
                    float randomZ = selfRotation.GetRandInRange();
                    Vector3 randomAngularVelocity = new Vector3(randomX, randomY, randomZ);
                    rigid.AddRelativeTorque(randomAngularVelocity);

                    // 拋向中心的向量
                    Vector3 dirToCenter = centerTarget.position - spawnPos;
                    dirToCenter.Normalize();
                    float offsetAngle = angleOffsetRange.GetRandInRange();
                    float sign = Random.value > 0.5f ? 1f : -1f;
                    float finalAngle = offsetAngle * sign;
                    Vector3 finalDirection = Quaternion.AngleAxis(finalAngle, Vector3.up) * dirToCenter;
                    float speed = VelocityRange.GetRandInRange();
                    rigid.velocity = finalDirection * speed;

                    await UniTask.WaitForSeconds(interval.GetRandInRange(), cancellationToken: _ct);
                } else {
                    await UniTask.Yield(PlayerLoopTiming.Update, _ct);
                }
            }
        }
    }
}

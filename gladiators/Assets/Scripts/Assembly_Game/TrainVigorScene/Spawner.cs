using Cysharp.Threading.Tasks;
using Scoz.Func;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Spawner : MonoBehaviour {
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] Transform centerTarget;
        [SerializeField] MinMaxF heightRange = new MinMaxF(5f, 10f);
        [SerializeField] float spawnRadius = 20f;
        [SerializeField] MinMaxF velocityRange = new MinMaxF(5f, 15f);
        [SerializeField] MinMaxF angleOffsetRange = new MinMaxF(0f, 30f);
        [SerializeField] MinMaxF selfRotation = new MinMaxF(30, 720f);
        [SerializeField] MinMaxF interval = new MinMaxF(1f, 4f);

        bool shooting = false;

        public void StartShoot() {
            shooting = true;
            shoot().Forget();
        }
        public void StopShoot() {
            shooting = false;
        }

        /// <summary>
        /// 射出隨機投射物
        /// </summary>
        async UniTask shoot() {
            while (shooting) {
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
                // 設定射出角度偏差
                float offsetAngle = angleOffsetRange.GetRandInRange();
                // 決定順/逆時針隨機( ±offsetAngle )
                float sign = Random.value > 0.5f ? 1f : -1f;
                float finalAngle = offsetAngle * sign;

                // 以 Y 軸為例，對 dir 向量進行旋轉
                Vector3 finalDirection = Quaternion.AngleAxis(finalAngle, Vector3.up) * dirToCenter;
                float speed = velocityRange.GetRandInRange();
                rigid.velocity = finalDirection * speed;

                await UniTask.WaitForSeconds(interval.GetRandInRange());
            }
        }
    }
}
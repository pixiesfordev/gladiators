using UnityEngine;
// 若你使用 UniTask，需要先安裝並引用 Cysharp.Threading.Tasks
using Cysharp.Threading.Tasks;
using Scoz.Func;

namespace Gladiators.Hunt {
    public class Spawner : MonoBehaviour {
        [SerializeField] GameObject projectilePrefab;  // 子彈預置物
        [SerializeField] Transform Trans_Target;       // 目標 (玩家) Transform
        [SerializeField] float spawnRadius = 8f;       // 生成子彈時，距離目標的半徑
        [SerializeField] MinMaxF spawnInterval;        // 幾秒生成一次子彈 (隨機範圍)
        [SerializeField] float speed = 10f;            // 子彈速度

        bool shooting = false;

        private void Start() {
            StartShoot();
        }

        public void StartShoot() {
            shooting = true;
            shoot().Forget();
        }

        public void StopShoot() {
            shooting = false;
        }

        async UniTaskVoid shoot() {
            while (shooting) {
                spawnProjectile();
                float waitTime = spawnInterval.GetRandInRange();
                await UniTask.WaitForSeconds(waitTime);
            }
        }

        /// <summary>
        /// 生成一顆子彈並射向目標 (2D 用)
        /// </summary>
        void spawnProjectile() {
            float angle = Random.Range(0f, Mathf.PI);

            Vector2 spawnPos2D = (Vector2)Trans_Target.position + new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * spawnRadius;
            Vector3 spawnPos3D = new Vector3(spawnPos2D.x, spawnPos2D.y, 0f);
            GameObject bullet = Instantiate(projectilePrefab, spawnPos3D, Quaternion.identity);

            Vector2 dir = (Vector2)Trans_Target.position - spawnPos2D;

            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
            if (rb2D != null) {
                rb2D.velocity = dir.normalized * speed;
            }
        }
    }
}

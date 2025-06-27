using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scoz.Func;
using UnityEngine;

namespace Gladiators.TrainCave {
    public class AttackObjSpawner : MonoBehaviour {
        [SerializeField] AttackObj projectilePrefab;  // 子彈預置物
        [SerializeField] AttackMagicObj magicProjectile;
        [SerializeField] AttackPhysicsObj physicsProjectile;
        
        [SerializeField] Transform Trans_Target;       // 目標 (玩家) Transform
        [SerializeField] MinMaxF spawnInterval;        // 幾秒生成一次子彈 (隨機範圍)
        [SerializeField] float speed = 10f;            // 子彈速度

        [Tooltip("只產生物理攻擊")][SerializeField] bool OnlyPhy;
        [Tooltip("只產生魔法攻擊")][SerializeField] bool OnlyMag;

        bool shooting = false;

        private void Start() {
            //StartShoot();
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

        //TODO:修改產生邏輯 魔法類攻擊要特別處理 要把產生邏輯直接寫在繼承AttackObj的Class底下的物件

        /// <summary>
        /// 生成一顆子彈並射向目標 (2D 用)
        /// </summary>
        void spawnProjectile() {
            float angle = Random.Range(0f, Mathf.PI);

            //挑選子彈種類 之後看遊戲有幾種子彈值就取多少
            int rand = Random.Range(0, 2);
            //int rand = 2;
            if (OnlyPhy) rand = 1;
            else if (OnlyMag) rand = 2;
            
            float spawnRadius;
            switch (rand)
            {
                case 1:
                    spawnRadius = physicsProjectile.SpawnRadius;
                    break;
                default:
                    spawnRadius = magicProjectile.SpawnRadius;
                    break;
            }

            //決定子彈的起始位置
            Vector2 spawnPos2D = (Vector2)Trans_Target.position + new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * spawnRadius;
            Vector3 spawnPos3D = (Vector3)spawnPos2D;

            //產生子彈物件
            AttackObj bullet;
            switch (rand)
            {
                case 1:
                    bullet = Instantiate(physicsProjectile, spawnPos3D, Quaternion.identity, TrainCaveUI.Instance.AttackObjTrans);
                    break;
                default:
                    bullet = Instantiate(magicProjectile, spawnPos3D, Quaternion.identity, TrainCaveUI.Instance.AttackObjTrans);
                    break;
            }
            //AttackObj bullet = Instantiate(projectilePrefab, spawnPos3D, Quaternion.identity, TrainCaveUI.Instance.AttackObjTrans);

            bullet.Init();

            Vector2 dir = (Vector2)Trans_Target.position - spawnPos2D;

            //算出子彈物件的角度
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            //給予子彈加速度
            bullet.SetSpeed(dir.normalized * speed);
        }
    }
}


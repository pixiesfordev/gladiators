using DamageNumbersPro;
using dnlib.DotNet.MD;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.TrainVigor {
    public class Chara_TrainVigor : MonoBehaviour {
        [SerializeField] float spd;
        [SerializeField][Range(0f, 1f)] float MouseControllStrength = 0.3f; // 滑鼠控制強度
        [SerializeField] Transform sphereCenter;         // ← 新增：球心
        [SerializeField][Range(0f, 1f)] float cancelSlideStrength = 1f;  // 滑落抵消強度

        [SerializeField] float dmgProjectileSpd;
        [SerializeField] MinMaxF dmgRange;
        [SerializeField] DamageNumber dmgPrefab;
        [SerializeField] Vector3 dmgPopupOffset;
        [SerializeField] float dmgNumScal;

        Vector3 defaultCharPos;
        Rigidbody rigid;
        MinMaxF projectileSpdRange;

        public void Init(MinMaxF _projectileSpdRange) {
            rigid = GetComponent<Rigidbody>();
            projectileSpdRange = _projectileSpdRange;
            defaultCharPos = transform.position;
        }

        public void ResetChar() {
            transform.position = defaultCharPos;
            rigid.velocity = Vector3.zero;
        }

        public void Move(Vector2 _dir) {
            // 原本的力
            Vector3 force = new Vector3(_dir.x, 0, _dir.y) * spd;
            if (!TrainVigorManager.Instance.MobileControl) force *= MouseControllStrength;
            rigid.AddForce(force);

            // 1. 計算當前球面法線
            Vector3 normal = (transform.position - sphereCenter.position).normalized;
            // 2. 計算「滑落」分量（重力在切線上的部分）
            Vector3 slideG = Vector3.ProjectOnPlane(Physics.gravity, normal);
            // 3. 如果玩家往上坡（force 方向和滑落方向夾角 > 90°），就抵消滑落
            if (Vector3.Dot(force.normalized, slideG.normalized) < 0f) {
                rigid.AddForce(-slideG * cancelSlideStrength, ForceMode.Acceleration);
            }
        }

        public void SetKinematic(bool _isKinematic) {
            rigid.isKinematic = _isKinematic;
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.name != "Projectile") return;
            var prb = collision.gameObject.GetComponent<Rigidbody>();
            if (prb == null) return;
            var velocity = prb.velocity.magnitude;
            if (velocity < dmgProjectileSpd) return;
            hit(velocity);
        }

        void hit(float _velocity) {
            var dmg = Mathf.RoundToInt(dmgRange.X + dmgRange.GetSub() * (_velocity / projectileSpdRange.GetSum()));
            var dmgNum = dmgPrefab.Spawn(transform.position + dmgPopupOffset, dmg);
            dmgNum.transform.localScale = Vector3.one * dmgNumScal;
            TrainVigorSceneUI.Instance.AddHP(-dmg);
        }

        public void AddForceToChar(Vector3 _force) {
            rigid.AddForce(_force);
        }
    }
}

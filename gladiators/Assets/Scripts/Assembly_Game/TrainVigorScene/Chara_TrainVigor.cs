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
        [SerializeField] float dmgProjectileSpd; // 速度高於多少的障礙物才會對角色造成傷害
        [SerializeField] MinMaxF dmgRange; // 傷害最大最小值
        [SerializeField] DamageNumber dmgPrefab;
        [SerializeField] Vector3 dmgPopupOffset; // 跳血座標偏移
        [SerializeField] float dmgNumScal; // 跳血縮放

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
            var force = new Vector3(_dir.x, 0, _dir.y) * spd;
            rigid.AddForce(force);
        }
        public void SetKinematic(bool _isKinematic) {
            rigid.isKinematic = _isKinematic;
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.name != "Projectile") return;
            var rigid = collision.gameObject.GetComponent<Rigidbody>();
            if (rigid == null) return;
            var velocity = rigid.velocity.magnitude;
            if (velocity < dmgProjectileSpd) return;
            hit(velocity);
        }
        void hit(float _velocity) {
            var dmg = Mathf.RoundToInt(dmgRange.X + dmgRange.GetSub() * (_velocity / projectileSpdRange.GetSum()));
            var dmgNum = dmgPrefab.Spawn(transform.position + dmgPopupOffset, dmg);
            dmgNum.transform.localScale = Vector3.one * dmgNumScal;
            TrainVigorSceneUI.Instance.AddHP(-dmg);
        }
    }

}
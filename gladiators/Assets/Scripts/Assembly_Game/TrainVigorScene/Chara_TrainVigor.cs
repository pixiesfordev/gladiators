using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gladiators.TrainVigor {
    public class Chara_TrainVigor : MonoBehaviour {
        [SerializeField] float Spd;

        Rigidbody rigid;

        public void Init() {
            rigid = GetComponent<Rigidbody>();
        }
        public void Move(Vector2 _dir) {
            var force = new Vector3(_dir.x, 0, _dir.y) * Spd;
            rigid.AddForce(force);
        }
        public void SetKinematic(bool _isKinematic) {
            rigid.isKinematic = _isKinematic;
        }
    }

}
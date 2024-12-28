using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.TrainCave {
    public class AttackObj : MonoBehaviour
    {
        [SerializeField] SpriteRenderer renderer;

        public TrainCaveShield.ShieldType DefednType {get; private set;} = TrainCaveShield.ShieldType.NONE;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Init(TrainCaveShield.ShieldType type) {
            DefednType = type;
            if (DefednType == TrainCaveShield.ShieldType.Physics)
                renderer.color = Color.red;
            else if (DefednType == TrainCaveShield.ShieldType.Magic)
                renderer.color = Color.blue;
        }

        void OnTriggerEnter2D(Collider2D coll) {
            Debug.Log ("-------开始碰撞------------");
            Debug.Log(coll.gameObject.name);
            var shield = coll.gameObject.GetComponent<TrainCaveShield>();
            if (shield != null) {
                if (shield.DefendType == DefednType) {
                    if (DefednType == TrainCaveShield.ShieldType.Magic)
                        TrainCaveUI.Instance.AddMagicScore();
                    else if (DefednType == TrainCaveShield.ShieldType.Physics)
                        TrainCaveUI.Instance.AddPhysicsScore();
                } else {
                    TrainCaveUI.Instance.PlayerHitted(this);    
                }
            } else {
                TrainCaveUI.Instance.PlayerHitted(this);
            }
            Destroy(gameObject);
        }

        void OnTriggerStay2D(Collider2D coll) {
            //Debug.Log ("------正在碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        void OnTriggerExit2D(Collider2D coll) {
            //Debug.Log ("------结束碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Gladiators.Main;
using Scoz.Func;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gladiators.TrainCave {
    public class AttackObj : MonoBehaviour {
        [SerializeField] Image AttackImg;
        [SerializeField] BoxCollider2D Collider2D;

        public TrainCaveShield.ShieldType DefednType { get; private set; } = TrainCaveShield.ShieldType.NONE;

        Vector2 ColiderOffset = new Vector2(10f, 10f);

        /*
        v1.改圖案 SpriteRenderer改成Image
        2.改攻擊演出方式 >> 圖案快速戳過去 戳到任何物體後就退回去
        */

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void Init(TrainCaveShield.ShieldType type) {
            DefednType = type;
            string imgSourceName = "";
            AttackImg.gameObject.SetActive(true);
            if (DefednType == TrainCaveShield.ShieldType.Physics) {
                imgSourceName = "attack01";
                AttackImg.transform.localScale = Vector3.one + Vector3.left + Vector3.left;
            }
            else if (DefednType == TrainCaveShield.ShieldType.Magic) {
                imgSourceName = "attack";
                AttackImg.transform.localScale = Vector3.one;
            }

            if (!string.IsNullOrEmpty(imgSourceName)) {
                AttackImg.gameObject.SetActive(true);
                AssetGet.GetSpriteFromAtlas("TrainCaveUI", imgSourceName, (sprite) => {
                    if (sprite != null)
                        AttackImg.sprite = sprite;
                    else {
                        AssetGet.GetSpriteFromAtlas("TrainCaveUI", "attack", (sprite) => {
                            AttackImg.sprite = sprite;
                            WriteLog.LogErrorFormat("怪物攻擊圖像不存在 使用替用圖案.攻擊類別:{0}", DefednType);
                        });
                    }
                });
                AttackImg.SetNativeSize();
                Collider2D.size = AttackImg.rectTransform.sizeDelta - ColiderOffset;
            } else {
                AttackImg.gameObject.SetActive(false);
                WriteLog.LogErrorFormat("怪物類別不存在. 攻擊類別:{0}", DefednType);
            }
        }

        void OnTriggerEnter2D(Collider2D coll) {
            //Debug.Log ("-------开始碰撞------------");
            //Debug.Log(coll.gameObject.name);
            var shield = coll.gameObject.GetComponent<TrainCaveShield>();
            if (shield != null) {
                if (shield.DefendType == DefednType) {
                    if (DefednType == TrainCaveShield.ShieldType.Magic)
                        TrainCaveManager.Instance.AddMagicScore();
                    else if (DefednType == TrainCaveShield.ShieldType.Physics)
                        TrainCaveManager.Instance.AddPhysicsScore();
                } else {
                    TrainCaveManager.Instance.PlayerHitted(this);
                }
            } else {
                TrainCaveManager.Instance.PlayerHitted(this);
            }
            Destroy(gameObject);
            /*測試用 物件碰撞後停止其速度
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.velocity = Vector2.zero;
            */
        }

        void OnTriggerStay2D(Collider2D coll) {
            //Debug.Log ("------正在碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        void OnTriggerExit2D(Collider2D coll) {
            //Debug.Log ("------结束碰撞-------------");
            //Debug.Log(coll.gameObject.name);
        }

        public void SetSpeed(Vector2 speed) {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null) {
                rb2D.velocity = speed;
            }
        }
    }
}

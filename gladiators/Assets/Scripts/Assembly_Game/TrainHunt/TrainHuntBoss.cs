using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHuntBoss : MonoBehaviour
{
    [SerializeField] Animator aniConroller;
    [SerializeField] SpineAnimationController HittedSpine;
    [SerializeField] TrainHuntBossHP BossHP;

    // Start is called before the first frame update
    void Start()
    {
        HittedSpine.Init();
    }

    public void Hitted(Vector3 spinePos, string spineAniName)
    {
        //Debug.LogErrorFormat("打擊Spine名稱: {0}", spineAniName);
        //播放Boss被打中的動畫
        aniConroller.Play("boss repel", -1, 0f);
        //配合隨機打中位置移動被擊中Spine
        HittedSpine.transform.localPosition = spinePos;
        HittedSpine.gameObject.SetActive(true);
        //播放被打中的Spine
        HittedSpine.PlayAnimation(spineAniName, false);
    }

    public void HittedOver()
    {
        HittedSpine.PlayAnimation("harm04_off", false);
        HittedSpine.gameObject.SetActive(false);
    }

    public void Move()
    {
        aniConroller.Play("boss move", -1, 0f);
    }

    public void ResetHP()
    {
        BossHP.Reset();
    }

    public void InitHP(int maxHP, int curHP)
    {
        BossHP.InitHP(maxHP, curHP);
    }

    public void ReduceHP(int val)
    {
        BossHP.ReduceHP(val);
    }

    public void SetHPAngle(Vector3 angle)
    {
        BossHP.transform.localRotation = Quaternion.Euler(-angle);
    }

}

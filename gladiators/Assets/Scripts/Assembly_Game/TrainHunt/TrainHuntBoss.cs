using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHuntBoss : MonoBehaviour
{
    [SerializeField] Animator aniConroller;
    [SerializeField] SpineAnimationController HittedSpine;

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
        //TODO:等美術做出空白狀態 不然現在直接放會當掉
        //HittedSpine.StopAnimation();
        HittedSpine.gameObject.SetActive(false);
    }

    public void Move()
    {
        aniConroller.Play("boss move", -1, 0f);
    }

}

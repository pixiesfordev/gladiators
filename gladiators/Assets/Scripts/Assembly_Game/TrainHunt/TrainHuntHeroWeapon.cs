using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrainHuntHeroWeapon : MonoBehaviour
{
    [SerializeField] SpineAnimationController Controller;
    [SerializeField] RectTransform RandomOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(string objName)
    {
        transform.localPosition = Vector3.zero;
        name = objName;
        Controller.Init();
    }

    public void Move(string prefix, Vector3 offset, float duration)
    {
        string animName = prefix + "_rotate";
        Controller.PlayAnimation(animName, true);
        RandomOffset.DOLocalMove(offset, duration);
    }

    public void Hit(string prefix)
    {
        string animName = prefix + "_hit";
        Controller.PlayAnimation(animName, false);
    }

    public void Poss(string prefix)
    {
        string animName = prefix + "_poss";
        Controller.PlayAnimation(animName, false);
    }

    public void Hide()
    {
        Controller.StopAnimation();
    }

}

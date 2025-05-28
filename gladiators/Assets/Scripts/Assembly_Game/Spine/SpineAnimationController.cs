using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineAnimationController : MonoBehaviour
{

    [SpineAnimation] [SerializeField] string animationName;

    SkeletonGraphic skeletonAnimation;
    [SerializeField] Spine.AnimationState spineAnimationState;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonGraphic>();
        spineAnimationState = skeletonAnimation.AnimationState;
        //Debug.LogError("初始化Spine");
    }

    public void PlayAnimation(string animName, bool loop)
    {
        spineAnimationState.SetAnimation(0, animName, loop);
    }

    public void StopAnimation()
    {
        spineAnimationState.SetEmptyAnimation(0, 0.5f);
    }

}

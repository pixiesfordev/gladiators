using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineAnimationController : MonoBehaviour
{

    [SpineAnimation][SerializeField] string animationName;

    SkeletonGraphic skeletonAnimation;
    [SerializeField] Spine.AnimationState spineAnimationState;

    void Start()
    {

    }

    public void Init()
    {
        skeletonAnimation = GetComponent<SkeletonGraphic>();
        spineAnimationState = skeletonAnimation.AnimationState;
    }

    public void PlayAnimation(string animName, bool loop)
    {
        //Debug.LogErrorFormat("嘗試撥放Spine動畫: {0} 是否重複撥放: {1}", animName, loop);
        spineAnimationState.SetAnimation(0, animName, loop);
    }

    public void StopAnimation()
    {
        spineAnimationState.SetEmptyAnimation(0, 0.5f);
    }

    public void SetTimeScale(float timeScale)
    {
        skeletonAnimation.timeScale = timeScale;
    }

}

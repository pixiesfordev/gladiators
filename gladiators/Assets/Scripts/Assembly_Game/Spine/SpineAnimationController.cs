using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineAnimationController : MonoBehaviour
{

    [SpineAnimation][SerializeField] string animationName;

    SkeletonGraphic _skeletonGraphic;
    SkeletonAnimation _skeletonAnimation;
    [SerializeField] Spine.AnimationState spineAnimationState;
    [SerializeField] bool Is3D = false;

    void Start()
    {

    }

    public void Init()
    {
        if (!Is3D)
        {
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
            spineAnimationState = _skeletonGraphic.AnimationState;
        }
        else
        {
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = _skeletonAnimation.AnimationState;
        }  
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
        _skeletonGraphic.timeScale = timeScale;
    }

}

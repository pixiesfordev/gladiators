using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class TrainHuntHero : MonoBehaviour
{
    [Tooltip("英雄最大偏移角度(即往前跑的最遠距離)")][SerializeField] float MaxOffsetAngle;
    [Tooltip("移動所需最久時間")][SerializeField] float MaxMoveTime;
    [Tooltip("移動所需最少時間")][SerializeField] float MinMoveTime;
    [Tooltip("英雄追逐移動曲線")][SerializeField] AnimationCurve HeroMoveCurve;

    float moveDuration = 0f;
    bool moveForward = true;

    // Start is called before the first frame update
    void Start()
    {
        PickMoveTime();
        HeroMoveAni().Forget();
    }

    public void SetHero(string id)
    {
        //TODO:根據傳入ID設定英雄圖像
    }

    async UniTask HeroMoveAni()
    {
        float passTime = 0f;
        Vector3 curAngle = Vector3.zero;
        while (gameObject.activeInHierarchy)
        {
            passTime = moveForward ? passTime + Time.deltaTime : passTime - Time.deltaTime;
            curAngle.z = HeroMoveCurve.Evaluate(passTime / moveDuration) * MaxOffsetAngle;
            transform.parent.localRotation = Quaternion.Euler(curAngle);
            await UniTask.Yield();
            if (passTime >= moveDuration)
            {
                moveForward = false;
                PickMoveTime();
            }
            else if (passTime <= 0f)
            {
                moveForward = true;
                PickMoveTime();
            }
        }
    }

    void PickMoveTime()
    {
        moveDuration = UnityEngine.Random.Range(MinMoveTime, MaxMoveTime);
    }

}

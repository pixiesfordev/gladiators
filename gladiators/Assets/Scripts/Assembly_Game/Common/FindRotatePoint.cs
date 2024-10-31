using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 工具 用來找基於某點從A點旋轉到B點的向量位置
/// </summary>
public class FindRotatePoint : MonoBehaviour
{
    [SerializeField] Vector3 CenterPos; //旋轉中心點
    [SerializeField] Vector3 OriginPos; //A點
    [SerializeField] float RotationZDegree; //旋轉角度 目前只有Z軸
    [SerializeField] bool Compete; //計算 輸入上面三個數值後點計算即可得出答案

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Compete) {
            Compete = false;
            PrintResult();
        }
    }

    void PrintResult()
    {
        Vector3 result = Vector3.zero;
        float sinVal = Mathf.Sin(RotationZDegree * Mathf.Deg2Rad);
        float cosVal = Mathf.Cos(RotationZDegree * Mathf.Deg2Rad);
        Debug.LogFormat("Sin: {0} cos: {1}", sinVal, cosVal);
        result.x = (OriginPos.x - CenterPos.x) * cosVal - (OriginPos.y - CenterPos.y) * sinVal + CenterPos.x;
        result.y = (OriginPos.x - CenterPos.x) * sinVal + (OriginPos.y - CenterPos.y) * cosVal + CenterPos.y;
        Debug.LogFormat("中心點: {0} 起始點: {1} 旋轉角度: {2} 目標點: {3}", CenterPos, OriginPos, RotationZDegree, result);
    }
}

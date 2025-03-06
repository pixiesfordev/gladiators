using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrainTimeObj : MonoBehaviour
{
    [SerializeField] RectTransform Pointer;

    Vector2 StartPointerPos;
    Vector2 EndPointerPos;

    Vector3 curPointerPos;

    void Start() {
        curPointerPos = Pointer.localPosition;
        StartPointerPos = new Vector2(-155f, 16f);
        EndPointerPos = new Vector2(155f, 16f);
    }

    /// <summary>
    /// 設定時間指針位置
    /// </summary>
    /// <param name="remainSec">傳入目前遊戲剩餘秒數</param>
    public void SetPointerPos(float remainSec) {
        curPointerPos.x = Mathf.Lerp(StartPointerPos.x, EndPointerPos.x, remainSec);
        Pointer.localPosition = curPointerPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHuntTimeObj : MonoBehaviour
{
    [SerializeField] RectTransform Pointer;

    Vector2 StartPointerPos;
    Vector2 EndPointerPos;

    Vector3 curPointerPos;

    // Start is called before the first frame update
    void Start() {
        curPointerPos = new Vector3(Pointer.sizeDelta.x, Pointer.sizeDelta.y, 0f);
        StartPointerPos = new Vector2(-155f, 16f);
        EndPointerPos = new Vector2(155f, 16f);
    }

    public void SetPointerPos(float posRate) {
        curPointerPos.x = Mathf.Lerp(StartPointerPos.x, EndPointerPos.x, posRate);
        Pointer.localPosition = curPointerPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineCardSelectEffect : MonoBehaviour
{
    [SerializeField] Animator Controller;

    void Start() {
        //把off最終狀態設定為預設
        Controller.Play("card_OFF", -1, 1f);
    }

    public void PlayCardMaskOn() {
        Controller.Play("card_ON", -1, 0f);
    }

    public void PlayCardMaskOff() {
        Controller.Play("card_OFF", -1, 0f);
    }

    public void PlayCardMaskCycle() {
        //由PlayCardMaskOn播放完畢後的Event自動呼叫
        Controller.Play("card_cycle", -1, 0f);
    }

    public void PlayCardMaskDecide() {
        Controller.Play("card_Decide", -1, 0f);
    }
}

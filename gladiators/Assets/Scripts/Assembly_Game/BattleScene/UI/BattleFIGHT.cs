using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFIGHT : MonoBehaviour
{
    [SerializeField] Animator BattleFight;

    public void StartBattle() {
        gameObject.SetActive(true);
        BattleFight.Play("FIGHTAnimation", -1, 0f);
    }

    public void AfterStartBattleEvent() {
        //由BattleFIGHT的FIGHTAnimation呼叫
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHuntBoss : MonoBehaviour
{
    [SerializeField] Animator aniConroller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hitted() {
        aniConroller.Play("boss repel", -1, 0f);
    }

    public void Move() {
        aniConroller.Play("boss move", -1, 0f);
    }

}

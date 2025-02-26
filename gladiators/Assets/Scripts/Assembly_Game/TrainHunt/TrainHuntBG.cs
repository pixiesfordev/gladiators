using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using UnityEngine;

public class TrainHuntBG : MonoBehaviour
{
    [SerializeField] RectTransform SphreaFloor; //戰鬥場景地板
    [SerializeField] RectTransform SphreaTrees; //樹木
    [SerializeField] RectTransform SphreaPlants; //附屬於地板上的植物
    [SerializeField] RectTransform SphereSteleFar; //附屬於地板上遠處的石碑(角色後)
    [SerializeField] RectTransform SteleNear; //會遮住角色的石碑(所以不附屬於Sphrea)
    
    [SerializeField] RectTransform FarBG; //遠處背景
    [SerializeField] RectTransform FarBGImgRt; //遠處背景圖

    //TODO:添加對背景大樹林與主背景圖的移動控制
    [HeaderAttribute("==============參數區域(地板區塊)==============")]
    [Tooltip("每秒轉動角度數(以每秒60楨為基礎)")][SerializeField] float RotateDegreePerSec = 10f;
    [Tooltip("地板轉動速度倍率(基於RotateDegreePerSec)")][SerializeField] float FloorRotateRate = 1f;
    [Tooltip("地板上的植物(近)轉動速度倍率(基於RotateDegreePerSec)")][SerializeField] float PlantsRotateRate = 1f;
    [Tooltip("地板上的植物(遠)轉動速度倍率(基於RotateDegreePerSec)")][SerializeField] float TreesRotateRate = 0.5f;
    [Tooltip("地板上的石碑轉動速度倍率(基於RotateDegreePerSec)")][SerializeField] float FarSteleRotateRate = 0.3f;
    [Tooltip("角色前的近景石碑轉動速度倍率(基於RotateDegreePerSec)")][SerializeField] float NearSteleRotateRate = 0.1f;
    [HeaderAttribute("==============參數區域(橫條背景)==============")]
    [Tooltip("遠處背景每秒移動距離")][SerializeField] float FarBGMovePerSec = 1f;
    [Tooltip("更新遠處背景移動速度")][SerializeField] bool updateBGMoveSpeed = false;

    float baseRotateFrame = 60f;

    CancellationTokenSource RotateCTS; //用來控制中斷旋轉
    CancellationTokenSource MoveBGCTS; //用來控制中斷移動遠處背景

    // Start is called before the first frame update
    void Start() {
        SetBGFarMoveParameter();
        BGFarStartMove();
    }

    void Update() {
        if (updateBGMoveSpeed) {
            updateBGMoveSpeed = false;
            SetBGFarMoveParameter();
            BGFarStartMove();
        }
    }

    void SetBGFarMoveParameter() {
        var oldSize = FarBGImgRt.sizeDelta;
        //因為狩獵季節固定為30秒 所以圖片寬度以螢幕寬度為基礎加上30秒內要移動的距離 多加5秒保險 以免延遲導致破綻出現
        //先不調整背景大小
        //FarBGImgRt.sizeDelta = new Vector2(Screen.width + 35 * FarBGMovePerSec, oldSize.y);
        FarBGImgRt.localPosition = new Vector3(-35 * FarBGMovePerSec / 2, 0f, 0f);
    }

    public void BGFarStartMove() {
        FarBG.localPosition = Vector3.zero;
        MoveBGCTS?.Cancel();
        MoveBGCTS = new CancellationTokenSource();
        MoveFarBG().Forget();
    }

    async UniTaskVoid MoveFarBG() {
        float duration = 30f;
        float startTime = Time.time;
        float passTime = startTime;
        Vector3 endPos = new Vector3(duration * FarBGMovePerSec, 0f, 0f);
        while (passTime - startTime <= duration) {
            passTime += Time.deltaTime;
            FarBG.localPosition = Vector3.Lerp(Vector3.zero, endPos, (passTime - startTime) / duration);
            await UniTask.Yield(MoveBGCTS.Token);
        }
    }

    public void StartRotate() {
        StopRotate();
        RotateCTS = new CancellationTokenSource();
        Rotate().Forget();
    }

    void StopRotate() {
        RotateCTS?.Cancel();
        RotateCTS = null;
    }

    private void OnDisable() {
        StopRotate();
    }

    async UniTaskVoid Rotate() {
        float degreePerFrame = RotateDegreePerSec / baseRotateFrame;
        float perFrameSec = 1f / baseRotateFrame;
        Vector3 floorVec = new (0f, 0f, degreePerFrame * FloorRotateRate);
        Vector3 plantsVec = new (0f, 0f, degreePerFrame * PlantsRotateRate);
        Vector3 treeVec = new (0f, 0f, degreePerFrame * TreesRotateRate);
        Vector3 steleFarVec = new(0f, 0f, degreePerFrame * FarSteleRotateRate);
        Vector3 steleNearVec = new (0f, 0f, degreePerFrame * NearSteleRotateRate);
        while (true) {
            SphreaFloor.Rotate(-floorVec);
            SphreaPlants.Rotate(-plantsVec);
            SphreaTrees.Rotate(-treeVec);
            SphereSteleFar.Rotate(-steleFarVec);
            SteleNear.Rotate(-steleNearVec);
            await UniTask.WaitForSeconds(perFrameSec, cancellationToken:RotateCTS.Token);
        }
    }

}

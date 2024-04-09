using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using Codice.Utils;
using System.Threading;
using Unity.Mathematics;
using Unity.Entities.UniversalDelegates;
using DG.Tweening;

/// <summary>
/// 上方角鬥士資訊
/// </summary>
public class BattleGladiatorInfo : MonoBehaviour {
    
    [HeaderAttribute("==============UI==============")]
    [SerializeField] MyText HeroName;
    [SerializeField] Image HeroIcon;
    [SerializeField] Image HPChangeBar;
    [SerializeField] Image HPBar;
    //[SerializeField] DOTweenAnimation HeartBeatTween; //心跳Tween物件
    [SerializeField] Transform HeartBeatIconTrans; //心跳Icon物件
    [SerializeField] Transform BuffIconTrans;

    //TODO:buff管理集合
    //Dictionary<

    [HeaderAttribute("==============TEST==============")]
    [HeaderAttribute("=========受擊血量變動測試=========")]
    [Tooltip("測試血量扣減演出")][SerializeField] bool PerformHPReduce = false;
    [Tooltip("血量起始值 0~1 此值大於BarFinalVal表示扣血")][SerializeField] float BarStartVal = 1f;
    [Tooltip("血量剩餘值 0~1 此值大於BarStartVal表示回血")][SerializeField] float BarFinalVal = 0.7f;
    [Tooltip("血量變化停滯秒數 就是被打掉的或者恢復量的血條殘留時間")][SerializeField] float BarChangeSecDelay = 1f;
    [Tooltip("血量變化演出秒數 越短就越快")][SerializeField] float BarChangeSecNeed = 1f;
    [Tooltip("血量變化演出偵數 即每秒血條變化張數")][SerializeField] float BarChangeFrame = 60f;
    [HeaderAttribute("==============心臟跳動==============")]
    //[Tooltip("")][SerializeField];
    [Tooltip("更新心跳參數")][SerializeField] bool UpdateHeartBeatParameter = false;
    [Tooltip("滿血時每次心跳前等待所需秒數 即滿血時每幾秒跳一次(即最慢速)")][SerializeField] float HeartBeatDelayMax = 3f;
    [Tooltip("1%血量時每次心跳前等待所需秒數 即1%血量時每幾秒跳一次(即最快速)")][SerializeField] float HeartBeatDelayMin = 0.5f;
    [Tooltip("心跳放大倍數 1為原本比例沒放大 2就是放大一倍 建議1~2之間")][SerializeField] float HeartBeatTweenScale = 1.3f;
    [Tooltip("心跳所需時間 即從原本尺寸到放大再到變回來的所需時間 這個會自動隨著血量減少等比加快")][SerializeField] float HeartBeatDuration = 1f;
    [Tooltip("心跳需要反向 敵人頭像這個要打勾")][SerializeField] bool HeartBeatReverse = false;

    [HeaderAttribute("=========設定當前血量測試=========")]
    [Tooltip("更新當前血量")][SerializeField] bool UpdateCurrentHP;
    [Tooltip("設定當前HP剩餘量 最大為1(滿血) 最小為0(空血)")][SerializeField] float SettingCurrentHP = 1f;

    //TODO:
    //1.扣血/回血 血條(Bar)演出 血條總長度396 Right值越大長度越短 396就等於0%
      //1.心臟跳動效果(公式:100~1 >> Min~Max) >> 已完成(4/9)
      //2.受擊 >> 心跳變黑白 往左搖晃一下 之後變回原本顏色 血條要變全白(可能需要動Shader 如果不好做可能得請美術多裁一條全白血條素材)
      //3.原本血條也要變黑白跟著縮減 縮減後變化血條不用隱藏 而是保留最後一小段
    //2.buff圖案

    CancellationTokenSource CurrentCTS; //用來中斷目前的血條演出

    //血條演出區塊參數
    RectTransform ChangeBarRect; //血條rt參考
    float HPChangeBarWidth; //血條長度
    float HPChangeBarOffsetY; //血條高度偏移量
    float CurrentHPRate = 1f; //目前血量比率

    //心跳演出區塊參數
    Tweener HeartBeatScaleTween1;
    Tweener HeartBeatScaleTween2;

    private void Start()
    {
        ChangeBarRect = HPChangeBar.GetComponent<RectTransform>();
        HPChangeBarWidth = ChangeBarRect.rect.width;
        HPChangeBarOffsetY = ChangeBarRect.offsetMax.y;
        SetHeartBeatParameter();
    }

    void Update()
    {
        //測試用 做血條演出
        if (PerformHPReduce)
        {
            UniTask.Void(async () => {
                if (CurrentCTS != null)
                    CurrentCTS.Cancel();
                CurrentCTS = new CancellationTokenSource();
                HPChange(CurrentCTS).Forget();
                });
            PerformHPReduce = false;
        }
        //測試用 做心跳演出
        if (UpdateHeartBeatParameter)
        {
            SetHeartBeatParameter();
            UpdateHeartBeatParameter = false;
        }
        //測試用 直接設定當前HP
        if (UpdateCurrentHP)
        {
            SetCurrentHP();
            UpdateCurrentHP = false;
        }
    }

    public void SetData()
    {
        //TODO:
        //英雄頭像設定
        //英雄名字設定
        //血量設定
    }

    //血條演出 之後改傳入起始值跟終值作演出判斷
    async UniTaskVoid HPChange(CancellationTokenSource ctk)
    {
        if (BarStartVal == BarFinalVal)
            return;
        Debug.Log("------開始演出血量-------");
        //TODO:之後改為接入實際扣減的數值 需要換算血量比例
        bool isReduce = BarStartVal >= BarFinalVal;
        float changeStartVal = isReduce ? BarStartVal : BarFinalVal;
        float changeFinalVal = isReduce ? BarFinalVal : BarStartVal;
        float currentVal = changeStartVal;
        CurrentHPRate = changeFinalVal;
        HPBar.fillAmount = BarFinalVal;
        SetHPBarChangeLength(changeStartVal);
        //等待血條停滯時間
        await UniTask.WaitForSeconds(BarChangeSecDelay, cancellationToken:ctk.Token);
        //算出每禎變化值
        float delta = math.abs(BarStartVal - BarFinalVal) / BarChangeSecNeed / BarChangeFrame;
        float duration = 1f / BarChangeFrame;
        Debug.Log("------開始漸變------");
        Debug.Log("每秒變化量: " + delta + " 每次變化所需秒數: " + duration);
        //血量變化
        if (isReduce)
        {
            while(currentVal >= changeFinalVal) {
                await UniTask.WaitForSeconds(duration, cancellationToken:ctk.Token);
                currentVal -= delta;
                SetHPBarChangeLength(currentVal);
                Debug.Log("數值減少 目前百分比值: " + currentVal);
            }
        }
        else
        {
            while(currentVal <= changeFinalVal){
                await UniTask.WaitForSeconds(duration, cancellationToken:ctk.Token);
                currentVal += delta;
                SetHPBarChangeLength(currentVal);
                Debug.Log("數值增加 目前百分比值: " + currentVal);
            }
        }
        await UniTask.WaitForEndOfFrame(ctk.Token);
        //血量變化更改心跳速度
        SetHeartBeatRate();
        HideHPBarChange();
    }

    //設定血條變化量長度 作為血條變化演出 傳入值為0~1
    void SetHPBarChangeLength(float percent)
    {
        //100%為0 0%為血量條初始長度
        float val = 1f - percent;
        //TODO:之後改定量運算 盡量不要一直new Vector2(改成類似Vector.down * val這樣的方式 但之前測試算出來有問題 需要檢查一下原因)
        ChangeBarRect.offsetMax = new Vector2(-val * HPChangeBarWidth, HPChangeBarOffsetY);
    }

    //隱藏血條變化量
    void HideHPBarChange()
    {
        SetHPBarChangeLength(0f);
        Debug.Log("隱藏血條變化量!");
    }

    void SetHeartBeatParameter()
    {
        //不為null就停止並殺掉原本的Tween
        if (HeartBeatScaleTween1 != null)
        {
            HeartBeatScaleTween1.Pause();
            HeartBeatScaleTween1.Kill();
        }
        if (HeartBeatScaleTween2 != null)
        {
            HeartBeatScaleTween2.Pause();
            HeartBeatScaleTween2.Kill();
        }
        //重置Scale 不然多次調整後初始Scale可能會錯誤
        HeartBeatIconTrans.localScale = HeartBeatReverse ? Vector3.left + Vector3.up + Vector3.forward : Vector3.one;
        //新建立Tween
        HeartBeatScaleTween1 = HeartBeatIconTrans.DOScale(new Vector3(HeartBeatReverse ? -HeartBeatTweenScale : HeartBeatTweenScale, HeartBeatTweenScale, 1f), HeartBeatDuration/2);
        HeartBeatScaleTween1.SetAutoKill(false);
        HeartBeatScaleTween1.Pause();
        HeartBeatScaleTween1.SetDelay(HeartBeatDelayMax);
        //HeartBeatScaleTween1.SetLoops(0);
        HeartBeatScaleTween1.SetEase(Ease.OutSine);
        HeartBeatScaleTween1.OnComplete(HeartBeatBack);//OnComplete弄成循環

        HeartBeatScaleTween2 = HeartBeatIconTrans.DOScale(HeartBeatReverse ? Vector3.left + Vector3.up + Vector3.forward : Vector3.one, HeartBeatDuration/2);
        HeartBeatScaleTween2.SetAutoKill(false);
        HeartBeatScaleTween2.Pause();
        HeartBeatScaleTween2.SetEase(Ease.OutSine);
        HeartBeatScaleTween2.OnComplete(HeartBeat);//OnComplete弄成循環
        //建立完後根據血量決定演出的速度(TimeScale)
        SetHeartBeatRate();
        //開始演出
        HeartBeatScaleTween1.PlayForward();
    }

    //更新心跳率
    void SetHeartBeatRate()
    {
        //根據目前血量換算心跳速度應該多快
        float HeartBeatChangeRate = (HeartBeatDelayMax - HeartBeatDelayMin) / 100f; //每百分比加快的秒數
        float CurrentHPDelay = HeartBeatDelayMax - HeartBeatChangeRate * ((1f - CurrentHPRate) * 100f); //換算現在血量對應的delay時間
        if (HeartBeatScaleTween1 != null)
        {
            HeartBeatScaleTween1.timeScale = 1f / (CurrentHPDelay / HeartBeatDelayMax); //換算TimeScale應該加快多少
            Debug.Log("轉換心跳演出加速倍率: " + HeartBeatScaleTween1.timeScale);
        }
        if (HeartBeatScaleTween2 != null)
            HeartBeatScaleTween2.timeScale = 1f / (CurrentHPDelay / HeartBeatDelayMax);
    }

    public void HeartBeatBack()
    {
        HeartBeatScaleTween2.Restart();
        //Debug.Log("心跳跳回去");
    }

    public void HeartBeat()
    {
        HeartBeatScaleTween1.Restart();
        //Debug.Log("重新心跳");
    }

    void SetCurrentHP()
    {
        CurrentHPRate = SettingCurrentHP;
        HPBar.fillAmount = CurrentHPRate;
        SetHPBarChangeLength(CurrentHPRate);
        SetHeartBeatRate();
    }

}
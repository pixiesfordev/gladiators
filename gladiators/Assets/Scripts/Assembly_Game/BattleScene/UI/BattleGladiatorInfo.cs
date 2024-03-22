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

/// <summary>
/// 上方角鬥士資訊
/// </summary>
public class BattleGladiatorInfo : MonoBehaviour {
    
    [HeaderAttribute("==============UI==============")]
    [SerializeField] MyText HeroName;
    [SerializeField] Image HeroIcon;
    [SerializeField] Image HPChangeBar;
    [SerializeField] Image HPBar;
    [SerializeField] Transform BuffIconTrans;

    //TODO:buff管理集合
    //Dictionary<

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("測試血量扣減演出")][SerializeField] bool PerformHPReduce = false;
    [Tooltip("血量起始值 0~1 此值大於BarFinalVal表示扣血")][SerializeField] float BarStartVal = 1f;
    [Tooltip("血量剩餘值 0~1 此值大於BarStartVal表示回血")][SerializeField] float BarFinalVal = 0.7f;
    [Tooltip("血量變化停滯秒數 就是被打掉的或者恢復量的血條殘留時間")][SerializeField] float BarChangeSecDelay = 1f;
    [Tooltip("血量變化演出秒數 越短就越快")][SerializeField] float BarChangeSecNeed = 1f;
    [Tooltip("血量變化演出偵數 即每秒血條變化張數")][SerializeField] float BarChangeFrame = 60f;

    //TODO:
    //1.扣血/回血 血條(Bar)演出 血條總長度396 Right值越大長度越短 396就等於0% >> 3/22完成
    //2.buff圖案
    //3.一些基礎英雄資訊 前端先自己弄

    CancellationTokenSource currentCTS; //用來中斷目前的血條演出

    //血條演出初始化區塊參數
    RectTransform changeBarRect; //血條rt參考
    float HPChangeBarWidth; //血條長度
    float HPChangeBarOffsetY; //血條高度偏移量

    private void Start()
    {
        changeBarRect = HPChangeBar.GetComponent<RectTransform>();
        HPChangeBarWidth = changeBarRect.rect.width;
        HPChangeBarOffsetY = changeBarRect.offsetMax.y;
    }

    void Update()
    {
        //測試用 做血條演出
        if (PerformHPReduce)
        {
            UniTask.Void(async () => {
                if (currentCTS != null)
                    currentCTS.Cancel();
                currentCTS = new CancellationTokenSource();
                HPChange(currentCTS).Forget();
                });
            PerformHPReduce = false;
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
        HideHPBarChange();
    }

    //設定血條變化量長度 作為血條變化演出 傳入值為0~1
    void SetHPBarChangeLength(float percent)
    {
        //100%為0 0%為血量條初始長度
        float val = 1f - percent;
        changeBarRect.offsetMax = new Vector2(-val * HPChangeBarWidth, HPChangeBarOffsetY);
    }

    //隱藏血條變化量
    void HideHPBarChange()
    {
        SetHPBarChangeLength(0f);
        Debug.Log("隱藏血條變化量!");
    }

}
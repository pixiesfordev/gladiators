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
    [SerializeField] Image HPReduceBar;
    [SerializeField] Image HPBar;
    [SerializeField] Image HPRecoveryBar;
    [SerializeField] MyText HPVal;
    [SerializeField] Transform BuffIconTrans;

    //TODO:buff管理集合
    //Dictionary<

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("測試血量扣減演出")][SerializeField] bool PerformHPReduce = false;
    [Tooltip("血量起始值 0~1 此值大於BarFinalVal表示扣血")][SerializeField] float BarStartVal;
    [Tooltip("血量剩餘值 0~1 此值大於BarStartVal表示回血")][SerializeField] float BarFinalVal;
    [Tooltip("血量變化停滯秒數 就是被打掉的或者恢復量的血條殘留時間")][SerializeField] float BarChangeSecDelay;
    [Tooltip("血量變化演出秒數 越短就越快")][SerializeField] float BarChangeSecNeed;
    [Tooltip("多段攻擊次數 預設為1")][SerializeField] int HitCount = 1;
    //TODO:
    //1.扣血/回血 血條(Bar)演出
    //2.扣血/回血 血條數字(Val)演出
    //3.buff圖案
    //4.一些基礎英雄資訊 這個需要討論是後端訂好規格還是我們前端自己弄

    CancellationTokenSource cancelToken;

    private void Start()
    {
        cancelToken = new CancellationTokenSource();
    }

    void Update()
    {
        if (PerformHPReduce)
        {
            cancelToken.Cancel();
            HPChange().Forget();
            PerformHPReduce = false;
        }
    }

    public void SetData()
    {
        //TODO:
        //英雄頭像設定
        //英雄名字設定
        //血量捨定
    }

    async UniTaskVoid HPChange()
    {
        if (BarStartVal == BarFinalVal)
            return;
        //TODO:之後改為接入實際扣減的數值 需要換算血量比例
        bool isReduce = BarStartVal >= BarFinalVal;
        var changeBar = isReduce ? HPReduceBar : HPRecoveryBar;
        float targetVal = isReduce ? BarStartVal : BarFinalVal;
        HPBar.fillAmount = isReduce ? BarFinalVal : BarStartVal;
        HPReduceBar.fillAmount = isReduce ? BarStartVal : 0;
        HPRecoveryBar.fillAmount = isReduce ? 0 : BarStartVal;
        await UniTask.WaitForSeconds(BarChangeSecDelay, cancellationToken:cancelToken.Token);
        
        float changePerFrame = math.abs(BarStartVal - BarFinalVal) / BarChangeSecNeed / 40;
        if (isReduce)
        {
            //扣血
            //while (changeBar.fillAmount)
        }
        else
        {
            //加血

        }
        
    }

}
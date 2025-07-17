using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class TrainHPObj : MonoBehaviour
{
    [SerializeField] Image CurrentHP;
    [SerializeField] Image GrayHP;

    [HeaderAttribute("=========受擊血量變動參數=========")]
    [Tooltip("血量變化停滯秒數 就是被打掉的或者恢復量的血條殘留時間")][SerializeField] float BarChangeSecDelay = 1f;
    [Tooltip("血量變化演出秒數 越短就越快")][SerializeField] float BarChangeSecNeed = 1f;
    [Tooltip("血量變化演出禎數 即每秒血條變化張數")][SerializeField] float BarChangeFrame = 60f;
    [Tooltip("灰色血量變化停滯秒數 就是被打掉的血條殘留時間")][SerializeField] float HPGarySecDelay = 0.4f;
    [Tooltip("灰色血條演出秒數 越短就越快")][SerializeField] float HPGaryChangeSecNeed = 0.4f;
    [Tooltip("顯示瀕死血量比率值 即血量剩下百分之多少的時候會變成瀕死的血量顏色")][SerializeField] float ShowDeadlyHPColorRate = 0.25f;

    int MaxHP = 0; //最大血量
    int CurHP = 0; //目前血量
    float DisplayHPRate = 0f; //顯示血量百分比

    CancellationTokenSource CurrentCTS; //用來中斷目前的血條演出

    Color normalHPColor = new Color(218f / 255f, 242f / 255f, 41f / 225f); //普通血量顏色
    Color deadlyHPColor = new Color(242f / 255f, 41f / 225f, 41f / 225f); //瀕死血量顏色

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitHP(int maxHP, int curHP)
    {
        if (maxHP == 0)
        {
            maxHP = 1;
            Debug.LogWarning("Boss最大HP不可為0 設定錯誤!");
        }
        MaxHP = maxHP;
        CurHP = curHP;
        DisplayHPRate = curHP / maxHP;
        SetHPColor(false);
        Debug.LogFormat("初始化HP. 最大HP:{0} 目前HP:{1} 目前顯示百分比:{2}", maxHP, curHP, DisplayHPRate * 100);
    }

    public void Reset()
    {
        InitHP(MaxHP, MaxHP);
        CurrentHP.fillAmount = DisplayHPRate;
        GrayHP.fillAmount = DisplayHPRate;
    }

    void CreateCTK()
    {
        if (CurrentCTS != null)
            CurrentCTS.Cancel();
        CurrentCTS = new CancellationTokenSource();
    }

    public void ReduceHP(int val)
    {
        CreateCTK();
        CurHP -= val;
        HPChange().Forget();
    }

    async UniTaskVoid HPChange()
    {
        float BarStartVal = DisplayHPRate;
        float BarFinalVal = (float)CurHP / MaxHP;
        Debug.LogFormat("血量準備變化. 起始值:{0} 最終值:{1}", BarStartVal, BarFinalVal);
        if (BarStartVal == BarFinalVal)
            return;
        CurrentHP.fillAmount = BarStartVal;
        GrayHP.fillAmount = BarStartVal;

        //防呆 避免設定錯誤導致遊戲炸掉
        if (BarChangeSecNeed <= 0)
            BarChangeSecNeed = 1f;

        //等待血條停滯時間
        await UniTask.WaitForSeconds(BarChangeSecDelay, cancellationToken: CurrentCTS.Token);
        Debug.Log("------開始演出血量-------");
        float delta = math.abs(BarStartVal - BarFinalVal) / BarChangeSecNeed / BarChangeFrame;
        float duration = 1f / BarChangeFrame;
        float grayDelta = math.abs(BarStartVal - BarFinalVal) / HPGaryChangeSecNeed / BarChangeFrame; //灰色血條演出所需每次變化量
        //灰色血條另外開一個Task跑
        HPGrayChange(DisplayHPRate, BarFinalVal, duration, grayDelta).Forget();

        while (DisplayHPRate >= BarFinalVal)
        {
            await UniTask.WaitForSeconds(duration, cancellationToken: CurrentCTS.Token);
            DisplayHPRate -= delta;
            //目前血條(彩色)開始變化 & 變化血條位移
            CurrentHP.fillAmount = DisplayHPRate;
            //如果瀕死血條顏色變色
            if (DisplayHPRate <= ShowDeadlyHPColorRate)
                SetHPColor(true);
            //Debug.LogFormat("數值減少 目前百分比值: {0}.", HeroDisplayHPRate);
        }
        await UniTask.Yield();
        //目前血條設定至定量避免計算有偏差
        CurrentHP.fillAmount = BarFinalVal;
        DisplayHPRate = BarFinalVal;
    }

    /// <summary>
    /// 血條(灰色)變化演出
    /// </summary>
    /// <param name="currentVal">目前血量數值 為0~1</param>
    /// <param name="changeFinalVal">最終血量數值 為0~1</param>
    /// <param name="duration">每次變化所需時間</param>
    /// <param name="delta">每次血條變化量</param>
    /// <returns></returns>
    async UniTaskVoid HPGrayChange(float currentVal, float changeFinalVal, float duration, float delta)
    {
        //等待設置延遲的秒數
        await UniTask.WaitForSeconds(HPGarySecDelay, cancellationToken: CurrentCTS.Token);
        //以設置的演出時間開始變化血量
        while (currentVal >= changeFinalVal)
        {
            await UniTask.WaitForSeconds(duration, cancellationToken: CurrentCTS.Token);
            currentVal -= delta;
            //目前血條(黑白)開始變化
            GrayHP.fillAmount = currentVal;
            //Debug.Log("黑白數值減少 目前百分比值: " + currentVal);
        }
        await UniTask.Yield(CurrentCTS.Token);
        //目前血條(灰色)設定至定量避免計算有偏差
        GrayHP.fillAmount = changeFinalVal;
    }

    /// <summary>
    /// 設定血條顏色
    /// </summary>
    /// <param name="isDeadly">是否瀕死</param>
    void SetHPColor(bool isDeadly)
    {
        CurrentHP.color = isDeadly ? deadlyHPColor : normalHPColor;
    }

    public bool HeroISDead()
    {
        return CurHP <= 0;
    }
}

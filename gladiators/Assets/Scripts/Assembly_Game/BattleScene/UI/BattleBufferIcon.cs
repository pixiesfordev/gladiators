using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;
using Gladiators.Main;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BufferIconData {
    /// <summary>
    /// Buffer效果名稱
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Buffer數值(時間/層數)
    /// </summary>
    public int Val { get; set; }
    /// <summary>
    /// 數值類型
    /// </summary>
    public SkillExtension.BuffIconValType ValType { get; private set; }
    /// <summary>
    /// 是否需要更新
    /// </summary>
    public bool NeedUpdate { get; set; } = false;


    public BufferIconData(string effectType, int val, SkillExtension.BuffIconValType valType) {
        Name = effectType;
        Val = val;
        ValType = valType;
    }
}

/// <summary>
/// 上方角鬥士buff圖案
/// </summary>
public class BattleBufferIcon : MonoBehaviour {

    [SerializeField] Image Bg;
    [SerializeField] Image Icon;
    [SerializeField] Image Border;
    [SerializeField] MyTextPro Val;

    CancellationTokenSource ShineCTS;
    bool isShining = false;

    private void OnDisable() {
        if (ShineCTS != null)
            StopShine();
        }

    private void OnDestroy() {
        if (ShineCTS != null) {
            ShineCTS.Cancel();
            ShineCTS.Dispose();
        }
    }

    public void SetEffect(BufferIconData bufferData) {
        AssetGet.GetSpriteFromAtlas("BufferIcon", bufferData.Name, (sprite) => {
            gameObject.SetActive(true);
            if (sprite != null) {
                Icon.sprite = sprite;
            } else {
                AssetGet.GetSpriteFromAtlas("BufferIcon", "defaultBufferIcon", (sprite) => {
                    Icon.sprite = sprite;
                    WriteLog.LogWarningFormat("圖片缺少! 用替用圖代替顯示!");
                });
            }
        });

        
        //停止閃爍並還原色彩
        if (ShineCTS != null) {
            //判斷是否已經在閃爍 已經在閃爍就不重設
            if (bufferData.ValType == SkillExtension.BuffIconValType.Time && bufferData.Val <= 1) {
                //Debug.LogError("buffer需要閃爍!");
            } else {
                StopShine();
            }
        }

        switch (bufferData.ValType) {
            case SkillExtension.BuffIconValType.Passive:
                //被動 不顯示數值
                Val.text = "";
                break;
            case SkillExtension.BuffIconValType.Stack:
                //層數 顯示數值
                Val.text = bufferData.Val.ToString();
                break;
            case SkillExtension.BuffIconValType.Time:
                //時間 顯示數值(剩下一秒要閃爍)
                int leftTime = bufferData.Val;
                Val.text = leftTime.ToString();
                if (leftTime <= 1 && !isShining)
                    Shine().Forget();
                break;
            default:
                Val.text = "";
                break;    
        }
    }

    /// <summary>
    /// 閃爍演出
    /// </summary>
    /// <returns></returns>
    async UniTaskVoid Shine() {
        //Debug.LogError("buffer閃爍!");
        ShineCTS = new CancellationTokenSource();
        float duration = 0.25f;
        float passTime = 0f;
        float startAlpha = 1f;
        float endAlpha = 0f;
        Color tempColor = Color.white;
        isShining = true;
        while (true) {
            passTime += Time.deltaTime;
            tempColor.a = Mathf.Lerp(startAlpha, endAlpha, passTime / duration);
            Bg.color = tempColor;
            Icon.color = tempColor;
            Border.color = tempColor;
            Val.color = tempColor;
            await UniTask.Yield(ShineCTS.Token);
            if (tempColor.a == 1f) {
                startAlpha = 0f;
                endAlpha = 1f;
                passTime = 0f;
            } else if (tempColor.a == 0f) {
                startAlpha = 1f;
                endAlpha = 0f;
                passTime = 0f;
            }
        }
    }

    void StopShine() {
        ShineCTS.Cancel();
        isShining = false;
        Bg.color = Color.white;
        Icon.color = Color.white;
        Border.color = Color.white;
        Val.color = Color.white;
        //WriteLog.LogError("停止閃爍!");
    }
}
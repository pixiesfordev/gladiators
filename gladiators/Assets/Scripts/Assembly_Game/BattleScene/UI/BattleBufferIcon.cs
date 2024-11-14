using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;
using Gladiators.Main;
using Cysharp.Threading.Tasks;

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

    [SerializeField] Image Icon;
    [SerializeField] MyTextPro Val;

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
        Val.text = bufferData.Val > 0 ? bufferData.Val.ToString() : "";
    }
}
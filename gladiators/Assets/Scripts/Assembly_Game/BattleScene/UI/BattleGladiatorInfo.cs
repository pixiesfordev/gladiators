using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;

/// <summary>
/// 上方角鬥士資訊
/// </summary>
public class BattleGladiatorInfo : MonoBehaviour {
    
    [SerializeField] MyText HeroName;
    [SerializeField] Image HeroIcon;
    [SerializeField] Image HPReduceBar;
    [SerializeField] Image HPBar;
    [SerializeField] MyText HPVal;
    
    //TODO:
    //1.扣血/回血 血條(Bar)演出
    //2.扣血/回血 血條數字(Val)演出
    //3.英雄頭像設定(等有圖後實現)
    //4.英雄名字設定
}
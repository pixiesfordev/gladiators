using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineCandle : MonoBehaviour
{
    [SerializeField] Animator AniPlayer;

    /*
        蠟燭演出調用 >> 前兩種隨機選用 撥放完後呼叫方法去隨機挑選下一次演出的動畫 該方法也要能設定初始值 第一次起始值隨機 第二次後一律從頭放
        1.candle_combustion >> 第一種燃燒方式
        2.candle_combustion01 >> 第二種燃燒方式
        3.candle_go out >> 熄滅撥放此動畫
    */

    // Start is called before the first frame update
    void Start()
    {
        ResetCandle();
    }

    public void ResetCandle() {
        PlayCombustion(Random.Range(0, 2), Random.Range(0.0f, 1.0f));
    }

    public void NextCandleClipEvent() {
        //TODO:需要判斷重新回到神祉介面時會不會再播放 還有disable的時候就不要播放
        PlayCombustion(Random.Range(0, 2), 0f);
    }

    /// <summary>
    /// 播放燃燒動畫
    /// </summary>
    /// <param name="seed">種子</param>
    /// <param name="startTime">起始時間</param>
    void PlayCombustion(int seed, float startTime) {
        if (seed == 0)
            AniPlayer.Play("candle_combustion", -1, startTime);
        else
            AniPlayer.Play("candle_combustion01", -1, startTime);
    }

    /// <summary>
    /// 是否正在播放燃燒動畫
    /// </summary>
    /// <returns>True if is combusting, otherwise is false</returns>
    public bool IsCombusting() {
        return !AniPlayer.GetCurrentAnimatorStateInfo(0).IsName("candle_go out");
    }

    /// <summary>
    /// 熄滅蠟燭
    /// </summary>
    public void GoOutCandle() {
        AniPlayer.Play("candle_go out", -1, 0.0f);
    }
}

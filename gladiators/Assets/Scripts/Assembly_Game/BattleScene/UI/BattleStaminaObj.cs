using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scoz.Func;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戰鬥體力條元件
/// </summary>
public class BattleStaminaObj : MonoBehaviour {

    [SerializeField] Image Icon;
    [SerializeField] Text CurrentVal;
    [SerializeField] Text MaxVal;
    [SerializeField] MyTextPro[] SkillVigorVals;
    [SerializeField] MyTextPro SkillVigorValNext;
    [SerializeField] Image[] SkillMasks;
    [SerializeField] Image[] Bar_lattices;
    [SerializeField] Image[] Bar_lattices_pre;

    float CurrentMaxVal = 0f; //目前最大能量值
    Color ValOriginColor;
    Color MaskOriginColor;
    float MaskLightAlpha = 0f;
    float MaskDarkAlpha = 0.9f;

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("測試更新體力值")][SerializeField] bool TestLattices;
    [Tooltip("測試設定目前體力值 值為0~20")][SerializeField] float TestLatticeVal;
    [Tooltip("測試更新體力值")][SerializeField] bool TestInitLattices;
    [Tooltip("測試設定最大體力值 值為10~20")][SerializeField] float TestMaxLatticeVal;

    //TODO:
    //1.體力條消耗演出
    //2.體力條補充演出
    //3.技能消耗數值

    void Start() {
        ValOriginColor = SkillVigorVals[0].color;
        MaskOriginColor = SkillMasks[0].color;
    }

    void Update() {
        if (TestLattices) {
            TestLattices = !TestLattices;
            SetVigor(TestLatticeVal);
        }

        if (TestInitLattices) {
            TestInitLattices = !TestInitLattices;
            if (TestMaxLatticeVal < 10)
                TestMaxLatticeVal = 10;
            InitVigor(TestLatticeVal, TestMaxLatticeVal);
        }
    }

    /// <summary>
    /// 初始化體力值
    /// </summary>
    /// <param name="curVal">目前數值</param>
    /// <param name="maxVal">最大值</param>
    public void InitVigor(float curVal, float maxVal) {
        SetValText(curVal, CurrentVal);
        SetValText(maxVal, MaxVal);
        CurrentMaxVal = maxVal;
        BattleSceneUI.Instance.CheckVigor(curVal);
    }

    void SetValText(float val, Text obj) {
        float showVal = Mathf.Floor(val);
        if (obj != null)
            obj.text = showVal.ToString();
    }

    /// <summary>
    /// 同步體力數值
    /// </summary>
    /// <param name="val">數值</param>
    public void SetVigor(float val) {
        SetLattices(val);
        SetValText(val, CurrentVal);
        //BattleSceneUI.Instance.CheckVigor(val);
    }

    void SetLattices(float val) {
        //TODO:添加一組比較淡色的Lattices 修改這組的FillAmount 原本的那組則是滿則SetActive(true)並撥放動畫 不滿1就SetActive(false)
        //根據格子的物件數換算體力數值 對應演出格子數
        float realVal = GetRealVal(val);
        //設定格子顯示數量
        for (int i = 0; i < Bar_lattices.Length; i++)
            Bar_lattices[i].fillAmount = CountLatticeVal(realVal - i);
    }

    float GetRealVal(float val) {
        //利用交叉相乘計算 格子數量 * 目前體力值 = 最大體力值 * val >> 格子數量 * 目前體力值 / 最大體力值 = val
        return Bar_lattices.Length * val / CurrentMaxVal;
    }

    float CountLatticeVal(float val) {
        return val > 1 ? 1 : val < 0 ? 0 : val;
    }

    public void SetSkillVigorVal(int pos, int val) {
        if (CheckSkillPosVaild(pos))
            SkillVigorVals[pos].text = val.ToString();
        else
            SkillVigorValNext.text = val.ToString();
    }

    /// <summary>
    /// 消耗體力條
    /// </summary>
    /// <param name="vigor">消耗體力</param>
    public void ConsumeVigorBySkill(int vigor) {
        //TODO:取出最後幾個能量條打亮並消耗掉
        //需要做左右方Fill方向切換
    }

    bool CheckSkillPosVaild(int pos) {
        return pos < SkillVigorVals.Length && pos >= 0;
    }

    /// <summary>
    /// 消耗能量淡出與體力值遮罩淡入(數字隱藏與變暗)
    /// </summary>
    /// <param name="pos">技能位置</param>
    public void FadeOutSkillVigorVal(int pos) {
        if (CheckSkillPosVaild(pos)) {
            DoSkillVigorValFadeOut(SkillVigorVals[pos], SkillMasks[pos]).Forget();
        } else {
            WriteLog.LogErrorFormat("Fade out skill vigor val index error! Pos: {0}", pos);
        }
    }

    async UniTaskVoid DoSkillVigorValFadeOut(MyTextPro _text, Image _mask) {
        //體力消耗數字淡出 & 體力遮罩變暗(淡入)
        Color textColor = ValOriginColor;
        Color maskColor = MaskOriginColor;
        float duration = 0.14f;
        float passTime = 0f;
        float textAlpha;
        float maskAlpha;
        while (passTime < duration) {
            passTime += Time.deltaTime;

            textAlpha = Mathf.Lerp(ValOriginColor.a, 0f, passTime / duration);
            textColor.a = textAlpha;
            _text.color = textColor;

            maskAlpha = Mathf.Lerp(MaskLightAlpha, MaskDarkAlpha, passTime / duration);
            maskColor.a = maskAlpha;
            _mask.color = maskColor;
            
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 消耗能量數字淡入(數字出現)
    /// </summary>
    /// <param name="pos">技能位置</param>
    public void FadeInSkillVigorVal(int pos) {
        WriteLog.LogErrorFormat("Fade in skill vigor val. Pos: {0}", pos);
        if (CheckSkillPosVaild(pos)) {
            DoSkillVigorValFadeIn(SkillVigorVals[pos]).Forget();
        } else {
            WriteLog.LogErrorFormat("Fade in skill vigor val index error! Pos: {0}", pos);
        }
    }

    async UniTaskVoid DoSkillVigorValFadeIn(MyTextPro _text) {
        //體力消耗數字淡入
        Color textColor = ValOriginColor;
        float duration = 0.26f;
        float passTime = 0f;
        float textAlpha;
        while (passTime < duration) {
            passTime += Time.deltaTime;
            textAlpha = Mathf.Lerp(0f, ValOriginColor.a, passTime / duration);
            textColor.a = textAlpha;
            _text.color = textColor;
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 消耗體力值打亮
    /// </summary>
    /// <param name="pos">技能位置</param>
    public void BrigtenMask(int pos) {
        if (CheckSkillPosVaild(pos)) {
            DoBrigtenMask(SkillMasks[pos]).Forget();
        } else {
            WriteLog.LogErrorFormat("Brigten mask index error! Pos: {0}", pos);
        }
    }

    async UniTaskVoid DoBrigtenMask(Image _mask) {
        //體力遮罩變亮(淡出)
        Color maskColor = MaskOriginColor;
        float duration = 0.31f;
        float passTime = 0f;
        float maskAlpha;
        while (passTime < duration) {
            passTime += Time.deltaTime;

            maskAlpha = Mathf.Lerp(MaskDarkAlpha, MaskLightAlpha, passTime / duration);
            maskColor.a = maskAlpha;
            _mask.color = maskColor;
            
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 設定體力遮罩亮度
    /// </summary>
    /// <param name="pos">技能位置</param>
    /// <param name="bright">是否打亮</param>
    public void SetVigorMaskBrightness(int pos, bool bright) {
        if (CheckSkillPosVaild(pos)) {
            Color maskColor = MaskOriginColor;
            maskColor.a = bright ? MaskLightAlpha : MaskDarkAlpha;
            SkillMasks[pos].color = maskColor;
        } else {
            WriteLog.LogErrorFormat("Set mask color index error! Pos: {0}", pos);
        }
    }
}
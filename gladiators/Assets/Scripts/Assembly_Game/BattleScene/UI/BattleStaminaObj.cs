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
    [SerializeField] MyTextPro SkillVigorVal1;
    [SerializeField] MyTextPro SkillVigorVal2;
    [SerializeField] MyTextPro SkillVigorVal3;
    [SerializeField] MyTextPro SkillVigorValNext;
    [SerializeField] MyTextPro SkillVal;
    [SerializeField] Image[] Bar_lattices;

    float CurrentMaxVal = 0f; //目前最大能量值
    Color ValOriginColor; 

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
        ValOriginColor = SkillVigorVal1.color;
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
        BattleSceneUI.Instance?.CheckVigor(curVal);
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
        //BattleSceneUI.Instance?.CheckVigor(val);
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
        switch (pos) {
            case 1:
                SkillVigorVal1.text = val.ToString();
                break;
            case 2:
                SkillVigorVal2.text = val.ToString();
                break;
            case 3:
                SkillVigorVal3.text = val.ToString();
                break;
            default:
                SkillVigorValNext.text = val.ToString();
                break;
        }
    }

    public void ConsumeVigorBySkill(int vigor) {
        //TODO:取出最後幾個能量條打亮並消耗掉
    }

    public void FadeOutSkillVigorVal(int pos) {
        switch(pos) {
            case 1:
                DoSkillVigorValFadeOut(SkillVigorVal1);
                break;
            case 2:
                DoSkillVigorValFadeOut(SkillVigorVal2);
                break;
            case 3:
                DoSkillVigorValFadeOut(SkillVigorVal3);
                break;
            default:
                Debug.LogErrorFormat("Fade out skill vigor val index error!");
                break;
        }
    }

    void DoSkillVigorValFadeOut(MyTextPro _text) {
        //TODO:改用Lerp
        //體力消耗數字淡出
        //體力遮罩變暗
        Color endColor = ValOriginColor;
        endColor.a = 0f;
        //float passTime = 0f;
        //while (passTime <)
    }

    public void FadeInSkillVigorVal()
    {

    }
}
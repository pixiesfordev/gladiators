using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scoz.Func;
using Unity.Mathematics;
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
    [SerializeField] Image[] Bar_lattices; //體力條(fillAmount滿1)
    [SerializeField] Image[] Bar_lattices_pre; //恢復體力條(fillAmount未滿1)
    [SerializeField] Transform BarFxTrans;
    [SerializeField] Animator BarFxAni;
    [SerializeField] Animator EnergyAni;
    [SerializeField] Material BarLatticesConsumeMaterial;
    //[SerializeField] Texture2D[] BarLatticesTex;

    List<Material> BarLatticesCloneMaterialList;

    float RealLatticVal;
    float OldLatticVal;
    Quaternion zeroQuaternion;
    Quaternion halfQuaternion;
    Quaternion verticalQuaternion;

    float CurrentMaxVal = 0f; //目前最大能量值
    Color ValOriginColor;
    Color MaskOriginColor;
    readonly float MaskLightAlpha = 0f;
    readonly float MaskDarkAlpha = 0.9f;

    bool PerformConsumeVigor = false;

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("測試更新體力值")][SerializeField] bool TestLattices;
    [Tooltip("測試設定目前體力值 值為0~20")][SerializeField] float TestLatticeVal;
    [Tooltip("測試更新體力值")][SerializeField] bool TestInitLattices;
    [Tooltip("測試設定最大體力值 值為10~20")][SerializeField] float TestMaxLatticeVal;

    //TODO:之後有時間再研究BarLatticesConsumeMaterial使用的Shader grapher要如何正確製作 目前先用純白材質替代

    void Start() {
        ValOriginColor = SkillVigorVals[0].color;
        MaskOriginColor = SkillMasks[0].color;
        zeroQuaternion = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        halfQuaternion = Quaternion.Euler(new Vector3(0f, 180f, 45f));
        verticalQuaternion = Quaternion.Euler(new Vector3(0f, 180f, 90f));

        BarLatticesCloneMaterialList = new List<Material>();
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
        //這個function每秒都會被呼叫數次 所以盡量不要在裡面宣告物件(包含底下呼叫的Function)
        //如果正在演出消耗體力 先不做同步數值
        if (PerformConsumeVigor) return;
        SetLattices(val);
        SetValText(val, CurrentVal);
        //BattleSceneUI.Instance.CheckVigor(val); //此行為StaminaObj測試用 正式版應註解掉 不然流程會多不必要的計算
    }

    void SetLattices(float val) {
        //根據格子的物件數換算體力數值 對應演出格子數
        RealLatticVal = GetRealVal(val);
        //設定格子顯示數量
        for (int i = 0; i < Bar_lattices.Length; i++) {
            OldLatticVal = Bar_lattices_pre[i].fillAmount;
            Bar_lattices_pre[i].fillAmount = CountLatticeVal(RealLatticVal - i);
            if (OldLatticVal < 1 && Bar_lattices_pre[i].fillAmount == 1)
                PlayFx(i);
            Bar_lattices[i].gameObject.SetActive(Bar_lattices_pre[i].fillAmount == 1);
        }
    }

    float GetRealVal(float val) {
        //利用交叉相乘計算 格子數量 * 目前體力值 = 最大體力值 * val >> 格子數量 * 目前體力值 / 最大體力值 = val
        return Bar_lattices.Length * val / CurrentMaxVal;
    }

    float CountLatticeVal(float val) {
        return val > 1 ? 1 : val < 0 ? 0 : val;
    }

    void PlayFx(int indexOfLattices) {
        BarFxTrans.localPosition = Bar_lattices[indexOfLattices].transform.localPosition;
        if (indexOfLattices < 7 && indexOfLattices >= 0)
            BarFxTrans.localRotation = verticalQuaternion;
        else if (indexOfLattices < 14 && indexOfLattices >= 7)
            BarFxTrans.localRotation = halfQuaternion;
        else
            BarFxTrans.localRotation = zeroQuaternion;
        BarFxAni.Play("Energy bar recovery", -1, 0.0f);
        EnergyAni.Play("Energy recovery", -1, 0.0f);
        //WriteLog.LogErrorFormat("Play Fx! index: {0}", indexOfLattices);
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
        //鎖住SetEnergy 先專注做演出
        PerformConsumeVigor = true;
        //停止閃電演出
        BarFxAni.Play("Energy bar recovery", -1, 1f);
        //隱藏最後未滿格的體力條 & 抓出消耗的體力格做閃爍演出
        DoCastLattices(vigor, HideLastNotFullLattices()).Forget();
    }

    int HideLastNotFullLattices() {
        for (int i = Bar_lattices_pre.Length - 1; i >= 0; i--) {
            if (Bar_lattices_pre[i].fillAmount > 0 && Bar_lattices_pre[i].fillAmount < 1) {
                Bar_lattices_pre[i].fillAmount = 0;
                return i - 1;
            } else if (Bar_lattices_pre[i].fillAmount == 1 && i == Bar_lattices_pre.Length - 1){
                //如果連最後一格都滿就表示已經全滿 不用繼續找 也不用隱藏任何一格
                return i;
            }
        }
        return 0;
    }

    async UniTaskVoid DoCastLattices(int vigor, int lastLatticeStartIndex) {
        for (int i = lastLatticeStartIndex; i > lastLatticeStartIndex - vigor; i--) {
            //恢復體力圖圖關閉
            Bar_lattices_pre[i].fillAmount = 0f;
            //賦予material準備演出
            Bar_lattices[i].material = GetBarLatticeCloneMaterial(lastLatticeStartIndex - i, i);
        }
        await UniTask.WaitForSeconds(0.08f);
        //發亮演出
        //經測試設定材質會無法正常演出(基底是使用Skill 01複製出來的Material 但不同貼圖調整亮度效果不一樣 導致演出效果很差)
        //所以先改用純白色材質球 純調整alpha值
        float duration = 0.14f;
        float passTime = 0f;
        //float exposureVal;
        float latticeAlpha;
        Color tempColor = Color.white;
        while (passTime < duration) {
            passTime += Time.deltaTime;
            latticeAlpha = Mathf.Lerp(0f, 1f, passTime / duration);
            tempColor.a = latticeAlpha;
            //exposureVal = Mathf.Lerp(1f, 6f, passTime / duration);
            for (int i = lastLatticeStartIndex; i > lastLatticeStartIndex - vigor; i--) {
                //Bar_lattices[i].material.SetFloat("_exposure", exposureVal);
                Bar_lattices[i].color = tempColor;
            }
            await UniTask.Yield();
        }

        //扣體力值
        if (int.TryParse(CurrentVal.text, out int currentVigor)) {
            CurrentVal.text = (currentVigor - vigor).ToString();
        }

        //淡出
        duration = 0.14f;
        passTime = 0f;
        
        while (passTime < duration) {
            passTime += Time.deltaTime;
            latticeAlpha = Mathf.Lerp(1f, 0f, passTime / duration);
            tempColor.a = latticeAlpha;
            for (int i = lastLatticeStartIndex; i > lastLatticeStartIndex - vigor; i--) {
                Bar_lattices[i].color = tempColor;
            }
            await UniTask.Yield();
        }

        //關閉能量條與還原
        for (int i = lastLatticeStartIndex; i > lastLatticeStartIndex - vigor; i--) {
            Bar_lattices[i].gameObject.SetActive(false);
            Bar_lattices[i].color = Color.white;
            Bar_lattices[i].material = null;
        }

        //演出完解鎖 繼續SetEnergy同步
        PerformConsumeVigor = false;
    }

    Material GetBarLatticeCloneMaterial(int index, int barLatticePos) {
        Material clone = null;
        if (index >= BarLatticesCloneMaterialList.Count) {
            clone = Instantiate(BarLatticesConsumeMaterial);
            //clone.name = string.Format("Clone{0}", index);
            BarLatticesCloneMaterialList.Add(clone);
        } else {
            clone = BarLatticesCloneMaterialList[index];
        }
        //目前設定材質有問題(基底是使用Skill 01複製出來的Material 但是測試發現不同貼圖調整亮度會不一樣 導致演出效果很差)
        //clone.SetTexture("_main_Tex", BarLatticesTex[barLatticePos]);
        //WriteLog.LogErrorFormat("material name: {0}, texture name: {1}", clone.name, clone.mainTexture.name);
        return clone;
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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Codice.Utils;
using Cysharp.Threading.Tasks;
using Gladiators.Main;
using Scoz.Func;
using UnityEngine;
using UnityEngine.UI;

public class NextBattleSkillButton : MonoBehaviour
{
    [SerializeField] Image SkillIcon;
    [SerializeField] Image Mask;
    [SerializeField] Material IconMaterial;
    [SerializeField] MyTextPro Val;

    JsonSkill SkillData;
    Material CloneMaterial;
    int NextSkillID;

    CancellationTokenSource CTS;
    CancellationToken CT;
    Color IconColor;

    // Start is called before the first frame update
    void Start()
    {
        CloneMaterial = Instantiate(IconMaterial);
        CTS = new CancellationTokenSource();
        CT = CTS.Token;
        IconColor = SkillIcon.color;
        SkillIcon.material = CloneMaterial;
    }

    /// <summary>
    /// 設定技能資料
    /// </summary>
    /// <param name="_skill">技能資料</param>
    public void SetData(JsonSkill _skill) {
        SkillData = _skill;
        Debug.LogFormat("技能物件:{0}設定技能資料! 技能ID: {1}", gameObject.name, SkillData.ID);
        //設定技能圖片 要改Material 因為圖片是直接設定在材質球上的Tex
        if (SkillData != null && !string.IsNullOrEmpty(SkillData.Ref)) {
            //設定SkillIcon
            AssetGet.GetSpriteFromAtlas("SpellIcon", SkillData.Ref, (sprite) => {
                if (sprite != null) {
                    SkillIcon.gameObject.SetActive(true);
                    CloneMaterial.SetTexture("_main_Tex", sprite.texture);
                }
                else
                    SetIconFail();
            });
        } else
            SetIconFail();
    }

    void SetIconFail()
    {
        SkillIcon.gameObject.SetActive(false);
        Debug.LogWarning("Next battle skill button try set skill icon fail!");
    }

    /// <summary>
    /// 存放下一個技能
    /// </summary>
    /// <param name="_skillId">技能ID</param>
    public void CacheSkillId(int _skillId)
    {
        NextSkillID = _skillId;
        Debug.LogErrorFormat("Next skill cache next skill. ID: {0}", _skillId);
    }

    public int GetNextSkillId()
    {
        //取得ID後要把SkillData空掉 為了應對發生施放立即技能後立刻碰到發動近戰技能 而包還沒回來的情況(發生機率極低)
        int id = SkillData != null ? SkillData.ID : 0;
        SkillData = null;
        return id;
    }

    public void ChangeSkill()
    {
        if (CT != null) {
            CTS.Cancel();
            CTS = new CancellationTokenSource();
            CT = CTS.Token;
        }
        PlayChangeSkill().Forget();
    }

    async UniTaskVoid PlayChangeSkill()
    {
        //只淡出淡入Icon跟Val(背景那些如果美術要求要淡出入 必須跟美術說明因為遮罩導致沒辦法正常淡出淡入)
        float alpha = 0f;
        float passTime = 0f;
        float duration = 0.04f;
        //TODO:能量值淡出
        //淡出
        while (passTime < duration) {
            passTime += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, passTime / duration);
            IconColor.a = alpha;
            SkillIcon.color = IconColor;
            Val.alpha = alpha;
            await UniTask.Yield(CT);
        }
        var _skill = GameDictionary.GetJsonData<JsonSkill>(NextSkillID);
        //改圖
        SetData(_skill);
        //改能量消耗值
        BattleSceneUI.Instance.SetNextSkillVigorCost(_skill != null ? _skill.Vigor : 0);
        //改完後清理掉技能ID
        NextSkillID = 0;
        duration = 0.26f;
        passTime = 0f;
        //TODO:能量值淡入
        //淡入
        while(passTime < duration) {
            passTime += Time.deltaTime;
            alpha = Mathf.Lerp(0f, 1f, passTime / duration);
            IconColor.a = alpha;
            SkillIcon.color = IconColor;
            Val.alpha = alpha;
            await UniTask.Yield(CT);
        }
    }
}

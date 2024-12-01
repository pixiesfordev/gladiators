using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gladiators.Battle;
using Gladiators.Main;
using Scoz.Func;
using UnityEngine;
using UnityEngine.UI;
using static BattleSceneUI;

/// <summary>
/// 戰鬥技能按鈕
/// </summary>
public class BattleSkillButton : MonoBehaviour {

    [SerializeField] Image SkillIcon;
    [SerializeField] Image WhiteRotate;
    [SerializeField] Image White;
    [SerializeField] Image White01;
    [SerializeField] Image ButtonLight;
    [SerializeField] Image ExplosiveCast;
    [SerializeField] Image Jitter;
    [SerializeField] Image ButtonLightRotate;
    [SerializeField] Button Btn;
    [SerializeField] Material SkillIconMaterial;
    [SerializeField] Material GrayMaterial;
    [SerializeField] Animator BtnAni;
    [SerializeField] Texture2D SkillMaterialMask;

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("使用技能外移位置")][SerializeField] Vector3 UsedSkillMoveOutPosition;
    [Tooltip("使用技能外移所需時間")][SerializeField] float UsedSkillMoveOutTime = 1f;
    //[Tooltip("")][SerializeField] ;

    JsonSkill SkillData;
    public bool SkillSelected { get; private set; } = false; //技能被選上 等待觸發 碰撞觸發技能使用

    //TODO:之後有足夠時間改設計成陣列 根據演出需求動態添加材質球 目的為不能共用材質球 >> 先用簡單寫死的方式 看有幾個物件會用到就建立幾個
    Material IconMaterial;
    Material GrayIconMaterial;
    Material WhiteMaterial;
    Material WhiteMaterial01;
    Material ButtonLightMaterial;
    Material ExplosiveCastMaterial;
    Material JitterMaterial;

    //CancellationTokenSource CurrentCTS;
    //CancellationToken CurrentCT; //用來中斷UniTask.Yield
    float clickWaitDuration = 0f;

    bool IsEnergyEnough = false;
    bool OldEnergyEnough = true;
    float EnergyRate = 0f;
    //Color IconGrayNormalColor = new(0.5f, 0.5f, 0.5f);
    //Color PressColor = new(0.78f, 0.78f, 0.78f);
    //Color HideColor = new(1f, 1f, 1f, 0f);
    //Vector3 ZoomInScale = new(1.2f, 1.2f, 1f);

    enum SkillAniState {
        IDLE,
        AVAILABLE,
        CHANGE_SKILL,
        ENOUGH_ENERGY_PRESS,
        ENOUGH_ENERGY_NORMAL,
        ENOUGH_ENERGY_RELEASED,
        INSUFFICIENT_ENERGY_PRESS,
        INSUFFICIENT_ENERGY_ROTATION, //等於INSUFFICIENT_ENERGY_NORMAL 因為能量不足一定就會播放轉圈動畫
        INSUFFICIENT_ENERGY_RELEASED,
        START_CANCEL,
        START_CAST,
        START_SCALE,
        START_VIBRATE,
        END_CHANGE_SKILL,
        INSTANT_WAIT_SERVER,
    }
    SkillAniState curAniState = SkillAniState.IDLE;
    SkillAniState oldAniState = SkillAniState.IDLE;

    bool btnLocking = false;
    int CacheSKillId;

    void Start() {
        //初始化演出用材質球副本 避免每次演出一直產生新的object
        IconMaterial = Instantiate(SkillIconMaterial);
        GrayIconMaterial = Instantiate(GrayMaterial);
        WhiteMaterial = Instantiate(SkillIconMaterial);
        WhiteMaterial01 = Instantiate(SkillIconMaterial);
        ButtonLightMaterial = Instantiate(SkillIconMaterial);
        ExplosiveCastMaterial = Instantiate(SkillIconMaterial);
        JitterMaterial = Instantiate(SkillIconMaterial);

        WhiteMaterial01.SetFloat("_exposure", 2f);
        WhiteMaterial01.SetFloat("_color_saturation", 0f);
        //先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
        //White01.material = WhiteMaterial01;

        WhiteMaterial.SetFloat("_exposure", 2f);
        WhiteMaterial.SetFloat("_color_saturation", 0);
        //先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
        //White.material = WhiteMaterial;

        IconMaterial.SetTexture("_main_mask01", SkillMaterialMask);
        //CreateCTS();
    }

    /*
    void CreateCTS()
    {
        //目前先不使用 本來設計用意為用來中斷UniTask.Yield 但相關演出秒數都相當短 都是0.0X秒 不太可能發生需要中斷Yield的相關演出情況
        if (CurrentCTS != null)
            CurrentCTS.Cancel();
        CurrentCTS = new CancellationTokenSource();
        CurrentCT = CurrentCTS.Token;
    }
    */

    /// <summary>
    /// 設定技能資料
    /// </summary>
    /// <param name="_skill">技能資料</param>
    /// /// <param name="_init">是否為初始化</param>
    public void SetData(JsonSkill _skill, bool _init) {
        SkillData = _skill;
        Debug.LogWarningFormat("技能物件:{0}設定技能資料! 技能ID: {1}", gameObject.name, SkillData != null ? SkillData.ID : 0);
        if (SkillData != null && !string.IsNullOrEmpty(SkillData.Ref)) {
            //設定SkillIcon
            AssetGet.GetSpriteFromAtlas("SpellIcon", SkillData.Ref, (sprite) => {
                SkillIcon.gameObject.SetActive(true);
                if (sprite != null) {
                    SkillIcon.sprite = sprite;
                    WhiteRotate.sprite = sprite;
                    //WriteLog.LogWarningFormat("設定圖片! 技能物件: {0} 是否為初始化: {1} 能量是否足夠: {2}", name, _init, IsEnergyEnough);
                    if (_init)
                        SetSkillIconGray(!IsEnergyEnough);
                }
                else
                    AssetGet.GetSpriteFromAtlas("SpellIcon", "sprint", (sprite) => {
                        SkillIcon.sprite = sprite;
                        WhiteRotate.sprite = sprite;
                        if (_init)
                            SetSkillIconGray(!IsEnergyEnough);
                        //WriteLog.LogWarningFormat("圖片缺少! 用衝刺圖代替顯示! ID: {0}", SkillData.Ref);
                    });
            });
        } else {
            SkillIcon.gameObject.SetActive(false);
            Debug.LogWarning("無法設定圖片 技能資料可能為空或Ref為空值!");
        }
    }

    /// <summary>
    /// 同步後端送來的技能選上狀態
    /// </summary>
    /// <param name="_on">是否選上</param>
    public void SetSkillOn(bool _on) {
        //只做同步 演出會在前端先做 所以這裡不做其他演出動作
        SkillSelected = _on;
    }

    /// <summary>
    /// 取消選上按鈕 會在其他近戰技能被選上時由前端呼叫來取消其他技能的選上狀態
    /// </summary>
    public void CancelSelected() {
        //Start Cancel >> AfterCancelEvent
        btnLocking = true;
        SkillSelected = false;
        curAniState = SkillAniState.START_CANCEL;
        BtnAni.Play(IsEnergyEnough ? "Start_Cancel" : "Start_Cancel_Insufficient");
        //Debug.LogErrorFormat("其他技能被選上 取消已選上技能: {0}", name);
    }

    public void AfterCancelEvent() {
        //由Start_Cancel和Start_Cancel_Insufficient播放完畢後呼叫 >> 恢復普通狀態
        if (IsEnergyEnough) {
            //能量足夠普通狀態
            PlayButtonNormal();
            //Debug.LogError("取消動畫播放完畢 能量足夠");
        } else {
            //能量不足普通狀態
            PlayButtonInsufficientNormal();
            //Debug.LogError("取消動畫播放完畢 能量不足");
        }
        //解鎖按鈕
        btnLocking = false;
    }

    public void AfterScaleEvent() {
        //由Start_Scale和Start_Scale_Insufficient播放完畢後呼叫 >> 開始抖動
        PlayButtonVibrate();
        //Debug.LogErrorFormat("放大動畫播放完畢 能量是否足夠? {0}", IsEnergyEnough);
    }

    public void AfterReleasedEvent() {
        //由Enough_Energy_Released與Insufficient_energy_released播放完畢後呼叫
        //判斷是否之前是抖動中狀態 如果是必須回歸回抖動狀態 
        //因為有可能玩家點了技能後悔不想施放 這時候如果拖曳滑鼠沒觸發OnClick事件會導致抖動直接不正常停止
        if (SkillSelected) {
            PlayButtonVibrate();
            //Debug.LogError("點了按鈕後放開按鈕 但不取消技能 恢復抖動狀態!");
        }
        //如果是能量不足還需要關閉物件
        if (!IsEnergyEnough) {
            White.gameObject.SetActive(false);
            White01.gameObject.SetActive(false);
        }
    }

    public void AfterVibruteEvent() {
        //由start_vibrate與start_vibrate_Insufficient播放完畢後呼叫 
        if (SkillData.Activation == SkillActivation.Instant) {
            //立即施放類 等待後端發送的封包後才演出
            if (IsEnergyEnough) {
                curAniState = SkillAniState.INSTANT_WAIT_SERVER;
                //Debug.LogError("立即釋放技能 等待Server送包");
            }
        } else if (SkillData.Activation == SkillActivation.Melee) {
            //近戰類到此就解鎖 因為後續觸發碰撞發動是由後端發送 非前端觸發
            btnLocking = false;
        }
    }

    public void ModelCastSkill() {
        //由start_cast播放到0.22秒的時候呼叫 配合按鈕最亮的時候才讓模型開始發動技能演出
        //TODO:發送命令給BattleManager去放技能
        //技能體力值釋放技能演出
        BattleSceneUI.Instance.BattleSkillBtnCastHideVigorVal(this);
        Debug.LogWarning("要求模型播放技能動畫");
    }

    public void CastSkillSetIconGrayEvent() {
        //由start_cast播放到0.28秒的時候呼叫 把圖片打灰
        SetSkillIconGray(true);
    }

    public void AfterCastSkillEvent() {
        //由start_cast播放完畢後呼叫
        curAniState = SkillAniState.CHANGE_SKILL;
        BtnAni.Play("Change skills");
        //通知BattleSceneUI開始更換NextSkillBtn 演出時間要一致
        BattleSceneUI.Instance.NextSkillBtnChangeSkill();
        //Debug.LogError("釋放技能播放完畢");
    }

    public void ChangeSkillEvent() {
        //由BtnAni的Change skills呼叫此事件 更換技能圖片
        var _jsonSkill = GameDictionary.GetJsonData<JsonSkill>(CacheSKillId);
        SetData(_jsonSkill, false);
        BattleSceneUI.Instance.SetSKillVigorCost(this, _jsonSkill != null ? _jsonSkill.Vigor : 0);
        //Debug.LogErrorFormat("更換技能資料! ID: {0}", CacheSKillId);
    }

    public void AfterChangeSkillEvent() {
        //由BtnAni的Change skills播放完畢後呼叫
        //設定狀態 準備讓SetEnergy重新判斷狀態
        curAniState = SkillAniState.END_CHANGE_SKILL;
        //Debug.LogError("技能更換完畢 解鎖按鈕");
    }

    public void AfterAvailableEvent() {
        //播放完此狀態要判斷撥放前是否為抖動狀態 是則要恢復抖動狀態演出
        if (oldAniState == SkillAniState.START_VIBRATE) {
            PlayButtonVibrate();
            //Debug.LogErrorFormat("After available event! energy enough? {0}", IsEnergyEnough);
        } else {
            //其他情況一定只有能量足夠普通/能量不足普通狀態 會有不足狀態是因為可能播放動畫期間有其他技能施放 導致能量又不足
            if (IsEnergyEnough) {
                //能量足夠普通狀態
                PlayButtonNormal();
                //Debug.LogError("After available event! 能量足夠");
            } else {
                //能量不足普通狀態
                PlayButtonInsufficientNormal();
                //Debug.LogError("After available event! 能量不足");
            }
        }
    }

    /// <summary>
    /// 收到Server回傳後 施展立即技能
    /// </summary>
    /// <param name="_skillId"></param>
    public void CastInstantSkill(int _skillId) {
        PlayButtonCast();
        CacheSKillId = _skillId;
        WriteLog.LogWarning("收到立即釋放技能回傳封包 開始施展技能!");
    }

    void PlayButtonCast() {
        curAniState = SkillAniState.START_CAST;
        BtnAni.Play("start_cast");
        BattleSceneUI.Instance.BattleSkillBtnCastStaminaConsume(SkillData.Vigor);
    }

    void PlayButtonNormal() {
        SetSkillIconGray(false);
        curAniState = SkillAniState.ENOUGH_ENERGY_NORMAL;
        BattleSceneUI.Instance.BattleSkillBtnSetVigorEnergyState(this, true);
        BtnAni.Play("Enough energy_Normal");
    }

    void PlayButtonInsufficientNormal() {
        SetSkillIconGray(true);
        curAniState = SkillAniState.INSUFFICIENT_ENERGY_ROTATION;
        BattleSceneUI.Instance.BattleSkillBtnSetVigorEnergyState(this, false);
        BtnAni.Play("Insufficient Energy_Normal");
    }

    void PlayButtonVibrate() {
        curAniState = SkillAniState.START_VIBRATE;
        BtnAni.Play(IsEnergyEnough ? "start_vibrate" : "start_vibrate_Insufficient");
    }

    /// <summary>
    /// 接收後端肉搏(近戰)施放的封包 施放技能
    /// </summary>
    /// <param name="_nextSkillId">下一個技能ID</param>
    public void CastMeleeSkill(int _nextSkillId) {
        WriteLog.LogError("CastMeleeSkill: " + _nextSkillId);
        SkillSelected = false;
        btnLocking = true;
        CacheSKillId = _nextSkillId;
        //Debug.LogErrorFormat("Cast melee skill. Cache next skill. ID: {0}", _nextSkillId);
        //start cast(0.22秒時ModelCastSkill 0.28秒時CastSkillSetIconGrayEvent) >> AfterCastSkillEvent >>
        //Change skills(ChangeSkillEvent) >> AfterChangeSkillEvent 
        PlayButtonCast();
    }

    void SetSkillIconGray(bool _bGray) {
        SkillIcon.material = _bGray ? GrayIconMaterial : null;
        //WriteLog.LogWarningFormat("設定圖片是否打灰: {0} 按鈕: {1}", _bGray, name);
    }

    /// <summary>
    /// 更新能量表(轉圈演出)
    /// </summary>
    /// <param name="val">體力值</param>
    public void SetEnergy(float val) {
        //這個方法幾乎是每秒都會被呼叫數次 所以盡量不要在裡面宣告物件
        if (SkillData == null) {
            Debug.LogWarningFormat("Set energy fail! Skill Data null! Obj: {0}", name);
            return;
        }

        //確認能量狀態 這裡不能被下面演出狀態擋掉 不然會導致能量足夠與否的判斷錯誤
        //Debug.LogErrorFormat("Set energy. cur val: {0} cost val: {1}", val, SkillData.Vigor);
        EnergyRate = val / SkillData.Vigor;
        IsEnergyEnough = EnergyRate >= 1f;

        //判斷演出狀態 阻擋演出被中斷
        if (curAniState == SkillAniState.CHANGE_SKILL ||
            curAniState == SkillAniState.START_CAST ||
            curAniState == SkillAniState.START_SCALE ||
            curAniState == SkillAniState.START_CANCEL ||
            curAniState == SkillAniState.AVAILABLE ||
            curAniState == SkillAniState.INSTANT_WAIT_SERVER) {
            //WriteLog.LogWarningFormat("不能點按鈕! 特殊演出狀態: {0} 按鈕: {1}", curAniState, name);
            return;
        }

        if (EnergyRate < 1f && EnergyRate > 0f) {
            //介於0~1之間才填入fillAmount以免出錯
            WhiteRotate.fillAmount = EnergyRate;
            ButtonLightRotate.fillAmount = EnergyRate;
        } else {
            //不介於0~1之間表示已經充滿不需再顯示轉圈條或設定錯誤(小於0的情況)
            EnergyRate = 1f;
            WhiteRotate.fillAmount = 0f;
            ButtonLightRotate.fillAmount = 0f;
        }
        if (curAniState == SkillAniState.END_CHANGE_SKILL) {
            //剛結束轉換技能演出 這時候OldEnergyEnough肯定是True 因為True才能施放技能 必須要特別寫判定
            if (IsEnergyEnough) {
                //能量足夠
                PlayButtonNormal();
                //Debug.LogError("轉換完技能 能量足夠施放技能!");
            } else {
                //能量不足
                PlayButtonInsufficientNormal();
                //Debug.LogError("轉換完技能 能量不夠施放技能!");
            }
            //按鈕解鎖(不在AfterChangeSkillEvent就解鎖 因為還要跑一次判定能量重設按鈕狀態 不然可能會有無法預期的演出錯誤)
            btnLocking = false;
        } else {
            //普通情況
            if (IsEnergyEnough != OldEnergyEnough) {
                //保存舊演出狀態以便演出後變回原狀態
                oldAniState = curAniState;
                if (!OldEnergyEnough) {
                    //能量從不足變足夠 >> 演出available
                    SetSkillIconGray(false);
                    curAniState = SkillAniState.AVAILABLE;
                    SetAvailableMaterial().Forget();
                    BtnAni.Play("available");
                    //Debug.LogWarning("能量足夠 演出Available!");
                } else {
                    //能量從足夠變不足
                    //先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
                    PlayButtonInsufficientNormal();
                    //Debug.LogWarning("能量不足 按鈕變回不足狀態!");
                }
            }
        }
        OldEnergyEnough = IsEnergyEnough;
    }

    async UniTaskVoid SetAvailableMaterial() {
        //先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
        //鎖定按鈕以免出錯
        btnLocking = true;
        IconMaterial.SetFloat("_exposure", 1f);
        IconMaterial.SetFloat("_color_saturation", 0f);
        //SkillIcon.material = IconMaterial;
        await DoSaturationGradient(0f, 1f, 0.02f, IconMaterial);
        await UniTask.WaitForSeconds(0.29f);
        //演出完畢解鎖按鈕
        btnLocking = false;
    }

    public void SkillEnergyEnough() {
        //現在先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
        //由BtnAni的available呼叫此事件(結束時)
        //SetSkillIconGray(false);
        SkillIcon.color = Color.white;
        ButtonLight.color = Color.white;
    }

    public void PressSkill() {
        //Debug.LogErrorFormat("press skill. Time: {0}", Time.time);
        if (btnLocking) {
            Debug.LogWarningFormat("Press skill but btn is locking!");
            return;
        }
        if (IsEnergyEnough)
            EnoughEnergyPress();
        else
            InsufficientEnergyPress();
    }

    void EnoughEnergyPress() {
        //Debug.LogErrorFormat("enough energy press! Time: {0}", Time.time);
        curAniState = SkillAniState.ENOUGH_ENERGY_PRESS;
        BtnAni.Play("Enough_Energy_Press");
    }

    void InsufficientEnergyPress() {
        //Debug.LogErrorFormat("Insufficient energy press! Time: {0}", Time.time);
        curAniState = SkillAniState.INSUFFICIENT_ENERGY_PRESS;
        BtnAni.Play("Insufficient_Energy_Press");
        //現在先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
        //DoInsufficientPress().Forget();
        /*
        float duration = 0.03f;
        //設定white01材質
        WhiteMaterial01.SetFloat("_exposure", 8f);
        //設定white材質
        WhiteMaterial.SetFloat("_exposure", 8f);
        */
    }

    public void StartInsufficientEnergyPressEvent() {
        //Insufficient energy released動畫開始撥放時呼叫 可以從BtnAni看到調用
        White.gameObject.SetActive(true);
        White01.gameObject.SetActive(true);
    }

    /*現在先把使用自訂Material相關的邏輯註解掉 因為無法正常裁切 需要找方法正確裁切
    async UniTask DoInsufficientPress() {
        float duration = 0.03f;
        float startVal = 2f;
        float endVal = 8f;
        DoExposureGradient(startVal, endVal, duration, WhiteMaterial).Forget();
        DoExposureGradient(startVal, endVal, duration, WhiteMaterial01).Forget();
        await UniTask.WaitForSeconds(duration);
    }
    */

    public void ReleasedSkill() {
        //Debug.LogErrorFormat("released skill Time: {0}", Time.time);
        if (btnLocking) {
            Debug.LogWarningFormat("Released skill but btn is locking!");
            return;
        }
        //技能動畫演出
        if (IsEnergyEnough) {
            clickWaitDuration = 0.1f;
            EnoughEnergyReleased();
        } else {
            clickWaitDuration = 0.03f;
            InsufficientEnergyReleased();
        }
    }

    void EnoughEnergyReleased() {
        //Debug.LogErrorFormat("enough energy released! Time: {0}", Time.time);
        curAniState = SkillAniState.ENOUGH_ENERGY_RELEASED;
        BtnAni.Play("Enough_Energy_Released");
    }

    void InsufficientEnergyReleased() {
        //Debug.LogErrorFormat("insufficient energy released! Time: {0}", Time.time);
        curAniState = SkillAniState.INSUFFICIENT_ENERGY_RELEASED;
        BtnAni.Play("Insufficient_Energy_Released");
        /*先把材質相關設定註解掉 目前沒辦法正確在Mask下使用Material
        //white011材質變更
        WhiteMaterial01.SetFloat("_exposure", 2f);
        //white材質變更
        WhiteMaterial.SetFloat("_exposure", 2f);
        */
    }

    //點擊釋放技能
    async UniTaskVoid CastSkill() {
        //必須等待ReleasedBtn的演出結束
        await UniTask.WaitForSeconds(clickWaitDuration);
        //判斷技能是否存在
        if (SkillData == null) {
            Debug.LogWarning("Skill Data is null!");
            return;
        }
        //判斷是否已經有其他技能在施放中
        if (BattleSceneUI.Instance.CanSpellInstantSkill) return;
        //判斷按鈕是否鎖定中 比如放開按鈕演出完到判斷施法之間發生Available的演出動畫 美術要求必須重新點按鈕 所以視為無效點擊
        if (btnLocking) {
            Debug.LogWarning("Cast skill! 但按鈕被鎖定了!");
            return;
        }
        //檢查技能型態
        if (SkillData.Activation == SkillActivation.Instant) {
            //立即施放 >> 判斷EnergyEnough >> 足夠就Start Scale並送包給後端告知開始施法 不夠就沒事
            //StartScale >> AfterScaleEvent >> Start vibrute >> AfterVibruteEvent >> CastInstantSkill
            //start_cast(0.22秒時ModelCastSkill 0.28秒時CastSkillSetIconGrayEvent) >>
            //AfterCastSkillEvent >> Change skills(ChangeSkillEvent) >> AfterChangeSkillEvent 
            if (IsEnergyEnough) {
                btnLocking = true;
                BattleSceneUI.Instance.CastingInstantSKill(this, SkillData.ID);
                curAniState = SkillAniState.START_SCALE;
                BtnAni.Play("Start_Scale");
                //Debug.LogError("開始釋放立即施放技能!");
            }
        } else if (SkillData.Activation == SkillActivation.Melee) {
            //判斷是否已經被選上
            if (SkillSelected) {
                //已選上 >> Start Cancel >> AfterCancelEvent
                btnLocking = true;
                curAniState = SkillAniState.START_CANCEL;
                BtnAni.Play(IsEnergyEnough ? "Start_Cancel" : "Start_Cancel_Insufficient");
                SkillSelected = !SkillSelected;
                AllocatedRoom.Instance.ActiveSkill(SkillData.ID, SkillSelected);
                //Debug.LogError("取消近戰技能!");
            } else {
                //未選上 >> 判斷能量是否足夠 足夠才可以選取
                if (IsEnergyEnough) {
                    //Start Scale >> AfterScaleEvent >> Start vibrute >> AfterVibruteEvent
                    btnLocking = true;
                    curAniState = SkillAniState.START_SCALE;
                    BtnAni.Play(IsEnergyEnough ? "Start_Scale" : "Start_Scale_Insufficient");
                    SkillSelected = !SkillSelected;
                    AllocatedRoom.Instance.ActiveSkill(SkillData.ID, SkillSelected);
                    //Debug.LogError("選上近戰技能!");
                    //檢查其他按鈕取消選上
                    BattleSceneUI.Instance.CancelOtherSelectedSKill(this);
                }
            }
        }
    }

    public void OnClick() {
        //準備判斷點擊事件 因為要配合點錯技能拖曳滑鼠或手指到按鈕外取消施法的操作 所以不在Released判斷 而是在Click事件判斷
        CastSkill().Forget();
        //Debug.LogErrorFormat("觸發點擊按鈕! Time: {0}", Time.time);
    }

    /*配合Shader材質的演出 先註解 因為現在Shader有問題
    async UniTaskVoid StartCastEvent()
    {
        float startVal = 1.5f;
        float endVal = 15f;
        //初始化材質球
        IconMaterial.SetFloat("_exposure", startVal);
        SkillIcon.material = IconMaterial;
        await UniTask.WaitForSeconds(0.11f);
        //第一段漸變 exposureVal從1.5 >> 15 費時0.12秒
        await DoExposureGradient(startVal, endVal, 0.12f, IconMaterial);
        startVal = endVal;
        endVal = 1f;
        //第二段漸變 exposureVal從15 >> 1 費時0.03秒
        await DoExposureGradient(startVal, endVal, 0.03f, IconMaterial);
    }
    */

    /// <summary>
    /// 做材質球的Exposure漸變
    /// </summary>
    /// <param name="startVal"></param>
    /// <param name="endVal"></param>
    /// <param name="duration"></param>
    /// <param name="_material"></param>
    /// <returns></returns>
    async UniTask DoExposureGradient(float startVal, float endVal, float duration, Material _material) {
        float exposureVal = startVal;
        float passTime = 0f;
        while (passTime < duration) {
            passTime += Time.deltaTime;
            exposureVal = Mathf.Lerp(startVal, endVal, passTime / duration);
            _material.SetFloat("_exposure", exposureVal);
            await UniTask.Yield();
        }
    }

    async UniTask DoSaturationGradient(float startVal, float endVal, float duration, Material _material) {
        float saturationVal = startVal;
        float passTime = 0f;
        while (passTime < duration) {
            passTime += Time.deltaTime;
            saturationVal = Mathf.Lerp(startVal, endVal, passTime / duration);
            _material.SetFloat("_color_saturation", saturationVal);
            await UniTask.Yield();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Mathematics;
using DG.Tweening;
using System;
using Gladiators.Main;
using System.Collections.Generic;
using System.Linq;
using Loxodon.Framework.Binding;
using Unity.Collections;
namespace Gladiators.Battle {
    /// <summary>
    /// 上方角鬥士資訊
    /// </summary>
    public class BattleGladiatorInfo : MonoBehaviour {

        [HeaderAttribute("==============UI==============")]
        [SerializeField] MyText HeroName;
        [SerializeField] Image HeroIcon;
        [SerializeField] Image HPChangeBar;
        [SerializeField] Transform CloneHPChangerBarTrans;
        [SerializeField] Image HPBar;
        [SerializeField] Image HPBarGray;
        [SerializeField] Image HPBarWhite;
        [SerializeField] Transform HeartBeatIconTrans; //心跳Icon物件
        [SerializeField] Image HeartBeatIcon;
        [SerializeField] Transform HeartBeatGrayIconTrans;
        [SerializeField] Image HeartBeatGrayIcon;
        [SerializeField] Transform BuffIconGridTrans;
        [SerializeField] BattleBufferIcon BufferIcon;

        [HeaderAttribute("==============TEST==============")]
        [HeaderAttribute("=========受擊血量變動測試=========")]
        [Tooltip("初始化血量條件")][SerializeField] bool InitHPCondition = false;
        [Tooltip("英雄測試最大血量 必須是整數(int)")][SerializeField] int HeroTestMaxHP = 0;
        [Tooltip("英雄測試目前血量 必須是整數(int)")][SerializeField] int HeroTestCurHP = 0;
        [Tooltip("測試血量變化演出")][SerializeField] bool PerformHPChangeFlag = false;
        [Tooltip("英雄變化血量 必須是整數(int)")][SerializeField] int HeroChangeHP = 0;
        [Tooltip("血量變化停滯秒數 就是被打掉的或者恢復量的血條殘留時間")][SerializeField] float BarChangeSecDelay = 1f;
        [Tooltip("血量變化演出秒數 越短就越快")][SerializeField] float BarChangeSecNeed = 1f;
        [Tooltip("血量變化演出偵數 即每秒血條變化張數")][SerializeField] float BarChangeFrame = 60f;
        [Tooltip("心跳受擊旋轉角度")][SerializeField] Vector3 HittedRotateAngle = new Vector3(0f, 0f, 10f);
        [Tooltip("心跳受擊旋轉演出時間 即旋轉過去加轉回去的時間")][SerializeField] float HittedRotateDuration = 1f;
        [Tooltip("心跳受擊變色持續時間")][SerializeField] float HittedColorDuration = 1f;
        [Tooltip("受擊白色血條顯示時間")][SerializeField] float HittedHPBarWhiteDuration = 0.5f;
        [Tooltip("每扣多少%血量就產生一個殘影 數值為0(不含0)~1 數值越小越耗效能 殘影也越密集")][SerializeField] float HPRateGenerateAfterImage = 0.05f;
        [Tooltip("殘影滯留秒數")][SerializeField] float AfterImageDuration = 0.5f;
        [Tooltip("黑白血量變化停滯秒數 就是被打掉的或者恢復量的血條殘留時間")][SerializeField] float BarGrayChangeSecDelay = 0.4f;
        [Tooltip("黑白血條演出秒數 越短就越快")][SerializeField] float BarGrayChangeSecNeed = 0.4f;
        [HeaderAttribute("==============心臟跳動==============")]
        //[Tooltip("")][SerializeField];
        [Tooltip("更新心跳參數")][SerializeField] bool UpdateHeartBeatParameter = false;
        [Tooltip("滿血時每次心跳前等待所需秒數 即滿血時每幾秒跳一次(即最慢速)")][SerializeField] float HeartBeatDelayMax = 3f;
        [Tooltip("1%血量時每次心跳前等待所需秒數 即1%血量時每幾秒跳一次(即最快速)")][SerializeField] float HeartBeatDelayMin = 0.5f;
        [Tooltip("心跳放大倍數 1為原本比例沒放大 2就是放大一倍 建議1~2之間")][SerializeField] float HeartBeatTweenScale = 1.3f;
        [Tooltip("心跳所需時間 即從原本尺寸到放大再到變回來的所需時間 這個會自動隨著血量減少等比加快")][SerializeField] float HeartBeatDuration = 1f;
        [Tooltip("心跳需要反向 敵人頭像這個要打勾")][SerializeField] bool HeartBeatReverse = false;
        [HeaderAttribute("==============變更英雄頭像==============")]
        [Tooltip("測試變更英雄頭像 要搭配英雄最大&目前血量一起設定")][SerializeField] bool UpdateHeroIcon = false;
        [Tooltip("測試英雄頭像ID")][SerializeField] int HeroTestID = 0;

        [HeaderAttribute("==============測試Buffer==============")]
        [Tooltip("測試bufferIcon")][SerializeField] string[] TestEffectStr;
        [Tooltip("測試bufferIcon")][SerializeField] int[] TestEffectNum;
        [Tooltip("測試bufferIcon")][SerializeField] SkillExtension.BuffIconValType[] TestValTypes;
        [Tooltip("測試bufferIcon")][SerializeField] bool ShowTestEffect;

        //TODO:
        //1.扣血/回血 血條(Bar)演出 血條總長度396 Right值越大長度越短 396就等於0% >> 已完成(9/17改版)
        //1.心臟跳動效果(公式:100~1 >> Min~Max) >> 已完成(4/9)
        //2.受擊 >> 心跳變黑白 往左搖晃一下 之後變回原本顏色 血條瞬間全白後開始扣減 >> 已完成(4/11)
        //3.原本血條也要變黑白跟著縮減 縮減後變化血條不用隱藏 而是保留最後一小段 >> 已完成(4/11)
        //4.buff圖案

        CancellationTokenSource CurrentCTS; //用來中斷目前的血條演出

        //血條演出區塊參數
        RectTransform ChangeBarRect; //血條rt參考
        float CurrentHPRate = 1f; //目前血量比率
        Vector3 ChangeBarOriginPos;
        Color HideColor = new Color(1f, 1f, 1f, 0f);

        //心跳演出區塊參數
        Tweener HeartBeatScaleTween1;
        Tweener HeartBeatScaleTween2;
        Tweener HeartBeatGrayScaleTween1;
        Tweener HeartBeatGrayScaleTween2;

        int HeroMaxHP = 0; //英雄最大血量
        int HeroCurHP = 0; //英雄目前血量
        float HeroDisplayHPRate = 0f; //英雄顯示血量百分比

        List<BufferIconData> BufferDatas = new List<BufferIconData>();
        List<BattleBufferIcon> BufferIconObjList;

        private void Start() {
            ChangeBarRect = HPChangeBar.GetComponent<RectTransform>();
            ChangeBarOriginPos = ChangeBarRect.anchoredPosition3D;
            SetHeartBeatParameter();
            BufferIconObjList = new List<BattleBufferIcon> { BufferIcon };
            BufferIcon.gameObject.SetActive(false);
        }

        void Update() {
            //測試用 初始化血條
            if (InitHPCondition) {
                InitHP(HeroTestMaxHP, HeroTestCurHP);
                SetHPBar(HeroDisplayHPRate);
                InitHPCondition = false;
            }

            //測試用 做血條演出
            if (PerformHPChangeFlag) {
                AddHP(HeroChangeHP);
                PerformHPChangeFlag = false;
            }
            //測試用 做心跳演出
            if (UpdateHeartBeatParameter) {
                SetHeartBeatParameter();
                UpdateHeartBeatParameter = false;
            }
            //測試用 改變英雄頭像
            if (UpdateHeroIcon) {
                Init(HeroTestMaxHP, HeroTestCurHP, HeroTestID);
                UpdateHeroIcon = false;
            }

            //測試用 顯示bufferIcon
            if (ShowTestEffect) {
                ShowTestEffect = false;
                BufferIconData[] datas = new BufferIconData[TestEffectStr.Length];
                for (int i = 0; i < datas.Length; i++) {
                    datas[i] = new BufferIconData(TestEffectStr[i], TestEffectNum[i], TestValTypes[i]);
                }
                SetBufferIcon(datas.ToList());
            }
        }

        /// <summary>
        /// 初始化角鬥士資訊
        /// </summary>
        /// <param name="maxHP">最大血量</param>
        /// <param name="curHP">目前血量</param>
        /// <param name="heroID">英雄圖片ID</param>
        public void Init(int maxHP, int curHP, int heroID) {
            //血量設定
            InitHP(maxHP, curHP);

            JsonGladiator heroData = GameDictionary.GetJsonData<JsonGladiator>(heroID);
            if (heroData == null) {
                WriteLog.LogErrorFormat("英雄資料不存在.英雄ID:{0}", heroID);
                return;
            }

            //英雄頭像設定
            if (!string.IsNullOrEmpty(heroData.Ref)) {
                HeroIcon.gameObject.SetActive(true);
                AssetGet.GetSpriteFromAtlas("HeroIcon", heroData.Ref, (sprite) => {
                    if (sprite != null)
                        HeroIcon.sprite = sprite;
                    else {
                        AssetGet.GetSpriteFromAtlas("HeroIcon", "1", (sprite) => {
                            HeroIcon.sprite = sprite;
                            WriteLog.LogErrorFormat("英雄頭像不存在 使用替用圖案.英雄ID:{0}", heroID);
                        });
                    }
                });
            } else {
                HeroIcon.gameObject.SetActive(false);
                WriteLog.LogErrorFormat("英雄頭像不存在.英雄ID:{0}", heroID);
            }
            //英雄名字設定
            HeroName.text = heroData.Name;
        }

        /// <summary>
        /// 血量變化
        /// </summary>
        /// <param name="_value">變化量</param>
        public void AddHP(int _value) {
            //傳入變化血量 中斷目前演出 計算終值 往終值演出
            HeroCurHP += _value;
            if (HeroCurHP < 0)
                HeroCurHP = 0;
            else if (HeroCurHP > HeroMaxHP)
                HeroCurHP = HeroMaxHP;
            Debug.LogFormat("血量變化. 變化量:{0} 目前血量:{1}", _value, HeroCurHP);
            PerformHPChange();
        }

        public void AddMaxHP(int _value) {
            //傳入變化血量 中斷目前演出 計算終值 往終值演出
            HeroMaxHP += _value;
            if (HeroMaxHP <= 0) HeroMaxHP = 1;

            Debug.LogFormat("最大血量變化. 變化量:{0} 目前最大血量:{1}", _value, HeroMaxHP);
            PerformHPChange();
        }

        public int CheckMaxHP() {
            return HeroMaxHP;
        }
        public int CheckHP() {
            return HeroCurHP;
        }

        void InitHP(int maxHP, int curHP) {
            if (maxHP == 0) {
                maxHP = 1;
                WriteLog.LogError("血量最大值為0! 請檢查資料是否有問題");
            }
            HeroMaxHP = maxHP;
            HeroCurHP = curHP;
            HeroDisplayHPRate = curHP / maxHP;
            Debug.LogFormat("初始化HP. 最大HP:{0} 目前HP:{1} 目前顯示百分比:{2}", maxHP, curHP, HeroDisplayHPRate * 100);
        }

        //執行血條演出
        void PerformHPChange() {
            if (CurrentCTS != null)
                CurrentCTS.Cancel();
            CurrentCTS = new CancellationTokenSource();
            HPChange(CurrentCTS).Forget();
        }

        //血條演出內容
        async UniTaskVoid HPChange(CancellationTokenSource ctk) {
            //接入實際扣減的數值 換算血量比例
            float BarStartVal = HeroDisplayHPRate;
            float BarFinalVal = (float)HeroCurHP / HeroMaxHP;
            Debug.LogFormat("血量準備變化. 起始值:{0} 最終值:{1}", BarStartVal, BarFinalVal);
            if (BarStartVal == BarFinalVal)
                return;
            Debug.Log("------開始演出血量-------");
            bool isReduce = BarStartVal >= BarFinalVal;
            CurrentHPRate = BarFinalVal;
            //血條(彩色)設定為血量變化起始長度 & 設置變化血條起始位置
            SetHPBar(HeroDisplayHPRate);
            //血條(白色)設定為血量變化起始長度
            HPBarGray.fillAmount = BarStartVal;
            //血條(黑白)設定為血量變化起始長度
            HPBarWhite.fillAmount = BarStartVal;
            //顯示變化血條
            ShowHPBarChange(true);

            //還原設定
            HeartBeatIconTrans.localEulerAngles = Vector3.zero; //先歸0以免連續觸發演出導致角度不正常
            HeartBeatGrayIconTrans.localEulerAngles = Vector3.zero; //先歸0以免連續觸發演出導致角度不正常
            HeartBeatGrayIcon.color = HideColor; //還原顏色設定以免多次演出導致顏色異常
            HPBarWhite.color = HideColor;//還原顏色設定以免多次演出導致顏色異常

            //防呆 避免設定錯誤導致遊戲炸掉
            if (BarChangeSecNeed <= 0)
                BarChangeSecNeed = 1f;

            //等待血條停滯時間
            await UniTask.WaitForSeconds(BarChangeSecDelay, cancellationToken: ctk.Token);
            //算出每禎變化值
            float delta = math.abs(BarStartVal - BarFinalVal) / BarChangeSecNeed / BarChangeFrame;
            float duration = 1f / BarChangeFrame;

            //受擊演出 心跳變黑白旋轉 血條全白 顯示變化血條(直接接在血條末端)
            //旋轉演出
            Tweener HeartBeatRotate = HeartBeatIconTrans.DORotate(HittedRotateAngle, HittedRotateDuration / 2, RotateMode.Fast);
            HeartBeatRotate.SetAutoKill(true);
            HeartBeatRotate.Pause();
            HeartBeatRotate.SetEase(Ease.OutSine);
            HeartBeatRotate.OnComplete(DOHeartBeatRotateBack);
            HeartBeatRotate.Restart();

            Tweener HeartBeatGrayRotate = HeartBeatGrayIconTrans.DORotate(HittedRotateAngle, HittedRotateDuration / 2, RotateMode.Fast);
            HeartBeatGrayRotate.SetAutoKill(true);
            HeartBeatGrayRotate.Pause();
            HeartBeatGrayRotate.SetEase(Ease.OutSine);
            HeartBeatGrayRotate.OnComplete(DOHeartBeatGrayGotateBack);
            HeartBeatGrayRotate.Restart();

            //心臟變色演出
            Tweener HeartBeatGrayCutIn = HeartBeatGrayIcon.DOColor(Color.white, HittedColorDuration / 2);
            HeartBeatGrayCutIn.SetAutoKill(true);
            HeartBeatGrayCutIn.Pause();
            HeartBeatGrayCutIn.SetEase(Ease.Linear);
            HeartBeatGrayCutIn.OnComplete(DoHeartBeatGrayHide);
            HeartBeatGrayCutIn.Restart();

            //血條變色(白色)演出 >> 原本是全透明 快速淡入又淡出
            Tweener HPBarShowWhite = HPBarWhite.DOColor(Color.white, HittedHPBarWhiteDuration / 2);
            HPBarShowWhite.SetAutoKill(true);
            HPBarShowWhite.Pause();
            HPBarShowWhite.SetEase(Ease.Linear);
            HPBarShowWhite.OnComplete(HideHPBarWhite);
            HPBarShowWhite.Restart();

            //等待血條變色演出結束
            await UniTask.WaitForSeconds(HittedHPBarWhiteDuration, cancellationToken: ctk.Token);

            Debug.Log("------開始漸變------");
            Debug.Log("每次變化量: " + delta + " 每次變化所需秒數: " + duration);
            //Debug.Log("current Val: " + HeroDisplayHPRate +  " Final Val: " + BarFinalVal);
            //血量變化 >> 變化血條長度固定並隨著血條末端位移 要有殘影滯留逐步縮退 彩色血條先 底下一條灰階的血條快速跟隨縮退
            //黑白血條演出 另外開一個UniTask去跑
            float grayDelta = math.abs(BarStartVal - BarFinalVal) / BarGrayChangeSecNeed / BarChangeFrame;//黑白血條演出所需每次變化量
            float grayDuration = 1f / BarChangeFrame;//黑白血條演出每次間隔時間
                                                     //紀錄上次殘影出現血量百分比
            float lastAfterImageHPRate = 0f;
            HPGrayChange(CurrentCTS, isReduce, HeroDisplayHPRate, BarFinalVal, grayDuration, grayDelta).Forget();
            if (isReduce) {
                while (HeroDisplayHPRate >= BarFinalVal) {
                    await UniTask.WaitForSeconds(duration, cancellationToken: ctk.Token);
                    HeroDisplayHPRate -= delta;
                    //目前血條(彩色)開始變化 & 變化血條位移
                    SetHPBar(HeroDisplayHPRate);
                    //製造殘影(比對上一次產生殘影的血量 高於等於設定值就產生殘影)
                    if (lastAfterImageHPRate == 0f || (Math.Abs(lastAfterImageHPRate - HeroDisplayHPRate) >= HPRateGenerateAfterImage)) {
                        lastAfterImageHPRate = HeroDisplayHPRate;
                        GenerateHPBarChangeAfterImage();
                    }
                    //Debug.LogFormat("數值減少 目前百分比值: {0}.", HeroDisplayHPRate);
                }
            } else {
                while (HeroDisplayHPRate <= BarFinalVal) {
                    await UniTask.WaitForSeconds(duration, cancellationToken: ctk.Token);
                    HeroDisplayHPRate += delta;
                    //目前血條(彩色)開始變化 & 變化血條位移
                    SetHPBar(HeroDisplayHPRate);
                    //製造殘影(比對上一次產生殘影的血量 高於等於設定值就產生殘影)
                    if (lastAfterImageHPRate == 0f || (Math.Abs(lastAfterImageHPRate - HeroDisplayHPRate) >= HPRateGenerateAfterImage)) {
                        lastAfterImageHPRate = HeroDisplayHPRate;
                        GenerateHPBarChangeAfterImage();
                    }
                    //Debug.LogFormat("數值增加 目前百分比值: {0}.", HeroDisplayHPRate);
                }
            }
            await UniTask.Yield(ctk.Token);
            //血量變化更改心跳速度
            SetHeartBeatRate();
            //目前血條(彩色)設定至定量避免計算有偏差 & 變化血條設定至定量避免計算有偏差 >> 經過實測不加這行會縮退太多 因為上面計算一定不會剛好停止
            SetHPBar(BarFinalVal);
        }

        /// <summary>
        /// 血條(黑白)變化演出
        /// </summary>
        /// <param name="ctk">取消token</param>
        /// <param name="isReduce">是否血量減少</param>
        /// <param name="currentVal">目前血量數值 為0~1</param>
        /// <param name="changeFinalVal">最終血量數值 為0~1</param>
        /// <param name="duration">每次變化所需時間</param>
        /// <param name="delta">每次血條變化量</param>
        /// <returns></returns>
        async UniTaskVoid HPGrayChange(CancellationTokenSource ctk, bool isReduce, float currentVal, float changeFinalVal,
            float duration, float delta) {
            //等待設置延遲的秒數
            await UniTask.WaitForSeconds(BarGrayChangeSecDelay, cancellationToken: ctk.Token);
            //以設置的演出時間開始變化血量
            if (isReduce) {
                while (currentVal >= changeFinalVal) {
                    await UniTask.WaitForSeconds(duration, cancellationToken: ctk.Token);
                    currentVal -= delta;
                    //目前血條(黑白)開始變化
                    HPBarGray.fillAmount = currentVal;
                    //Debug.Log("黑白數值減少 目前百分比值: " + currentVal);
                }
            } else {
                while (currentVal <= changeFinalVal) {
                    await UniTask.WaitForSeconds(duration, cancellationToken: ctk.Token);
                    currentVal += delta;
                    //目前血條(黑白)開始變化
                    HPBarGray.fillAmount = currentVal;
                    //Debug.Log("黑白數值增加 目前百分比值: " + currentVal);
                }
            }
            await UniTask.Yield(ctk.Token);
            //目前血條(黑白)設定至定量避免計算有偏差
            HPBarGray.fillAmount = changeFinalVal;
        }

        /// <summary>
        /// 設定血條
        /// </summary>
        /// <param name="percent">血量百分比</param>
        void SetHPBar(float percent) {
            HPBar.fillAmount = percent; //設定目前血條(彩色)
            SetHPBarChangePos(percent); //變化血條位移
        }

        //產生變化血條殘影
        void GenerateHPBarChangeAfterImage() {
            //複製Image 設置位置
            GameObject CloneChangeHPBar = Instantiate(HPChangeBar.gameObject, CloneHPChangerBarTrans);
            //淡出並銷毀
            Tweener CutOutTween = CloneChangeHPBar.GetComponent<Image>().DOColor(HideColor, AfterImageDuration);
            CutOutTween.SetAutoKill(true);
            CutOutTween.Pause();
            CutOutTween.SetEase(Ease.Linear);
            CutOutTween.OnComplete(() => { Destroy(CloneChangeHPBar); });
            CutOutTween.Restart();
        }

        //設定血條變化量位置 作為血條變化演出 傳入值為0~1
        void SetHPBarChangePos(float percent) {
            //0.94~1X位移變化量約為1.6666667 因為圖案不是規則矩形導致斜率不是單一
            //公式為血量FillAmount為1時的所在位置(380)減去0.94時所在位置(370)除於區間差值(100-94)下去轉換變化量 所在位置(ChangeBar的PosX)是手動調整所得
            //float FirstChangeRate = (380 - 370) / 6f; //0.94~1的斜率變化量 
            //0.16~0.93的X位移變化量約為3.935 因為圖案不是規則矩形導致斜率不是單一(直到0都採此變化量 小於0.16血條已被心跳蓋住看不見)
            //公式為血量FillAmount為0.93時的所在位置(367)減去0.16時所在位置(64)除於區間差值(93-16)下去轉換變化量 所在位置(ChangeBar的PosX)是手動調整所得
            //float SecondChangeRate = (367 - 64) / 77f; //0~0.93的斜率變化量
            bool UseRate1 = percent >= 0.94f;
            float PercentStart = UseRate1 ? 1f : 0.93f;
            float XPosStart = UseRate1 ? 380f : 367f;
            float XPosEnd = UseRate1 ? 370f : 64f;
            float RateDenominator = UseRate1 ? 6f : 77f;
            float RealRate = (XPosStart - XPosEnd) / RateDenominator;
            float RealPosX = XPosStart - (PercentStart - percent) * RealRate * 100f;
            ChangeBarRect.anchoredPosition3D = new Vector3(RealPosX, ChangeBarOriginPos.y, ChangeBarOriginPos.z);
            //Debug.Log("Real Pos X: " + RealPosX);
        }

        void ShowHPBarChange(bool bShow) {
            HPChangeBar.color = bShow ? Color.white : HideColor;
        }

        void SetHeartBeatParameter() {
            //不為null就停止並殺掉原本的Tween
            if (HeartBeatScaleTween1 != null) {
                HeartBeatScaleTween1.Pause();
                HeartBeatScaleTween1.Kill();
            }
            if (HeartBeatScaleTween2 != null) {
                HeartBeatScaleTween2.Pause();
                HeartBeatScaleTween2.Kill();
            }
            if (HeartBeatGrayScaleTween1 != null) {
                HeartBeatGrayScaleTween1.Pause();
                HeartBeatGrayScaleTween1.Kill();
            }
            if (HeartBeatGrayScaleTween2 != null) {
                HeartBeatGrayScaleTween2.Pause();
                HeartBeatGrayScaleTween2.Kill();
            }
            //重置Scale 不然多次調整後初始Scale可能會錯誤
            HeartBeatIconTrans.localScale = HeartBeatReverse ? Vector3.left + Vector3.up + Vector3.forward : Vector3.one;
            HeartBeatGrayIconTrans.localScale = HeartBeatReverse ? Vector3.left + Vector3.up + Vector3.forward : Vector3.one;
            //新建立Tween
            HeartBeatScaleTween1 = HeartBeatIconTrans.DOScale(new Vector3(HeartBeatReverse ? -HeartBeatTweenScale : HeartBeatTweenScale, HeartBeatTweenScale, 1f), HeartBeatDuration / 2);
            HeartBeatScaleTween1.SetAutoKill(false);
            HeartBeatScaleTween1.Pause();
            HeartBeatScaleTween1.SetDelay(HeartBeatDelayMax);
            HeartBeatScaleTween1.SetEase(Ease.OutSine);
            HeartBeatScaleTween1.OnComplete(HeartBeatBack);//OnComplete弄成循環

            HeartBeatScaleTween2 = HeartBeatIconTrans.DOScale(HeartBeatReverse ? Vector3.left + Vector3.up + Vector3.forward : Vector3.one, HeartBeatDuration / 2);
            HeartBeatScaleTween2.SetAutoKill(false);
            HeartBeatScaleTween2.Pause();
            HeartBeatScaleTween2.SetEase(Ease.OutSine);
            HeartBeatScaleTween2.OnComplete(HeartBeat);//OnComplete弄成循環

            HeartBeatGrayScaleTween1 = HeartBeatGrayIconTrans.DOScale(new Vector3(HeartBeatReverse ? -HeartBeatTweenScale : HeartBeatTweenScale, HeartBeatTweenScale, 1f), HeartBeatDuration / 2);
            HeartBeatGrayScaleTween1.SetAutoKill(false);
            HeartBeatGrayScaleTween1.Pause();
            HeartBeatGrayScaleTween1.SetDelay(HeartBeatDelayMax);
            HeartBeatGrayScaleTween1.SetEase(Ease.OutSine);
            HeartBeatGrayScaleTween1.OnComplete(HeartBeatGrayBack);//OnComplete弄成循環

            HeartBeatGrayScaleTween2 = HeartBeatGrayIconTrans.DOScale(HeartBeatReverse ? Vector3.left + Vector3.up + Vector3.forward : Vector3.one, HeartBeatDuration / 2);
            HeartBeatGrayScaleTween2.SetAutoKill(false);
            HeartBeatGrayScaleTween2.Pause();
            HeartBeatGrayScaleTween2.SetEase(Ease.OutSine);
            HeartBeatGrayScaleTween2.OnComplete(HeartGrayBeat);//OnComplete弄成循環
                                                               //建立完後根據血量決定演出的速度(TimeScale)
            SetHeartBeatRate();
            //開始演出
            HeartBeatScaleTween1.PlayForward();
            HeartBeatGrayScaleTween1.PlayForward();
        }

        //更新心跳率
        void SetHeartBeatRate() {
            //根據目前血量換算心跳速度應該多快
            float HeartBeatChangeRate = (HeartBeatDelayMax - HeartBeatDelayMin) / 100f; //每百分比加快的秒數
            float CurrentHPDelay = HeartBeatDelayMax - HeartBeatChangeRate * ((1f - CurrentHPRate) * 100f); //換算現在血量對應的delay時間
            if (HeartBeatScaleTween1 != null) {
                HeartBeatScaleTween1.timeScale = 1f / (CurrentHPDelay / HeartBeatDelayMax); //換算TimeScale應該加快多少
                Debug.Log("轉換心跳演出加速倍率: " + HeartBeatScaleTween1.timeScale);
            }
            if (HeartBeatScaleTween2 != null)
                HeartBeatScaleTween2.timeScale = 1f / (CurrentHPDelay / HeartBeatDelayMax);
            if (HeartBeatGrayScaleTween1 != null)
                HeartBeatGrayScaleTween1.timeScale = 1f / (CurrentHPDelay / HeartBeatDelayMax);
            if (HeartBeatGrayScaleTween2 != null)
                HeartBeatGrayScaleTween2.timeScale = 1f / (CurrentHPDelay / HeartBeatDelayMax);
        }

        //心跳跳回去(縮小回原尺寸)
        void HeartBeatBack() {
            HeartBeatScaleTween2.Restart();
            //Debug.Log("心跳跳回去");
        }

        //心跳跳動(放大尺寸)
        void HeartBeat() {
            HeartBeatScaleTween1.Restart();
            //Debug.Log("重新心跳");
        }

        //心跳跳回去(黑白圖 縮小回原尺寸)
        void HeartBeatGrayBack() {
            HeartBeatGrayScaleTween2.Restart();
            //Debug.Log("心跳跳回去");
        }

        //心跳跳動(黑白圖 放大尺寸)
        void HeartGrayBeat() {
            HeartBeatGrayScaleTween1.Restart();
            //Debug.Log("重新心跳");
        }

        //心臟受擊旋轉回原位置
        void DOHeartBeatRotateBack() {
            Tweener HeartBeatRotateBack = HeartBeatIconTrans.DORotate(Vector3.zero, HittedRotateDuration / 2, RotateMode.Fast);
            HeartBeatRotateBack.SetAutoKill(true);
            HeartBeatRotateBack.Pause();
            HeartBeatRotateBack.SetEase(Ease.OutSine);
            HeartBeatRotateBack.Restart();
        }

        //心臟受擊(黑白)旋轉回原位置
        void DOHeartBeatGrayGotateBack() {
            Tweener HeartBeatRotateBack = HeartBeatGrayIconTrans.DORotate(Vector3.zero, HittedRotateDuration / 2, RotateMode.Fast);
            HeartBeatRotateBack.SetAutoKill(true);
            HeartBeatRotateBack.Pause();
            HeartBeatRotateBack.SetEase(Ease.OutSine);
            HeartBeatRotateBack.Restart();
        }

        //心臟受擊(黑白)隱藏
        void DoHeartBeatGrayHide() {
            Tweener HeartBeatGray = HeartBeatGrayIcon.DOColor(HideColor, HittedColorDuration / 2);
            HeartBeatGray.SetAutoKill(true);
            HeartBeatGray.Pause();
            HeartBeatGray.SetEase(Ease.Linear);
            HeartBeatGray.Restart();
        }

        //白色血條隱藏
        void HideHPBarWhite() {
            Tweener HideTween = HPBarWhite.DOColor(HideColor, HittedHPBarWhiteDuration / 2);
            HideTween.SetAutoKill(true);
            HideTween.Pause();
            HideTween.SetEase(Ease.Linear);
            HideTween.Restart();
        }

        /// <summary>
        /// 更新Buffer
        /// </summary>
        /// <param name="effectTypes">Buffer列表</param>
        public void SetBufferIcon(List<BufferIconData> effectTypes) {
            int oldDataIndex = -1;
            for (int i = 0; i < effectTypes.Count; i++) {
                //WriteLog.LogErrorFormat("type cnt: {0} effect cnt: {1} cur index: {2} Effect: {3}", 
                //    effectTypes.Count, BufferIconObjList.Count, i, effectTypes[i]);
                //比對新舊資料
                effectTypes[i].NeedUpdate = true;
                oldDataIndex = BufferDatas.FindIndex(x => x.Name.Equals(effectTypes[i].Name));
                if (oldDataIndex >= 0) {
                    //舊資料已經有就更新資料
                    BufferDatas[oldDataIndex] = effectTypes[i];
                } else {
                    //舊資料沒有則代表是新增資料
                    BufferDatas.Add(effectTypes[i]);
                }
            }
            for (int i = BufferDatas.Count - 1; i >= 0; i--) {
                //遍歷資料集合 不需要更新者表示需要移除
                if (!BufferDatas[i].NeedUpdate)
                    BufferDatas.Remove(BufferDatas[i]);
            }
            for (int i = 0; i < BufferDatas.Count; i++) {
                //將資料賦予給物件
                BattleBufferIcon _obj;
                if (i >= BufferIconObjList.Count) {
                    _obj = Instantiate(BufferIcon);
                    _obj.name = string.Format("BufferIcon" + i);
                    _obj.transform.SetParent(BuffIconGridTrans);
                    _obj.transform.localPosition = Vector3.zero;
                    _obj.transform.localScale = Vector3.one;
                    BufferIconObjList.Add(_obj);
                } else {
                    _obj = BufferIconObjList[i];
                }
                BufferDatas[i].NeedUpdate = false;
                _obj.SetEffect(BufferDatas[i]);
                _obj.gameObject.SetActive(true);
            }
            //WriteLog.LogErrorFormat("effect type cnt: {0}. buff data cnt: {1} icon list cnt: {2}", effectTypes.Count
            // , BufferDatas.Count, BufferIconObjList.Count);
            //其他沒顯示的要關閉
            for (int i = effectTypes.Count; i < BufferIconObjList.Count; i++) {
                //WriteLog.LogErrorFormat("close icon index: {0}", i);
                BufferIconObjList[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 判斷英雄血量是否歸0
        /// </summary>
        /// <returns></returns>
        public bool HeroIsDead() {
            return HeroCurHP <= 0;
        }

        public void ResetHPBarToFull() {
            SetHPBar(100f);
        }

    }
}
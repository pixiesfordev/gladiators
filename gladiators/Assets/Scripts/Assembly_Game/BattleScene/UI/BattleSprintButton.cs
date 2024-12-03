using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Gladiators.Battle;
using System.Threading;
using Cysharp.Threading.Tasks;

public class BattleSprintButton : MonoBehaviour {
    //兩種操作模式 做兩種用意是為了測試手感哪種比較好 決定用哪種之後再確定介面要怎麼演出
    //1.長壓模式(就是傳統的Press)
    //2.點擊切換On/off

    [SerializeField] Text StateText;
    [SerializeField] Animator ButtonAnimator;
    [SerializeField] Button Btn;

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("打勾為長壓模式 否則為ON/OFF模式 ON/OFF模式操作方式為點一下ON再點一下OFF")][SerializeField] bool PressMode = true;

    bool IsOn = false; //On/Off模式 true為On false為Off 預設為off

    float MinimumSprintVigorNeed = 0f; //衝刺最少需要體力 目前設定是只要衝刺就停止恢復體力 所以相當於0

    bool VigorEnoughCurState = true; //目前體力是否足夠衝刺
    bool VigorEnoughCheck = false; //檢查體力是否足夠衝刺

    /*
    0.Idld >> 預設要改這個 不然會一直抖動
    1.rush_NO >> 體力足夠衝刺時播放(就是打亮放大又縮小一下)
    2.rush_OFF >> 體力不夠衝刺時播放(就是打暗縮小一下)
    3.rush_shock >> 衝刺啟用中
    4.rush_start >> 開始衝刺(放大)
    5.rush_start反著放 >> 取消衝刺(縮小) >> 這個藥用
    */

    void Start() {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            PressMode = true;
        }
    }

    /// <summary>
    /// 檢查體力條狀態
    /// </summary>
    /// <param name="vigor"></param>
    public void CheckVigor(float vigor) {
        //檢查體力足夠衝刺狀態是否有改變 有改變才演出 不然其他演出會無法出現 因為CheckVigor一直都在跑
        VigorEnoughCheck = vigor > MinimumSprintVigorNeed;
        if (VigorEnoughCurState != VigorEnoughCheck) {
            VigorEnoughCurState = VigorEnoughCheck;
            //足夠衝刺則播放可以衝刺動畫 否則播放不能衝刺動畫
            if (!VigorEnoughCurState) {
                ButtonAnimator.Play("rush_OFF");
            } else {
                ButtonAnimator.Play("rush_NO");
            }
        }
    }

    public void PressMode_Run() {
        if (PressMode) {
            Run();
            HideText();
            ButtonAnimator.Play("rush_shock");
        }
    }

    public void PressMode_Stop() {
        if (PressMode) {
            StopRun();
            HideText();
            ButtonAnimator.SetFloat("speed", -1.0f);
            ButtonAnimator.Play("rush_start");
        }
    }

    public void OnOffMode_Click() {
        if (!PressMode) {
            IsOn = !IsOn;
            if (IsOn)
                OnOffMode_Run();
            else
                OnOffMode_Stop();
        }
    }

    void OnOffMode_Run() {
        if (!PressMode) {
            Run();
            //OnText();
            ButtonAnimator.SetFloat("speed", 1.0f);
            ButtonAnimator.Play("rush_start", 0, 0f);
        }
    }

    void OnOffMode_Stop() {
        if (!PressMode) {
            StopRun();
            //OffText();
            ButtonAnimator.SetFloat("speed", -1.0f);
            ButtonAnimator.Play("rush_start", 0, 1f);
        }
    }

    public void ClickToShock() {
        if (!PressMode && IsOn) {
            ButtonAnimator.Play("rush_shock");
        }
    }

    void OnText() {
        StateText.text = "衝刺中";
    }

    void OffText() {
        StateText.text = "行走中";
    }

    void HideText() {
        StateText.text = "";
    }

    void Run() {
        BattleManager.Instance.GoRun(true);
        Debug.LogWarning("衝刺中~~~");
    }

    void StopRun() {
        BattleManager.Instance.GoRun(false);
        Debug.LogWarning("停止衝刺~~~~");
    }
}

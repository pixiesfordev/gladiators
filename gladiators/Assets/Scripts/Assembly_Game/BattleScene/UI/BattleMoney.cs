using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace Gladiators.Battle {
    public class BattleMoney : MonoBehaviour {
        [SerializeField] HorizontalLayoutGroup BgController;
        [SerializeField] Text MoneyText;


        [HeaderAttribute("==============TEST==============")]
        [Tooltip("獲得金幣演出測試")][SerializeField] bool TestAddMoney;
        [Tooltip("獲得金幣演出曲線")][SerializeField] AnimationCurve AddMoneyCurve;
        [Tooltip("獲得金幣演出時間")][SerializeField] float AddMoneyAniDur;
        [Tooltip("獲得金幣演出文字跳動幅度(Y軸移動幅度)")][SerializeField] float AddMoneyTextJumDistance = 10f;

        Color YellowColor = new Color(1f, 0.937f, 0.157f);
        CancellationTokenSource AddMoneyCTK;

        void Start() {
            AddMoneyCTK = new CancellationTokenSource();
        }

        void Update() {
            if (TestAddMoney) {
                TestAddMoney = false;
                StopAddMoney();
                DoMoneyAnimator().Forget();
            }
        }

        void StopAddMoney() {
            if (AddMoneyCTK != null) {
                AddMoneyCTK.Cancel();
                AddMoneyCTK = new CancellationTokenSource();
            }
        }

        public void AddMoney(int money) {
            if (int.TryParse(MoneyText.text, out int curVal)) {
                StopAddMoney();
                curVal += money;
                MoneyText.text = curVal.ToString();
                DoMoneyAnimator().Forget();
            }
        }

        async UniTask DoMoneyAnimator() {
            float passTime = 0f;
            Vector3 originPos = MoneyText.transform.localPosition;
            Vector3 tempPos = originPos;
            float curPosY = originPos.y;
            float curveVal = 0f;
            BgController.enabled = false;
            MoneyText.color = YellowColor;
            while (passTime < AddMoneyAniDur) {
                passTime += Time.deltaTime;
                curveVal = AddMoneyCurve.Evaluate(passTime / AddMoneyAniDur);
                curPosY = Mathf.Lerp(originPos.y, originPos.y + AddMoneyTextJumDistance, curveVal);
                tempPos.y = curPosY;
                MoneyText.transform.localPosition = tempPos;
                await UniTask.Yield(AddMoneyCTK.Token);
            }
            MoneyText.transform.localPosition = originPos;
            MoneyText.color = Color.white;
            BgController.enabled = true;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Gladiators.Main;
using Scoz.Func;
using UnityEngine;
using UnityEngine.UI;
namespace Gladiators.Battle {
    public class TestShader : MonoBehaviour {
        [SerializeField] Material TestMaterial;
        [SerializeField] Image TestImage1;
        [SerializeField] Image TestImage2;
        [SerializeField] Image TestImage3;

        [HeaderAttribute("==============TEST==============")]
        [SerializeField] Texture2D _MainTex;
        [SerializeField] Color _Color;
        [SerializeField] float _exposure;
        [SerializeField] float _saturation;
        [SerializeField] Texture2D _Mask01;
        [SerializeField] bool GiveMaterial;
        [SerializeField] bool UpdateTex;
        [SerializeField] bool UpdateColor;
        [SerializeField] bool UpdateExposure;
        [SerializeField] bool UpdateSaturation;
        [SerializeField] bool UpdateMask;
        [SerializeField] bool UpdateImage1;
        [SerializeField] bool UpdateImage2;
        [SerializeField] bool UpdateImage3;

        [SerializeField] float StartVal;
        [SerializeField] float EndtVal;
        [SerializeField] float CountTime;
        [SerializeField] bool TestLerp;

        [SerializeField] float FirstVal;
        [SerializeField] float SecondVal;
        [SerializeField] float ThirdVal;
        [SerializeField] float Time1;
        [SerializeField] float Time2;
        [SerializeField] bool TestMultiUniTask;
        [SerializeField] bool TestStopTask;

        [SerializeField] bool TestUniTaskTime;

        [SerializeField] bool TestGetSpriteOfSpellIcon;
        [SerializeField] string TestSpellIconRef;

        Material _SkillMaterial1;
        Material _SkillMaterial2;
        Material _SkillMaterial3;

        CancellationTokenSource CTS;
        CancellationToken CT;

        // Start is called before the first frame update
        void Start() {
            if (TestMaterial != null) {
                _SkillMaterial1 = Instantiate(TestMaterial);
                _SkillMaterial2 = Instantiate(TestMaterial);
                _SkillMaterial3 = Instantiate(TestMaterial);
            }
            CTS = new CancellationTokenSource();
            CT = CTS.Token;
        }

        // Update is called once per frame
        void Update() {
            if (UpdateImage1) {
                UpdateImage1 = false;
                UpdateTargetImage(TestImage1, _SkillMaterial1);
            }
            if (UpdateImage2) {
                UpdateImage2 = false;
                UpdateTargetImage(TestImage2, _SkillMaterial2);
            }
            if (UpdateImage3) {
                UpdateImage3 = false;
                UpdateTargetImage(TestImage3, _SkillMaterial3);
            }
            if (TestLerp) {
                TestLerp = false;
                TryLerp().Forget();
            }
            if (TestMultiUniTask) {
                TestMultiUniTask = false;
                TryWaitTask().Forget();
            }
            if (TestStopTask) {
                TestStopTask = false;
                StopTask();
            }
            if (TestUniTaskTime) {
                TestUniTaskTime = false;
                DoTestUniTaskTime().Forget();
            }
            if (TestGetSpriteOfSpellIcon) {
                TestGetSpriteOfSpellIcon = false;
                SetTextureByRef();
            }
        }

        async UniTaskVoid TryLerp() {
            float resultVal = 0f;
            float passTime = 0f;
            while (passTime <= CountTime) {
                passTime += Time.deltaTime;
                resultVal = Mathf.Lerp(StartVal, EndtVal, passTime / CountTime);
                Debug.LogFormat("Pass time: {0} CurVal: {1}", passTime, resultVal);
                await UniTask.Yield(CT);
            }
        }

        async UniTaskVoid TryWaitTask() {
            Debug.LogWarningFormat("Before do first: {0}", Time.time);
            await FirstWaitTask();
            Debug.LogWarningFormat("Before do second: {0}", Time.time);
            await SecondWaitTask();
            Debug.LogWarningFormat("All done: {0}", Time.time);
        }

        async UniTask FirstWaitTask() {
            float resultVal = 0f;
            float passTime = 0f;
            while (passTime <= Time1) {
                passTime += Time.deltaTime;
                resultVal = Mathf.Lerp(FirstVal, SecondVal, passTime / Time1);
                Debug.LogFormat("First wait Pass time: {0} CurVal: {1}", passTime, resultVal);
                await UniTask.Yield(CT);
            }
        }

        async UniTask SecondWaitTask() {
            float resultVal = 0f;
            float passTime = 0f;
            while (passTime <= Time2) {
                passTime += Time.deltaTime;
                resultVal = Mathf.Lerp(SecondVal, ThirdVal, passTime / Time2);
                Debug.LogFormat("Second wait Pass time: {0} CurVal: {1}", passTime, resultVal);
                await UniTask.Yield(CT);
            }
        }

        void StopTask() {
            //測試停止UniTask.Yield()
            if (CT != null) {
                CTS.Cancel();
                CTS = new CancellationTokenSource();
                CT = CTS.Token;
            }
        }

        void UpdateTargetImage(Image _target, Material _material) {
            if (GiveMaterial) {
                if (UpdateTex) _material.SetTexture("_main_Tex", _MainTex);
                if (UpdateColor) _material.SetColor("_Color", _Color);
                if (UpdateExposure) _material.SetFloat("_exposure", _exposure);
                if (UpdateSaturation) _material.SetFloat("_color_saturation", _saturation);
                if (UpdateMask) _material.SetTexture("_main_mask01", _Mask01);
                _target.material = _material;
            } else {
                _target.material = null;
            }
        }

        async UniTask DoTestUniTaskTime() {
            WriteLog.LogError("測試UniTask是否受ScaleTime影響");
            float totalProgress = 5f;
            float progress = 0f;
            Time.timeScale = 0f;
            while (progress < totalProgress) {
                progress += 0.05f;
                await UniTask.Yield();
                WriteLog.LogErrorFormat("目前進度: {0}", progress);
            }
        }

        void SetTextureByRef() {
            if (!string.IsNullOrEmpty(TestSpellIconRef)) {
                //設定SkillIcon
                AssetGet.GetSpriteFromAtlas("SpellIcon", TestSpellIconRef, (sprite) => {
                    TestImage1.gameObject.SetActive(true);
                    if (sprite != null) {
                        // Get the texture of the atlas(此行只能取到Atlas 無法正確設定圖片)
                        Texture2D texture = sprite.texture;
                        // Get the texture of the sprite in the atlas(一定要重新建立一個Texture 不然沒辦法正確取到我們要的圖...)
                        Texture2D spriteTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
                        spriteTexture.SetPixels(texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height));
                        spriteTexture.Apply();
                        _SkillMaterial1.SetTexture("_main_Tex", spriteTexture);
                        //WriteLog.LogWarningFormat("設定圖片! 技能物件: {0} 是否為初始化: {1} 能量是否足夠: {2}", name, _init, IsEnergyEnough);
                    } else
                        AssetGet.GetSpriteFromAtlas("SpellIcon", "sprint", (sprite) => {
                            // Get the texture of the atlas(此行只能取到Atlas 無法正確設定圖片)
                            Texture2D texture = sprite.texture;
                            // Get the texture of the sprite in the atlas(一定要重新建立一個Texture 不然沒辦法正確取到我們要的圖...)
                            Texture2D spriteTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
                            spriteTexture.SetPixels(texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height));
                            spriteTexture.Apply();
                            _SkillMaterial1.SetTexture("_main_Tex", sprite.texture);
                            //WriteLog.LogWarningFormat("圖片缺少! 用衝刺圖代替顯示! ID: {0}", SkillData.Ref);
                        });
                });
            } else {
                TestImage1.gameObject.SetActive(false);
                Debug.LogWarning("無法設定圖片 技能資料可能為空或Ref為空值!");
            }
        }
    }
}
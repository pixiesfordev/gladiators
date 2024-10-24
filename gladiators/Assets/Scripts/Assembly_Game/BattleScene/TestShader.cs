using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.UI;

public class TestShader : MonoBehaviour
{
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

    Material _SkillMaterial1;
    Material _SkillMaterial2;
    Material _SkillMaterial3;

    // Start is called before the first frame update
    void Start()
    {
        _SkillMaterial1 = Instantiate(TestMaterial);
        _SkillMaterial2 = Instantiate(TestMaterial);
        _SkillMaterial3 = Instantiate(TestMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateImage1)
        {
            UpdateImage1 = false;
            UpdateTargetImage(TestImage1, _SkillMaterial1);
        }
        if (UpdateImage2)
        {
            UpdateImage2 = false;
            UpdateTargetImage(TestImage2, _SkillMaterial2);
        }
        if (UpdateImage3)
        {
            UpdateImage3 = false;
            UpdateTargetImage(TestImage3, _SkillMaterial3);
        }
    }

    void UpdateTargetImage(Image _target, Material _material)
    {
        if (GiveMaterial)
        {
            if (UpdateTex) _material.SetTexture("_main_Tex", _MainTex);
            if (UpdateColor) _material.SetColor("_Color", _Color);
            if (UpdateExposure) _material.SetFloat("_exposure", _exposure);
            if (UpdateSaturation) _material.SetFloat("_color_saturation", _saturation);
            if (UpdateMask) _material.SetTexture("_main_mask01", _Mask01);
            _target.material = _material;
        }
        else
        {
            _target.material = null;
        }
    }
}

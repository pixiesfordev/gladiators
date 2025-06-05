using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainHuntHeroWeaponParameter
{
    /// <summary>
    /// 英雄武器隨機偏移量(插中Boss身體位置)
    /// </summary>
    public Vector3 HeroWeaponOffset { get; private set; }
    /// <summary>
    /// Boss受擊Spine中心位置
    /// </summary>
    public Vector3 BossHittedSpinePos { get; private set; }

    public TrainHuntHeroWeaponParameter()
    {
        int rand = Random.Range(0, 8);
        switch (rand)
        {
            case 1:
                HeroWeaponOffset = new Vector3(-60f, 128f, 0f);
                BossHittedSpinePos = new Vector3(1.66f, 4.38f, 0f);
                break;
            case 2:
                HeroWeaponOffset = new Vector3(-16f, 16f, 0f);
                BossHittedSpinePos = new Vector3(2.59f, 3.17f, 0f);
                break;
            case 3:
                HeroWeaponOffset = new Vector3(27f, -7f, 0f);
                BossHittedSpinePos = new Vector3(2.69f, 2.42f, 0f);
                break;
            case 4:
                HeroWeaponOffset = new Vector3(91f, 68f, 0f);
                BossHittedSpinePos = new Vector3(2.44f, 1.39f, 0f);
                break;
            case 5:
                HeroWeaponOffset = new Vector3(163f, -60f, 0f);
                BossHittedSpinePos = new Vector3(1.83f, 0.78f, 0f);
                break;
            case 6:
                HeroWeaponOffset = new Vector3(76f, -80f, 0f);
                BossHittedSpinePos = new Vector3(2.6f, 1.52f, 0f);
                break;
            default:
                HeroWeaponOffset = Vector3.zero;
                BossHittedSpinePos = new Vector3(2.65f, 2.91f, 0f);
                break;
        }
    }

}

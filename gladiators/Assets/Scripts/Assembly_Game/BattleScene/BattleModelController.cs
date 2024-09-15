using Gladiators.Battle;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class BattleModelController : MonoBehaviour {
    [SerializeField] GameObject terrainPrefab;
    [SerializeField] Character characterPrefab;

    [SerializeField] GameObject terrainArea;
    [SerializeField] GameObject charactersArea;

    Character leftChar = null;
    Character rightChar = null;

    [SerializeField] bool BattleIsEnd = false;

    public const float WALLPOS = 20f;// 牆壁位置
    public const float KnockAngleRange = 30;// 擊退最大水平角度
    Dictionary<string, Character> CharDic;

    public static BattleModelController Instance { get; private set; }

    public void Init() {
        Instance = this;
    }

    private void Update() {
        if (leftChar != null) leftChar.Move(curKnockAngle);
        if (rightChar != null) rightChar.Move(curKnockAngle);
    }

    public void CreateCharacter(PackPlayer _myPlayerPack, PackPlayer _opponentPack) {
        leftChar = Instantiate(characterPrefab, charactersArea.transform);
        rightChar = Instantiate(characterPrefab, charactersArea.transform);

        leftChar.name = _myPlayerPack.DBID;
        leftChar.tag = "leftobj";
        leftChar.Init((float)_myPlayerPack.MyPackGladiator.CurPos, rightChar, RightLeft.Right, curKnockAngle);
        BattleManager.Instance.vTargetGroup.AddMember(leftChar.transform, 1.8f, 8);
        rightChar.name = _opponentPack.DBID;
        rightChar.tag = "rightobj";
        rightChar.Init((float)_opponentPack.MyPackGladiator.CurPos, leftChar, RightLeft.Left, curKnockAngle);
        BattleManager.Instance.vTargetGroup.AddMember(rightChar.transform, 1, 8);

        CharDic = new Dictionary<string, Character>();
        CharDic.Add(_myPlayerPack.DBID, leftChar);
        CharDic.Add(_opponentPack.DBID, rightChar);
    }



    public IEnumerator WaitCharacterCreate() {
        while (leftChar == null || rightChar == null) {
            yield return new WaitForEndOfFrame();
        }
    }
    void BattleReset(float leftPos, float rightPos) {
        leftChar.transform.position = new Vector3(leftPos, 0, 0);
        leftChar.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightChar.transform.position = new Vector3(rightPos, 0, 0);
        rightChar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void BattleStart() {
        BattleReset(-16, 16);
        BattleIsEnd = false;
    }

    public void BattleEnd() {
        if (BattleIsEnd) return;

        BattleIsEnd = true;
    }

    float curKnockAngle = 0; // 目前碰撞擊退的角度

    public void UpdateGladiatorsState(PackPlayerState _leftPlayer, PackPlayerState _rightPlayer) {
        if (_leftPlayer == null || _rightPlayer == null) return;

        // 計算server的中心點
        float serverCenterPos = (float)(_rightPlayer.GladiatorState.CurPos + _leftPlayer.GladiatorState.CurPos) / 2.0f;
        float leftToCenter = Mathf.Abs(serverCenterPos - (float)_leftPlayer.GladiatorState.CurPos);
        float rightToCenter = Mathf.Abs(serverCenterPos - (float)_rightPlayer.GladiatorState.CurPos);

        // 計算client的中心點
        Vector3 clientCenterPos = (rightChar.transform.localPosition + leftChar.transform.localPosition) / 2.0f;

        // 更新角色狀態
        leftChar.SetState(_leftPlayer.GladiatorState, clientCenterPos, leftToCenter, curKnockAngle);
        rightChar.SetState(_rightPlayer.GladiatorState, clientCenterPos, rightToCenter, curKnockAngle);
    }


    public void Melee(PackPlayerState _leftPlayer, PackPlayerState _rightPlayer, PackAttack _leftAttack, PackAttack _rightAttack) {
        if (_leftPlayer == null || _rightPlayer == null) return;

        UpdateGladiatorsState(_leftPlayer, _rightPlayer);

        curKnockAngle += UnityEngine.Random.Range(-KnockAngleRange, KnockAngleRange);
        BattleManager.Instance.SetVCamTargetRot(-curKnockAngle);

        var leftState = _leftPlayer.GladiatorState;
        leftChar.HandleMelee((float)_leftAttack.AttackPos, (float)_leftAttack.Knockback, (float)leftState.CurPos, curKnockAngle, _leftAttack.SkillID);

        var rightState = _rightPlayer.GladiatorState;
        rightChar.HandleMelee((float)_rightAttack.AttackPos, (float)_rightAttack.Knockback, (float)rightState.CurPos, curKnockAngle, _rightAttack.SkillID);



        //產生特效
        AddressablesLoader.GetParticle("Battle/MeleeHit", (prefab, handle) => {
            var go = Instantiate(prefab);
            var midPos = (rightChar.transform.position + leftChar.transform.position) / 2.0f;
            go.transform.position = midPos + Vector3.up * 3;
        });

    }

    public void Run(string _playerID, bool _run) {
        if (CharDic.ContainsKey(_playerID)) CharDic[_playerID].SetRush(_run);
    }

    public void Skill(string _playerID, int _skillID, bool _on) {
        WriteLog.LogError($"_playerID={_playerID} _skillID={_skillID} _on={_on}");
    }

}

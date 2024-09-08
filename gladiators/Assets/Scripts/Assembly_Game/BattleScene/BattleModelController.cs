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

    [SerializeField] Character leftChar = null;
    [SerializeField] Character rightChar = null;

    [SerializeField] bool BattleIsEnd = false;

    public const float WALLPOS = 20f;
    Dictionary<string, Character> CharDic;

    public static BattleModelController Instance { get; private set; }

    public void Init() {
        Instance = this;
        CreateTerrain();
    }

    void CreateTerrain() {
        Instantiate(terrainPrefab, terrainArea.transform);
    }

    public void CreateCharacter(PackPlayer _myPlayerPack, PackPlayer _opponentPack) {
        leftChar = Instantiate(characterPrefab, charactersArea.transform);
        rightChar = Instantiate(characterPrefab, charactersArea.transform);

        leftChar.name = _myPlayerPack.DBID;
        leftChar.tag = "leftobj";
        leftChar.Init((float)_myPlayerPack.MyPackGladiator.CurPos, rightChar, RightLeft.Right);

        rightChar.name = _opponentPack.DBID;
        rightChar.tag = "rightobj";
        rightChar.Init((float)_opponentPack.MyPackGladiator.CurPos, leftChar, RightLeft.Left);

        leftChar.SetFaceToTarget();
        rightChar.SetFaceToTarget();

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


    public void UpdateGladiatorsState(PackPlayerState _leftPlayer, PackPlayerState _rightPlayer) {
        if (_leftPlayer != null) {
            leftChar.SetState(_leftPlayer.GladiatorState);
        }

        if (_rightPlayer != null) {
            rightChar.SetState(_rightPlayer.GladiatorState);
        }
    }

    public void Melee(PackPlayerState leftPlayer, PackPlayerState rightPlayer, PackAttack _leftAttack, PackAttack _rightAttack) {

        if (leftPlayer != null) {
            var state = leftPlayer.GladiatorState;
            leftChar.SetState(state);
            //WriteLog.LogError("AttackPos=" + _leftAttack.AttackPos + "  CurPos=" + state.CurPos);
            leftChar.HandleMelee(_leftAttack.SkillID, (float)_rightAttack.Knockback, (float)_leftAttack.AttackPos, (float)state.CurPos);

        }

        if (rightPlayer != null) {
            var state = rightPlayer.GladiatorState;
            rightChar.SetState(state);
            //WriteLog.LogError("AttackPos=" + _rightAttack.AttackPos + "  CurPos=" + state.CurPos);
            rightChar.HandleMelee(_rightAttack.SkillID, (float)_leftAttack.Knockback, (float)_rightAttack.AttackPos, (float)state.CurPos);
        }

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

    }

}

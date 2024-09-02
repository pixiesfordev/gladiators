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

    public void CreateTerrain() {
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

    public void Movement(PackPlayerState leftPlayer, PackPlayerState rightPlayer) {
        if (leftPlayer != null) {
            leftChar.SetState(leftPlayer.GladiatorState);
        }

        if (rightPlayer != null) {
            rightChar.SetState(rightPlayer.GladiatorState);
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
    }

}

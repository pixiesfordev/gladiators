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

    void Start() {
        //Init();
    }

    public void Init() {
        //test create
        //CreateTerrain(0);
        //CreateCharacter(0, 0);
    }

    public float minDistance = 5f; // 郔苤擒褩
    public float maxDistance = 20f; // 郔湮擒褩
    public float minFOV = 30f; // 郔苤?珧
    public float maxFOV = 60f; // 郔湮?珧
    public float distanceOffset = 2f; // 擒褩痄講ㄛ蚚黺捼淕cam腔擒褩

    void Update() {
        //Attack();
    }

    public void CreateTerrain(int terrainID) {
        //GameObject prefab = Resources.Load<GameObject>("Prefabs/Battle/test/terrain" + terrainID);
        var terrain = Instantiate(terrainPrefab, terrainArea.transform);
    }

    public void CreateCharacter(int leftCharID, int rightCharID, PackPlayer[] _packPlayers) {
        //Character leftPrefab = Resources.Load<Character>("Prefabs/Battle/test/Character" + leftCharID);
        leftChar = Instantiate(characterPrefab, charactersArea.transform);
        leftChar.name = _packPlayers[0].DBID;
        leftChar.tag = "leftobj";
        leftChar.isRightPlayer = false;

        //Character rightPrefab = Resources.Load<Character>("Prefabs/Battle/test/Character" + rightCharID);
        rightChar = Instantiate(characterPrefab, charactersArea.transform);
        rightChar.name = _packPlayers[1].DBID;
        rightChar.tag = "rightobj";
        rightChar.isRightPlayer = true;

        leftChar.setCharacter(_packPlayers[0].Gladiator, rightChar);
        rightChar.setCharacter(_packPlayers[1].Gladiator, leftChar);

        leftChar.transform.position = new Vector3((float)_packPlayers[0].Gladiator.StagePos, 0, 0);
        rightChar.transform.position = new Vector3((float)_packPlayers[1].Gladiator.StagePos, 0, 0);
    }

    public IEnumerator WaitCharacterCreate() {
        while (leftChar == null || rightChar == null) {
            yield return new WaitForEndOfFrame();
        }
    }
    public void BattleReset(float leftPos, float rightPos) {
        leftChar.transform.position = new Vector3(leftPos, 0, 0);
        leftChar.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightChar.transform.position = new Vector3(rightPos, 0, 0);
        rightChar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void BattleStart() {
        BattleIsEnd = false;
        leftChar.BattleStart();
        rightChar.BattleStart();
    }

    public void BattleEnd() {
        if (BattleIsEnd) return;

        BattleIsEnd = true;
        leftChar.BattleEnd();
        rightChar.BattleEnd();
    }

    public void Movement(PackPlayerState leftPlayer, PackPlayerState rightPlayer) {
        if (leftPlayer != null) {
            leftChar.Movement(leftPlayer.GladiatorState);
        }

        if (rightPlayer != null) {
            rightChar.Movement(rightPlayer.GladiatorState);
        }
    }

    [SerializeField] public float distanceValue = 2.0f;
    [SerializeField] public float PlayerDistance = 0.0f;
    public void GetAttack(PackPlayerState leftPlayer, PackPlayerState rightPlayer) {
        if (leftPlayer != null) {
            leftChar.isGetAttack(leftPlayer.GladiatorState);
        }

        if (rightPlayer != null) {
            rightChar.isGetAttack(rightPlayer.GladiatorState);
        }
    }
}

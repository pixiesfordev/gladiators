using Gladiators.Battle;
using Gladiators.Main;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
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
        Init();
    }

    public void Init() {
        //test create
        CreateTerrain(0);
        CreateCharacter(0, 0);
    }

    public float minDistance = 5f; // 最小距離
    public float maxDistance = 20f; // 最大距離
    public float minFOV = 30f; // 最小視野
    public float maxFOV = 60f; // 最大視野
    public float distanceOffset = 2f; // 距離偏移量，用於調整cam的距離

    void Update() {
        Attack();
    }

    public void CreateTerrain(int terrainID) {
        //GameObject prefab = Resources.Load<GameObject>("Prefabs/Battle/test/terrain" + terrainID);
        var terrain = Instantiate(terrainPrefab, terrainArea.transform);
    }

    public void CreateCharacter(int leftCharID, int rightCharID) {
        //Character leftPrefab = Resources.Load<Character>("Prefabs/Battle/test/Character" + leftCharID);
        leftChar = Instantiate(characterPrefab, charactersArea.transform);
        leftChar.name = "leftCharacter";
        leftChar.tag = "leftobj";
        leftChar.isRightPlayer = false;

        //Character rightPrefab = Resources.Load<Character>("Prefabs/Battle/test/Character" + rightCharID);
        rightChar = Instantiate(characterPrefab, charactersArea.transform);
        rightChar.name = "rightCharacter";
        rightChar.tag = "rightobj";
        rightChar.isRightPlayer = true;

        leftChar.setCharacter(leftCharID, rightChar);
        rightChar.setCharacter(rightCharID, leftChar);

        leftChar.transform.position = new Vector3(-16, 0, 0);
        rightChar.transform.position = new Vector3(16, 0, 0);
    }

    [SerializeField] public float distanceValue = 2.0f;
    [SerializeField] public float PlayerDistance = 0.0f;
    public void Attack() {
        if (BattleIsEnd) return;
        PlayerDistance = Vector3.Distance(leftChar.transform.position, rightChar.transform.position);
        if (PlayerDistance <= distanceValue) {
            leftChar.isGetAttack(leftChar.transform.forward);
            rightChar.isGetAttack(rightChar.transform.forward);
        }
    }

    public IEnumerator BattleReset() {
        while (leftChar == null || rightChar == null) {
            yield return new WaitForEndOfFrame();
        }

        leftChar.transform.position = new Vector3(-16, 0, 0);
        leftChar.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightChar.transform.position = new Vector3(16, 0, 0);
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
}

using Gladiators.Battle;
using Gladiators.Main;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModelController : MonoBehaviour {
    [SerializeField] GameObject terrainPrefab;
    [SerializeField] Character characterPrefab;

    [SerializeField] GameObject terrainArea;
    [SerializeField] GameObject charactersArea;

    Character leftChar = null;
    Character rightChar = null;

    bool BattleIsEnd = false;

    void Start() {
        Init();
    }

    public void Init() {
        //test create
        CreateTerrain(0);
        CreateCharacter(0, 0);
    }

    void Update() {

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

    public void BattleReset() {

        //leftChar.transform.position = new Vector3(30, 0, 0);
        //leftChar.transform.rotation = Quaternion.Euler(0, 0, 0);
        //rightChar.transform.position = new Vector3(0, 0, 0);
        //rightChar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void BattleStart() {
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

using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gladiators.Socket.Matchgame;
using Gladiators.TrainRock;
using Gladiators.TrainVigor;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TrainRockManager : MonoBehaviour {
    public static TrainRockManager Instance;

    public GameObject rockPrefab; // 石頭的 Prefab
    public LineRenderer trajectoryLine; // 用於顯示拋物線的 LineRenderer
    public int trajectoryResolution = 100; // 拋物線的節點數
    public float maxThrowForce = 20f; // 最大投擲力度
    public float dragSensitivity = 10f; // 拖曳靈敏度
    public float initialVerticalSpeed = 10f; //拋物線向上高

    [SerializeField] Camera mainCamera;
    public Camera BattleCam => mainCamera;

    private Vector3 dragStartPos; // 拖曳起點
    private Vector3 dragEndPos; // 拖曳終點
    private bool isDragging = false; // 是否正在拖曳

    void Start() {
        trajectoryLine.enabled = false;
    }

    public void Init() {
        Instance = this;
        trajectoryLine.enabled = false;

        CreateCharacterTest();
    }

    // Update is called once per frame
    void Update() {
        HandleInput();
    }

    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            isDragging = true;
            dragStartPos = GetWorldPointFromMouse(Input.mousePosition);
        } else if (Input.GetMouseButton(0) && isDragging) {
            dragEndPos = GetWorldPointFromMouse(Input.mousePosition);
            Vector3 dragDelta = dragStartPos - dragEndPos;
            ShowTrajectory(dragStartPos, dragDelta);
        } else if (Input.GetMouseButtonUp(0) && isDragging) {
            isDragging = false;
            dragEndPos = GetWorldPointFromMouse(Input.mousePosition);
            Vector3 dragDelta = dragStartPos - dragEndPos;

            ThrowStone(dragStartPos, dragDelta);
            trajectoryLine.enabled = false;
        }
    }

    private Vector3 GetWorldPointFromMouse(Vector3 mousePosition) {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter)) {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }

    private void ThrowStone(Vector3 startPosition, Vector3 dragDelta) {
        Vector3 throwDirection = dragDelta.normalized;
        float throwForce = Mathf.Clamp(dragDelta.magnitude * dragSensitivity, 0, maxThrowForce);

        Vector3 velocity = throwDirection * throwForce;
        velocity.y = initialVerticalSpeed;

        GameObject stone = Instantiate(rockPrefab, startPosition, Quaternion.identity);
        Rigidbody rb = stone.GetComponent<Rigidbody>();

        if (rb != null) {
            rb.velocity = velocity;
        }
    }

    private void ShowTrajectory(Vector3 startPosition, Vector3 dragDelta) {
        trajectoryLine.enabled = true;

        Vector3 throwDirection = dragDelta.normalized;
        float throwForce = Mathf.Clamp(dragDelta.magnitude * dragSensitivity, 0, maxThrowForce);

        Vector3 velocity = throwDirection * throwForce;
        velocity.y = initialVerticalSpeed;

        Vector3[] points = new Vector3[trajectoryResolution];
        float timeStep = 0.1f;

        for (int i = 0; i < trajectoryResolution; i++) {
            float t = i * timeStep;
            points[i] = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;
        }

        trajectoryLine.positionCount = trajectoryResolution;
        trajectoryLine.SetPositions(points);
    }

    [SerializeField] Character characterPrefab;
    [SerializeField] GameObject charactersParent;
    public Character MyChar = null;
    public void CreateCharacter(PackPlayer _myPlayerPack) {
        MyChar = Instantiate(characterPrefab, charactersParent.transform);

        MyChar.name = _myPlayerPack.DBID;
        MyChar.tag = "leftobj";
        MyChar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        MyChar.PlayAni("idle", true);
        MyChar.Init(_myPlayerPack.MyPackGladiator.JsonID, new Vector2(0, -0.5f), null, RightLeft.Right);
    }
    public void CreateCharacterTest() { //測試用
        MyChar = Instantiate(characterPrefab, charactersParent.transform);

        MyChar.name = "1223123132";
        MyChar.tag = "leftobj";
        MyChar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        MyChar.PlayAni("idle", true);
        MyChar.InitTest(new Vector2(0, -0.5f), null, RightLeft.Right);
    }

    public async UniTask doDamage() {
        int dmg = 10;
        int addHP = 20;
        MyChar.ShowBattleNumber(NumType.Dmg_Small, dmg);
        TrainRockSceneUI.Instance.AddHP(-dmg);
        TrainRockSceneUI.Instance.AddMaxHP(addHP);

        MyChar.PlayAni("stun");
        await UniTask.NextFrame();
        MyChar.PlayAni("idle", true);
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gladiators.Hunt;
using Gladiators.Socket.Matchgame;
using Gladiators.TrainRock;
using Gladiators.TrainVigor;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TrainRockManager : MonoBehaviour {
    public static TrainRockManager Instance;

    public GameObject rockPrefab; // 石頭的 Prefab
    public LineRenderer trajectoryLine; // 用於顯示拋物線的 LineRenderer
    public int trajectoryResolution = 10; // 拋物線的節點數
    public float maxThrowForce = 20f; // 最大投擲力度
    public float dragSensitivity = 10f; // 拖曳靈敏度
    public float initialVerticalSpeed = 10f; //拋物線向上高
    public Image leftLimitArea; // 左侧限制区域
    public Image rightLimitArea; // 右侧限制区域

    [SerializeField] Camera MyCam;
    public Camera RockCam => MyCam;

    private Vector3 dragStartPos; // 拖曳起點
    private Vector3 dragEndPos; // 拖曳終點
    private bool isDragging = false; // 是否正在拖曳

    void Start() {
        trajectoryLine.enabled = false;
    }

    public void Init() {
        Instance = this;
        trajectoryLine.enabled = false;
        setCam();
        CreateCharacterTest();
    }

    void setCam() {
        //因為戰鬥場景的攝影機有分為場景與UI, 要把場景攝影機設定為Base, UI設定為Overlay, 並在BaseCamera中加入Camera stack
        UICam.Instance.SetRendererMode(CameraRenderType.Overlay);
        addCamStack(UICam.Instance.MyCam);
    }
    void addCamStack(Camera _cam) {
        if (_cam == null) return;
        var cameraData = MyCam.GetUniversalAdditionalCameraData();
        if (cameraData == null) return;
        cameraData.cameraStack.Add(_cam);
    }

    // Update is called once per frame
    void Update() {
        HandleInput();
    }

    private void HandleInput() {
        if (!playing) return;

        if (Input.GetMouseButtonDown(0)) {
            Vector3 touchPosition = Input.mousePosition;

            if (IsTouchWithinImage(touchPosition, leftLimitArea) || IsTouchWithinImage(touchPosition, rightLimitArea)) {
                isDragging = true;
                dragStartPos = GetWorldPointFromStartMouse(touchPosition);
            } else {
                Debug.Log("超出圖片範圍~");
            }
        } else if (Input.GetMouseButton(0) && isDragging) {
            Vector3 touchPosition = Input.mousePosition;

            dragEndPos = GetWorldPointFromEndMouse(touchPosition);
            Vector3 dragDelta = dragStartPos - dragEndPos;
            ShowTrajectory(dragStartPos, dragDelta);
        } else if (Input.GetMouseButtonUp(0) && isDragging) {
            Vector3 touchPosition = Input.mousePosition;

            isDragging = false;
            dragEndPos = GetWorldPointFromEndMouse(touchPosition);
            Vector3 dragDelta = dragStartPos - dragEndPos;

            ThrowStone(dragStartPos, dragDelta);
            trajectoryLine.enabled = false;
        }
    }

    private bool IsTouchWithinImage(Vector3 touchPosition, Image image) {
        Vector2 localPoint;
        RectTransform imageRectTransform = image.rectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRectTransform, touchPosition, image.canvas.worldCamera, out localPoint);

        return imageRectTransform.rect.Contains(localPoint);
    }

    private Vector3 GetWorldPointFromStartMouse(Vector3 mousePosition) {
        Ray ray = MyCam.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(new Vector3(0, 0.2f, 1f), new Vector3(0, 0, 0));

        if (groundPlane.Raycast(ray, out float enter)) {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }

    private Vector3 GetWorldPointFromEndMouse(Vector3 mousePosition) {
        Ray ray = MyCam.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(new Vector3(0, 0f, 1f), new Vector3(0, 0f, 1f));

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

    public bool playing = false;
    public int curLeftTime;
    void restartGame() {
        TrainRockSceneUI.Instance.ShowCountingdown(false);
        StartGame(30);
    }
    public void StartGame(int StartSec) {
        playing = true;
        // 開始倒數計時
        UniTask.Void(async () => {
            curLeftTime = StartSec;
            TrainRockSceneUI.Instance.SetCountdownImg(curLeftTime);
            while (curLeftTime > 0) {
                if (!playing) break;
                await UniTask.Delay(1000);
                curLeftTime--;
                if (TrainRockSceneUI.Instance.CheckHP() <= 0) { 
                    endGame();
                    break;
                }
                TrainRockSceneUI.Instance.SetCountdownImg(curLeftTime);
            }
            if (playing) endGame();
        });
    }
    void endGame() {
        playing = false;
        PopupUI.ShowAttributeUI($"最大血量增加{allAddHP}，目前最大血量為{TrainRockSceneUI.Instance.CheckMaxHP()}/s", restartGame);
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
        MyChar.Init(_myPlayerPack.MyPackGladiator.JsonID, new Vector2(0, -1f), null, RightLeft.Right);
    }
    public void CreateCharacterTest() {
        MyChar = Instantiate(characterPrefab, charactersParent.transform);

        MyChar.name = "Test";
        MyChar.tag = "leftobj";
        MyChar.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        MyChar.PlayAni("idle", true);
        MyChar.Init(7, new Vector2(0, -1f), null, RightLeft.Right);
    }


    float allAddHP = 0;
    public async UniTask doDamage() {
        int dmg = 10;
        int addHP = 20;
        MyChar.ShowBattleNumber(NumType.Dmg_Small, dmg);
        allAddHP += addHP;
        TrainRockSceneUI.Instance.AddHP(-dmg);
        TrainRockSceneUI.Instance.AddMaxHP(addHP);

        MyChar.PlayAni("stun");
        await UniTask.NextFrame();
        MyChar.PlayAni("idle", true);
    }
}

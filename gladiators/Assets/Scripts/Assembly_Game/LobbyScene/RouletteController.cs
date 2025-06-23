using UnityEngine;
using UnityEngine.EventSystems;

namespace Gladiators.Main {
    public class RouletteController : MonoBehaviour {
        [Header("拖曳控制區域")]
        public RectTransform arrowArea;
        public Camera uiCamera;

        [Header("拖曳參數")]
        [Tooltip("拖曳最小距離(px)，低於此不觸發")]
        public float minDragDistance = 50f;
        [Tooltip("像素速度 → 角速度 的轉換倍數")]
        public float speedFactor = 0.1f;
        [Tooltip("初速上限 (度/秒)")]
        public float maxAngularSpeed = 500f;

        [Header("轉盤元件")]
        public Roulette roulette;

        // 內部
        private Vector2 _startPos;
        private float _startTime;
        private bool _validStart;

        public void Init() {
            if (uiCamera == null) uiCamera = Camera.main;
        }

        public void SetRoulette(Roulette _roulette) {
            roulette = _roulette;
        }

        private void Update() {
            // 觸控
            if (Input.touchCount > 0) {
                var t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began) {
                    _validStart = RectTransformUtility.RectangleContainsScreenPoint(arrowArea, t.position, uiCamera);
                    if (_validStart) {
                        _startPos = t.position;
                        _startTime = Time.time;
                    }
                } else if (t.phase == TouchPhase.Ended && _validStart) {
                    TrySpin(t.position, Time.time - _startTime);
                    _validStart = false;
                }
            }

            // 滑鼠
            if (Input.GetMouseButtonDown(0)) {
                _validStart = RectTransformUtility.RectangleContainsScreenPoint(arrowArea, Input.mousePosition, uiCamera);
                if (_validStart) {
                    _startPos = Input.mousePosition;
                    _startTime = Time.time;
                }
            } else if (Input.GetMouseButtonUp(0) && _validStart) {
                TrySpin((Vector2)Input.mousePosition, Time.time - _startTime);
                _validStart = false;
            }
        }

        private void TrySpin(Vector2 endPos, float deltaTime) {
            Vector2 delta = endPos - _startPos;
            float dist = delta.magnitude;
            if (dist < minDragDistance || deltaTime <= 0f) return;

            // 方向：水平優先
            int dir = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y)
                ? (delta.x > 0 ? 1 : -1)
                : (delta.y > 0 ? 1 : -1);

            // pixel/sec → deg/sec
            float pixPerSec = dist / deltaTime;
            float angVel = pixPerSec * speedFactor * dir;
            // 限制初速上限
            angVel = Mathf.Clamp(angVel, -maxAngularSpeed, maxAngularSpeed);

            if (roulette != null) {
                roulette.Init();
                roulette.StartSpin(-angVel);
            }
        }
    }
}

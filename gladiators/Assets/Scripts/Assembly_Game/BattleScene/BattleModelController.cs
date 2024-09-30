using Cysharp.Threading.Tasks;
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

    [SerializeField] bool BattleIsEnd = false;

    Character leftChar = null;
    Character rightChar = null;
    Dictionary<string, Character> CharDic;

    const float MOVE_DURATION_SECS = 0.08f; // client收到server座標更新後幾秒內要平滑移動至目標秒數



    public const float WALLPOS = 20f;// 牆壁位置
    public const float KnockAngleRange = 30;// 擊退最大水平角度
    float curKnockAngle = 0; // 目前碰撞擊退的角度
    SortedDictionary<long, ServerPosPack> PosBuffer = new SortedDictionary<long, ServerPosPack>(); // 移動插植緩衝
    public class ServerPosPack {
        public long PackID;
        public float LeftPos;
        public float RightPos;
        public long Timestamp;
    }


    public static BattleModelController Instance { get; private set; }

    public void Init() {
        Instance = this;
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
        MoveLoop().Forget();
    }

    public void BattleEnd() {
        if (BattleIsEnd) return;
        BattleIsEnd = true;
    }

    /// <summary>
    /// 插值移動循環
    /// </summary>
    private async UniTaskVoid MoveLoop() {
        while (!BattleIsEnd) {
            // 計算渲染時間戳
            long renderTimestamp = AllocatedRoom.Instance.RenderTimestamp;

            // 找緩衝區中兩個相鄰的封包
            ServerPosPack before = null;
            ServerPosPack after = null;
            var keys = PosBuffer.Keys;
            long? beforeKey = null;
            long? afterKey = null;

            // 清理已經不需要的插植緩衝(只保留最近1秒內的資料)
            long cleanupTime = renderTimestamp - 1000;
            List<long> keysToRemove = new List<long>();

            foreach (var key in keys) {
                if (key < cleanupTime) {
                    keysToRemove.Add(key);
                } else if (key <= renderTimestamp) {
                    beforeKey = key;
                } else {
                    afterKey = key;
                    break;
                }
            }

            // 移除不需要插植緩衝
            foreach (var key in keysToRemove) {
                PosBuffer.Remove(key);
            }

            // 進行插值計算目前本地的座標
            if (beforeKey.HasValue && afterKey.HasValue) {
                before = PosBuffer[beforeKey.Value];
                after = PosBuffer[afterKey.Value];

                // 計算插值因子alpha
                float alpha = (float)(renderTimestamp - before.Timestamp) / (float)(after.Timestamp - before.Timestamp);
                alpha = Mathf.Clamp01(alpha);

                // 計算插值位置
                float leftPos = Mathf.Lerp(before.LeftPos, after.LeftPos, alpha);
                float rightPos = Mathf.Lerp(before.RightPos, after.RightPos, alpha);
                //WriteLog.Log("leftPos=" + leftPos + " rightPos=" + rightPos);

                // 計算一維座標的中心點
                float serverCenterPos = (float)(rightPos + leftPos) / 2.0f;
                float leftToCenter = Mathf.Abs(serverCenterPos - (float)leftPos);
                float rightToCenter = Mathf.Abs(serverCenterPos - (float)rightPos);

                // 計算client二維座標的中心點
                Vector3 clientCenterPos = (rightChar.transform.localPosition + leftChar.transform.localPosition) / 2.0f;

                // 根據目前client角度來計算出目前該有的client座標
                float angle_left = curKnockAngle + 180f;
                float angle_right = curKnockAngle;
                float angleInRadians_left = angle_left * Mathf.Deg2Rad;
                float angleInRadians_right = angle_right * Mathf.Deg2Rad;
                float offsetX_left = Mathf.Cos(angleInRadians_left) * leftToCenter;
                float offsetZ_left = Mathf.Sin(angleInRadians_left) * leftToCenter;
                float offsetX_right = Mathf.Cos(angleInRadians_right) * rightToCenter;
                float offsetZ_right = Mathf.Sin(angleInRadians_right) * rightToCenter;
                Vector3 pos_left = new Vector3(clientCenterPos.x + offsetX_left, 0, clientCenterPos.z + offsetZ_left);
                Vector3 pos_right = new Vector3(clientCenterPos.x + offsetX_right, 0, clientCenterPos.z + offsetZ_right);

                // 更新角色位置
                leftChar.MoveClientToPos(pos_left, MOVE_DURATION_SECS).Forget();
                rightChar.MoveClientToPos(pos_right, MOVE_DURATION_SECS).Forget();
            }

            await UniTask.Yield();
        }
    }



    public void UpdateGladiatorsState(long _packID, long _time, PACKGLADIATORSTATE _leftState, PACKGLADIATORSTATE _rightState) {
        if (_leftState == null || _rightState == null) return;

        leftChar.UpdateEffectTypes(_leftState.EffectTypes);
        rightChar.UpdateEffectTypes(_rightState.EffectTypes);

        // 新增移動插植緩衝
        ServerPosPack pack = new ServerPosPack();
        pack.PackID = _packID;
        pack.Timestamp = _time;
        pack.LeftPos = (float)_leftState.CurPos;
        pack.RightPos = (float)_rightState.CurPos;
        PosBuffer[_time] = pack;
    }


    public void Melee(PackMelee _leftMelee, PackMelee _rightMelee) {
        if (_leftMelee == null || _rightMelee == null) return;

        curKnockAngle += UnityEngine.Random.Range(-KnockAngleRange, KnockAngleRange);
        BattleManager.Instance.SetVCamTargetRot(-curKnockAngle);

        leftChar.HandleMelee(_leftMelee.EffectTypes, (float)_leftMelee.MeleePos, (float)_rightMelee.Knockback, (float)_leftMelee.CurPos, curKnockAngle, _leftMelee.SkillID);

        rightChar.HandleMelee(_rightMelee.EffectTypes, (float)_rightMelee.MeleePos, (float)_leftMelee.Knockback, (float)_rightMelee.CurPos, curKnockAngle, _rightMelee.SkillID);



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

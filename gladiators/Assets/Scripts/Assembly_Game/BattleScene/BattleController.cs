using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour {
    [SerializeField] GameObject terrainPrefab;
    [SerializeField] Character characterPrefab;

    [SerializeField] GameObject terrainArea;
    [SerializeField] GameObject charactersArea;

    [SerializeField] bool BattleIsEnd = false;

    public Character leftChar = null;
    Character rightChar = null;
    Dictionary<string, Character> CharDic;

    const float MOVE_DURATION_SECS = 0.08f; // client收到server座標更新後幾秒內要平滑移動至目標秒數
    const int KEEP_ServerPosPack_MILISECS = 1000; // 移除多久以前的移動封包(毫秒)
    const float MAX_EXTRAPOLATION_SECS = 1; // 最大外推秒數(限制外推時間，避免過長導致不準確)

    public const float WALLPOS = 20f;// 牆壁位置
    public const float KnockAngleRange = 30;// 擊退最大水平角度
    float curKnockAngle = 0; // 目前碰撞擊退的角度
    SortedDictionary<long, ServerStatePack> StateBuffer = new SortedDictionary<long, ServerStatePack>(); // 狀態封包Buffer
    public class ServerStatePack {
        public long PackID;
        public float LeftPos;
        public float LeftSpd;
        public float LeftVigor;
        public bool LeftRush;
        public float RightPos;
        public float RightSpd;
        public bool RightRush;
        public long Timestamp; // server送封包的時間戳
    }


    public static BattleController Instance { get; private set; }

    public void Init() {
        Instance = this;
    }


    public void CreateCharacter(PackPlayer _myPlayerPack, PackPlayer _opponentPack) {
        leftChar = Instantiate(characterPrefab, charactersArea.transform);
        rightChar = Instantiate(characterPrefab, charactersArea.transform);

        leftChar.name = _myPlayerPack.DBID;
        leftChar.tag = "leftobj";
        leftChar.Init(_myPlayerPack.MyPackGladiator.JsonID, (float)_myPlayerPack.MyPackGladiator.CurPos, rightChar, RightLeft.Right, curKnockAngle);
        BattleSceneUI.Instance.InitVigor((float)_myPlayerPack.MyPackGladiator.CurVigor, 20);
        BattleManager.Instance.vTargetGroup.AddMember(leftChar.transform, 1f, 8);
        rightChar.name = _opponentPack.DBID;
        rightChar.tag = "rightobj";
        rightChar.Init(_opponentPack.MyPackGladiator.JsonID, (float)_opponentPack.MyPackGladiator.CurPos, leftChar, RightLeft.Left, curKnockAngle);
        BattleManager.Instance.vTargetGroup.AddMember(rightChar.transform, 1.1f, 8);

        CharDic = new Dictionary<string, Character>();
        CharDic.Add(_myPlayerPack.DBID, leftChar);
        CharDic.Add(_opponentPack.DBID, rightChar);

        BattleManager.Instance.SetVCamTargetRot(-curKnockAngle);
        BattleManager.Instance.SetCamFov(GetDistBetweenChars());
    }



    public IEnumerator WaitCharacterCreate() {
        while (leftChar == null || rightChar == null) {
            yield return new WaitForEndOfFrame();
        }
    }

    public void BattleStart() {
        BattleIsEnd = false;
        StateUpdateLoop().Forget();
    }

    public void BattleEnd() {
        if (BattleIsEnd) return;
        BattleIsEnd = true;
    }
    public void UpdateGladiatorsState(long _packID, long _time, PACKGLADIATORSTATE _leftState, PACKGLADIATORSTATE _rightState) {
        if (_leftState == null || _rightState == null) return;

        leftChar.UpdateEffectTypes(_leftState.EffectDatas);
        rightChar.UpdateEffectTypes(_rightState.EffectDatas);
        List<BufferIconData> selfBuffer = new List<BufferIconData>();
        foreach (var effectData in _leftState.EffectDatas) {
            var (success, effectType) = JsonSkillEffect.ConvertStrToEffectType(effectData.EffectName);
            if (success) {
                selfBuffer.Add(new BufferIconData(effectData.EffectName, (int)effectData.Duration, effectType.GetStackType()));
            }
        }
        List<BufferIconData> enemyBuffer = new List<BufferIconData>();
        foreach (var effectData in _rightState.EffectDatas) {
            var (success, effectType) = JsonSkillEffect.ConvertStrToEffectType(effectData.EffectName);
            if (success) {
                enemyBuffer.Add(new BufferIconData(effectData.EffectName, (int)effectData.Duration, effectType.GetStackType()));
            }
        }
        BattleSceneUI.Instance.PlayerGladiatorInfo.SetBufferIcon(selfBuffer);
        BattleSceneUI.Instance.EnemyGladiatorInfo.SetBufferIcon(enemyBuffer);
        // 新增移動插植緩衝
        ServerStatePack pack = new ServerStatePack();
        pack.PackID = _packID;
        pack.Timestamp = _time;
        pack.LeftPos = (float)_leftState.CurPos;
        pack.LeftSpd = (float)_leftState.CurSpd;
        pack.LeftVigor = (float)_leftState.CurVigor;
        pack.LeftRush = _leftState.Rush;
        pack.RightPos = (float)_rightState.CurPos;
        pack.RightSpd = (float)_rightState.CurSpd;
        pack.RightRush = _rightState.Rush;
        StateBuffer[_time] = pack;
    }
    /// <summary>
    /// 狀態更新循環
    /// </summary>
    private async UniTaskVoid StateUpdateLoop() {
        while (!BattleIsEnd) {
            // 計算渲染時間戳
            long renderTimestamp = AllocatedRoom.Instance.RenderTimestamp;
            // 找緩衝區中兩個相鄰的封包
            ServerStatePack before = null;
            ServerStatePack after = null;
            var keys = StateBuffer.Keys;
            long? beforeKey = null;
            long? afterKey = null;

            // 清理已經不需要的插植緩衝(只保留最近1秒內的資料)
            long cleanupTime = renderTimestamp - KEEP_ServerPosPack_MILISECS;
            List<long> keysToRemove = new List<long>();

            foreach (var key in keys) {
                if (key < cleanupTime) {
                    //keysToRemove.Add(key);
                } else if (key <= renderTimestamp) {
                    beforeKey = key;
                } else {
                    afterKey = key;
                    break;
                }
            }

            // 移除不需要插植緩衝
            foreach (var key in keysToRemove) {
                StateBuffer.Remove(key);
            }

            // <<<<<<<<衝刺>>>>>>>>>
            ServerStatePack lastPack = null;
            if (StateBuffer.Count >= 1) {
                lastPack = StateBuffer.Values.Last();
                leftChar.SetRush(lastPack.LeftRush);
                rightChar.SetRush(lastPack.RightRush);
            }

            if (beforeKey.HasValue && afterKey.HasValue) { // 封包沒延遲時進行插值(interpolation)


                // <<<<<<<<計算插值alpha>>>>>>>>>
                before = StateBuffer[beforeKey.Value];
                after = StateBuffer[afterKey.Value];
                // 計算插值alpha
                float alpha = (float)(renderTimestamp - before.Timestamp) / (float)(after.Timestamp - before.Timestamp);
                alpha = Mathf.Clamp01(alpha);



                // <<<<<<<<腳色位置>>>>>>>>>
                // 計算插值位置
                float leftPos = Mathf.Lerp(before.LeftPos, after.LeftPos, alpha);
                float rightPos = Mathf.Lerp(before.RightPos, after.RightPos, alpha);
                //WriteLog.Log("leftPos=" + leftPos + " rightPos=" + rightPos);
                var clientPos = ConvertTo3DPos(leftPos, rightPos);
                // 更新角色位置
                leftChar.MoveClientToPos(clientPos.Item1, MOVE_DURATION_SECS, true).Forget();
                rightChar.MoveClientToPos(clientPos.Item2, MOVE_DURATION_SECS, true).Forget();
                BattleManager.Instance.SetCamFov(GetDistBetweenChars());

                // <<<<<<<<體力>>>>>>>>>
                float leftVigor = Mathf.Lerp(before.LeftVigor, after.LeftVigor, alpha);
                BattleSceneUI.Instance.SetVigor(leftVigor);

            } else { // 封包延遲時進行外推(extrapolation)
                if (lastPack != null) {

                    // 計算從最後封包到當前渲染時間的時間差(秒)
                    float extrapolateTime = (renderTimestamp - lastPack.Timestamp) / 1000f;
                    // 限制外推時間，避免過長導致不準確
                    extrapolateTime = Mathf.Min(extrapolateTime, MAX_EXTRAPOLATION_SECS);


                    // <<<<<<<<腳色位置>>>>>>>>>
                    // 取得速度
                    float leftSpd = lastPack.LeftSpd;
                    float rightSpd = lastPack.RightSpd;
                    // 計算外推位置
                    float extrapolatedLeftPos = lastPack.LeftPos + leftSpd * extrapolateTime;
                    float extrapolatedRightPos = lastPack.RightPos + rightSpd * extrapolateTime;
                    // 使用插值來平滑外推位置
                    extrapolatedLeftPos = Mathf.Lerp(lastPack.LeftPos, extrapolatedLeftPos, extrapolateTime / MOVE_DURATION_SECS);
                    extrapolatedRightPos = Mathf.Lerp(lastPack.RightPos, extrapolatedRightPos, extrapolateTime / MOVE_DURATION_SECS);
                    // 轉換為3D座標
                    var clientExtrapolatedPos = ConvertTo3DPos(extrapolatedLeftPos, extrapolatedRightPos);
                    // 更新角色位置
                    leftChar.MoveClientToPos(clientExtrapolatedPos.Item1, MOVE_DURATION_SECS, false).Forget();
                    rightChar.MoveClientToPos(clientExtrapolatedPos.Item2, MOVE_DURATION_SECS, false).Forget();

                    // <<<<<<<<體力>>>>>>>>>
                    float leftVigor = lastPack.LeftVigor;
                    // 計算外推體力
                    float extrapolatedLeftVigor = lastPack.LeftVigor + 1 * extrapolateTime; // 體力回復都是1
                    // 使用插值來平滑外推體力
                    extrapolatedLeftVigor = Mathf.Lerp(lastPack.LeftVigor, extrapolatedLeftVigor, extrapolateTime / MOVE_DURATION_SECS);

                } else {
                    // 尚未收到封包前腳色不會移動
                }
            }

            await UniTask.Yield();
        }
    }


    /// <summary>
    /// 將server的1維座標轉client的3維座標
    /// </summary>
    Tuple<Vector3, Vector3> ConvertTo3DPos(float rightPos, float leftPos) {
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
        return new Tuple<Vector3, Vector3>(pos_left, pos_right);
    }


    public void Melee(PackMelee _leftMelee, PackMelee _rightMelee) {
        if (_leftMelee == null || _rightMelee == null) return;
        curKnockAngle += UnityEngine.Random.Range(-KnockAngleRange, KnockAngleRange);
        BattleManager.Instance.SetVCamTargetRot(-curKnockAngle);
        var clientPos = ConvertTo3DPos((float)_leftMelee.CurPos, (float)_rightMelee.CurPos);
        leftChar.HandleMelee(clientPos.Item1, _leftMelee.EffectDatas, (float)_leftMelee.Knockback, (float)_leftMelee.CurPos, curKnockAngle, _leftMelee.SkillID);
        rightChar.HandleMelee(clientPos.Item2, _rightMelee.EffectDatas, (float)_rightMelee.Knockback, (float)_rightMelee.CurPos, curKnockAngle, _rightMelee.SkillID);

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

    /// <summary>
    /// 呼叫腳色播放立即技能，收到即時技能施放封包時會呼叫此Func
    /// </summary>
    /// <param name="_playerID">玩家DBID</param>
    /// <param name="_skillID">技能JsonID</param>
    public void PlayInstantSkill(string _playerID, int _skillID) {
        if (CharDic == null || !CharDic.ContainsKey(_playerID)) {
            WriteLog.LogError("PlayInstantSkill錯誤");
            return;
        }
        var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_skillID);
        if (jsonSkill == null) return;
        CharDic[_playerID].MyEffectSpeller.PlaySpellEffect(jsonSkill);
    }
    /// <summary>
    /// 呼叫腳色播放肉搏技能，收到肉搏技能施放封包時會呼叫此Func
    /// </summary>
    public void PlayMeleeSkill(BEFORE_MELEE_TOCLIENT _pack) {
        if (leftChar == null || rightChar == null) {
            WriteLog.LogError("PlayMeleeSkill錯誤");
            return;
        }
        if (_pack.MySkillID != 0) {
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_pack.MySkillID);
            if (jsonSkill == null) return;
            leftChar.MyEffectSpeller.PlaySpellEffect(jsonSkill);
        }
        if (_pack.OpponentSkillID != 0) {
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_pack.OpponentSkillID);
            if (jsonSkill == null) return;
            rightChar.MyEffectSpeller.PlaySpellEffect(jsonSkill);
        }

    }

    // Server收到血量變化封包後，呼叫此Func顯示跳血
    public void ShowBattleNumber(string _playerID, int _value, string _effectTypeStr) {
        if (!CharDic.ContainsKey(_playerID) || CharDic[_playerID] == null) return;

        EffectType effectType;
        if (!MyEnum.TryParseEnum(_effectTypeStr, out effectType)) {
            WriteLog.LogError($"收到HPChange封包的effectType({_effectTypeStr})錯誤，需要更新Enum EffectType");
            return;
        }
        NumType numType = NumType.Dmg_Small;
        switch (effectType) {
            case EffectType.PDmg:
            case EffectType.MDmg:
            case EffectType.TrueDmg:
                if (_value > 0) {
                    WriteLog.LogError($"收到HPChange封包的格式錯誤 effectType: {effectType}  Value: {_value}");
                }
                int absDmgValue = Mathf.Abs(_value);
                if (absDmgValue <= 30)
                    numType = NumType.Dmg_Small;
                else if (absDmgValue <= 60)
                    numType = NumType.Dmg_Medium;
                else
                    numType = NumType.Dmg_Large;
                break;
            case EffectType.RestoreHP:
                if (_value < 0) {
                    WriteLog.LogError($"收到HPChange封包的格式錯誤 effectType: {effectType}  Value: {_value}");
                }
                numType = NumType.Restore_Hp;
                break;
            case EffectType.Bleeding:
            case EffectType.Poison:
            case EffectType.Burning:
                if (_value > 0) {
                    WriteLog.LogError($"收到HPChange封包的格式錯誤 effectType: {effectType}  Value: {_value}");
                }
                switch (effectType) {
                    case EffectType.Bleeding:
                        numType = NumType.Dmg_Bleeding;
                        break;
                    case EffectType.Poison:
                        numType = NumType.Dmg_Poison;
                        break;
                    case EffectType.Burning:
                        numType = NumType.Dmg_Burning;
                        break;
                }
                break;
            default:
                WriteLog.LogError($"收到HPChange封包的，{effectType}要顯示的NumType尚未定義");
                return;
        }
        CharDic[_playerID].ShowBattleNumber(numType, _value);
    }

    public float GetDistBetweenChars() {
        if (leftChar == null || rightChar == null) {
            WriteLog.LogError($"character為null leftChar: {leftChar}   rightChar: {rightChar}");
            return 0f;
        }
        float dist = Vector3.Distance(leftChar.transform.position, rightChar.transform.position);
        return dist;
    }


}

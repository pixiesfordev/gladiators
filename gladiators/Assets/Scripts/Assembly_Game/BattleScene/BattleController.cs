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
    [SerializeField] Character characterPrefab;
    [SerializeField] GameObject charactersParent;

    public Character LeftChar = null;
    public Character RightChar = null;
    Dictionary<string, Character> CharDic;

    const float MOVE_DURATION_SECS = 0.08f; // client收到server座標更新後幾秒內要平滑移動至目標秒數
    const int KEEP_ServerPosPack_MILISECS = 1000; // 移除多久以前的移動封包(毫秒)
    const float MAX_EXTRAPOLATION_SECS = 1; // 最大外推秒數(限制外推時間，避免過長導致不準確)

    public const float WALLPOS = 20f;// 牆壁位置
    public const float KnockAngleRange = 30;// 擊退最大水平角度
    SortedDictionary<long, ServerStatePack> StateBuffer = new SortedDictionary<long, ServerStatePack>(); // 狀態封包Buffer
    public class ServerStatePack {
        public long PackID;
        public Vector2 LeftPos;
        public float LeftSpd;
        public float LeftVigor;
        public bool LeftRush;
        public Vector2 RightPos;
        public float RightSpd;
        public bool RightRush;
        public long Timestamp; // server送封包的時間戳
    }


    public static BattleController Instance { get; private set; }

    public void Init() {
        Instance = this;
    }


    public void CreateCharacter(PackPlayer _myPlayerPack, PackPlayer _opponentPack) {
        LeftChar = Instantiate(characterPrefab, charactersParent.transform);
        RightChar = Instantiate(characterPrefab, charactersParent.transform);

        LeftChar.name = _myPlayerPack.DBID;
        LeftChar.tag = "leftobj";
        LeftChar.Init(_myPlayerPack.MyPackGladiator.JsonID, _myPlayerPack.MyPackGladiator.CurPos.ToUnityVector2(), RightChar, RightLeft.Left);
        BattleSceneUI.Instance.InitVigor((float)_myPlayerPack.MyPackGladiator.CurVigor, 20);
        BattleManager.Instance.vTargetGroup.AddMember(LeftChar.transform, 1f, 8);
        RightChar.name = _opponentPack.DBID;
        RightChar.tag = "rightobj";
        RightChar.Init(_opponentPack.MyPackGladiator.JsonID, _opponentPack.MyPackGladiator.CurPos.ToUnityVector2(), LeftChar, RightLeft.Right);
        BattleManager.Instance.vTargetGroup.AddMember(RightChar.transform, 1.1f, 8);

        CharDic = new Dictionary<string, Character>();
        CharDic.Add(_myPlayerPack.DBID, LeftChar);
        CharDic.Add(_opponentPack.DBID, RightChar);

        BattleManager.Instance.UpdateVCamTargetRot();
        BattleManager.Instance.SetCamValues(GetDistBetweenChars());
    }



    public IEnumerator WaitCharacterCreate() {
        while (LeftChar == null || RightChar == null) {
            yield return new WaitForEndOfFrame();
        }
    }

    public void BattleStart() {
        StateUpdateLoop().Forget();
    }
    public void UpdateGladiatorsState(long _packID, long _time, PACKGLADIATORSTATE _leftState, PACKGLADIATORSTATE _rightState) {
        if (_leftState == null || _rightState == null) return;

        // 設定左方玩家的Buff
        List<BufferIconData> selfBuffer = new List<BufferIconData>();
        HashSet<EffectType> selfEffects = new HashSet<EffectType>();
        foreach (var effectData in _leftState.EffectDatas) {
            var (success, effectType) = JsonSkillEffect.ConvertStrToEffectType(effectData.EffectName);
            if (success) {
                selfBuffer.Add(new BufferIconData(effectData.EffectName, (int)effectData.Duration, effectType.GetStackType()));
                selfEffects.Add(effectType);
            }
        }
        LeftChar.UpdateEffectTypes(selfEffects);
        BattleSceneUI.Instance.PlayerGladiatorInfo.SetBufferIcon(selfBuffer);
        // 設定右方玩的BuffIcon
        List<BufferIconData> enemyBuffer = new List<BufferIconData>();
        HashSet<EffectType> eneyEffects = new HashSet<EffectType>();
        foreach (var effectData in _rightState.EffectDatas) {
            var (success, effectType) = JsonSkillEffect.ConvertStrToEffectType(effectData.EffectName);
            if (success) {
                enemyBuffer.Add(new BufferIconData(effectData.EffectName, (int)effectData.Duration, effectType.GetStackType()));
                eneyEffects.Add(effectType);
            }
        }
        RightChar.UpdateEffectTypes(eneyEffects);
        BattleSceneUI.Instance.EnemyGladiatorInfo.SetBufferIcon(enemyBuffer);

        var leftPos = _leftState.CurPos.ToUnityVector2();
        var rightPos = _rightState.CurPos.ToUnityVector2();
        // 新增移動插植緩衝
        ServerStatePack pack = new ServerStatePack();
        pack.PackID = _packID;
        pack.Timestamp = _time;
        pack.LeftPos = leftPos;
        pack.LeftSpd = (float)_leftState.CurSpd;
        pack.LeftVigor = (float)_leftState.CurVigor;
        pack.LeftRush = _leftState.Rush;
        pack.RightPos = rightPos;
        pack.RightSpd = (float)_rightState.CurSpd;
        pack.RightRush = _rightState.Rush;
        StateBuffer[_time] = pack;
    }
    private async UniTaskVoid StateUpdateLoop() {
        while (AllocatedRoom.Instance.CurGameState == AllocatedRoom.GameState.GameState_Fighting) {
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
                    // keysToRemove.Add(key);
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
                LeftChar.SetRush(lastPack.LeftRush);
                RightChar.SetRush(lastPack.RightRush);
            }

            if (beforeKey.HasValue && afterKey.HasValue) {
                // 封包沒延遲時進行插值(interpolation)
                before = StateBuffer[beforeKey.Value];
                after = StateBuffer[afterKey.Value];

                // 計算插值alpha
                float alpha = (float)(renderTimestamp - before.Timestamp) / (float)(after.Timestamp - before.Timestamp);
                alpha = Mathf.Clamp01(alpha);

                // <<<<<<<<腳色位置>>>>>>>>>
                // 計算插值位置
                Vector2 leftPos = Vector2.Lerp(before.LeftPos, after.LeftPos, alpha);
                Vector2 rightPos = Vector2.Lerp(before.RightPos, after.RightPos, alpha);

                // 更新角色位置到棋盤
                LeftChar.MoveClientToPos(new Vector3(leftPos.x, 0, leftPos.y), MOVE_DURATION_SECS, true).Forget();
                RightChar.MoveClientToPos(new Vector3(rightPos.x, 0, rightPos.y), MOVE_DURATION_SECS, true).Forget();

                // 更新攝影機
                BattleManager.Instance.SetCamValues(GetDistBetweenChars());

                // <<<<<<<<體力>>>>>>>>>
                float leftVigor = Mathf.Lerp(before.LeftVigor, after.LeftVigor, alpha);
                BattleSceneUI.Instance.SetVigor(leftVigor);
            } else {
                // 封包延遲時進行外推(extrapolation)
                if (lastPack != null) {
                    // 計算從最後封包到當前渲染時間的時間差(秒)
                    float extrapolateTime = (renderTimestamp - lastPack.Timestamp) / 1000f;
                    extrapolateTime = Mathf.Min(extrapolateTime, MAX_EXTRAPOLATION_SECS);

                    // <<<<<<<<腳色位置>>>>>>>>>
                    Vector2 leftSpd = new Vector2(lastPack.LeftSpd, 0); // 假設速度是平面上的向量
                    Vector2 rightSpd = new Vector2(lastPack.RightSpd, 0);

                    Vector2 extrapolatedLeftPos = lastPack.LeftPos + leftSpd * extrapolateTime;
                    Vector2 extrapolatedRightPos = lastPack.RightPos + rightSpd * extrapolateTime;

                    // 平滑外推位置
                    extrapolatedLeftPos = Vector2.Lerp(lastPack.LeftPos, extrapolatedLeftPos, extrapolateTime / MOVE_DURATION_SECS);
                    extrapolatedRightPos = Vector2.Lerp(lastPack.RightPos, extrapolatedRightPos, extrapolateTime / MOVE_DURATION_SECS);

                    // 更新角色位置
                    LeftChar.MoveClientToPos(new Vector3(extrapolatedLeftPos.x, 0, extrapolatedLeftPos.y), MOVE_DURATION_SECS, false).Forget();
                    RightChar.MoveClientToPos(new Vector3(extrapolatedRightPos.x, 0, extrapolatedRightPos.y), MOVE_DURATION_SECS, false).Forget();

                    // <<<<<<<<體力>>>>>>>>>
                    float extrapolatedLeftVigor = lastPack.LeftVigor + 1 * extrapolateTime;
                    extrapolatedLeftVigor = Mathf.Lerp(lastPack.LeftVigor, extrapolatedLeftVigor, extrapolateTime / MOVE_DURATION_SECS);
                }
            }

            await UniTask.Yield();
        }
    }




    public void Knockback(KNOCKBACK_TOCLIENT _knockback) {
        if (_knockback == null) return;
        if (!CharDic.ContainsKey(_knockback.PlayerID)) return;

        if (_knockback.PlayerID == GamePlayer.Instance.PlayerID) {
            CharDic[_knockback.PlayerID].HandleKnockback(_knockback.BeforePos.ToUnityVector2(), _knockback.AfterPos.ToUnityVector2(), _knockback.KnockWall);
        } else {
            CharDic[_knockback.PlayerID].HandleKnockback(_knockback.BeforePos.ToUnityVector2(), _knockback.AfterPos.ToUnityVector2(), _knockback.KnockWall);
        }
    }


    public void Melee() {
        //產生特效
        AddressablesLoader.GetParticle("Battle/MeleeHit", (prefab, handle) => {
            var go = Instantiate(prefab);
            var midPos = (RightChar.transform.position + LeftChar.transform.position) / 2.0f;
            go.transform.position = midPos + Vector3.up * 3;
        });
    }

    public void Rush(string _playerID, bool _run) {
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
        if (LeftChar == null || RightChar == null) {
            WriteLog.LogError("PlayMeleeSkill錯誤");
            return;
        }
        if (_pack.MySkillID != 0) {
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_pack.MySkillID);
            if (jsonSkill == null) return;
            LeftChar.MyEffectSpeller.PlaySpellEffect(jsonSkill);
        }
        if (_pack.OpponentSkillID != 0) {
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_pack.OpponentSkillID);
            if (jsonSkill == null) return;
            RightChar.MyEffectSpeller.PlaySpellEffect(jsonSkill);
        }

    }

    public void CallDie(string _playerID) {
        if (!CharDic.ContainsKey(_playerID)) {
            return;
        }
        CharDic[_playerID].DieKnockout().Forget();
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
            case EffectType.RegenHP:
                if (_value < 0) {
                    WriteLog.LogError($"收到HPChange封包的格式錯誤 effectType: {effectType}  Value: {_value}");
                }
                numType = NumType.Restore_Hp;
                break;
            case EffectType.RestoreVigor:
            case EffectType.RegenVigor:
                numType = NumType.Restore_Vigor;
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
        if (LeftChar == null || RightChar == null) {
            WriteLog.LogError($"character為null leftChar: {LeftChar}   rightChar: {RightChar}");
            return 0f;
        }
        float dist = Vector3.Distance(LeftChar.transform.position, RightChar.transform.position);
        return dist;
    }


}

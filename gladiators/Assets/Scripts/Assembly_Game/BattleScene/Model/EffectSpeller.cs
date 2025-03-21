using Scoz.Func;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Gladiators.Main;


namespace Gladiators.Battle {
    public class EffectSpeller : MonoBehaviour {

        public EffectSpeller Instance { get; private set; }
        Character myslef;
        Character opponent;


        public void Init(Character _opponent) {
            Instance = this;
            myslef = GetComponent<Character>();
            opponent = _opponent;
        }

        bool checkDataValid(JsonSkill _jsonSkill) {
            if (_jsonSkill == null) {
                WriteLog.LogError("Speller checkDataValid傳入null _jsonSkill");
                return false;
            }
            return true;
        }
        void setRoleSpacePos(Transform _trans, Character _target, RoleSpace _space) {
            switch (_space) {
                case RoleSpace.Top:
                    _trans.position = _target.TopPos;
                    break;
                case RoleSpace.Center:
                    _trans.position = _target.CenterPos;
                    break;
                case RoleSpace.Bot:
                    _trans.position = _target.BotPos;
                    break;
                default:
                    WriteLog.LogError($"setRoleSpacePos尚未實作的RoleSpace: {_space}");
                    break;
            }
        }
        public void PlaySpellEffect(JsonSkill _jsonSkill) {
            if (!checkDataValid(_jsonSkill)) return;
            if (_jsonSkill.Effect_Self != null) playSelfEffect(_jsonSkill.Effect_Self);
            if (_jsonSkill.Effect_Target != null) playTargetEffect(_jsonSkill.Effect_Target);
            if (_jsonSkill.Effect_Projector != null) playProjectorEffect(_jsonSkill);
        }

        Dictionary<EffectType, GameObject> buffs = new Dictionary<EffectType, GameObject>();
        public void PlayBuffEffect(HashSet<EffectType> _effects) {

            // 移除 Buff
            var keysToRemove = buffs.Keys.Where(k => !_effects.Contains(k)).ToList();
            foreach (var key in keysToRemove) {
                Destroy(buffs[key]);
                buffs.Remove(key);
            }

            // 新增 Buff
            foreach (var effectType in _effects) {
                if (!buffs.ContainsKey(effectType)) {
                    buffs[effectType] = null; // 先設為 null 表示正在載入中
                    AddressablesLoader.GetPrefab($"Particles/Buff/{effectType}", (prefab, handle) => {
                        if (prefab == null || !myslef || !myslef.BuffParent) return;
                        var go = Instantiate(prefab);
                        go.transform.SetParent(myslef.BuffParent);
                        setBuffPos(go.transform, effectType);
                        buffs[effectType] = go;
                    });
                }
            }
        }
        void setBuffPos(Transform _trans, EffectType _type) {
            switch (_type) {
                case EffectType.Bleeding:
                case EffectType.Burning:
                case EffectType.Poison:
                    _trans.position = myslef.CenterPos;
                    break;
                case EffectType.Dizzy:
                    _trans.position = myslef.TopPos;
                    break;
                default:
                    WriteLog.LogError($"setBuffPos尚未定義此Buff類型({_type})的位置");
                    _trans.position = myslef.CenterPos;
                    break;

            }
        }
        /// <summary>
        /// 播放自身特效
        /// </summary>
        void playSelfEffect(SpellEffect _effect) {
            if (_effect == null) return;
            AddressablesLoader.GetPrefab($"Particles/Skill/{_effect.Name}", (prefab, handle) => {
                if (prefab == null) return;
                var go = Instantiate(prefab);
                if (_effect.MySpace == SpaceType.Local) {
                    go.transform.SetParent(myslef.transform);
                } else {
                    go.transform.SetParent(BattleManager.Instance.WorldEffectParent);
                }
                setRoleSpacePos(go.transform, myslef, _effect.MyRoleSpace);
                // 面相目標
                Vector3 dir = (opponent.transform.position - transform.position).normalized;
                if (dir != Vector3.zero) go.transform.rotation = Quaternion.LookRotation(dir);
            });
        }

        /// <summary>
        /// 播放目標特效
        /// </summary>
        void playTargetEffect(SpellEffect _effect) {
            if (_effect == null) return;
            AddressablesLoader.GetPrefab($"Particles/Skill/{_effect.Name}", (prefab, handle) => {
                if (prefab == null) return;
                var go = Instantiate(prefab);
                if (_effect.MySpace == SpaceType.Local) {
                    go.transform.SetParent(opponent.transform);
                } else {
                    go.transform.SetParent(BattleManager.Instance.WorldEffectParent);
                }
                setRoleSpacePos(go.transform, opponent, _effect.MyRoleSpace);
                // 面相自己
                Vector3 dir = (transform.position - opponent.transform.position).normalized;
                if (dir != Vector3.zero) go.transform.rotation = Quaternion.LookRotation(dir);

            });
        }
        /// <summary>
        /// 播放投射特效
        /// </summary>
        void playProjectorEffect(JsonSkill _json) {
            if (_json == null) return;

            // 如果Init是0就直接播放命中特效
            if (_json.Init <= 0) {
                playHitEffect(_json);
                return;
            }

            AddressablesLoader.GetPrefab($"Particles/Skill/{_json.Effect_Projector.Name}", (prefab, handle) => {
                if (prefab == null) return;
                var projectorGo = Instantiate(prefab);
                projectorGo.transform.SetParent(BattleManager.Instance.WorldEffectParent);
                setRoleSpacePos(projectorGo.transform, myslef, _json.Effect_Projector.MyRoleSpace);
                var charDist = BattleController.Instance.GetDistBetweenChars();
                float timeToTarget = 0;
                timeToTarget = charDist / (float)_json.Init - (float)(AllocatedRoom.Instance.Lantency / 1000d);
                projectorGo.AddComponent<Projector>().Init(opponent, timeToTarget, () => {
                    playHitEffect(_json);
                    Destroy(projectorGo);
                });
            });
        }
        void playHitEffect(JsonSkill _json) {
            // 命中後執行
            AddressablesLoader.GetPrefab($"Particles/Skill/{_json.Effect_ProjectorHit.Name}", (prefab, handle) => {
                var hitGo = Instantiate(prefab);
                if (_json.Effect_ProjectorHit.MySpace == SpaceType.Local) {
                    hitGo.transform.SetParent(opponent.transform);
                    setRoleSpacePos(hitGo.transform, opponent, _json.Effect_ProjectorHit.MyRoleSpace);
                } else {
                    hitGo.transform.SetParent(BattleManager.Instance.WorldEffectParent);
                    hitGo.transform.position = opponent.transform.position;
                }
                // 面相自己
                Vector3 dir = (transform.position - opponent.transform.position).normalized;
                if (dir != Vector3.zero) hitGo.transform.rotation = Quaternion.LookRotation(dir);
            });
        }
    }
}
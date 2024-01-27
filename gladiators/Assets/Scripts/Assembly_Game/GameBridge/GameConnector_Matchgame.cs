﻿using Cysharp.Threading.Tasks;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using Newtonsoft.Json.Linq;
using Scoz.Func;
using Service.Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gladiators.Socket {
    public partial class GameConnector : MonoBehaviour {
        /// <summary>
        /// 設定使用英雄ID
        /// </summary>
        public void SetHero(int _heroID, string _heroSkinID) {
            SocketCMD<SETHERO> cmd = new SocketCMD<SETHERO>(new SETHERO(_heroID, _heroSkinID));
            Socket.TCPSend(cmd);
        }
        /// <summary>
        /// 離開遊戲房
        /// </summary>
        public void LeaveRoom() {
            SocketCMD<LEAVE> cmd = new SocketCMD<LEAVE>(new LEAVE());
            Socket.TCPSend(cmd);
        }
        /// <summary>
        /// 攻擊
        /// </summary>
        public void Attack(int _attackID, string _spellJsonID, int _monsterIdx, bool _attackLock, Vector3 _attackPos, Vector3 _attackDir) {
            SocketCMD<ATTACK> cmd = new SocketCMD<ATTACK>(new ATTACK(_attackID, _spellJsonID, _monsterIdx, _attackLock, _attackPos, _attackDir));
            Socket.TCPSend(cmd);
        }
        /// <summary>
        /// 擊中
        /// </summary>
        public void Hit(int _attackID, int[] _monsterIdxs, string _spellJsonID) {
            SocketCMD<HIT> cmd = new SocketCMD<HIT>(new HIT(_attackID, _monsterIdxs, _spellJsonID));
            Socket.TCPSend(cmd);
        }
        /// <summary>
        /// 掉落施法
        /// </summary>
        public void DropSpell(int _dropSpellJsonID) {
            SocketCMD<DROPSPELL> cmd = new SocketCMD<DROPSPELL>(new DROPSPELL(_dropSpellJsonID));
            Socket.TCPSend(cmd);
        }
        /// <summary>
        /// 場景狀態更新
        /// </summary>
        public void UpdateScene() {
            SocketCMD<UPDATESCENE> cmd = new SocketCMD<UPDATESCENE>(new UPDATESCENE());
            Socket.TCPSend(cmd);
        }

        public void Auto(bool isAuto) {
            SocketCMD<AUTO> cmd = new SocketCMD<AUTO>(new AUTO(isAuto));
            Socket.TCPSend(cmd);
        }
    }
}
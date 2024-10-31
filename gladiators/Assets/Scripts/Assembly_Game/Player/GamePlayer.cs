using System.Collections.Generic;
using Scoz.Func;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Gladiators.Main {

    public partial class GamePlayer : MyPlayer {
        public new static GamePlayer Instance { get; private set; }

        public Dictionary<DBDataType, DBData> dbDatas;

        /// <summary>
        /// 登入後會先存裝置UID到DB，存好後AlreadSetDeviceUID會設為true，所以之後從DB取到的裝置的UID應該都跟目前的裝置一致，若不一致代表是有其他裝置登入同個帳號
        /// </summary>
        public bool AlreadSetDeviceUID { get; set; } = false;

        public GamePlayer()
        : base() {
            Instance = this;
            dbDatas = new Dictionary<DBDataType, DBData>();
        }
        public override void LoadLocoData() {
            base.LoadLocoData();
            LoadAllDataFromLoco();
        }

        /// <summary>
        /// 設定DB資料
        /// </summary>
        public void SetDBData<T>(T _data) where T : DBData {
            if (dbDatas == null) {
                WriteLog.LogError("SetDBData dbDatas 為null");
                return;
            }
            var dataType = GetDBDataTypeByT<T>();
            dbDatas[dataType] = _data;
        }

        /// <summary>
        /// 取得DB資料
        /// </summary>
        public T GetDBData<T>() where T : DBData {
            if (dbDatas == null) {
                WriteLog.LogError("GetDBData dbDatas 為null");
                return default(T);
            }
            var dataType = GetDBDataTypeByT<T>();
            if (!dbDatas.ContainsKey(dataType) || dbDatas[dataType] == null) {
                WriteLog.LogWarning($"dbDatas 的 {typeof(T)} 類資料為null");
                return default(T);
            }
            return dbDatas[dataType] as T;
        }

        DBDataType GetDBDataTypeByT<T>() where T : DBData {
            // 繼承自DBData的類的對應DBDataType都是該類名稱去掉"DB", 例如DBPlayer類的DBDataType就是"Player"
            string dataName = typeof(T).Name.Replace("DB", "");
            DBDataType dataType;
            if (!MyEnum.TryParseEnum(dataName, out dataType)) {
                WriteLog.LogError($"GetDBData 時 class {typeof(T).Name} 的 dataName 無法轉為 DBDataType");
            }
            return dataType;
        }

        /// <summary>
        /// 登入時寫入設定玩家資料
        /// </summary>
        public void SigninSetPlayerData(DBPlayer _data, bool _saveLocoData) {
            if (_data == null) return;
            SetDBData(_data);
            PlayerID = _data.ID;
            MyAuthType = _data.AuthType;
            DeviceUID = _data.DeviceUID;
            if (_saveLocoData) SaveSettingToLoco();
        }

    }
}
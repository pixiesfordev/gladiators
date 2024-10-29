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
        }
        public override void LoadLocoData() {
            base.LoadLocoData();
            LoadAllDataFromLoco();
        }

        /// <summary>
        /// 取得DB資料
        /// </summary>
        public T GetDBData<T>() where T : DBData {
            if (dbDatas == null) {
                WriteLog.LogError("dbDatas 為null");
                return default(T);
            }
            var dataType = GetDBDataTypeByT<T>();
            if (!dbDatas.ContainsKey(dataType) || dbDatas[dataType] == null) {
                WriteLog.LogError($"dbDatas 的 {typeof(T)} 類資料為null");
                return default(T);
            }
            return dbDatas[dataType] as T;
        }

        /// <summary>
        /// 取得最新DB資料
        /// </summary>
        public async UniTask<T> GetNewestDBData<T>() where T : DBData {
            var dataType = GetDBDataTypeByT<T>();
            switch (dataType) {
                case DBDataType.GameState:
                    var dbPlayer = GetDBData<DBPlayer>();
                    return await APIManager.GameState(dbPlayer.ConnToken) as T;
            }
            return default(T);
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


    }
}
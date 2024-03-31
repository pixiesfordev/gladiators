using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LitJson;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
using Scoz.Func;

namespace Service.Realms {
    public static partial class RealmManager {

        public static async UniTask<BsonDocument> Query_GetDoc(string _dbCol, string _id) {
            WriteLog.LogError("_dbCol=" + _dbCol);
            WriteLog.LogError("_id=" + _id);
            var col = MyDB.GetCollection<BsonDocument>(_dbCol);
            try {
                var doc = await col.FindOneAsync(new BsonDocument { { "_id", _id } });
                return doc;
            } catch (Exception _e) {
                WriteLog.LogError("Query_GetDoc錯誤:" + _e);
                return null;
            }
        }
    }
}
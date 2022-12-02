using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PlayerActionData
{
    [JsonProperty("action")]
    public string action;

    [JsonProperty("room_id")]
    public int? room_id;

    [JsonProperty("user")]
    public string user;

    [JsonProperty("rote_y")]
    public float rote_y;

    [JsonProperty("spead")]
    public float spead;

    [JsonProperty("direction")]
    public string direction;

    /// クライアントからサーバへ送信するデータをJSON形式に変換
    public string ToJson()
    {
        // オブジェクトをjsonに変換
        return JsonConvert.SerializeObject(this, Formatting.None);
    }

    /// サーバーから送信してきたJSONデータを配列データに変換
    public static Dictionary<string, PlayerActionData> FromJson(JObject jsonHash, int roomNo)
    {
        // 戻り値のDictionaryの初期化
        var playerActionHash = new Dictionary<string, PlayerActionData>();

        // jsonの中に該当のルーム番号の情報がなければ空のDictionaryを返却
        if (Convert.ToInt32(jsonHash["room_id"]) != roomNo)
        {
            return playerActionHash;
        }

        // ルームの中にユーザ情報が含まれているのでPlayerActionData型に変換
        var PlayerActionData = new PlayerActionData
        {
            user = jsonHash["user"].ToString(),
            action = jsonHash["action"].ToString(),
            rote_y = float.Parse(jsonHash["rote_y"].ToString()),
            spead= float.Parse(jsonHash["spead"].ToString()),
            direction= jsonHash["direction"].ToString(),
        };
        playerActionHash.Add(PlayerActionData.user, PlayerActionData);
        return playerActionHash;

    }
}
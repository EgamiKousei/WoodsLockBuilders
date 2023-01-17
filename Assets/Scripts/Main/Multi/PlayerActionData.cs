using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PlayerActionData
{
    [JsonProperty("action")]
    public string action;

    [JsonProperty("room_id")]
    public int room_id;

    [JsonProperty("user")]
    public string user;

    [JsonProperty("pos_x")]
    public float pos_x;

    [JsonProperty("pos_y")]
    public float pos_y;

    [JsonProperty("pos_z")]
    public float pos_z;

    [JsonProperty("rote_y")]
    public float rote_y;

    [JsonProperty("color")]
    public string color;

    /// クライアントからサーバへ送信するデータをJSON形式に変換
    public string ToJson()
    {
        // オブジェクトをjsonに変換
        return JsonConvert.SerializeObject(this, Formatting.None);
    }

    /// サーバーから送信してきたJSONデータを配列データに変換
    public static Dictionary<string, PlayerActionData> FromJson(JObject jsonHash,int roomNo)
    {
        // 戻り値のDictionaryの初期化
        var playerActionHash = new Dictionary<string, PlayerActionData>();

        if (Convert.ToInt32(jsonHash["room_id"]) != roomNo)
        {
            return playerActionHash;
        }

        // ルームの中にユーザ情報が含まれているのでPlayerActionData型に変換
        var PlayerActionData = new PlayerActionData
        {
            user = jsonHash["user"].ToString(),
            action = jsonHash["action"].ToString(),
            pos_x = float.Parse(jsonHash["pos_x"].ToString()),
            pos_y = float.Parse(jsonHash["pos_y"].ToString()),
            pos_z = float.Parse(jsonHash["pos_z"].ToString()),
            rote_y = float.Parse(jsonHash["rote_y"].ToString()),
            color = jsonHash["color"].ToString(),
        };
        playerActionHash.Add(PlayerActionData.user, PlayerActionData);
        return playerActionHash;

    }
}

public class RoomDataMulti
{
    [JsonProperty("action")]
    public string action;

    [JsonProperty("room_id")]
    public int room_id;

    [JsonProperty("data")]
    public string data;
    public string ToJson()
    {
        // オブジェクトをjsonに変換
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}
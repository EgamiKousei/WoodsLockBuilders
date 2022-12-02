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

    /// �N���C�A���g����T�[�o�֑��M����f�[�^��JSON�`���ɕϊ�
    public string ToJson()
    {
        // �I�u�W�F�N�g��json�ɕϊ�
        return JsonConvert.SerializeObject(this, Formatting.None);
    }

    /// �T�[�o�[���瑗�M���Ă���JSON�f�[�^��z��f�[�^�ɕϊ�
    public static Dictionary<string, PlayerActionData> FromJson(JObject jsonHash, int roomNo)
    {
        // �߂�l��Dictionary�̏�����
        var playerActionHash = new Dictionary<string, PlayerActionData>();

        // json�̒��ɊY���̃��[���ԍ��̏�񂪂Ȃ���΋��Dictionary��ԋp
        if (Convert.ToInt32(jsonHash["room_id"]) != roomNo)
        {
            return playerActionHash;
        }

        // ���[���̒��Ƀ��[�U��񂪊܂܂�Ă���̂�PlayerActionData�^�ɕϊ�
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
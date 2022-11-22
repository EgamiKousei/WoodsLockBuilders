using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMulti : MonoBehaviour
{
    private GameObject playerPrefab = null;     // プレイヤーのリソース(プレハブ)

    // 全プレイヤーの行動情報
    private Dictionary<string, PlayerActionData> PlayerActionMap;

    // 全プレイヤーのオブジェクト情報
    private readonly Dictionary<string, GameObject> playerObjectMap = new Dictionary<string, GameObject>();

    public static UnityAction<Dictionary<string, PlayerActionData>> recieveCompletedHandler;

    Rigidbody rb;

    /// 定期更新
    /// </summary>
    void Update()
    {
        // ユーザーの行動情報があったら同期処理を行い、ユーザーの行動情報を初期化
        if (PlayerActionMap != null)
        {
            Synchronaize();
            PlayerActionMap = null;
        }
    }

    void Start()
    {
        var otherNameText = transform.Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = PlayerData.PlayerName;

        // WebSocketのメッセージ受信メソッドの設定
        recieveCompletedHandler += OnReciveMessage;

        // 自プレイヤーの初期情報をWebSocketに送信
        Multicast.SendPlayerAction("connect", new Vector3(0,0.5f,-20f), transform.rotation.y);
    }

    /// <summary>
    ///  (ユーザーの行動情報)受信メソッド
    /// </summary>
    /// <param name="synchronizeData"></param>
    private void OnReciveMessage(Dictionary<string, PlayerActionData> PlayerActionMap)
    {
        // 同期情報を取得
        this.PlayerActionMap = PlayerActionMap;
    }

    private void Synchronaize()
    {
        // 退出した他プレイヤーの検索
        /*List<string> otherPlayerNameList = new List<string>(playerObjectMap.Keys);
        foreach (var otherPlayerName in otherPlayerNameList)
        {
            // 退出したプレイヤーの削除
            if (!PlayerActionMap.ContainsKey(otherPlayerName))
            {
                Destroy(playerObjectMap[otherPlayerName]);
                playerObjectMap.Remove(otherPlayerName);
            }
        }*/
        // プレイヤーの位置を更新
        foreach (var playerAction in PlayerActionMap.Values)
        {            // 自分は移動済みなのでスルー
            if (PlayerData.PlayerName == playerAction.user)
            {
                continue;
            }
            // 入室中の他プレイヤーの移動
            if (playerObjectMap.ContainsKey(playerAction.user))
            {
                switch (playerAction.action)
                {
                    case "jump":
                        rb = playerObjectMap[playerAction.user].GetComponent<Rigidbody>();
                        rb.AddForce(transform.up * PlayerManager.JumpGravi, ForceMode.Impulse);
                        break;
                    default:
                        playerObjectMap[playerAction.user].transform.position = new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z);

                        //ローテーションの追加
                        var tes = playerObjectMap[playerAction.user].transform.rotation;
                        tes.y = playerAction.rote_y;
                        playerObjectMap[playerAction.user].transform.rotation = tes;
                        break;
                }

                // 入室中した他プレイヤーの生成
            }
            else
            {
                // 他プレイヤーの作成
                var player = MakePlayer(new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z), playerAction.user);

                // 他プレイヤーリストへの追加
                playerObjectMap.Add(playerAction.user, player);
            }
        }
    }

    /// <summary>
    /// プレイヤーを作成
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    private GameObject MakePlayer(Vector3 pos, string name)
    {
        // プレイヤーのリソース(プレハブ)を取得 ※初回のみ
        playerPrefab = playerPrefab ?? (GameObject)Resources.Load("SphPlayer");

        // プレイヤーを生成
        var player = Instantiate(playerPrefab, pos, Quaternion.identity);

        // プレイヤーのネームプレートの設定
        var otherNameText = player.transform.Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = name;
        return player;
    }
}
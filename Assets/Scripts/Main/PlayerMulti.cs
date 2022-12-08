using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    Animator anim;
    private CancellationTokenSource _cts;

    private Thread rcvThread; //受信用スレッド

    private void Awake()
    {
        rcvThread = new Thread(new ThreadStart(Synchronaize));//受信スレッド生成
        rcvThread.Start();//受信スレッド開始*
    }

    // 定期更新
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

    //  (ユーザーの行動情報)受信メソッド
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
                anim = playerObjectMap[playerAction.user].GetComponent<Animator>();
                rb = playerObjectMap[playerAction.user].GetComponent<Rigidbody>();
                switch (playerAction.action)
                {
                    case "Jump":
                        anim.SetBool("Jump", true);
                        //anim.SetBool("Attack", false);
                        rb.AddForce(transform.up * PlayerManager.JumpGravi, ForceMode.Impulse);
                        StartCoroutine(JumpEnd(anim));
                        break;
                    case "MoveEnd":
                        anim.SetBool("Move", false);
                        break;
                    case "logout":
                        var otherColor = playerObjectMap[playerAction.user].transform.Find("Body_08b").gameObject;
                        //Destroy(otherColor.GetComponent<SkinnedMeshRenderer>().materials[1].shader);
                        Destroy(playerObjectMap[playerAction.user]);
                        playerObjectMap.Remove(playerAction.user);
                        PlayerData.NameList.Remove(playerAction.user);
                        break;
                    case "Move":
                        //ローテーションの追加
                        var tes = playerObjectMap[playerAction.user].transform.rotation;
                        tes.y = playerAction.rote_y;
                        playerObjectMap[playerAction.user].transform.rotation = tes;
                        playerObjectMap[playerAction.user].transform.position = new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z);
                        anim.SetBool("Move", true);
                        break;
                    case "Attack":
                        anim.SetBool("Attack", true);
                        StartCoroutine(AttackEnd(anim));
                        break;
                }

                // 入室中した他プレイヤーの生成
            }
            else
            {
                // 他プレイヤーの作成
                var player = MakePlayer(new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z), playerAction.user, playerAction.action);

                // 他プレイヤーリストへの追加
                playerObjectMap.Add(playerAction.user, player);
                PlayerData.NameList.Add(playerAction.user);
            }
        }
    }
    private IEnumerator AttackEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.45f);
        anim.SetBool("Attack", false);
    }
    private IEnumerator JumpEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.8f);
        anim.SetBool("Jump", false);
    }

    // プレイヤーを作成
    private GameObject MakePlayer(Vector3 pos, string name,string color)
    {
        // プレイヤーのリソース(プレハブ)を取得 ※初回のみ
        playerPrefab = playerPrefab ?? (GameObject)Resources.Load("OtherPlayer");

        // プレイヤーを生成
        var player = Instantiate(playerPrefab, pos, Quaternion.identity);

        // プレイヤーのネームプレートの設定
        var otherNameText = player.transform.Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = name;

        //プレイヤーの色の設定

        //var otherColor = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/HEAD_CONTAINER/Head_08b").gameObject;
        var otherColor = player.transform.Find("Body_08b").gameObject;
        var sh = otherColor.GetComponent<SkinnedMeshRenderer>().materials[1].shader;
        
        Material mat = new Material(sh);
        float r = (Convert.ToInt32(color, 16) >> 16) & 0xff;
        float g = (Convert.ToInt32(color, 16) >> 8) & 0xff;
        float b = Convert.ToInt32(color, 16) & 0xff;
        mat.color =
           new Color(r / 255, g / 255, b / 255);
        Debug.Log("create");
        
        /*otherColor.GetComponent<MeshRenderer>().materials[1] = mat;
        otherColor = player.transform.Find("Body_08b").gameObject;
        otherColor.GetComponent<MeshRenderer>().materials[1] = mat;
        otherColor = player.transform.Find("shield_12").gameObject;*/
        otherColor.GetComponent<SkinnedMeshRenderer>().materials[1] = mat;
        return player;
    }

    private void OnApplicationQuit()
    {
        Multicast.SendPlayerAction("logout", Vector3.zero, 0.0f);
        rcvThread.Abort();
        Debug.Log("OnApplicationQuit");
    }
}
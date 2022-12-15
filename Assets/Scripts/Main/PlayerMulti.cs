using System;
using System.Collections;
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

    // 全プレイヤーの座標情報
    private Dictionary<string, Transform> playerTransform = new Dictionary<string, Transform>();

    // 全プレイヤーのアニメーター情報
    private Dictionary<string, Animator> playerAnim = new Dictionary<string, Animator>();

    public static UnityAction<Dictionary<string, PlayerActionData>> recieveCompletedHandler;

    Rigidbody rb;

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
        Multicast.SendPlayerAction("connect", new Vector3(0, 0.5f, -20f), transform.rotation.y);
    }

    //  (ユーザーの行動情報)受信メソッド
    private void OnReciveMessage(Dictionary<string, PlayerActionData> PlayerActionMap)
    {
        // 同期情報を取得
        this.PlayerActionMap = PlayerActionMap;
    }

    private void Synchronaize()
    {
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
                    case "Jump":
                        rb = playerObjectMap[playerAction.user].GetComponent<Rigidbody>();
                        playerAnim[playerAction.user].SetBool(PlayerManager.jumpParamHash, true);
                        rb.AddForce(playerTransform[playerAction.user].up * PlayerManager.JumpGravi, ForceMode.Impulse);
                        StartCoroutine(JumpEnd(playerAnim[playerAction.user]));
                        break;
                    case "MoveEnd":
                        playerAnim[playerAction.user].SetBool(PlayerManager.moveParamHash, false);
                        break;
                    case "logout":
                        var otherColor = playerTransform[playerAction.user].Find("Body_08b").gameObject;
                        Destroy(otherColor.GetComponent<SkinnedMeshRenderer>().materials[1].shader);
                        Destroy(playerObjectMap[playerAction.user]);
                        //オブジェクトリストとオブジェクトリスト
                        playerObjectMap.Remove(playerAction.user);
                        PlayerData.NameList.Remove(playerAction.user);
                        //座標リストとアニメーションリスト
                        playerTransform.Remove(playerAction.user);
                        playerAnim.Remove(playerAction.user);
                        break;
                    case "Move":
                        var tes = playerTransform[playerAction.user].localRotation;
                        tes.y = playerAction.rote_y;
                        playerTransform[playerAction.user].localRotation = tes;
                        playerTransform[playerAction.user].position = new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z);
                        if(playerAnim[playerAction.user].GetBool(PlayerManager.moveParamHash)==false)
                        playerAnim[playerAction.user].SetBool(PlayerManager.moveParamHash, true);
                        break;
                    case "Attack":
                        playerAnim[playerAction.user].SetBool(ActionManager.attackParamHash, true);
                        StartCoroutine(AttackEnd(playerAnim[playerAction.user]));
                        break;
                }

                // 入室中した他プレイヤーの生成
            }
            else
            {
                // 他プレイヤーの作成
                var player = MakePlayer(new Vector3(playerAction.pos_x, playerAction.pos_y, playerAction.pos_z), playerAction.user, playerAction.color);

                // 他プレイヤーリストへの追加
                playerObjectMap.Add(playerAction.user, player);
                PlayerData.NameList.Add(playerAction.user);
            }
        }
    }
    private IEnumerator AttackEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.45f);
        anim.SetBool(ActionManager.attackParamHash, false);
    }
    private IEnumerator JumpEnd(Animator anim)
    {
        yield return new WaitForSeconds(0.7f);
        anim.SetBool(PlayerManager.jumpParamHash, false);
    }

    // プレイヤーを作成
    private GameObject MakePlayer(Vector3 pos, string name,string color)
    {
        // プレイヤーのリソース(プレハブ)を取得 ※初回のみ
        playerPrefab = playerPrefab ?? (GameObject)Resources.Load("OtherPlayer");

        // プレイヤーを生成
        var player = Instantiate(playerPrefab, pos, Quaternion.identity);

        //座標リストとアニメーションリスト
        playerTransform.Add(name, player.transform);
        playerAnim.Add(name, player.GetComponent<Animator>());

        // プレイヤーのネームプレートの設定
        var otherNameText = playerTransform[name].Find("TxtUserName").gameObject;
        otherNameText.GetComponent<TextMesh>().text = name;

        //プレイヤーの色の設定
        var otherColor1 = playerTransform[name].Find("Body_08b").gameObject;
        var otherColor2 = playerTransform[name].Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/HEAD_CONTAINER/Head_08b").gameObject;
        var otherColor3 = playerTransform[name].Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand/L_shield_container/shield_12").gameObject;
        float r = (Convert.ToInt32(color, 16) >> 16) & 0xff;
        float g = (Convert.ToInt32(color, 16) >> 8) & 0xff;
        float b = Convert.ToInt32(color, 16) & 0xff;
        otherColor1.GetComponent<SkinnedMeshRenderer>().materials[1].color = new Color(r / 255, g / 255, b / 255);
        otherColor2.GetComponent<MeshRenderer>().materials[1].color = new Color(r / 255, g / 255, b / 255);
        otherColor3.GetComponent<MeshRenderer>().materials[1].color = new Color(r / 255, g / 255, b / 255);
        return player;
    }

    private void OnApplicationQuit()
    {
        Multicast.SendPlayerAction("logout", Vector3.zero, 0.0f);
        Resources.UnloadUnusedAssets();
    }
}
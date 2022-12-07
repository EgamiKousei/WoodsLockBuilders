using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MassageData
{
    [JsonProperty("ipAd")]
    public string ipAd;

    [JsonProperty("action")]
    public string action;

    [JsonProperty("message")]
    public string message;
}

public class LoginMulti : MonoBehaviour
{
    public UdpClient udpClient;
    public static int ClientPort = 9000;

    private string add;
    private Thread rcvThread; //受信用スレッド

    public Text hostId,name;

    void Awake()
    {
        // ホスト名からIPアドレスを取得する
        IPAddress[] adrList = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in adrList)
        {
            add = address.ToString();
            Debug.Log(add);
        }
        udpClient = new UdpClient(ClientPort);
        //メッセージを受け取る構えをする
        udpClient.BeginReceive(OnReceived, udpClient);
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);
        string json = Encoding.UTF8.GetString(getByte);
        Debug.Log(json);
                JObject deserialized = JObject.Parse(json);
                switch (deserialized["action"].ToString())
                {
                    case "login":
                        //同じ名前の人がいないか判定、ルームデータ受け渡し
                        if (!PlayerData.NameList.Contains(deserialized["name"].ToString()))
                        {
                            Debug.Log(deserialized["name"].ToString() + "がログイン");
                            PlayerData.NameList.Add(deserialized["message"].ToString());
                            //var message = string.Join(",", PlayerData.MapList);
                            SendPlayerAction("dataSend", deserialized["ipAd"].ToString(), IPAddress.Parse(add), "message");
                        }
                        break;
                    case "dataSend":
                        //ルームデータ受け取り
                        //PlayerData.NameList = deserialized["message"].ToString().Split(',').ToList();
                        GetComponent<LoginManager>().SpawnDoor();//受けとり後
                        var message = string.Join(",", PlayerData.NameList);
                        Debug.Log(message);
                        break;
                }
            
    }
    public void Login()
    {
        if (name.text == "")
            Debug.Log("名前が未入力");
        else
        {
            PlayerData.PlayerName = name.text;
            GetComponent<LoginManager>().SpawnDoor();
            PlayerData.NameList.Add(name.text);
        }
    }
    public void SendLogin()
    {
        if (name.text == "")
            Debug.Log("名前が未入力");
        else if (hostId.text == "")
            Debug.Log("ルームIDが未入力");
        else
        {
            PlayerData.PlayerName = name.text;
            //ルームデータ受け取り要請
            SendPlayerAction("login", add, IPAddress.Parse(hostId.text), name.text);
            Debug.Log("受け取り要請");
        }
    }
    public void SendPlayerAction(string action, string ipAd, IPAddress hostId, string message) //文字列を送信用ポートから送信先ポートに送信
    {
        try
        {
            var userActionData = new MassageData
            {
                ipAd = ipAd,
                action = action,
                message = message,
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userActionData, Formatting.None));
            udpClient.Connect(hostId, ClientPort);
            udpClient.Send(sendBytes, 0);
        }
        catch { }
    }

    private void OnApplicationQuit() //送受信用ポートを閉じつつ受信用スレッドも廃止
    {
        try
        {
            udpClient.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
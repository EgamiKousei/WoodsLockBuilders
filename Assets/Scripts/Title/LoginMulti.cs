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
    private static Socket Socket;
    private static int Port;

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

        Port = 10000;

        startMulticast();

        rcvThread = new Thread(new ThreadStart(receive));//受信スレッド生成
        rcvThread.Start();//受信スレッド開始*
    }

    private void startMulticast()
    {
        try
        {
            Socket = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Dgram,
                                     ProtocolType.Udp);

            IPAddress localIPAddr = IPAddress.Parse(add);

            //IPAddress localIP = IPAddress.Any;
            EndPoint localEP = new IPEndPoint(localIPAddr, Port);

            Socket.Bind(localEP);
        }

        catch (Exception e)
        {
            Debug.Log(e);
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
            //ルームデータ受け取り要請　SendPlayerAction
            GetComponent<LoginManager>().SpawnDoor();//受けとって判定後
        }
    }

    public void receive() //受信スレッドで実行される関数
    {
        bool done = false;
        ASCIIEncoding ASCII = new ASCIIEncoding();
        EndPoint remote_endpoint = new IPEndPoint(IPAddress.Any, 0);
        try
        {
            while (!done)
            {
                byte[] data = new byte[200];
                Socket.ReceiveFrom(data, ref remote_endpoint);
                var json = Encoding.UTF8.GetString(data);
                JObject deserialized = JObject.Parse(json);
                switch (deserialized["action"].ToString())
                {
                    case "login":
                        //ルームデータ、名前リスト受け渡し
                        break;
                    case "dataSend":
                        //ルームデータ、名前リスト受け取り。同じ名前の人がいないか判定
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public static void SendPlayerAction(string action, string ipAd, IPAddress hostId, string message) //文字列を送信用ポートから送信先ポートに送信
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
            IPEndPoint ClientOriginatordest = new IPEndPoint(hostId, Port);
            Socket.SendTo(sendBytes, ClientOriginatordest);
        }
        catch { }
    }

    private void OnApplicationQuit() //送受信用ポートを閉じつつ受信用スレッドも廃止
    {
        try
        {
            Socket.Close();
            rcvThread.Abort();
        }
        catch { }
    }
}
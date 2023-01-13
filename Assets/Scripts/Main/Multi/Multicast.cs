using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Multicast : MonoBehaviour
{
    private static IPAddress mcastAddress;
    private static int mcastPort;
    private static Socket mcastSocket;
    private static MulticastOption mcastOption;

    private string add;
    private Thread rcvThread; //受信用スレッド
    void Awake()
    {
        // ホスト名からIPアドレスを取得する
        IPAddress[] adrList = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in adrList)
        {
            add = address.ToString();
        }

        mcastAddress = IPAddress.Parse("224.168.100.2");
        mcastPort = 11000;

        // Start a multicast group.
        startMulticast();

        rcvThread = new Thread(new ThreadStart(receive));//受信スレッド生成
        rcvThread.Start();//受信スレッド開始*
    }
    private void startMulticast()
    {
        try
        {
            mcastSocket = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Dgram,
                                     ProtocolType.Udp);

            IPAddress localIPAddr = IPAddress.Parse(add);

            //IPAddress localIP = IPAddress.Any;
            EndPoint localEP = new IPEndPoint(localIPAddr, mcastPort);

            mcastSocket.Bind(localEP);

            mcastOption = new MulticastOption(mcastAddress, localIPAddr);

            mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                                        SocketOptionName.AddMembership,
                                        mcastOption);
            MulticastOptionProperties();
        }

        catch(Exception e) {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        SendPlayerAction("login",new Vector3(0f,0f,-20f), 0.0f);
    }

    private void MulticastOptionProperties()
    {
        if (mcastOption != null)
        {
            Debug.Log(mcastOption.Group+" , "+ mcastOption.LocalAddress);
        }
        else
        {
            Debug.Log("current multicast group is: none , current multicast local address is: none");
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
                byte[] data = new byte[200000];
                mcastSocket.ReceiveFrom(data, ref remote_endpoint);
                var json = Encoding.UTF8.GetString(data);
                JObject deserialized = JObject.Parse(json);
                switch (deserialized["action"].ToString())
                {
                    case "set":
                        PlayerData.SetRoom(deserialized["data"].ToString(),PlayerData.RoomPash);
                        break;
                    default:
                        var allUserActionHash = PlayerActionData.FromJson(deserialized);
                        PlayerMulti.recieveCompletedHandler?.Invoke(allUserActionHash);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void SendRoom(string data) //文字列を送信用ポートから送信先ポートに送信
    {
        try
        {
            var userActionData = new RoomDataMulti
            {
                action = "set",
                data = data,
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(userActionData.ToJson());
            IPEndPoint ClientOriginatordest = new IPEndPoint(mcastAddress, mcastPort);
            mcastSocket.SendTo(sendBytes, ClientOriginatordest);
        }
        catch { }
    }

    public static void SendPlayerAction(string action, Vector3 pos,float rote_y) //文字列を送信用ポートから送信先ポートに送信
    {
        try
        {
            var userActionData = new PlayerActionData
            {
                action = action,
                user = PlayerData.PlayerName,
                pos_x = pos.x,
                pos_y = pos.y,
                pos_z = pos.z,
                rote_y = rote_y,
                color = PlayerData.SaveData["color"],
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(userActionData.ToJson());
            IPEndPoint ClientOriginatordest = new IPEndPoint(mcastAddress, mcastPort);
            mcastSocket.SendTo(sendBytes, ClientOriginatordest);
        }
        catch { }
    }

    private void OnApplicationQuit() //送受信用ポートを閉じつつ受信用スレッドも廃止
    {
        try
        {
            mcastSocket.Close();
            rcvThread.Abort();
        }
        catch { }
    }
}
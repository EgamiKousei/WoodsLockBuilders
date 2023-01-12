using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class LoginClient : MonoBehaviour
{
    public UdpClient udpClient;
    public static int ClientPort = 9000;

    bool LoginDoor = false;
    public GameObject Server;

    public void Update()
    {
        if (LoginDoor == true)
        {
            Server.GetComponent<LoginManager>().SpawnDoor();//受けとり後
            LoginDoor = false;
        }
    }

    void Start()
    {
        udpClient = new UdpClient(ClientPort);
        //メッセージを受け取る構えをする
        udpClient.BeginReceive(OnReceived, udpClient);
        //DontDestroyOnLoad(gameObject);
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);
        string message = Encoding.UTF8.GetString(getByte);
        if (message.IndexOf("{") < 1)
        {
            if (!PlayerData.NameList.Contains(message))
            {
                Debug.Log(message + "がログイン");
                PlayerData.NameList.Add(message);
                //ルームデータ受け渡し
                var messageData = Encoding.UTF8.GetBytes(LoginServer.datastr);
                udpClient.Connect(ipEnd.Address.ToString(), ClientPort);
                udpClient.Send(messageData, messageData.Length);
                Debug.Log("ルーム情報受け渡し"+ ipEnd.Address.ToString());
            }
        }
        else
        {
            Debug.Log("ルーム情報受け取り");
            StreamWriter wreiter = new StreamWriter(LoginServer.PlayPash, false);
            wreiter.WriteLine(message);
            wreiter.Flush();
            wreiter.Close();
            LoginDoor = true;
        }
        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnApplicationQuit()
    {
        udpClient.Close();
    }
}

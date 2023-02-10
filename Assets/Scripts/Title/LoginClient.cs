using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LoginData
{
    [JsonProperty("action")]
    public string action;

    [JsonProperty("data")]
    public string data;

    [JsonProperty("ip")]
    public string ip;
    public string ToJson()
    {
        // オブジェクトをjsonに変換
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}

public class LoginClient : MonoBehaviour
{
    public UdpClient udpClient;
    public static int ClientPort = 9000;

    void Start()
    {
        udpClient = new UdpClient(ClientPort);
        //メッセージを受け取る構えをする
        udpClient.BeginReceive(OnReceived, udpClient);
        DontDestroyOnLoad(gameObject);
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        var json = Encoding.UTF8.GetString(getByte);
        JObject deserialized = JObject.Parse(json);

        switch (deserialized["action"].ToString())
        {
            case "name":
                Debug.Log(deserialized["data"].ToString() + "がログイン");
                PlayerData.NameList.Add(deserialized["data"].ToString());
                string send = deserialized["ip"].ToString();

                udpClient.Connect(ipEnd.Address, ClientPort);
                //udpClient.Connect(IPAddress.Parse(send), ClientPort);
                var titleData = new LoginData
                {
                    action = "room",
                    data = LoginServer.datastr,
                    ip =LoginServer.ipAd,
                };
                byte[] sendBytes = Encoding.UTF8.GetBytes(titleData.ToJson());

                udpClient.Send(sendBytes, sendBytes.Length);
                Debug.Log("ルーム情報受け渡し");
                break;
            case "room":
                Debug.Log("ルーム情報受け取り");
                StreamWriter wreiter = new StreamWriter(LoginServer.PlayPash, false);
                wreiter.WriteLine(deserialized["data"].ToString());
                wreiter.Flush();
                wreiter.Close();
                Debug.Log("受け取り完了");
                LoginServer.LoginDoor = true;
                break;
        }
        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnApplicationQuit()
    {
        udpClient.Close();
    }
}

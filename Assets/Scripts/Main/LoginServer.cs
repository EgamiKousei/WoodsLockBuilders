using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;

/*public class MassageData
{
    [JsonProperty("ipAd")]
    public string ipAd;

    [JsonProperty("action")]
    public string action;

    [JsonProperty("message")]
    public string message;
}*/

public class LoginServer : MonoBehaviour
{
    public UdpClient client;
    public static string ipAd;

    public Text hostId, PlayerName;

    private void Start()
    {
        PlayerData.NameList.Clear();

        // ホスト名からIPアドレスを取得する
        IPAddress[] adrList = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in adrList)
        {
            ipAd = address.ToString();
            Debug.Log(ipAd);
        }

        //最初のポートの開設
        client = new UdpClient();
    }

    public void Login()
    {
        if (PlayerName.text == "")
            Debug.Log("名前が未入力");
        else
        {
            PlayerData.PlayerName = PlayerName.text;
            GetComponent<LoginManager>().SpawnDoor();
            PlayerData.NameList.Add(PlayerName.text);
        }
    }

    public void SendLogin()
    {
        if (PlayerName.text == "")
            Debug.Log("名前が未入力");
        else if (hostId.text == "")
            Debug.Log("ルームIDが未入力");
        else
        {
            PlayerData.PlayerName = PlayerName.text;
            //ルームデータ受け取り要請
            client.Connect(hostId.text, LoginClient.ClientPort);
            var message = Encoding.UTF8.GetBytes(PlayerName.text);
            client.Send(message, message.Length);
            Debug.Log("受け取り要請");
        }
    }

    private void OnApplicationQuit()
    {
        client.Close();
    }
}

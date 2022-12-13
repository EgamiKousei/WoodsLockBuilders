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

        // �z�X�g������IP�A�h���X���擾����
        IPAddress[] adrList = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in adrList)
        {
            ipAd = address.ToString();
            Debug.Log(ipAd);
        }

        //�ŏ��̃|�[�g�̊J��
        client = new UdpClient();
    }

    public void Login()
    {
        if (PlayerName.text == "")
            Debug.Log("���O��������");
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
            Debug.Log("���O��������");
        else if (hostId.text == "")
            Debug.Log("���[��ID��������");
        else
        {
            PlayerData.PlayerName = PlayerName.text;
            //���[���f�[�^�󂯎��v��
            client.Connect(hostId.text, LoginClient.ClientPort);
            var message = Encoding.UTF8.GetBytes(PlayerName.text);
            client.Send(message, message.Length);
            Debug.Log("�󂯎��v��");
        }
    }

    private void OnApplicationQuit()
    {
        client.Close();
    }
}

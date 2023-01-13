using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using System.IO;

public class LoginServer : MonoBehaviour
{
    public UdpClient client;
    public static string ipAd;

    public Text hostId, PlayerName;

    public static string PlayPash, SavePash,datastr;
    public static bool LoginDoor = false;


    public void Update()
    {
        if (LoginDoor == true)
        {
            GetComponent<LoginManager>().SpawnDoor();//�󂯂Ƃ��
            LoginDoor = false;
        }
    }

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
        IPEndPoint localEP =new IPEndPoint(IPAddress.Parse(ipAd), 8000);
        client = new UdpClient(localEP);
        
         PlayPash = Application.dataPath + "/PlayRoomData.json";
         SavePash = Application.dataPath + "/SaveRoomData.json";

        StreamReader reader = new StreamReader(SavePash, System.Text.Encoding.UTF8);
        datastr = reader.ReadToEnd();
        reader.Close();
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

            StreamWriter wreiter = new StreamWriter(PlayPash, false);
            wreiter.WriteLine(datastr);
            wreiter.Flush();
            wreiter.Close();
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
            var titleData = new LoginData
            {
                action = "name",
                data = PlayerName.text,
                ip = ipAd,
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(titleData.ToJson());
            client.Connect(hostId.text, LoginClient.ClientPort);
            client.Send(sendBytes, sendBytes.Length);
            //LoginSocket.SendPlayerAction("name", PlayerName.text,IPAddress.Parse(hostId.text));
            Debug.Log("�󂯎��v��");
        }
    }
    
     private void OnDestroy()
    {
        client.Close();
    }
}

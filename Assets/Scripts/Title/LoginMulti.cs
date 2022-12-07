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
    private Thread rcvThread; //��M�p�X���b�h

    public Text hostId,name;

    void Awake()
    {
        // �z�X�g������IP�A�h���X���擾����
        IPAddress[] adrList = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in adrList)
        {
            add = address.ToString();
            Debug.Log(add);
        }
        udpClient = new UdpClient(ClientPort);
        //���b�Z�[�W���󂯎��\��������
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
                        //�������O�̐l�����Ȃ�������A���[���f�[�^�󂯓n��
                        if (!PlayerData.NameList.Contains(deserialized["name"].ToString()))
                        {
                            Debug.Log(deserialized["name"].ToString() + "�����O�C��");
                            PlayerData.NameList.Add(deserialized["message"].ToString());
                            //var message = string.Join(",", PlayerData.MapList);
                            SendPlayerAction("dataSend", deserialized["ipAd"].ToString(), IPAddress.Parse(add), "message");
                        }
                        break;
                    case "dataSend":
                        //���[���f�[�^�󂯎��
                        //PlayerData.NameList = deserialized["message"].ToString().Split(',').ToList();
                        GetComponent<LoginManager>().SpawnDoor();//�󂯂Ƃ��
                        var message = string.Join(",", PlayerData.NameList);
                        Debug.Log(message);
                        break;
                }
            
    }
    public void Login()
    {
        if (name.text == "")
            Debug.Log("���O��������");
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
            Debug.Log("���O��������");
        else if (hostId.text == "")
            Debug.Log("���[��ID��������");
        else
        {
            PlayerData.PlayerName = name.text;
            //���[���f�[�^�󂯎��v��
            SendPlayerAction("login", add, IPAddress.Parse(hostId.text), name.text);
            Debug.Log("�󂯎��v��");
        }
    }
    public void SendPlayerAction(string action, string ipAd, IPAddress hostId, string message) //������𑗐M�p�|�[�g���瑗�M��|�[�g�ɑ��M
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

    private void OnApplicationQuit() //����M�p�|�[�g�����M�p�X���b�h���p�~
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
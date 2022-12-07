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

        Port = 10000;

        startMulticast();

        rcvThread = new Thread(new ThreadStart(receive));//��M�X���b�h����
        rcvThread.Start();//��M�X���b�h�J�n*
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
        }
    }

    public void receive() //��M�X���b�h�Ŏ��s�����֐�
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
                        //�������O�̐l�����Ȃ�������A���[���f�[�^�󂯓n��
                        if (!PlayerData.NameList.Contains(deserialized["name"].ToString()))
                        { 
                            PlayerData.NameList.Add(deserialized["message"].ToString());
                            //var message = string.Join(",", PlayerData.MapList);
                            SendPlayerAction("dataSend", deserialized["ipAd"].ToString(), IPAddress.Parse(add), "message");
                        }
                        break;
                    case "dataSend":
                        //���[���f�[�^�󂯎��
                        //PlayerData.NameList = deserialized["message"].ToString().Split(',').ToList();
                        GetComponent<LoginManager>().SpawnDoor();//�󂯂Ƃ��Ĕ����
                        var message = string.Join(",", PlayerData.NameList);
                        Debug.Log(message);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public static void SendPlayerAction(string action, string ipAd, IPAddress hostId, string message) //������𑗐M�p�|�[�g���瑗�M��|�[�g�ɑ��M
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

    private void OnApplicationQuit() //����M�p�|�[�g�����M�p�X���b�h���p�~
    {
        try
        {
            Socket.Close();
            rcvThread.Abort();
        }
        catch { }
    }
}
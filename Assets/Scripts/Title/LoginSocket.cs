using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TitleData
{
    [JsonProperty("action")]
    public string action;

    [JsonProperty("data")]
    public string data;
    public string ToJson()
    {
        // �I�u�W�F�N�g��json�ɕϊ�
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}

    public class LoginSocket : MonoBehaviour
{
    private static IPAddress mcastAddress;
    private static int mcastPort;
    private static Socket mcastSocket;
    private static MulticastOption mcastOption;

    private string add;
    private Thread rcvThread; //��M�p�X���b�h

    void Awake()
    {
        // �z�X�g������IP�A�h���X���擾����
        IPAddress[] adrList = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress address in adrList)
        {
            add = address.ToString();
            Debug.Log(add);
        }
        mcastAddress = IPAddress.Parse("224.168.100.1");
        mcastPort = 11001;

        // Start a multicast group.
        startMulticast();

        rcvThread = new Thread(new ThreadStart(receive));//��M�X���b�h����
        rcvThread.Start();//��M�X���b�h�J�n*
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

        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private void MulticastOptionProperties()
    {
        if (mcastOption != null)
        {
            Debug.Log(mcastOption.Group + " , " + mcastOption.LocalAddress);
        }
        else
        {
            Debug.Log("current multicast group is: none , current multicast local address is: none");
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
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
                byte[] data = new byte[2000000];
                mcastSocket.ReceiveFrom(data, ref remote_endpoint);
                var json = Encoding.UTF8.GetString(data);
                JObject deserialized = JObject.Parse(json);
                switch (deserialized["action"].ToString())
                {
                    case "name":
                        Debug.Log(deserialized["data"].ToString() + "�����O�C��");
                        PlayerData.NameList.Add(deserialized["data"].ToString());
                        SendPlayerAction("room", LoginServer.datastr);
                        Debug.Log("���[�����󂯓n��"+ LoginServer.datastr);
                        break;
                    case "room":
                        Debug.Log("���[�����󂯎��");
                        StreamWriter wreiter = new StreamWriter(LoginServer.PlayPash, false);
                        wreiter.WriteLine(deserialized["data"].ToString());
                        wreiter.Flush();
                        wreiter.Close();
                        Debug.Log("�󂯎�芮��");
                        LoginServer.LoginDoor = true;
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public static void SendPlayerAction(string action, string data) //������𑗐M�p�|�[�g���瑗�M��|�[�g�ɑ��M
    {
        try
        {
            var titleData = new TitleData
            {
                action = action,
                data = data,
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(titleData.ToJson());
            IPEndPoint ClientOriginatordest = new IPEndPoint(mcastAddress, mcastPort);
            mcastSocket.SendTo(sendBytes, ClientOriginatordest);
        }
        catch { }
    }

    private void OnApplicationQuit() //����M�p�|�[�g�����M�p�X���b�h���p�~
    {
        try
        {
            mcastSocket.Close();
            rcvThread.Abort();
        }
        catch { }
    }
}
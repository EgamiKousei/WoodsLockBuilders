using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Multicast : MonoBehaviour
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
        }

        mcastAddress = IPAddress.Parse("224.168.100.2");
        mcastPort = 11000;

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

        catch(Exception e) {
            Debug.Log(e);
        }
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
                mcastSocket.ReceiveFrom(data, ref remote_endpoint);
                var json = Encoding.UTF8.GetString(data);
                JObject deserialized = JObject.Parse(json);
                Debug.Log(deserialized);
                var allUserActionHash = PlayerActionData.FromJson(deserialized, 1);
                PlayerMulti.recieveCompletedHandler?.Invoke(allUserActionHash);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public static void SendPlayerAction(string action, Vector3 pos,float rote_y) //������𑗐M�p�|�[�g���瑗�M��|�[�g�ɑ��M
    {
        try
        {
            var userActionData = new PlayerActionData
            {
                action = action,
                room_id = 1,
                user = PlayerData.PlayerName,
                pos_x = pos.x,
                pos_y = pos.y,
                pos_z = pos.z,
                rote_y = rote_y,
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(userActionData.ToJson());
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
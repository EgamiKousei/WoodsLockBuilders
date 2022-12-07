using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

public class LoginClient : MonoBehaviour
{
    public UdpClient udpClient;
    public static int ClientPort = 9000;

    void Start()
    {
        udpClient = new UdpClient(ClientPort);
        //���b�Z�[�W���󂯎��\��������
        udpClient.BeginReceive(OnReceived, udpClient);
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);
        string message = Encoding.UTF8.GetString(getByte);
        Debug.Log(message);
        if (message.IndexOf(",") < 1)
        {
            if (!PlayerData.NameList.Contains(message))
            {
                Debug.Log(message + "�����O�C��");
                PlayerData.NameList.Add(message);
                //���[���f�[�^�󂯓n��
                //var message = string.Join(",", PlayerData.MapList);
                udpClient.Connect(ipEnd.Address.ToString(), ClientPort);
                udpClient.Send(Encoding.UTF8.GetBytes("mes,sage"), 0);
            }
        }
        else
        {   
            //���[���f�[�^�󂯎��
            //PlayerData.NameList = deserialized["message"].ToString().Split(',').ToList();
            GetComponent<LoginManager>().SpawnDoor();//�󂯂Ƃ��
            Debug.Log("���[�����󂯎��");
        }
        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnApplicationQuit()
    {
        udpClient.Close();
    }
}

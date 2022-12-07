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

public class LoginMulti : MonoBehaviour
{
    public UdpClient udpClient;
    public UdpClient client;
    public static int ClientPort = 9000;

    private string add;
    private Thread rcvThread; //受信用スレッド

    public Text hostId,nameT;

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
                        //同じ名前の人がいないか判定、ルームデータ受け渡し
                        if (!PlayerData.NameList.Contains(deserialized["name"].ToString()))
                        {
                            Debug.Log(deserialized["name"].ToString() + "がログイン");
                            PlayerData.NameList.Add(deserialized["message"].ToString());
                            //var message = string.Join(",", PlayerData.MapList);
                            SendPlayerAction(udpClient,"dataSend", deserialized["ipAd"].ToString(), IPAddress.Parse(add), "message");
                        }
                        break;
                    case "dataSend":
                        //ルームデータ受け取り
                        //PlayerData.NameList = deserialized["message"].ToString().Split(',').ToList();
                        GetComponent<LoginManager>().SpawnDoor();//受けとり後
                        var message = string.Join(",", PlayerData.NameList);
                        Debug.Log(message);
                        break;
                }
            
    }
    public void Login()
    {
        if (nameT.text == "")
            Debug.Log("名前が未入力");
        else
        {
            PlayerData.PlayerName = nameT.text;
            GetComponent<LoginManager>().SpawnDoor();
            PlayerData.NameList.Add(nameT.text);
        }
    }
    public void SendLogin()
    {
        if (nameT.text == "")
            Debug.Log("名前が未入力");
        else if (hostId.text == "")
            Debug.Log("ルームIDが未入力");
        else
        {
            PlayerData.PlayerName = nameT.text;
            //ルームデータ受け取り要請
            SendPlayerAction(client,"login", add, IPAddress.Parse(hostId.text), nameT.text);
            Debug.Log("受け取り要請");
        }
    }
    public void SendPlayerAction(UdpClient udp, string action, string ipAd, IPAddress hostId, string message) //文字列を送信用ポートから送信先ポートに送信
    {
        try
        {
            /*var userActionData = new MassageData
            {
                ipAd = ipAd,
                action = action,
                message = message,
            };
            byte[] sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userActionData, Formatting.None));
            udp.Connect(hostId, ClientPort);
            udp.Send(sendBytes, 0);*/
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnApplicationQuit() //送受信用ポートを閉じつつ受信用スレッドも廃止
    {
        try
        {
            udpClient.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
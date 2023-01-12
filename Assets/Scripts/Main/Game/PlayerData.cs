using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public itemData[] data;
}

[System.Serializable]
public class itemData
{
    public string name;
    public string category;
    public int invnum;
    public int bagnum;
    public int invno;
    public int bagno;
}

[System.Serializable]
public class SaveData
{
    public int hp;
    public string color;
}

[System.Serializable]
public class RoomData
{
    public roomData[] data;
}

[System.Serializable]
public class roomData
{
    public string name;
    public float x;
    public float y;
    public float z;
    public float xr;
    public float yr;
    public float zr;
    public int num;
}
public class PlayerData : MonoBehaviour
{
    private string ItemDataPash;
    private string SavePash;
    private string RoomPash;

    //���O
    public static string PlayerName = "test2";

    //���O���X�g
    public static List<string> NameList = new List<string>();

    //�v���C�����f�[�^
    public static Dictionary<int, roomData> PlayMap = new Dictionary<int, roomData>();
    //���ԑуf�[�^�i���[���}�X�^�[���@�\�j

    //�v���C���[�f�[�^�@HP/�F
    public static Dictionary<string, string> SaveData = new Dictionary<string, string>();

    //�A�C�e���f�[�^�@�A�C�e����:�A�C�e����/���/���L��(�C���x���g��)/�z�u�ꏊ/���L��(�o�b�O)
    public static Dictionary<string, itemData> ItemBox = new Dictionary<string, itemData>();

    //�A�C�e����{�f�[�^�@�A�C�e����:�A�C�e����/����/�ϋv/�U����/�ޗ���1/�ޗ���2/�K�v��1/�K�v��2

    private void Awake()
    {
        ItemDataPash = Application.dataPath + "/ItemData.json";   // ItemData.json�܂ł̃p�X
        SavePash = Application.dataPath + "/SaveData.json";   // SaveData.json�܂ł̃p�X
        RoomPash = Application.dataPath + "/PlayRoomData.json";
        ItemData im = LoadFile(ItemDataPash);        // json�t�@�C�����[�h
        foreach (var i in im.data)
        {
            if (i.name != "")
                ItemBox.Add(i.name, i);
            else
                break;
        }
        SaveData sv = LoadSaveFile(SavePash);        // json�t�@�C�����[�h
        var json = JsonConvert.SerializeObject(sv);
        SaveData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        RoomData rm = LoadRoomFile(RoomPash);// json�t�@�C�����[�h
        foreach (var i in rm.data)
        {
            if (i.name != "")
                PlayMap.Add(i.num, i);
            else
                break;
        }
    }

    public ItemData LoadFile(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath);
        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<ItemData>(datastr);
    }

    public SaveData LoadSaveFile(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath, System.Text.Encoding.UTF8);
        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<SaveData>(datastr);
    }

    public RoomData LoadRoomFile(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath, System.Text.Encoding.UTF8);
        string datastr = reader.ReadToEnd();
        reader.Close();
        return JsonUtility.FromJson<RoomData>(datastr);
    }

    private void OnApplicationQuit()
    {
        StreamWriter wreiter = new StreamWriter(SavePash, false);
        var data = new SaveData
        {
            hp = int.Parse(SaveData["hp"]),
            color = SaveData["color"],
        };

        var jsonstr = JsonUtility.ToJson(data);
        wreiter.WriteLine(jsonstr);
        wreiter.Flush();
        wreiter.Close();

        wreiter = new StreamWriter(ItemDataPash, false);

        itemData[] Idata=new itemData[20];
        int n = 0;
        foreach (var i in ItemBox.Values)
        {
            Idata[n]= i;
            n++;
        }
        var data2 = new ItemData
        {
            data= Idata
        };
        jsonstr = JsonUtility.ToJson(data2);
            wreiter.WriteLine(jsonstr);
            wreiter.Flush();
            wreiter.Close();

        wreiter = new StreamWriter(RoomPash, false);

        roomData[] Rdata = new roomData[255];
        n = 0;
        foreach (var i in PlayMap.Values)
        {
            Rdata[n] = i;
            n++;
        }
        var data3 = new RoomData
        {
            data = Rdata
        };
        jsonstr = JsonUtility.ToJson(data3);
        wreiter.WriteLine(jsonstr);
        wreiter.Flush();
        wreiter.Close();
    }
}
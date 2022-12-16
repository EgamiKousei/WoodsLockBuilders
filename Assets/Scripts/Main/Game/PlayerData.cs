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
}

[System.Serializable]
public class SaveData
{
    public int hp;
    public string color;
}
public class PlayerData : MonoBehaviour
{
    private string ItemDataPash;
    private string SavePash;

    //名前
    public static string PlayerName = "test2";

    //名前リスト
    public static List<string> NameList = new List<string>();

    //プレイ部屋データ
    //時間帯データ（ルームマスター中機能）

    //個人部屋データ　マップ配置リスト

    //プレイヤーデータ　HP/色
    public static Dictionary<string, string> SaveData = new Dictionary<string, string>();

    //アイテムデータ　アイテム名:アイテム名/種類/所有数(インベントリ)/配置場所/所有数(バッグ)
    public static Dictionary<string, itemData> ItemBox = new Dictionary<string, itemData>();

    //アイテム基本データ　アイテム名:アイテム名/説明/耐久/攻撃力/材料名1/材料名2/必要数1/必要数2

    private void Awake()
    {
        ItemDataPash = Application.dataPath + "/ItemData.json";   // ItemData.jsonまでのパス
        SavePash = Application.dataPath + "/SaveData.json";   // SaveData.jsonまでのパス
        ItemData im = LoadFile(ItemDataPash);        // jsonファイルロード
        foreach (var i in im.data)
        {
            ItemBox.Add(i.name, i);
        }
        SaveData sv = LoadSaveFile(SavePash);        // jsonファイルロード
        var json = JsonConvert.SerializeObject(sv);
        SaveData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
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

        itemData[] Idata=new itemData[9];
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
    }
}
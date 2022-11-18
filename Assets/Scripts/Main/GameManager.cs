using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using UnityEditor;

public class GameManager : MonoBehaviour
{

}


/*
[System.Serializable]
public class ItemData
{
    public itemData[] data;
}


[System.Serializable]
public class itemData
{
    public int invnum;
    public int bagnum;
    public int invno;

}

public class GameManager : MonoBehaviour
{
    //　アイテムデータベース
    [SerializeField]
    private ItemDataBase itemDataBase;

    // インベントリ配列
    public string[] Inventory;

    // インベントリオブジェクト
    GameObject invNo;

    // 一旦格納するための変数
    GameObject rObj;

    public GameObject maxMap;   // 全体マップのパネル
    public GameObject bagList;  // バッグ一覧のパネル
    public GameObject pSetting; // 設定のパネル
    public GameObject wList;    // 武器リスト
    public GameObject iList;    // アイテムリスト
    public GameObject hList;    // 家具リスト
    public GameObject wPanel;   // 武器リストパネル
    public GameObject iPanel;   // アイテムリストパネル
    public GameObject hPanel;   // 家具リストパネル
    public GameObject pSet;     // プレイヤー設定のパネル
    public GameObject rSet;     // ルーム設定のパネル
    public GameObject oSet;     // オプション設定のパネル
    public GameObject nSet;     // 名前設定のパネル
    public GameObject hSet;     // 髪型設定のパネル
    public GameObject cSet;     // 髪色設定のパネル
    public GameObject aSet;     // 服装設定のパネル
    public GameObject rName;    // ルーム名のテキスト
    public GameObject rID;      // ルームIDのテキスト
    public GameObject onButton; // ONボタン
    public GameObject offButton;// OFFボタン
    public int itemType;        // アイテムの種類
    InputField bgmField;        // BGMを数値で調整するためのInputField
    InputField seField;         // SEを数値で調整するためのInputField
    InputField brightField;     // 明るさを数値で調整するためのInputField
    Slider seSlider;            // SEを調整するためのスライダー
    Slider bgmSlider;           // BGMを調整するためのスライダー
    Slider brightSlider;        // 明るさを調整するためのスライダー
    GameObject player;          // プレイヤー
    player1 plm;                // プレイヤースクリプト
    GameObject audioManager;    // BGM用オブジェクト
    GameObject seManager;       // SE用オブジェクト
    GameObject brightPanel;     // 明るさ変更用パネル
    bool isMax = false;         // マップを全体表示しているかどうか
    public bool isMenu = false;     // メニュー画面などが開いているかどうか
    public GameObject closePanel;   // 終了パネル 
    string datapath;            // Jsonデータを参照さるためのパス
    static ItemData im;        // Jsonデータ格納変数

    //public bool isItemMov = false;

    private void Awake()
    {
        //datapath = Application.dataPath + "/ItemData.json";   // Timatu.jsonまでのパス
    }

    // Start is called before the first frame update
    void Start()
    {
        // カーソルを非表示にして中央に固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // 様々な要素の取得
        player = GameObject.Find("Player");
        plm = player.GetComponent<player1>();
        audioManager = GameObject.Find("AudioManager");
        seManager = GameObject.Find("SEManager");
        brightPanel = GameObject.Find("BrightPanel");
        itemType = 1;

        im = new ItemData();


        // jsonファイルロード
        im = LoadFile(datapath);

        // インベントリとバッグにアイテムを格納する処理
        for (int i = 0; i < Inventory.Length; i++)
        {
            GetItem(Inventory[i]).SetInv(im.data[i].invnum);
            GetItem(Inventory[i]).SetBag(im.data[i].bagnum);
            GetItem(Inventory[i]).SetInvNo(im.data[i].invno);

            if (GetItem(Inventory[i]).GetInvNum() != 0)
            {
                invNo = GameObject.Find(GetItem(Inventory[i]).GetInvNo().ToString());
                GameObject obj = (GameObject)Resources.Load(Inventory[i] + "Image");
                Instantiate(obj, invNo.transform);
                this.gameObject.GetComponent<ItemManager>().Inventory[GetItem(Inventory[i]).GetInvNo() - 1] = GetItem(Inventory[i]).GetCode();
            }
            
            if(GetItem(Inventory[i]).GetBagNum() != 0)
            {
                GameObject obj = (GameObject)Resources.Load(Inventory[i] + "Image");
                GameObject imgPt = GameObject.Find("NewImage");
                if (GetItem(Inventory[i]).GetKindNo() == 1)
                {
                    rObj = Instantiate(obj, imgPt.transform);
                    rObj.gameObject.transform.parent = wPanel.transform;
                }
                else if(GetItem(Inventory[i]).GetKindNo() == 2)
                {
                    rObj = Instantiate(obj, imgPt.transform);
                    rObj.gameObject.transform.parent = iPanel.transform;
                }
                else
                {
                    rObj = Instantiate(obj, imgPt.transform);
                    rObj.gameObject.transform.parent = hPanel.transform;
                }
            }
        }

        // jsonファイルセーブ
        SaveFile(im);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMenu == true || plm.isArg == true)
        {
            // カーソルを表示して固定でなくする
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // カーソルを非表示にして中央に固定
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(Input.GetKeyDown(KeyCode.M) && isMenu == false)
        {
            // 全体マップを表示
            maxMap.SetActive(true);
            isMax = true;

            // プレイヤーを行動不能に
            plm.isMove = false;

            // 開いている状態に
            isMenu = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && isMenu == false)
        {
            // バッグを開く
            bagList.SetActive(true);

            // プレイヤーを行動不能に
            plm.isMove = false;

            // 開いている状態に
            isMenu = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && isMenu == false)
        {
            // 設定画面を開く
            pSetting.SetActive(true);

            // プレイヤーを行動不能に
            plm.isMove = false;

            // 開いている状態に
            isMenu = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            closePanel.SetActive(true);

            // プレイヤーを行動不能に
            plm.isMove = false;

            // 開いている状態に
            isMenu = true;
        }
    }

   // Jsonファイルロード関数
    public ItemData LoadFile(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath);
        string datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<ItemData>(datastr);
    }

    // Jsonファイルセーブ関数
    public void SaveFile(ItemData im)
    {
        string jsonstr = JsonUtility.ToJson(im);
        StreamWriter wreiter = new StreamWriter(datapath, false);
        wreiter.WriteLine(jsonstr);
        wreiter.Flush();
        wreiter.Close();
    }

    // プラスボタンを推したときの処理
    public void MaxMap()
    {
        // 全体マップを表示
        maxMap.SetActive(true);
        isMax = true;

        // プレイヤーを行動不能に
        plm.isMove = false;

        // 開いている状態に
        isMenu = true;
    }

    // アイテムリストボタンを押したときの処理
    public void BagList()
    {
        bagList.SetActive(true);
        // プレイヤーを行動不能に
        plm.isMove = false;

        // 開いている状態に
        isMenu = true;
    }

    // セッティングボタンを押したときの処理
    public void Settings()
    {
        pSetting.SetActive(true);
        // プレイヤーを行動不能に
        plm.isMove = false;

        // 開いている状態に
        isMenu = true;
    }

    // 武器リストボタンを押したときの処理
    public void WeaponList()
    {
        wList.SetActive(true);
        iList.SetActive(false);
        hList.SetActive(false);
        itemType = 1;
    }

    // アイテムリストボタンを押したときの処理
    public void ItemList()
    {
        wList.SetActive(false);
        iList.SetActive(true);
        hList.SetActive(false);
        itemType = 2;
    }

    // 家具リストボタンを押したときの処理
    public void HouseList()
    {
        wList.SetActive(false);
        iList.SetActive(false);
        hList.SetActive(true);
        itemType = 3;
    }

    // プレイヤーリストボタンを押したときの処理
    public void PlayerSet()
    {
        pSet.SetActive(true);
        rSet.SetActive(false);
        oSet.SetActive(false);
    }

    // ルームリストボタンを押したときの処理
    public void RoomSet()
    {
        pSet.SetActive(false);
        rSet.SetActive(true);
        oSet.SetActive(false);
    }

    // オプションリストボタンを押したときの処理
    public void OptionSet()
    {
        pSet.SetActive(false);
        rSet.SetActive(false);
        oSet.SetActive(true);
    }

    // 名前設定パネルボタンを押したときの処理
    public void NameSet()
    {
        nSet.SetActive(true);
        hSet.SetActive(false);
        cSet.SetActive(false);
        aSet.SetActive(false);
    }

    // 髪型設定パネルボタンを押したときの処理
    public void HairSet()
    {
        nSet.SetActive(false);
        hSet.SetActive(true);
        cSet.SetActive(false);
        aSet.SetActive(false);
    }

    // 髪色設定パネルボタンを押したときの処理
    public void HairColorSet()
    {
        nSet.SetActive(false);
        hSet.SetActive(false);
        cSet.SetActive(true);
        aSet.SetActive(false);
    }

    // 髪型設定パネルボタンを押したときの処理
    public void ClothesSet()
    {
        nSet.SetActive(false);
        hSet.SetActive(false);
        cSet.SetActive(false);
        aSet.SetActive(true);
    }

    // バックボタンを押したときの処理
    public void Back()
    {
        // バッグリストを非表示
        bagList.SetActive(false);

        // 設定リストを非表示
        pSetting.SetActive(false);

        // 全体マップを非表示
        maxMap.SetActive(false);
        isMax = false;

        // プレイヤーを行動可能に
        plm.isMove = true;

        // 閉じている状態に
        isMenu = false;
    }

    // コピーボタンを押したときの処理
    public void CopyRoomName()
    {
        //クリップボードへ文字を設定(コピー)
        GUIUtility.systemCopyBuffer = rName.GetComponent<Text>().text;
    }

    public void CopyRoomID()
    {
        //クリップボードへ文字を設定(コピー)
        GUIUtility.systemCopyBuffer = rID.GetComponent<Text>().text;
    }

    // ONボタンを押したとき
    public void PrivateON()
    {
        onButton.gameObject.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
        offButton.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    // OFFボタンを押したとき
    public void PrivateOFF()
    {
        onButton.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        offButton.gameObject.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
    }

    // BGM音量の数値を操作した時
    public void BGMField()
    {
        bgmField = GameObject.Find("BGMField").GetComponent<InputField>();
        bgmSlider = GameObject.Find("BGMVolume").GetComponent<Slider>();
        bgmSlider.value = Convert.ToInt32(bgmField.text);
        audioManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(bgmField.text) / 100;
    }

    // BGM音量のスライダーを操作した時
    public void BGMSlider()
    {
        bgmField = GameObject.Find("BGMField").GetComponent<InputField>();
        bgmSlider = GameObject.Find("BGMVolume").GetComponent<Slider>();
        bgmField.text= ((int)bgmSlider.value).ToString();
        audioManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(bgmField.text) / 100;
    }


    // SE音量の数値を操作した時
    public void SEField()
    {
        seField = GameObject.Find("SEField").GetComponent<InputField>();
        seSlider = GameObject.Find("SEVolume").GetComponent<Slider>();
        seSlider.value = Convert.ToInt32(seField.text);
        seManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(seField.text) / 100;
    }

    // BGM音量のスライダーを操作した時
    public void SESlider()
    {
        seField = GameObject.Find("SEField").GetComponent<InputField>();
        seSlider = GameObject.Find("SEVolume").GetComponent<Slider>();
        seField.text = ((int)seSlider.value).ToString();
        seManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(seField.text) / 100;
    }

    // SE音量の数値を操作した時
    public void BrightField()
    {
        brightField = GameObject.Find("BrightField").GetComponent<InputField>();
        brightSlider = GameObject.Find("BrightVolume").GetComponent<Slider>();
        brightSlider.value = Convert.ToInt32(brightField.text);
        brightPanel.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)(Convert.ToInt32(brightField.text) * 2));
    }

    // BGM音量のスライダーを操作した時
    public void BrightSlider()
    {
        brightField = GameObject.Find("BrightField").GetComponent<InputField>();
        brightSlider = GameObject.Find("BrightVolume").GetComponent<Slider>();
        brightField.text = ((int)brightSlider.value).ToString();
        brightPanel.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)(Convert.ToInt32(brightField.text) * 2));
    }

    public void Close()
    {
        // インベントリとバッグにアイテムを格納する処理
        for (int i = 0; i < Inventory.Length; i++)
        {
            im.data[i].invnum = GetItem(Inventory[i]).GetInvNum();
            im.data[i].bagnum = GetItem(Inventory[i]).GetBagNum();
            im.data[i].invno = GetItem(Inventory[i]).GetInvNo();
        }

        // jsonファイルセーブ
        SaveFile(im);
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }

    public void Cancel()
    {
        closePanel.SetActive(false);

        // プレイヤーを行動不能に
        plm.isMove = true;

        // 開いている状態に
        isMenu = false;
    }

    //　コードでアイテムを取得
    public Item GetItem(string searchCode)
    {
        return itemDataBase.GetItemLists().Find(code => code.GetCode() == searchCode);
    }
}*/

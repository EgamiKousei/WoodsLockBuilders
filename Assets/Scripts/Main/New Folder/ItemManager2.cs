using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemManager2 : MonoBehaviour
{
    //　アイテムデータベース
    [SerializeField]
    private ItemDataBase itemDataBase;

    // インベントリ確認用配列
    public string[] Inventory = new string[7];

    // プレイヤー
    GameObject player;

    // アイテムインベントリの1つ1つ
    GameObject Item;
    GameObject oldItem;

    // プレイヤースクリプト
    player1 pManager;

    // 選択アイテムについての変数
    int scNo = 1;
    int oldNo = 1;
    public string itemCode;
    public int kindNo = 0;

    // 前選択アイテム番号保持変数
    int oldI;

    // インベントリ内のアイテム数が0かどうか
    public bool isInv = true;

    // 子アイテムがあるかどうかを調べるための変数
    Transform children;
    GameObject child;

    private void Awake()
    {
        // インベントリ内初期化処理
        for (int i = 0; i < 7; i++)
        {
            Inventory[i] = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤースクリプト参照
        player = GameObject.Find("Player");
        pManager = player.GetComponent<player1>();
        // インベントリアウトラインオブジェクト
        Item = GameObject.Find("1");

        oldI = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // アイテムをインベントリに追加する処理
        if (pManager.code != null)
        {
            for(int i = 1; i < 8; i++)
            {
                // 中身のないインベントリNOを記録
                if (oldI == 0 && Inventory[i - 1] == null)
                {
                    oldI = i;
                }

                // インベントリ内にない時
                if (Inventory[i - 1] != pManager.code && i == 7)
                {
                    Debug.Log(i);
                    Inventory[oldI - 1] = pManager.code;
                    Item = GameObject.Find(oldI.ToString());
                    //Item.GetComponent<Image>().sprite = GetItem(pManager.code).GetIcon();
                    // ItemImageプレハブをGameObject型で取得
                    GameObject obj = (GameObject)Resources.Load(pManager.code + "Image");
                    // ItemImageプレハブを元に、インスタンスを生成、
                    Instantiate(obj,Item.transform);
                    GetItem(pManager.code).AddInv(1);
                    pManager.code = null;
                    break;
                // インベントリ内にある時
                }else if(Inventory[i - 1] == pManager.code)
                {
                    GetItem(pManager.code).AddInv(1);
                    pManager.code = null;
                    break;
                }
            }
            oldI = 0;
        }

        // 何かのキーが押下されたら
        if (Input.anyKeyDown)
        {
            // 入力されたキー名
            string keyStr = Input.inputString;
            //Debug.Log(keyStr);

            // 入力されたキーによってアイテムインベントリの番号を取得
            switch (keyStr)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                    // 現在の手持ちアイテムをOutlineによって強調表示
                    oldNo = scNo;
                    scNo = Convert.ToInt32(keyStr);
                    Item = GameObject.Find(keyStr);
                    oldItem = GameObject.Find(oldNo.ToString());
                    oldItem.GetComponent<Outline>().enabled = false;
                    Item.GetComponent<Outline>().enabled = true;
                   
                    break;
                default:
                    break;
            }

            // 子要素のオブジェクトの検索
            children = Item.gameObject.GetComponentInChildren<Transform>();
            // 子要素がいるならば
            if (children.childCount != 0)
            {
                child = children.gameObject.transform.GetChild(0).gameObject;
                itemCode = GetItem(child.GetComponent<Inventory>().code).GetCode();
                kindNo = GetItem(child.GetComponent<Inventory>().code).GetKindNo();

            }
        }

        // インベントリ内のアイテムが0になったら
        if(isInv == true)
        {
            kindNo = 0;
            Inventory[scNo - 1] = null;
            Destroy(child);
            isInv = false;

        }

    }

    //　コードでアイテムを取得
    public Item GetItem(string searchCode)
    {
        return itemDataBase.GetItemLists().Find(code => code.GetCode() == searchCode);
    }

}

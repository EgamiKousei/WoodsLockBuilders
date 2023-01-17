using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCreate : MonoBehaviour
{
    public static int itemNum=0;
    public static bool flug=true;
    public Image createItem;
    public Text num,category,coin,box,stone;
    public Sprite[] itemImage;
    string ItemName;
    public GameObject Script;

    private void Start()
    {
        SetItemNum();
    }
    // Update is called once per frame
    void Update()
    {
        if (ActionManager.playerScean == ActionManager.Player.Field && Input.GetKeyDown(KeyCode.LeftShift))
            Create();
        if (flug == true)
        {
            switch (itemNum)
            {
                case 0:
                    SetCreate("Wall");
                    break;
                case 1:
                    SetCreate("Stairs");
                    break;
                case 2:
                    SetCreate("Fence");
                    break;
                case 3:
                    SetCreate("Floor");
                    break;
                case 4:
                    SetCreate("Slope");
                    break;
                case 5:
                    SetCreate("Turret");
                    break;
                case 6:
                    SetCreate("Chest");
                    break;
                case 7:
                    SetCreate("LargeChest");
                    break;
                case 8:
                    SetCreate("Bed");
                    break;
                case 9:
                    SetCreate("Box");
                    break;
                case 10:
                    SetCreate("Stone");
                    break;
            }
            flug = false;
        }
    }
    void SetCreate(string item)
    {
        createItem.sprite = itemImage[PlayerData.ItemBox[item].create];
        num.text = PlayerData.ItemBox[item].num.ToString();
        category.text= PlayerData.ItemBox[item].category;
        ItemName = item;
    }

    void SetItemNum()
    {
        coin.text = (PlayerData.ItemBox["Coin"].invnum + PlayerData.ItemBox["Coin"].bagnum).ToString();
        box.text = (PlayerData.ItemBox["Box"].invnum + PlayerData.ItemBox["Box"].bagnum).ToString();
        stone.text = (PlayerData.ItemBox["Stone"].invnum + PlayerData.ItemBox["Stone"].bagnum).ToString();
    }
    public void Create()
    {
        string n="Coin";
        switch (PlayerData.ItemBox[ItemName].create)
        {
            case 0:
                n = "Coin";
                break;
            case 1:
                n = "Box";
                break;
            case 2:
                n = "Stone";
                break;
        }

        if (PlayerData.ItemBox[n].invnum + PlayerData.ItemBox[n].bagnum >= PlayerData.ItemBox[ItemName].num)
        {
            if (PlayerData.ItemBox[ItemName].invnum > 0)
                PlayerData.ItemBox[ItemName].invnum++;
            else if(PlayerData.ItemBox[ItemName].bagnum > 0)
                PlayerData.ItemBox[ItemName].bagnum++;
            else
            {
                int bagno = 1;
                foreach (var x in PlayerData.ItemBox.Values)
                {
                    if (x.bagno > bagno)
                        bagno = x.bagno;
                }
                PlayerData.ItemBox[ItemName].bagnum++;
                PlayerData.ItemBox[ItemName].bagno=bagno+1;
            }
                Script.GetComponent<ItemSet>().SetItem();
            if (PlayerData.ItemBox[n].bagnum >= PlayerData.ItemBox[ItemName].num)
                PlayerData.ItemBox[n].bagnum = PlayerData.ItemBox[n].bagnum - PlayerData.ItemBox[ItemName].num;
            else
            {
                PlayerData.ItemBox[n].invnum = 
                    PlayerData.ItemBox[n].invnum - PlayerData.ItemBox[ItemName].num+ PlayerData.ItemBox[n].bagnum;
                PlayerData.ItemBox[n].bagnum = 0;
            }
            SetItemNum();
        }
    }
}

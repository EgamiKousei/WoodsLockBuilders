using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSet : MonoBehaviour
{
   public static Transform[] ItemPanel, ItemBox;
    public Transform ob_ItemPanel, ob_ItemBox;
    public GameObject ItemBox_2;
    public Sprite[] itemImage;
    bool boxNow = false;
    public Sprite nullItem;

    // Start is called before the first frame update
    void Start()
    {
        ItemPanel = GetChildren(ob_ItemPanel);
        ItemBox = GetChildren(ob_ItemBox);
        SetItem();
    }

    public void SetItem()
    {
        //ボックスとリストにセット
        foreach (var i in PlayerData.ItemBox.Values)
        {
            var itemNo = GetItemNo(i.name);
            if (i.invnum > 0)
            {
                ItemPanel[i.invno - 1].Find("Image").gameObject.GetComponent<Image>().sprite = itemImage[itemNo];
                ItemPanel[i.invno - 1].Find("Text (Legacy)").gameObject.GetComponent<Text>().text = i.invnum.ToString();
            }
            if (i.bagnum > 0)
            {
                ItemBox[i.bagno-1].Find("Image").gameObject.GetComponent<Image>().sprite = itemImage[itemNo];
                ItemBox[i.bagno - 1].Find("Text (Legacy)").gameObject.GetComponent<Text>().text = i.bagnum.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && boxNow == false)
        {
            ItemBox_2.SetActive(true);
            boxNow = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && boxNow == true)
        {
            ItemBox_2.SetActive(false);
            boxNow = false;
        }
    }

    public void pushInv(int num)
    {
        bool flag = false;
        if (boxNow == true)
        {
            foreach (var i in PlayerData.ItemBox.Values)
            {
                if (i.invno == num&& i.invnum>0&& flag==false)
                {
                    i.invnum--;
                    i.bagnum++;
                    if (i.invnum == 0)
                    {
                        int invno=0;
                        foreach (var x in PlayerData.ItemBox.Values)
                        {
                            if (x.invno > invno)
                                invno = x.invno;
                            if (x.invno > i.invno)
                                x.invno--;
                        }
                        ItemPanel[invno-1].transform.Find("Text (Legacy)").gameObject.GetComponent<Text>().text = "0";
                        ItemPanel[invno-1].transform.Find("Image").gameObject.GetComponent<Image>().sprite = nullItem;
                        i.invno = 0;
                        flag = true;
                    }
                    if (i.bagno == 0)
                    {
                        int bagno =1;
                        foreach (var x in PlayerData.ItemBox.Values)
                        {
                            if (x.bagno >= bagno)
                            {
                                bagno = x.bagno + 1;
                            }
                        }
                        i.bagno = bagno;
                    }
                    SetItem();
                }
            }
        }
    }

    public void pushBox(int num)
    {
        bool flag = false;
        if (boxNow == true)
        {
            foreach (var i in PlayerData.ItemBox.Values)
            {
                if (i.bagno == num && i.bagnum > 0 && flag == false)
                {
                    i.invnum++;
                    i.bagnum--;
                    if (i.bagnum == 0)
                    {
                        int bagno = 0;
                        foreach (var x in PlayerData.ItemBox.Values)
                        {
                            if (x.bagno > bagno)
                                bagno = x.bagno;
                            if (x.bagno > i.bagno)
                                x.bagno--;
                        }
                        ItemBox[bagno-1].transform.Find("Text (Legacy)").gameObject.GetComponent<Text>().text = "0";
                        ItemBox[bagno-1].transform.Find("Image").gameObject.GetComponent<Image>().sprite = nullItem;
                        i.bagno = 0;
                        flag = true;
                    }
                    if (i.invno == 0)
                    {
                        int invno = 1;
                        foreach (var x in PlayerData.ItemBox.Values)
                        {
                            if (x.invno >= invno)
                                invno = x.invno + 1;
                        }
                        i.invno = invno;
                    }
                    SetItem();
                }
            }
        }
    }

    public static int GetItemNo(string item)
    {
        var itemNo=0;
        switch (item)
        {
            case "Coin":
                itemNo = 0;
                break;
            case "Box":
                itemNo = 1;
                break;
            case "Stone":
                itemNo = 2;
                break;
            case "Wall":
                itemNo = 3;
                break;
            case "Stairs":
                itemNo = 4;
                break;
            case "Fence":
                itemNo = 5;
                break;
            case "Floor":
                itemNo = 6;
                break;
            case "Slope":
                itemNo = 7;
                break;
            case "Turret":
                itemNo = 8;
                break;
            case "Chest":
                itemNo = 9;
                break;
            case "LargeChest":
                itemNo = 10;
                break;
            case "Bed":
                itemNo = 11;
                break;
        }
        return itemNo;
    }

    public static Transform[] GetChildren(Transform parent)
    {
        // 子オブジェクトを格納する配列作成
        var children = new Transform[parent.childCount];

        // 0～個数-1までの子を順番に配列に格納
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = parent.GetChild(i);
        }

        // 子オブジェクトが格納された配列
        return children;
    }

    public void BoxSet()
    {
        ItemBox_2.SetActive(true);
        boxNow = true;
    }

    public void BoxEnd()
    {
        ItemBox_2.SetActive(false);
        boxNow = false;
    }
}

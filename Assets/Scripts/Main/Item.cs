using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    // アイテムの種類　武器・消耗品・配置アイテム・素材アイテム
    public enum KindOfItem
    {
        Weapon,
        Useitem,
        Agitem
    }

    //　アイテムの種類
    [SerializeField]
    private KindOfItem kindOfItem;
    //　アイテムの種類ナンバー
    [SerializeField]
    private int kindNo;
    //　アイテムのアイコン
    [SerializeField]
    private Sprite icon;
    //　アイテムのプロジェクト上での名前
    [SerializeField]
    private string code;
    //　アイテムの名前
    [SerializeField]
    private string itemName;
    //　アイテムの情報
    [SerializeField]
    private string information;

    //　バッグ内個数の情報
    [SerializeField]
    private int bagNum;

    //　インベントリ内の情報
    [SerializeField]
    private int invNum;

    //　インベントリ番号
    [SerializeField]
    private int invNo;

    public KindOfItem GetKindOfItem()
    {
        return kindOfItem;
    }

    public int GetKindNo()
    {
        return kindNo;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetCode()
    {
        return code;
    }


    public string GetItemName()
    {
        return itemName;
    }

    public string GetInformation()
    {
        return information;
    }

    public int GetBagNum()
    {
        return bagNum;
    }

    public int GetInvNum()
    {
        return invNum;
    }

    public void AddBag(int n)
    {
        bagNum += n;
    }
    public void AddInv(int n)
    {
        invNum += n;
    }

    public void DecBag(int n)
    {
        bagNum -= n;
    }
    public void DecInv(int n)
    {
        invNum -= n;
    }

    public void SetBag(int n)
    {
        bagNum = n;
    }
    public void SetInv(int n)
    {
        invNum = n;
    }

    public int GetInvNo()
    {
        return invNo;
    }

    public void SetInvNo(int n)
    {
        invNo = n;
    }
}

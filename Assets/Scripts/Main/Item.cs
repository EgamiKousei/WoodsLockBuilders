using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    // �A�C�e���̎�ށ@����E���Օi�E�z�u�A�C�e���E�f�ރA�C�e��
    public enum KindOfItem
    {
        Weapon,
        Useitem,
        Agitem
    }

    //�@�A�C�e���̎��
    [SerializeField]
    private KindOfItem kindOfItem;
    //�@�A�C�e���̎�ރi���o�[
    [SerializeField]
    private int kindNo;
    //�@�A�C�e���̃A�C�R��
    [SerializeField]
    private Sprite icon;
    //�@�A�C�e���̃v���W�F�N�g��ł̖��O
    [SerializeField]
    private string code;
    //�@�A�C�e���̖��O
    [SerializeField]
    private string itemName;
    //�@�A�C�e���̏��
    [SerializeField]
    private string information;

    //�@�o�b�O�����̏��
    [SerializeField]
    private int bagNum;

    //�@�C���x���g�����̏��
    [SerializeField]
    private int invNum;

    //�@�C���x���g���ԍ�
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

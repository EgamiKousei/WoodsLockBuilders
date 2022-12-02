using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemManager2 : MonoBehaviour
{
    //�@�A�C�e���f�[�^�x�[�X
    [SerializeField]
    private ItemDataBase itemDataBase;

    // �C���x���g���m�F�p�z��
    public string[] Inventory = new string[7];

    // �v���C���[
    GameObject player;

    // �A�C�e���C���x���g����1��1��
    GameObject Item;
    GameObject oldItem;

    // �v���C���[�X�N���v�g
    player1 pManager;

    // �I���A�C�e���ɂ��Ă̕ϐ�
    int scNo = 1;
    int oldNo = 1;
    public string itemCode;
    public int kindNo = 0;

    // �O�I���A�C�e���ԍ��ێ��ϐ�
    int oldI;

    // �C���x���g�����̃A�C�e������0���ǂ���
    public bool isInv = true;

    // �q�A�C�e�������邩�ǂ����𒲂ׂ邽�߂̕ϐ�
    Transform children;
    GameObject child;

    private void Awake()
    {
        // �C���x���g��������������
        for (int i = 0; i < 7; i++)
        {
            Inventory[i] = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�X�N���v�g�Q��
        player = GameObject.Find("Player");
        pManager = player.GetComponent<player1>();
        // �C���x���g���A�E�g���C���I�u�W�F�N�g
        Item = GameObject.Find("1");

        oldI = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �A�C�e�����C���x���g���ɒǉ����鏈��
        if (pManager.code != null)
        {
            for(int i = 1; i < 8; i++)
            {
                // ���g�̂Ȃ��C���x���g��NO���L�^
                if (oldI == 0 && Inventory[i - 1] == null)
                {
                    oldI = i;
                }

                // �C���x���g�����ɂȂ���
                if (Inventory[i - 1] != pManager.code && i == 7)
                {
                    Debug.Log(i);
                    Inventory[oldI - 1] = pManager.code;
                    Item = GameObject.Find(oldI.ToString());
                    //Item.GetComponent<Image>().sprite = GetItem(pManager.code).GetIcon();
                    // ItemImage�v���n�u��GameObject�^�Ŏ擾
                    GameObject obj = (GameObject)Resources.Load(pManager.code + "Image");
                    // ItemImage�v���n�u�����ɁA�C���X�^���X�𐶐��A
                    Instantiate(obj,Item.transform);
                    GetItem(pManager.code).AddInv(1);
                    pManager.code = null;
                    break;
                // �C���x���g�����ɂ��鎞
                }else if(Inventory[i - 1] == pManager.code)
                {
                    GetItem(pManager.code).AddInv(1);
                    pManager.code = null;
                    break;
                }
            }
            oldI = 0;
        }

        // �����̃L�[���������ꂽ��
        if (Input.anyKeyDown)
        {
            // ���͂��ꂽ�L�[��
            string keyStr = Input.inputString;
            //Debug.Log(keyStr);

            // ���͂��ꂽ�L�[�ɂ���ăA�C�e���C���x���g���̔ԍ����擾
            switch (keyStr)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                    // ���݂̎莝���A�C�e����Outline�ɂ���ċ����\��
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

            // �q�v�f�̃I�u�W�F�N�g�̌���
            children = Item.gameObject.GetComponentInChildren<Transform>();
            // �q�v�f������Ȃ��
            if (children.childCount != 0)
            {
                child = children.gameObject.transform.GetChild(0).gameObject;
                itemCode = GetItem(child.GetComponent<Inventory>().code).GetCode();
                kindNo = GetItem(child.GetComponent<Inventory>().code).GetKindNo();

            }
        }

        // �C���x���g�����̃A�C�e����0�ɂȂ�����
        if(isInv == true)
        {
            kindNo = 0;
            Inventory[scNo - 1] = null;
            Destroy(child);
            isInv = false;

        }

    }

    //�@�R�[�h�ŃA�C�e�����擾
    public Item GetItem(string searchCode)
    {
        return itemDataBase.GetItemLists().Find(code => code.GetCode() == searchCode);
    }

}

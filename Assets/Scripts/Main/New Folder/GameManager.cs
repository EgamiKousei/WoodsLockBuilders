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
    //�@�A�C�e���f�[�^�x�[�X
    [SerializeField]
    private ItemDataBase itemDataBase;

    // �C���x���g���z��
    public string[] Inventory;

    // �C���x���g���I�u�W�F�N�g
    GameObject invNo;

    // ��U�i�[���邽�߂̕ϐ�
    GameObject rObj;

    public GameObject maxMap;   // �S�̃}�b�v�̃p�l��
    public GameObject bagList;  // �o�b�O�ꗗ�̃p�l��
    public GameObject pSetting; // �ݒ�̃p�l��
    public GameObject wList;    // ���탊�X�g
    public GameObject iList;    // �A�C�e�����X�g
    public GameObject hList;    // �Ƌ�X�g
    public GameObject wPanel;   // ���탊�X�g�p�l��
    public GameObject iPanel;   // �A�C�e�����X�g�p�l��
    public GameObject hPanel;   // �Ƌ�X�g�p�l��
    public GameObject pSet;     // �v���C���[�ݒ�̃p�l��
    public GameObject rSet;     // ���[���ݒ�̃p�l��
    public GameObject oSet;     // �I�v�V�����ݒ�̃p�l��
    public GameObject nSet;     // ���O�ݒ�̃p�l��
    public GameObject hSet;     // ���^�ݒ�̃p�l��
    public GameObject cSet;     // ���F�ݒ�̃p�l��
    public GameObject aSet;     // �����ݒ�̃p�l��
    public GameObject rName;    // ���[�����̃e�L�X�g
    public GameObject rID;      // ���[��ID�̃e�L�X�g
    public GameObject onButton; // ON�{�^��
    public GameObject offButton;// OFF�{�^��
    public int itemType;        // �A�C�e���̎��
    InputField bgmField;        // BGM�𐔒l�Œ������邽�߂�InputField
    InputField seField;         // SE�𐔒l�Œ������邽�߂�InputField
    InputField brightField;     // ���邳�𐔒l�Œ������邽�߂�InputField
    Slider seSlider;            // SE�𒲐����邽�߂̃X���C�_�[
    Slider bgmSlider;           // BGM�𒲐����邽�߂̃X���C�_�[
    Slider brightSlider;        // ���邳�𒲐����邽�߂̃X���C�_�[
    GameObject player;          // �v���C���[
    player1 plm;                // �v���C���[�X�N���v�g
    GameObject audioManager;    // BGM�p�I�u�W�F�N�g
    GameObject seManager;       // SE�p�I�u�W�F�N�g
    GameObject brightPanel;     // ���邳�ύX�p�p�l��
    bool isMax = false;         // �}�b�v��S�̕\�����Ă��邩�ǂ���
    public bool isMenu = false;     // ���j���[��ʂȂǂ��J���Ă��邩�ǂ���
    public GameObject closePanel;   // �I���p�l�� 
    string datapath;            // Json�f�[�^���Q�Ƃ��邽�߂̃p�X
    static ItemData im;        // Json�f�[�^�i�[�ϐ�

    //public bool isItemMov = false;

    private void Awake()
    {
        //datapath = Application.dataPath + "/ItemData.json";   // Timatu.json�܂ł̃p�X
    }

    // Start is called before the first frame update
    void Start()
    {
        // �J�[�\�����\���ɂ��Ē����ɌŒ�
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // �l�X�ȗv�f�̎擾
        player = GameObject.Find("Player");
        plm = player.GetComponent<player1>();
        audioManager = GameObject.Find("AudioManager");
        seManager = GameObject.Find("SEManager");
        brightPanel = GameObject.Find("BrightPanel");
        itemType = 1;

        im = new ItemData();


        // json�t�@�C�����[�h
        im = LoadFile(datapath);

        // �C���x���g���ƃo�b�O�ɃA�C�e�����i�[���鏈��
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

        // json�t�@�C���Z�[�u
        SaveFile(im);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMenu == true || plm.isArg == true)
        {
            // �J�[�\����\�����ČŒ�łȂ�����
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // �J�[�\�����\���ɂ��Ē����ɌŒ�
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(Input.GetKeyDown(KeyCode.M) && isMenu == false)
        {
            // �S�̃}�b�v��\��
            maxMap.SetActive(true);
            isMax = true;

            // �v���C���[���s���s�\��
            plm.isMove = false;

            // �J���Ă����Ԃ�
            isMenu = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && isMenu == false)
        {
            // �o�b�O���J��
            bagList.SetActive(true);

            // �v���C���[���s���s�\��
            plm.isMove = false;

            // �J���Ă����Ԃ�
            isMenu = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && isMenu == false)
        {
            // �ݒ��ʂ��J��
            pSetting.SetActive(true);

            // �v���C���[���s���s�\��
            plm.isMove = false;

            // �J���Ă����Ԃ�
            isMenu = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            closePanel.SetActive(true);

            // �v���C���[���s���s�\��
            plm.isMove = false;

            // �J���Ă����Ԃ�
            isMenu = true;
        }
    }

   // Json�t�@�C�����[�h�֐�
    public ItemData LoadFile(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath);
        string datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<ItemData>(datastr);
    }

    // Json�t�@�C���Z�[�u�֐�
    public void SaveFile(ItemData im)
    {
        string jsonstr = JsonUtility.ToJson(im);
        StreamWriter wreiter = new StreamWriter(datapath, false);
        wreiter.WriteLine(jsonstr);
        wreiter.Flush();
        wreiter.Close();
    }

    // �v���X�{�^���𐄂����Ƃ��̏���
    public void MaxMap()
    {
        // �S�̃}�b�v��\��
        maxMap.SetActive(true);
        isMax = true;

        // �v���C���[���s���s�\��
        plm.isMove = false;

        // �J���Ă����Ԃ�
        isMenu = true;
    }

    // �A�C�e�����X�g�{�^�����������Ƃ��̏���
    public void BagList()
    {
        bagList.SetActive(true);
        // �v���C���[���s���s�\��
        plm.isMove = false;

        // �J���Ă����Ԃ�
        isMenu = true;
    }

    // �Z�b�e�B���O�{�^�����������Ƃ��̏���
    public void Settings()
    {
        pSetting.SetActive(true);
        // �v���C���[���s���s�\��
        plm.isMove = false;

        // �J���Ă����Ԃ�
        isMenu = true;
    }

    // ���탊�X�g�{�^�����������Ƃ��̏���
    public void WeaponList()
    {
        wList.SetActive(true);
        iList.SetActive(false);
        hList.SetActive(false);
        itemType = 1;
    }

    // �A�C�e�����X�g�{�^�����������Ƃ��̏���
    public void ItemList()
    {
        wList.SetActive(false);
        iList.SetActive(true);
        hList.SetActive(false);
        itemType = 2;
    }

    // �Ƌ�X�g�{�^�����������Ƃ��̏���
    public void HouseList()
    {
        wList.SetActive(false);
        iList.SetActive(false);
        hList.SetActive(true);
        itemType = 3;
    }

    // �v���C���[���X�g�{�^�����������Ƃ��̏���
    public void PlayerSet()
    {
        pSet.SetActive(true);
        rSet.SetActive(false);
        oSet.SetActive(false);
    }

    // ���[�����X�g�{�^�����������Ƃ��̏���
    public void RoomSet()
    {
        pSet.SetActive(false);
        rSet.SetActive(true);
        oSet.SetActive(false);
    }

    // �I�v�V�������X�g�{�^�����������Ƃ��̏���
    public void OptionSet()
    {
        pSet.SetActive(false);
        rSet.SetActive(false);
        oSet.SetActive(true);
    }

    // ���O�ݒ�p�l���{�^�����������Ƃ��̏���
    public void NameSet()
    {
        nSet.SetActive(true);
        hSet.SetActive(false);
        cSet.SetActive(false);
        aSet.SetActive(false);
    }

    // ���^�ݒ�p�l���{�^�����������Ƃ��̏���
    public void HairSet()
    {
        nSet.SetActive(false);
        hSet.SetActive(true);
        cSet.SetActive(false);
        aSet.SetActive(false);
    }

    // ���F�ݒ�p�l���{�^�����������Ƃ��̏���
    public void HairColorSet()
    {
        nSet.SetActive(false);
        hSet.SetActive(false);
        cSet.SetActive(true);
        aSet.SetActive(false);
    }

    // ���^�ݒ�p�l���{�^�����������Ƃ��̏���
    public void ClothesSet()
    {
        nSet.SetActive(false);
        hSet.SetActive(false);
        cSet.SetActive(false);
        aSet.SetActive(true);
    }

    // �o�b�N�{�^�����������Ƃ��̏���
    public void Back()
    {
        // �o�b�O���X�g���\��
        bagList.SetActive(false);

        // �ݒ胊�X�g���\��
        pSetting.SetActive(false);

        // �S�̃}�b�v���\��
        maxMap.SetActive(false);
        isMax = false;

        // �v���C���[���s���\��
        plm.isMove = true;

        // ���Ă����Ԃ�
        isMenu = false;
    }

    // �R�s�[�{�^�����������Ƃ��̏���
    public void CopyRoomName()
    {
        //�N���b�v�{�[�h�֕�����ݒ�(�R�s�[)
        GUIUtility.systemCopyBuffer = rName.GetComponent<Text>().text;
    }

    public void CopyRoomID()
    {
        //�N���b�v�{�[�h�֕�����ݒ�(�R�s�[)
        GUIUtility.systemCopyBuffer = rID.GetComponent<Text>().text;
    }

    // ON�{�^�����������Ƃ�
    public void PrivateON()
    {
        onButton.gameObject.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
        offButton.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    // OFF�{�^�����������Ƃ�
    public void PrivateOFF()
    {
        onButton.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        offButton.gameObject.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
    }

    // BGM���ʂ̐��l�𑀍삵����
    public void BGMField()
    {
        bgmField = GameObject.Find("BGMField").GetComponent<InputField>();
        bgmSlider = GameObject.Find("BGMVolume").GetComponent<Slider>();
        bgmSlider.value = Convert.ToInt32(bgmField.text);
        audioManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(bgmField.text) / 100;
    }

    // BGM���ʂ̃X���C�_�[�𑀍삵����
    public void BGMSlider()
    {
        bgmField = GameObject.Find("BGMField").GetComponent<InputField>();
        bgmSlider = GameObject.Find("BGMVolume").GetComponent<Slider>();
        bgmField.text= ((int)bgmSlider.value).ToString();
        audioManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(bgmField.text) / 100;
    }


    // SE���ʂ̐��l�𑀍삵����
    public void SEField()
    {
        seField = GameObject.Find("SEField").GetComponent<InputField>();
        seSlider = GameObject.Find("SEVolume").GetComponent<Slider>();
        seSlider.value = Convert.ToInt32(seField.text);
        seManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(seField.text) / 100;
    }

    // BGM���ʂ̃X���C�_�[�𑀍삵����
    public void SESlider()
    {
        seField = GameObject.Find("SEField").GetComponent<InputField>();
        seSlider = GameObject.Find("SEVolume").GetComponent<Slider>();
        seField.text = ((int)seSlider.value).ToString();
        seManager.GetComponent<AudioSource>().volume = (float)Convert.ToInt32(seField.text) / 100;
    }

    // SE���ʂ̐��l�𑀍삵����
    public void BrightField()
    {
        brightField = GameObject.Find("BrightField").GetComponent<InputField>();
        brightSlider = GameObject.Find("BrightVolume").GetComponent<Slider>();
        brightSlider.value = Convert.ToInt32(brightField.text);
        brightPanel.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)(Convert.ToInt32(brightField.text) * 2));
    }

    // BGM���ʂ̃X���C�_�[�𑀍삵����
    public void BrightSlider()
    {
        brightField = GameObject.Find("BrightField").GetComponent<InputField>();
        brightSlider = GameObject.Find("BrightVolume").GetComponent<Slider>();
        brightField.text = ((int)brightSlider.value).ToString();
        brightPanel.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)(Convert.ToInt32(brightField.text) * 2));
    }

    public void Close()
    {
        // �C���x���g���ƃo�b�O�ɃA�C�e�����i�[���鏈��
        for (int i = 0; i < Inventory.Length; i++)
        {
            im.data[i].invnum = GetItem(Inventory[i]).GetInvNum();
            im.data[i].bagnum = GetItem(Inventory[i]).GetBagNum();
            im.data[i].invno = GetItem(Inventory[i]).GetInvNo();
        }

        // json�t�@�C���Z�[�u
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

        // �v���C���[���s���s�\��
        plm.isMove = true;

        // �J���Ă����Ԃ�
        isMenu = false;
    }

    //�@�R�[�h�ŃA�C�e�����擾
    public Item GetItem(string searchCode)
    {
        return itemDataBase.GetItemLists().Find(code => code.GetCode() == searchCode);
    }
}*/

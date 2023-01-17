using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�A�[�e�B�t�@�N�g�A�o�t�Ɋւ���X�N���v�g2
public class ArtifactSub : MonoBehaviour
{
    public int ArtiNum = 1;//�A�[�e�B�t�@�N�g�ԍ�1��2��3�h���b�v4�|��
    bool CanBuff = false;//�g���邩�ǂ���

    GameObject Carigge;
    CariggeManager cariggeManager;

    GameObject ArtiMain;//�A�[�e�B�t�@�N�g�̃��C���X�N���v�g
    ArtifactMain artiMain;

    int CoinQuantity;//�R�C��������

    BoxCollider col;

    public GameObject[] Deltree;//�폜�����|��

    int count = 0;//give�J�E���g

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        Carigge = GameObject.Find("Carigge");
        ArtiMain = GameObject.Find("Artifact");
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        artiMain = ArtiMain.GetComponent<ArtifactMain>();

        col = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCoinQuantity();//�����ŏ����R�C���𔻒�

        if (Input.GetKeyDown(KeyCode.F) && CanBuff == true && CoinQuantity >= 400)//F�L�[���������Ƃ��ɃR�C����400�ȏゾ�Ǝg�p�ł���
        {
            CariggeManager.HaveCoin -= 400;//�����R�C������400����
            col.enabled = false;//�R���C�_�[�𖳌���
            canvas.SetActive(false);
            if (ArtiNum == 1)//���ꂼ��̃^�C�v�ɉ���������
            {
                GiveWood();
            }
            else if (ArtiNum == 1)
            {
                GiveFlower();
            }
            else if (ArtiNum == 3)
            {
                DropUp();
            }
            else if (ArtiNum == 4)
            {
                Obstacle();
            }
        }
    }

    void GiveWood()//�؂�25�ǉ�
    {
        CanBuff = false;
        if (count <= 24)
        {
        cariggeManager.GetWood();
        count++;
        Invoke("GiveWood", 0.05f);
        }
    }

    void GiveFlower()//�Ԃ��T�ǉ�
    {       
        CanBuff = false;
        if (count <= 4)
        {
            cariggeManager.GetFlower();
            count++;
            Invoke("GiveFlower", 0.05f);
        }
    }

    void DropUp()//�؂̃h���b�v�ʂ���莞�ԂQ�{��
    {
        artiMain.DropMultiply = 2;
        CanBuff = false;
    }

    void Obstacle()//�t�߂̓|�؂�����
    {
        Deltree[0].SetActive(false);
        Deltree[1].SetActive(false);
        CanBuff = false;
    }

    void GetCoinQuantity()
    {
        CoinQuantity = CariggeManager.HaveCoin;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanBuff = true;
            canvas.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanBuff = false;
            canvas.SetActive(false);
        }
    }
}

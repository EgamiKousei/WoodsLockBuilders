using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NpcManager : MonoBehaviour
{
    string datapath;                        // �_�b�V���f�[�^�܂ł̃p�X
    NpcData npc;                            // NPC�f�[�^�i�[�ϐ�

    // npc�I�u�W�F�N�g�i�[�p�z��
    [SerializeField]
    GameObject[] npcObject;

    // Enemy�I�u�W�F�N�g�i�[�p�z��
    [SerializeField]
    GameObject[] enemyObject;

    // Item�I�u�W�F�N�g�i�[�p�z��
    [SerializeField]
    GameObject[] ItemObject;

    // npc�I�u�W�F�N�g�i�[�p�z��
    [SerializeField]
    float[] npcHp;

    // ��q���I�u�W�F�N�g
    [SerializeField]
    GameObject car;

    // npc�Ƃ��ꂼ��̓G�̋���
    [SerializeField]
    float[] npcDis;

    // npc1�Ƃ��ꂼ��̓G�̋���
    [SerializeField]
    float[] npc1Dis;

    // npc2�Ƃ��ꂼ��̓G�̋���
    [SerializeField]
    float[] npc2Dis;

    // npc3�Ƃ��ꂼ��̓G�̋���
    [SerializeField]
    float[] npc3Dis;

    // ���ꂼ���NPC�̃^�[�Q�b�g�
    string[] npcTps;

    // npc�̐�
    int npcNum;

    // npc��Taregt�Ƃ��Ă���I�u�W�F�N�g
    [SerializeField]
    GameObject[] targetObject;

    // �g���b�R�������肷�邽�߂̕ϐ�
    public bool isTro = false;
    public int troNo;

    // ���ꂼ��̈�ԋ߂��G���m�F���邽�߂̕ϐ�
    int close1 = 0;
    int close2 = 0;
    int close3 = 0;
    float closeDis1 = 1000;
    float closeDis2 = 1000;
    float closeDis3 = 1000;
    bool isChange1 = false;
    bool isChange2 = false;
    bool isChange3 = false;

    // �^�[�Q�b�g�ݒ菈�����s������
    public bool isChange = true;

    GameObject closeCarigge;

    public GameObject obstacle; // ��Q��
    GameObject closeObstacle;


    private void Awake()
    {
        datapath = Application.dataPath + "/Npc.json"; // NPC�f�[�^�܂ł̃p�X

    }

    // Start is called before the first frame update
    void Start()
    {
        // npc�i�[����
        npcObject = GameObject.FindGameObjectsWithTag("Npc");
        npcNum = npcObject.Length;

        // ����������
        targetObject = new GameObject[npcNum];
        npcDis = new float[npcNum];
        npcHp = new float[npcNum];
        npcTps = new string[npcNum];

        for (int i = 0; i < npcNum; i++) 
        {
            npcHp[i] = npcObject[i].GetComponent<Nav>().hp;
            npcTps[i] = npcObject[i].GetComponent<Nav>().tps;
        }

        npc = new NpcData();        // NPC�f�[�^�擾

        npc = LoadNPC(datapath);   // NPC�f�[�^���[�h

        SaveNPC(npc);               // NPC�f�[�^�Z�[�u

        //ChangeEnemy();

        GameObject.Find("GameManager").GetComponent<NpcManager>().Invoke("ActiveNav", 3f);

    }

    // NPC�f�[�^���[�h�֐�
    public NpcData LoadNPC(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath);
        string datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<NpcData>(datastr);
    }

    // NPC�f�[�^�Z�[�u�֐�
    public void SaveNPC(NpcData npc)
    {
        string jsonstr = JsonUtility.ToJson(npc);
        StreamWriter wreiter = new StreamWriter(datapath, false);
        wreiter.WriteLine(jsonstr);
        wreiter.Flush();
        wreiter.Close();
    }

    // �����ꂩ��HP���ω��������̏���
    public void changeHp()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcHp[i] = npcObject[i].GetComponent<Nav>().hp;
        }
    }

    public void ChangeEnemy()
    {
        for (int i = 0; i < npcNum; i++)
        {
            if (npcObject[i].GetComponent<Nav>().lv == 1)
            {
                npcObject[i].GetComponent<Nav>().Switch();
            }
            else if (npcObject[i].GetComponent<Nav>().lv == 2)
            {
                npcObject[i].GetComponent<Nav>().Switch2();
            }
            else if (npcObject[i].GetComponent<Nav>().lv == 3)
            {
                npcObject[i].GetComponent<Nav>().Switch3();
            }
            npcObject[i].GetComponent<Nav>().isChange = false;
        }
    }

    // ChangeEnemy�����܂ł̎��ԍ�
    public IEnumerator ChangeDelay()
    {
        // 1�b������҂��܂��B
        yield return new WaitForSeconds(0.1f);
        ChangeEnemy();
    }

    public void BoolChange()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcObject[i].GetComponent<Nav>().isChange = true;
        }
    }

    public void CloseCarigge()
    {
        float closeDist = 1000;
        foreach (GameObject y in npcObject)
        {
            // �R���\�[����ʂł̊m�F�p�R�[�h
            //print(Vector3.Distance(transform.position, t.transform.position));

            // ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
            float tDist = Vector3.Distance(car.transform.position, y.transform.position);

            // �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
            if (closeDist > tDist)
            {
                // �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
                // ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
                closeDist = tDist;

                // ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
                closeCarigge = y;

            }

            // �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
            //Invoke("SwitchOn", 0.5f);


        }
        closeCarigge.GetComponent<Nav>().isFire = true;
    }

    public void CloseObstacle()
    {
        float closeDist = 1000;
        foreach (GameObject y in npcObject)
        {
            // �R���\�[����ʂł̊m�F�p�R�[�h
            //print(Vector3.Distance(transform.position, t.transform.position));

            // ���̃I�u�W�F�N�g�i�C�e�j�ƓG�܂ł̋������v��
            float tDist = Vector3.Distance(obstacle.transform.position, y.transform.position);

            // �������u�����l�v�����u�v�������G�܂ł̋����v�̕����߂��Ȃ�΁A
            if (closeDist > tDist)
            {
                // �ucloseDist�v���utDist�i���̓G�܂ł̋����j�v�ɒu��������B
                // ������J��Ԃ����ƂŁA��ԋ߂��G�������o�����Ƃ��ł���B
                closeDist = tDist;

                // ��ԋ߂��G�̏���closeEnemy�Ƃ����ϐ��Ɋi�[����i���j
                closeObstacle = y;

            }

            // �C�e�����������0.5�b��ɁA��ԋ߂��G�Ɍ������Ĉړ����J�n����B
            //Invoke("SwitchOn", 0.5f);


        }
        closeObstacle.GetComponent<Nav>().isObstacle = true;
    }

    public void ResetObstacle()
    {
        for (int i = 0; i < npcNum; i++)
        {
            if(npcObject[i].GetComponent<Nav>().isObstacle == true)
            {
                npcObject[i].GetComponent<Nav>().GetPoint("Obstacle");
            }
            npcObject[i].GetComponent<Nav>().isObstacle = false;
            npcObject[i].GetComponent<Nav>().Switch();
        }
    }

    public void HealHp(float hp)
    {
        npc.healHp += hp;
        npc.healCnt++;
        //SaveNPC(npc);
    }

    public void HealFire(float fire)
    {
        npc.torch += fire;
        npc.torchCnt++;
        //SaveNPC(npc);
    }

    public void ActiveNav()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcObject[i].GetComponent<Nav>().enabled = true;
        }
    }

    public void DataSave()
    {
        SaveNPC(npc);

    }
}

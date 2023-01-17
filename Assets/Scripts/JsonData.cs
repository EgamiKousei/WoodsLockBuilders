using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC�f�[�^
[System.Serializable]
public class NpcData
{
    // �����̓_������
    public float torch;
    // �����_�����Ԃ����߂邽�߂̃J�E���g�f�[�^
    public int torchCnt;
    // HP�񕜍s����l
    public float healHp;
    // HP�񕜍s����ɕK�v�ȃJ�E���g
    public int healCnt;
}

// NPC�f�[�^
[System.Serializable]
public class Npc1Data
{
    // ���x��
    public int lv;
    // �G�̈ʒu�ɂ��D��x
    public string tpsEnemy;
    // �����̓_������
    public float torch;
    // �n�ԁE�g���b�R�̑ϋv�l�񕜂̊
    public int endu;
    // HP�񕜍s����l
    public float healHp;
    // �o���l
    public int point;
}

// NPC�f�[�^
[System.Serializable]
public class Npc2Data
{
    // ���x��
    public int lv;
    // �G�̈ʒu�ɂ��D��x
    public string tpsEnemy;
    // �����̓_������
    public float torch;
    // �n�ԁE�g���b�R�̑ϋv�l�񕜂̊
    public int endu;
    // HP�񕜍s����l
    public float healHp;
    // �o���l
    public int point;
}

// NPC�f�[�^
[System.Serializable]
public class Npc3Data
{
    // ���x��
    public int lv;
    // �G�̈ʒu�ɂ��D��x
    public string tpsEnemy;
    // �����̓_������
    public float torch;
    // �n�ԁE�g���b�R�̑ϋv�l�񕜂̊
    public int endu;
    // HP�񕜍s����l
    public float healHp;
    // �o���l
    public int point;
}

// NPC�}�C�Z�b�g�i�z��j
[System.Serializable]
public class SetData
{
    public setData[] data;
}

// �}�C�Z�b�g�f�[�^�i�ϐ��j
[System.Serializable]
public class setData
{
    // �G�̈ʒu�ɂ��D��x
    public string tpsEnemy;
    // �����̓_������
    public int torch;
    // �n�ԁE�g���b�R�̑ϋv�l�񕜂̊
    public int endu;
    // HP�񕜍s����l
    public int healHp;
}



public class JsonData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

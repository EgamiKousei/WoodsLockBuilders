using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    int enemyTime=0;
    // Use this for initialization
    void Start()
    {
        //���[���}�X�^�[��������
        SetEnemTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > enemyTime)
        {
            //���C���V�[�����̂݉ғ�
            //�n�_���߂ēG�o��
            Debug.Log("�o�ߎ���(�b)" + enemyTime);
            SetEnemTime();
        }
    }
    public void SetEnemTime()
    {
        enemyTime = (int)Time.time;
        enemyTime = enemyTime + Random.Range(300, 600);
    }
}
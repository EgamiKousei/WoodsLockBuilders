using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�l�~�[�X�|�[���X�N���v�g
public class EnemySpawn : MonoBehaviour
{
    //20�b���Ƃ�3�̃X�|�[��

    [SerializeField]
    GameObject[] SpawnPoint;//�X�|�[���ꏊ���@�U����

    [SerializeField]
    GameObject Enemy;//�G�l�~�[�̃v���n�u

    float Span = 20f;//�X�|�[���Ԋu
    private float currentTime = 0f;

    bool isDay = true;//�����ǂ���

    Vector3 Pos;//�X�|�[���ꏊ

    SwitchDayNight dayNight;//����؂�ւ��̃X�N���v�g
    public GameObject Light;

    CariggeManager cariggeManager;//�n��
    public GameObject Carigge;

    // Start is called before the first frame update
    void Start()
    {
        dayNight = Light.GetComponent<SwitchDayNight>();
        cariggeManager = Carigge.GetComponent<CariggeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cariggeManager.GameStart == true)//�n�Ԃ����Ԃ��Ă���ꍇ�̂݃X�|�[������
            Spawn();

        if (dayNight.DayNight == 1)//��ɂȂ��10�b���ƂɃX�|�[������
        {
            Span = 20f;
        }
        else
        {
            Span = 10f;
        }
        //MidiumBossSpawn();
    }

    void Spawn()
    {
        currentTime += Time.deltaTime;

        if (currentTime > Span)
        {
            if (dayNight.DayNight == 1)//��
            {
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
            }
            else//��
            {
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
            }

            currentTime = 0f;
        }
    }
}

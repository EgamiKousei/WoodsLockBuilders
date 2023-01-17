using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�؂̎��������Ɋւ��鏈��
public class ForestMetalSpawn : MonoBehaviour
{
    public GameObject MetalPrefab;//�؂̃v���n�u
    float TimeSpan = 0;//�����X�p��
    private float currentTime = 0f;
    public bool thisRight = true;//�E�̏ꍇ�̓`�F�b�N
    public bool stop = false;//�؂̐������~���邩

    GameObject Carigge;//�n�Ԃ̃X�N���v�g
    public CariggeManager cariggeManager;

    public bool CariggeStoped = false;//�n�Ԃ��~�܂��Ă��邩

    // Start is called before the first frame update
    void Start()
    {
        Carigge = GameObject.Find("Carigge");
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        TimeSpan = Random.Range(6.5f, 12);
    }

    // Update is called once per frame
    void Update()
    {
        CariggeStoped = cariggeManager.CariggeStop;

        currentTime += Time.deltaTime;

        if (currentTime > TimeSpan)
        {
            TreeSpawner();
            if (thisRight == true)
            {
                TimeSpan = Random.Range(30f, 40);
            }
            else if (thisRight == false)
            {
                TimeSpan = Random.Range(30f, 40);
            }
            currentTime = 0f;
        }
    }

    void TreeSpawner()//pos���w�肵�Ė؂��X�|�[�������� pos�̓I�u�W�F�N�g�̏ꏊ
    {
        if (stop == false && CariggeStoped == false && cariggeManager.GameStart == true)
        {
            Vector3 Pos = this.gameObject.transform.position;
            Instantiate(MetalPrefab, Pos, Quaternion.identity);
        }

    }
    private void OnTriggerStay(Collider other)//�؂̐������X�g�b�v����ꏊ�ɋ߂Â��Ɛ������X�g�b�v
    {
        if (other.gameObject.name == "TreeSpawnStop")
        {
            stop = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "TreeSpawnStop")
        {
            stop = false;
            //     Debug.Log("exittt");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�R�C���Ɋւ���X�N���v�g
public class DropCoin : MonoBehaviour
{
    public GameObject Carigge;
    public GameObject CoinCollectPos;//���W�ꏊ
    public GameObject Coinmesh;//�R�C���̃��f��

    Vector3 CollectPos;

    // Start is called before the first frame update

    void Start()
    {
        
        Rigidbody rb = this.GetComponent<Rigidbody>();
        
        CoinCollectPos = GameObject.Find("CoinCollectPos");

        float X = Random.Range(-4, 4);
        float Z = Random.Range(-4, 4);
        Vector3 force = new Vector3(X, 5.0f, Z);
        rb.AddForce(force, ForceMode.Impulse);//�h���b�v�����u�Ԃɔ�яo���� 
        CollectPos = CoinCollectPos.transform.position;

        Invoke("MoveToCarigge",1);
        

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void MoveToCarigge()
    {
        
        Collider col = this.GetComponent<Collider>();
        col.isTrigger = true;
        Invoke("PlusCoin", 0.55f);
        transform.DOMove(CollectPos, 0.5f);//carigge�Ɍ������Ĕ��ł���
    }

    void DeleteObj()
    {
        Destroy(this.gameObject);
    }

    void PlusCoin()
    {
        Carigge = GameObject.Find("Carigge");
        Carigge.GetComponent<CariggeManager>().GetCoin();//�����R�C�����Z
        Destroy(this.gameObject);
    }
}

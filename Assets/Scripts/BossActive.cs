using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//�v���C���[�߂Â����ۂɃ{�X���A�N�e�B�u�ɂ���X�N���v�g
public class BossAvtive : MonoBehaviour
{
    public GameObject Base;//�{��
    Enemy enmScript;
    ChangeTag change;
    BossAdditional bossAdd;//�ǉ��X�N���v�g
    NavMeshAgent nav;
    BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        enmScript = Base.GetComponent<Enemy>();
        change = Base.GetComponent<ChangeTag>();
        nav = Base.GetComponent<NavMeshAgent>();
        bossAdd = Base.GetComponent<BossAdditional>();
        col = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("aaa");
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            nav.enabled = true;
            col.enabled = false;
            change.enabled = true;
            enmScript.enabled = true;
            bossAdd.enabled = true;
            
        }
    }
}

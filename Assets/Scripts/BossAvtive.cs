using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//�v���C���[�߂Â����ۂɃ{�X���A�N�e�B�u�ɂ���X�N���v�g
public class BossActive : MonoBehaviour
{
    public GameObject Base;//�{��
    Enemy enmScript;
    BossAdditional bossAdd;//�ǉ��X�N���v�g
    NavMeshAgent nav;
    BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        enmScript = Base.GetComponent<Enemy>();
        nav = Base.GetComponent<NavMeshAgent>();
        bossAdd = Base.GetComponent<BossAdditional>();
        col = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            nav.enabled = true;
            col.enabled = false;
            enmScript.enabled = true;
            bossAdd.enabled = true;
            
        }
    }
}

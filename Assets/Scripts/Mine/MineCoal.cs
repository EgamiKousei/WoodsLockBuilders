using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MineCoal : MonoBehaviour
{

    int life = 3;
    public GameObject CoalMesh;

    Vector3 DropDir;
    public GameObject CoalBlock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (life == 2)
        {
           // CoalMesh.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
        }
        else if (life == 1)
        {
          //  CoalMesh.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (life <=0)
        {
            SpawnBlock();
            this.gameObject.SetActive(false);
        }
    }

    void DamageTree()
    {
        life -= 1;
        transform.DOPunchRotation(new Vector3(0, 0, 2), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            DamageTree();
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        Vector3 Pos = this.gameObject.transform.position;
        Pos.z -= 3;
        Instantiate(CoalBlock, Pos, Quaternion.identity);
    }

}

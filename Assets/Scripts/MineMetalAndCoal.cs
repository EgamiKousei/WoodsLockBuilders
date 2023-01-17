using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MineMetalAndCoal : MonoBehaviour
{
    int life = 3;
    //public GameObject[] MetalMesh;
    //public GameObject[] CoalMesh;

    Vector3 DropDir;
    public GameObject DropMetal;
    public GameObject DropCoal;

    //public GameObject MetalItem;
    //public GameObject CoalItem;

    public bool thisCoal;
    // Start is called before the first frame update
    void Start()
    {
        if(this.transform.position.z >= 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            SpawnItem();
            GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npcèàóù
            this.gameObject.SetActive(false);
        }
    }

    void DamageOre()
    {
        life -= 1;
        transform.DOPunchRotation(new Vector3(0, 0, 2), 0.5f);
    }

    void SpawnItem()
    {
        Vector3 Pos = this.gameObject.transform.position;
        Pos.z -= 3;
        if (thisCoal == true)
        {
            Instantiate(DropCoal, Pos, Quaternion.identity);
        }
        else
        {
            Instantiate(DropMetal, Pos, Quaternion.identity);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            DamageOre();
            SpawnItem();
        }
    }
}

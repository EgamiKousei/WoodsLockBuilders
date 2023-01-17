using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ForestFlower : MonoBehaviour
{
    [SerializeField]
    int life = 1;

    Vector3 DropDir;
    public GameObject FlowerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)//�P��@�����ƉԂ̃A�C�e�����h���b�v����@dropflower�Q��
        {
            SpawnFlower();
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            life -= 1;
        }
    }

    void SpawnFlower()
    {
        Vector3 Pos = this.gameObject.transform.position;
        GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc����
        Instantiate(FlowerPrefab, Pos, Quaternion.identity);
    }
}

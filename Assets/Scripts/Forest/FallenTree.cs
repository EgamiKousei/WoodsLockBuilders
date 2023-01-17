using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallenTree : MonoBehaviour
{
    int life = 7;
    public GameObject TreeMesh;

    Vector3 DropDir;
    public GameObject WoodBlock;
    public GameObject Mesh;
    bool braked = false;

    GameObject Carigge;
    CariggeManager cariggeManager;

    // NpcÇÃà◊Ç…ïKóvÇ»ïœêî
    public float cariggeDist;
    NpcManager npcManager;

    // Start is called before the first frame update
    void Start()
    {
        Carigge = GameObject.Find("Carigge");
        cariggeManager = Carigge.GetComponent<CariggeManager>();

        npcManager = GameObject.Find("GameManager").GetComponent<NpcManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life == 4)
        {
           // TreeMesh.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        if (life == 3)
        {
            //TreeMesh.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        if (life == 2)
        {
           // TreeMesh.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else if (life == 1)
        {
           // TreeMesh.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        if (life <= 0&&braked == false)
        {
            //cariggeManager.CanClashTrue();
            SpawnBlock();
            TreeMesh.SetActive(false);
            braked = true;
            cariggeManager.BrokeFallenTree();
        }

        // Npcèàóù
        cariggeDist = Vector3.Distance(transform.position, Carigge.transform.position);

        if(cariggeDist <= 20)
        {
            npcManager.obstacle = this.gameObject;
            npcManager.CloseObstacle();
        }
    }

    void DamageTree()
    {
        life -= 1;
        Mesh.transform.DOPunchRotation(new Vector3(0, 0, 10), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            DamageTree();
        }
        if (other.gameObject.name == "DeleteTree")
        {
            npcManager.ResetObstacle();
            this.gameObject.SetActive(false);
        }
    }

    void SpawnBlock()
    {
        Vector3 Pos = TreeMesh.gameObject.transform.position;
        Instantiate(WoodBlock, Pos, Quaternion.identity);
        Instantiate(WoodBlock, Pos, Quaternion.identity);
    }
}

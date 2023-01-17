using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//木に関するスクリプト
public class ForestTree : MonoBehaviour
{

    int life = 3;//木の耐久
    public GameObject Mesh;//モデル

    public GameObject WoodBlock;//ドロップする木ブロック
    public GameObject DropPos;//ドロップ場所

    GameObject ArtiMain;//アーティファクトのスクリプト　木のドロップ量に関係
    ArtifactMain artiMain;


    // Start is called before the first frame update
    void Start()
    {
        ArtiMain = GameObject.Find("Artifact");
        artiMain = ArtiMain.GetComponent<ArtifactMain>();
        //Mesh.transform.Rotate(0.0f, Random.Range(0, 360), 0.0f);

        if (this.transform.position.z <=0)//右側にある場合反転
        {
            this.transform.Rotate(0.0f, 180, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (life <=0)//life0で破壊
        {
            Broken();
            this.gameObject.SetActive(false);
        }
    }

    void DamageTree()//木を攻撃した際の処理
    {
        life -= 1;
        transform.DOPunchRotation(new Vector3(0, 0, 2), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")//木を攻撃した際の処理
        {
            DamageTree();
            SpawnBlock();
        }
    }

    void SpawnBlock()//木ブロックのスポーン　アーティファクトでドロップ２倍を購入している場合は2売ドロップ
    {
        Vector3 Pos = DropPos.gameObject.transform.position;
        if(artiMain.DropMultiply == 1)
        {
            Instantiate(WoodBlock, Pos, Quaternion.identity);
        }
        else if (artiMain.DropMultiply == 2)
        {
            Instantiate(WoodBlock, Pos, Quaternion.identity);
            Pos.x += 1.2f;
            Instantiate(WoodBlock, Pos, Quaternion.identity);
        }
    }

    void Broken()//破壊された際の処理
    {
        Vector3 Pos = DropPos.gameObject.transform.position;
        Pos.y += 1;
        GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc処理
        Instantiate(WoodBlock, Pos, Quaternion.identity);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//草に関するスクリプト2　メイン
public class WeedSway : MonoBehaviour
{
    public GameObject Coin;//ドロップするコイン
    int probability;//コインをドロップするかどうかの判定

    public GameObject Mesh;//モデル
    public GameObject BrokenMesh;//破壊後のモデル
    // Start is called before the first frame update
    void Start()
    {
        probability = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.tag == "Npc")//プレイヤー・NPCがふれると揺れる
        {
            transform.DOPunchRotation(new Vector3(5, 0, 5), 0.5f);
        }
        if (other.gameObject.name == "AttackHantei")//攻撃判定に触れると草が刈り取られコインが1/3の確率でスポーン
        {
            Vector3 Pos = this.gameObject.transform.position;
            if (probability == 3)
            {
                Instantiate(Coin, Pos, Quaternion.identity);
            }

            GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc処理
            Mesh.gameObject.SetActive(false);
            BrokenMesh.gameObject.SetActive(true);

            BoxCollider col = GetComponent<BoxCollider>();
            col.enabled = false;
        }
    }
}

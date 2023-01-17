using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MetalForest : MonoBehaviour
{
    public GameObject Carigge;

    bool CanGrab = false;
    bool Grabed = false;
    bool CanTrackBefore = false;
    bool CanTrack = false;

    public GameObject OreCollectPos;
    public GameObject Itemtrigger;

    GameObject Player;
    public PlayerManager2 playerManager;
    public bool PlayerHaving = false;

    bool npcHit = false; // NPCが触れているかどうか
    bool npcProcess = false;// NPCの処理中かどうか
    GameObject npcObject; // NPCオブジェクト

    public bool npcHaving = false;//NPCが何か持ってるか

    // Start is called before the first frame update
    void Start()
    {
        Carigge = GameObject.Find("Minecart");
        Player = GameObject.Find("Player");
        playerManager = Player.GetComponent<PlayerManager2>();
        OreCollectPos = GameObject.Find("WoodCollectPos");
        float X = Random.Range(-4, 4);
        float Z = Random.Range(-4, 4);


        Rigidbody rb = this.GetComponent<Rigidbody>();
        Vector3 force = new Vector3(X, 10.0f, Z);
        rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (npcHit == false)
        {
            PlayerHaving = playerManager.HandHaving;

            if (CanGrab == true && PlayerHaving == false && Input.GetMouseButtonDown(1))
            {
                npcProcess = false;
                GrabItem();
            }
            if (CanTrack == true && Grabed == true && Input.GetMouseButtonDown(1) && PlayerHaving == true)
            {
                npcProcess = false;
                Track();
            }
            if (Grabed == true)
            {
                this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        // NPCの処理
        else if (npcHit == true)
        {
            npcHaving = npcObject.GetComponent<Nav>().HandHaving;

            if (CanGrab == true && npcHaving == false)
            {
                npcProcess = true;
                GrabItem();//何も持ってない場合掴む
            }
            if (CanTrack == true && Grabed == true && npcHaving == true)
            {
                npcProcess = true;
                npcObject.GetComponent<Nav>().m_Agent.speed = 0;
                Track();//NPCが掴んでいる場合馬車に積む
            }
            if (Grabed == true)//ポジションのリセット
            {
                this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }

        }
    }

    public void Track()
    {
        if (npcProcess == false)
        {
            playerManager.HandHaving = false;
            Vector3 pos = OreCollectPos.transform.position;
            transform.parent = null;
            transform.DOLocalJump(pos, 3f, 1, 0.5f)
            .SetEase(Ease.Linear);
            CanTrack = false;
            Grabed = false;
            Invoke("PlusMetal", 0.6f);
        }
        else if (npcProcess == true)
        {
            npcObject.GetComponent<Nav>().HandHaving = false;
            Vector3 pos = OreCollectPos.transform.position;//posを指定
            transform.parent = null;//子を解除
            transform.DOLocalJump(pos, 3f, 1, 0.6f);//馬車に向かってジャンプアニメーション
            CanTrack = false;
            Grabed = false;
            Invoke("PlusMetal", 0.6f);//0.6秒後に木をプラスする
        }

    }
    public void GrabItem()
    {
        if (npcProcess == false)
        {
            playerManager.HandHaving = true;
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.useGravity = false;
            Collider col = this.GetComponent<MeshCollider>();
            col.isTrigger = true;
            transform.parent = GameObject.Find("GrabHand").transform;
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            this.transform.Rotate(0.0f, 0.0f, 0.0f);
            Grabed = true;
            CanGrab = false;
            Itemtrigger.transform.DOLocalMoveY(10, 0.2f)
            .OnComplete(() =>
            {
                Itemtrigger.SetActive(false);
            });

            Invoke("cantrackbeforeon", 0.2f);
        }
        else if (npcProcess == true)
        {
            npcObject.GetComponent<Nav>().HandHaving = true;

            Rigidbody rb = this.GetComponent<Rigidbody>();//rigidbodyとcoliderを無効にする
            rb.useGravity = false;
            Collider col = this.GetComponent<BoxCollider>();
            col.isTrigger = true;

            transform.parent = npcObject.transform.GetChild(0);//手の位置へ移動
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);//positionとrotationをリセット
            this.transform.Rotate(0.0f, 0.0f, 0.0f);
            Grabed = true;
            CanGrab = false;
            Itemtrigger.transform.DOLocalMoveY(10, 0.2f)//トリガーを避難
            .OnComplete(() =>
            {
                Itemtrigger.SetActive(false);
            });

            Invoke("cantrackbeforeon", 0.2f);//持ってから0.2秒後に積めるようになる
        }
    }
        void cantrackbeforeon()
    {
        CanTrackBefore = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (npcHit == false)
            {
                npcProcess = false;
            }
        }

        // NPCの処理
        if (other.gameObject.tag == "Npc")
        {
            npcHit = true;
            npcObject = other.gameObject;
        }

        if (other.gameObject.name == "GrabItemPoint" && Grabed == false)
        {
            CanGrab = true;
        }
        if (other.gameObject.name == "TrackZone" && CanTrackBefore == true)
        {
            CanTrack = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // NPCの処理
        if (other.gameObject.tag == "Npc")
        {
            npcHit = false;
        }

        if (other.gameObject.name == "GrabItemPoint")
        {
            CanGrab = false;
        }
        if (other.gameObject.name == "TrackZone")
        {
            CanTrack = false;
        }


    }
  
    void PlusMetal()
    {
        Carigge = GameObject.Find("Carigge");
        Carigge.GetComponent<CariggeManager>().GetMetal();
        if (npcProcess == true)
        {
            npcObject.GetComponent<Nav>().HandHaving = false;
            npcObject.GetComponent<Nav>().m_Agent.speed = 6.5f;
        }
        GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc処理
        Destroy(this.gameObject);
    }
}

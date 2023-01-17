using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropOre : MonoBehaviour
{
    public GameObject Cart;

    bool CanGrab = false;
    bool Grabed = false;
    bool CanTrackBefore = false;
    bool CanTrack = false;

    public GameObject OreCollectPos;
    public GameObject Itemtrigger;

    GameObject Player;
    public PlayerManager2 playerManager;
    public bool PlayerHaving = false;

    public bool thisCoal = true;
    // Start is called before the first frame update
    void Start()
    {
        Cart = GameObject.Find("Minecart");
        Player = GameObject.Find("Player");
        playerManager = Player.GetComponent<PlayerManager2>();
        OreCollectPos = GameObject.Find("OreCollectPos");
        float X = Random.Range(-4, 4);
        float Z = Random.Range(-4, 4);


        Rigidbody rb = this.GetComponent<Rigidbody>();
        Vector3 force = new Vector3(X, 10.0f, Z);
        rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHaving = playerManager.HandHaving;

        if (CanGrab == true && PlayerHaving == false && Input.GetMouseButtonDown(1))
        {
            GrabItem();
        }
        if (CanTrack == true && Grabed == true && Input.GetMouseButtonDown(1) && PlayerHaving == true)
        {
            Track();
        }
        if (Grabed == true)
        {
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    public void Track()
    {
        playerManager.HandHaving = false;
        Vector3 pos = OreCollectPos.transform.position;
        transform.parent = null;
        transform.DOLocalJump(pos,3f,1,0.5f)
        .SetEase(Ease.Linear);
        CanTrack = false;
        Grabed = false;
        if (thisCoal == true)
        {
            Invoke("PlusCoal", 0.6f);
        }
        else
        {
            Invoke("PlusMetal", 0.6f);
        }
        
    }
    public void GrabItem()
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
    void cantrackbeforeon()
    {
        CanTrackBefore = true;
    }
    private void OnTriggerStay(Collider other)
    {
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
        if (other.gameObject.name == "GrabItemPoint")
        {
            CanGrab = false;
        }
        if (other.gameObject.name == "TrackZone")
        {
            CanTrack = false;
        }

       
    }
    void PlusCoal()
    {
       // Cart = GameObject.Find("Minecart");
        Cart.GetComponent<MinecartManager>().GetCoal();
        Destroy(this.gameObject);
    }
    void PlusMetal()
    {
      //  Cart = GameObject.Find("Minecart");
        Cart.GetComponent<MinecartManager>().GetMetal();
        Destroy(this.gameObject);
    }
}

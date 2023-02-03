using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�؃u���b�N�Ɋւ���X�N���v�g
public class DropWoodBlock : MonoBehaviour
{
    public GameObject Carigge;//�n��

    bool CanGrab = false;//�͂߂邩
    bool Grabed = false;//�͂܂ꂽ
    bool CanTrackBefore = false;//�n�Ԃɐς߂邩
    bool CanTrack = false;

    public GameObject WoodCollectPos;//�l�܂��ꏊ
    public GameObject Itemtrigger;//�ς߂锻��

    GameObject Player;
    public PlayerManager2 playerManager;

    public bool PlayerHaving = false;//�v���C���[�����������Ă邩

    bool npcHit = false; // NPC���G��Ă��邩�ǂ���
    bool npcProcess = false;// NPC�̏��������ǂ���
    GameObject npcObject; // NPC�I�u�W�F�N�g

    public bool npcHaving = false;//NPC�����������Ă邩

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        playerManager = Player.GetComponent<PlayerManager2>();
        WoodCollectPos = GameObject.Find("WoodCollectPos");

        float X = Random.Range(-4, 4);//�h���b�v�����Ƃ��ɏ�Ƀ|�b�v����
        float Z = Random.Range(-4, 4);
        Rigidbody rb = this.GetComponent<Rigidbody>();  
        Vector3 force = new Vector3(X, 10.0f, Z);      
        rb.AddForce(force,ForceMode.Impulse);
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
                GrabItem();//���������ĂȂ��ꍇ�͂�
            }
            if (CanTrack == true && Grabed == true && Input.GetMouseButtonDown(1) && PlayerHaving == true)
            {
                npcProcess = false;
                Track();//�v���C���[���͂�ł���ꍇ�n�Ԃɐς�
            }
            if (Grabed == true)//�|�W�V�����̃��Z�b�g
            {
                this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        // NPC�̏���
        else if (npcHit == true)
        {
            npcHaving = npcObject.GetComponent<Nav>().HandHaving;

            if (CanGrab == true && npcHaving == false)
            {
                npcProcess = true;
                GrabItem();//���������ĂȂ��ꍇ�͂�
            }
            if (CanTrack == true && Grabed == true && npcHaving == true)
            {
                npcProcess = true;
                npcObject.GetComponent<Nav>().m_Agent.speed = 0;
                Track();//NPC���͂�ł���ꍇ�n�Ԃɐς�
            }
            if (Grabed == true)//�|�W�V�����̃��Z�b�g
            {
                this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(npcHit == false)
            {
                npcProcess = false;
            }
        }

        // NPC�̏���
        if(other.gameObject.tag == "Npc")
        {
            npcHit = true;
            npcObject = other.gameObject;
        }

        if (other.gameObject.name == "GrabItemPoint"&&Grabed ==false)
        {
            CanGrab = true;     
        }
        if (other.gameObject.name == "TrackZone"&& CanTrackBefore == true)
        {
            CanTrack = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // NPC�̏���
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

    public void GrabItem()
    {
        if (npcProcess == false)
        {
            playerManager.HandHaving = true;//�v���C���[�͎�ɂ��̂������Ă���


            Rigidbody rb = this.GetComponent<Rigidbody>();//rigidbody��colider�𖳌��ɂ���
            rb.useGravity = false;
            Collider col = this.GetComponent<BoxCollider>();
            col.isTrigger = true;

            transform.parent = GameObject.Find("GrabHand").transform;//��̈ʒu�ֈړ�
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);//position��rotation�����Z�b�g
            this.transform.Rotate(0.0f, 0.0f, 0.0f);
            Grabed = true;
            CanGrab = false;
            Itemtrigger.transform.DOLocalMoveY(10, 0.2f)//�g���K�[����
            .OnComplete(() =>
            {
                Itemtrigger.SetActive(false);
            });

            Invoke("cantrackbeforeon", 0.2f);//�����Ă���0.2�b��ɐς߂�悤�ɂȂ�
        }
        else if(npcProcess == true)
        {
            npcObject.GetComponent<Nav>().HandHaving = true;

            Rigidbody rb = this.GetComponent<Rigidbody>();//rigidbody��colider�𖳌��ɂ���
            rb.useGravity = false;
            Collider col = this.GetComponent<BoxCollider>();
            col.isTrigger = true;

            transform.parent = npcObject.transform.GetChild(0);//��̈ʒu�ֈړ�
            this.gameObject.transform.localPosition = new Vector3(0, 0, 0);//position��rotation�����Z�b�g
            this.transform.Rotate(0.0f, 0.0f, 0.0f);
            Grabed = true;
            CanGrab = false;
            Itemtrigger.transform.DOLocalMoveY(10, 0.2f)//�g���K�[����
            .OnComplete(() =>
            {
                Itemtrigger.SetActive(false);
            });

            Invoke("cantrackbeforeon", 0.2f);//�����Ă���0.2�b��ɐς߂�悤�ɂȂ�
        }
    }

    void cantrackbeforeon()
    {
        CanTrackBefore = true;
    }

    public void Track()//�n�Ԃɐς�
    {
        if (npcProcess == false)
        {
            playerManager.HandHaving = false;//�v���C���[�͎����ĂȂ�����
            Vector3 pos = WoodCollectPos.transform.position;//pos���w��
            transform.parent = null;//�q������
            transform.DOLocalJump(pos, 3f, 1, 0.6f);//�n�ԂɌ������ăW�����v�A�j���[�V����
            CanTrack = false;
            Grabed = false;
            Invoke("PlusWood", 0.6f);//0.6�b��ɖ؂��v���X����
        }
        else if (npcProcess == true)
        {
            npcObject.GetComponent<Nav>().HandHaving = false;
            Vector3 pos = WoodCollectPos.transform.position;//pos���w��
            transform.parent = null;//�q������
            transform.DOLocalJump(pos, 3f, 1, 0.6f);//�n�ԂɌ������ăW�����v�A�j���[�V����
            CanTrack = false;
            Grabed = false;
            Invoke("PlusWood", 0.6f);//0.6�b��ɖ؂��v���X����
        }
    }

    void PlusWood()//�؂̏��������v���X�������ƃf���[��
    {
        Carigge = GameObject.Find("Carigge");
        Carigge.GetComponent<CariggeManager>().GetWood();
        if(npcProcess == true)
        {
            npcObject.GetComponent<Nav>().HandHaving = false;
            npcObject.GetComponent<Nav>().m_Agent.speed = 6.5f;
            npcObject.GetComponent<Nav>().GetPoint("Wood");
        }
        GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc����
        Destroy(this.gameObject);
    }
}

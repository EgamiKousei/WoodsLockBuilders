using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�؂Ɋւ���X�N���v�g
public class ForestTree : MonoBehaviour
{

    int life = 3;//�؂̑ϋv
    public GameObject Mesh;//���f��

    public GameObject WoodBlock;//�h���b�v����؃u���b�N
    public GameObject DropPos;//�h���b�v�ꏊ

    GameObject ArtiMain;//�A�[�e�B�t�@�N�g�̃X�N���v�g�@�؂̃h���b�v�ʂɊ֌W
    ArtifactMain artiMain;


    // Start is called before the first frame update
    void Start()
    {
        ArtiMain = GameObject.Find("Artifact");
        artiMain = ArtiMain.GetComponent<ArtifactMain>();
        //Mesh.transform.Rotate(0.0f, Random.Range(0, 360), 0.0f);

        if (this.transform.position.z <=0)//�E���ɂ���ꍇ���]
        {
            this.transform.Rotate(0.0f, 180, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (life <=0)//life0�Ŕj��
        {
            Broken();
            this.gameObject.SetActive(false);
        }
    }

    void DamageTree()//�؂��U�������ۂ̏���
    {
        life -= 1;
        transform.DOPunchRotation(new Vector3(0, 0, 2), 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")//�؂��U�������ۂ̏���
        {
            DamageTree();
            SpawnBlock();
        }
    }

    void SpawnBlock()//�؃u���b�N�̃X�|�[���@�A�[�e�B�t�@�N�g�Ńh���b�v�Q�{���w�����Ă���ꍇ��2���h���b�v
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

    void Broken()//�j�󂳂ꂽ�ۂ̏���
    {
        Vector3 Pos = DropPos.gameObject.transform.position;
        Pos.y += 1;
        GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc����
        Instantiate(WoodBlock, Pos, Quaternion.identity);
    }

}

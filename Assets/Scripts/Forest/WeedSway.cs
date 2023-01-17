using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//���Ɋւ���X�N���v�g2�@���C��
public class WeedSway : MonoBehaviour
{
    public GameObject Coin;//�h���b�v����R�C��
    int probability;//�R�C�����h���b�v���邩�ǂ����̔���

    public GameObject Mesh;//���f��
    public GameObject BrokenMesh;//�j���̃��f��
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
        if (other.gameObject.name == "Player" || other.gameObject.tag == "Npc")//�v���C���[�ENPC���ӂ��Ɨh���
        {
            transform.DOPunchRotation(new Vector3(5, 0, 5), 0.5f);
        }
        if (other.gameObject.name == "AttackHantei")//�U������ɐG���Ƒ����������R�C����1/3�̊m���ŃX�|�[��
        {
            Vector3 Pos = this.gameObject.transform.position;
            if (probability == 3)
            {
                Instantiate(Coin, Pos, Quaternion.identity);
            }

            GameObject.Find("GameManager").GetComponent<NpcManager>().StartCoroutine("ChangeDelay");//Npc����
            Mesh.gameObject.SetActive(false);
            BrokenMesh.gameObject.SetActive(true);

            BoxCollider col = GetComponent<BoxCollider>();
            col.enabled = false;
        }
    }
}

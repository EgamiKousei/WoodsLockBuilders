using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�̕���̌���Ɋւ���X�N���v�g
public class RoadSignArrow : MonoBehaviour
{
    public GameObject obj;//�I�u�W�F�N�g
    public Material DefMat;//�f�t�H���g�̃}�e���A��
    public Material ShineMat;//�A�N�e�B�u�ȃ}�e���A��

    GameObject Carigge;

    public bool ThisRight = true;//�E�̏ꍇ�`�F�b�N
    public bool ThisP2 = true;//�t�F�[�Y2�̏ꍇ�`�F�b�N
    bool CanSelect = false;//�Z���N�g�ł��邩

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanSelect == true)//�Z���N�g�ł���ꏊ�̏�ɏ������Ԃ�F�������Ǝ��s�����
        {
            PhaseSet();
        }
    }

    private void OnTriggerEnter(Collider other)//���邢�}�e���A���ւ̕ύX
    {
        if(other.gameObject.name == "Player")
        {
            obj.GetComponent<MeshRenderer>().material = ShineMat;
            CanSelect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            obj.GetComponent<MeshRenderer>().material = DefMat;
            CanSelect = false;
        }
    }

    void PhaseSet()//���ꂼ��̕�����Z�b�g����
    {
        Carigge = GameObject.Find("Carigge");
       
        if (ThisRight == true&&ThisP2 == true)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase2R();
        }
        else if(ThisRight == false&&ThisP2 == true)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase2L();
        }
        else if(ThisRight == true&&ThisP2 == false)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase3R();
        }
        else if (ThisRight == false && ThisP2 == false)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase3L();
        }
    }

}

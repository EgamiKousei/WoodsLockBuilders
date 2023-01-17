using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//�W���ꏊ�A�Q�[���X�^�[�g�Ɋւ���X�N���v�g
public class AssemblyPoint : MonoBehaviour
{
    public Slider slider;//�X���C�_�[
    bool GetOnChara = false;//�L��������ɍڂ��Ă邩
    public GameObject Carigge;
    CariggeManager cariggeManager;

    public GameObject cnv;//UI�L�����o�X
    BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        sliderplus();
        if(slider.value >= 1)//�Q�[�W�}�b�N�X�ŊJ�n
        {
            Invoke("EndStartAnim", 3);
            cnv.SetActive(false);
            col.enabled = false;
        }
    }

    void EndStartAnim()//3�b�Ԃ��炢�̊J�n�A�j���[�V����������i�\��j
    {
        this.gameObject.SetActive(false);
        cariggeManager.AsembStart();
    }

    void sliderplus()//�L��������ɍڂ��Ă�Ƃ��Q�[�W�������Ă���
    {
        if(GetOnChara == true)
        {
            slider.value += 0.006f;
        }
        else
        {
            slider.value = 0f;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            GetOnChara = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            GetOnChara = false;
        }
    }
}

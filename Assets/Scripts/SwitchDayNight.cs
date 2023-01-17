using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

//����؂�ւ��̃X�N���v�g
public class SwitchDayNight : MonoBehaviour
{
    public int DayNight = 1;//1=�Ђ� 2=���

    public GameObject Light;//directinallight

    float Span = 60f;//�X�p��
    //float Span = 20f;//�X�p��
    private float currentTime = 0f;

    private Animator anim;//�A�j���[�^�[

    public GameObject Carigge;//�n��
    CariggeManager cariggeManager;
    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        anim = gameObject.GetComponent<Animator>();
    }

   
    void Update()//�A�j���[�^�[���g�p����light�̖��x��������
    {
        if (cariggeManager.GameStart == true)
        {
            currentTime += Time.deltaTime;

            if (currentTime > Span)
            {
                if (DayNight == 1)//��ɂȂ�
                {
                    DayNight = 2;
                    anim.SetBool("SetNight", true);
                    anim.SetBool("SetDay", false);
                    Span = 30;
                }
                else if (DayNight == 2)//���ɂȂ�
                {
                    DayNight = 1;
                    anim.SetBool("SetNight", false);
                    anim.SetBool("SetDay", true);
                    Span = 60;
                }
                currentTime = 0f;
            }
        }
        
      
    }

     
}


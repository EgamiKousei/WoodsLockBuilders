using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�v���C���[�Ɋւ���X�N���v�g
public class PlayerManager2 : MonoBehaviour
{
    float MaxHP = 100;//hp
    public float Hp = 100;
    

    public Slider HPbar;//HP�o�[
    public Image HealBar;//�񕜂���Ƃ��̃o�[

    public bool CanHeal = false;//�񕜂ł��邩
    public bool HealBarStart = false;//��UI�X�^�[�g
    public bool StartHeal = false;//�񕜃X�^�[�g


    public float HealInterval = 500;//���ɉ񕜉\�ɂȂ�܂ł̎��ԁi�t���[�����j

    public bool HandHaving = false;//�����A�C�e���������Ă邩

    public GameObject AttackHantei;//�U������
    bool CanAttack = true;//�U���\��

    public GameObject DeadEffect;//���S�G�t�F�N�g

    //public float tDist;
    //public GameObject carigge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //tDist = Vector3.Distance(transform.position, carigge.transform.position);

        HealthBar();
        HealStartPushKey();

        if (Hp >= MaxHP)
            Hp = MaxHP;
        if (Hp >= 100)//hp100���ƃq�[���s��
        {
            StartHeal = false;
        }

        if (HealBar.fillAmount >=1)
        {
            StartHeal = true;
            HealInterval = 0;
        }

        Attack();

        if (Hp <=0)
        {
            //DeadEffect.SetActive(true);
            this.gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        HealBarPlus();
        FlowerHeal();
    }



    void FlowerHeal()//��
    {
        if(StartHeal == true&& Hp < 100)
        {
            Hp += 0.1f;
            if (Hp >= 100)
            {
                StartHeal = false;
            }
        }
        HealInterval += 1;
    }

    void HealBarPlus()//�񕜃o�[�i�ۂ���j
    {
        if (HealBarStart == true)
        {
            HealBar.fillAmount += 0.01f;
        }
        if (CanHeal == false)
        {
            HealBar.fillAmount = 0f;
        }
    }


    void HealthBar()//HP�o�[
    {
        HPbar.value = Hp;
    }

    public void SwitchCanHeal()
    {
        CanHeal = true;   
    }

    void HealStartPushKey()//�Ԃ̉��ɗ�����F�L�[�������Ɖ�
    {
        if (Input.GetKeyDown(KeyCode.F)&& CanHeal == true&&HealInterval >= 500&&Hp < MaxHP)
        {
            HealBarStart = true;
        }
    }

    public void SwitchCanHealFalse()
    {
        CanHeal = false;
        HealBarStart = false;
    }

    void Attack()//�U��
    {
        if (Input.GetMouseButtonDown(0))//�}�E�X�N���b�N�ōU��
        {
            if (CanAttack == true&&HandHaving == false)
            {
                CanAttack = false;
                AttackHantei.SetActive(true);
                Invoke("HanteiDel", 0.2f);
                Invoke("CanAttackTrue", 0.5f);
            }
        }
    }
    void HanteiDel()
    {
        AttackHantei.SetActive(false);
    }

    void CanAttackTrue()
    {

        CanAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "blowHantei")
        {
            Hp -= 2;
        }

        if (other.gameObject.name == "CircleHantei")
        {
            Hp -= 4;
        }

        if (other.gameObject.name == "LineHantei")
        {
            Hp -= 3;
        }

        if (other.gameObject.name == "FanHantei")
        {
            Hp -= 3;
        }
    }
}

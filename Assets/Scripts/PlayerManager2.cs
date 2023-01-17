using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーに関するスクリプト
public class PlayerManager2 : MonoBehaviour
{
    float MaxHP = 100;//hp
    public float Hp = 100;
    

    public Slider HPbar;//HPバー
    public Image HealBar;//回復するときのバー

    public bool CanHeal = false;//回復できるか
    public bool HealBarStart = false;//回復UIスタート
    public bool StartHeal = false;//回復スタート


    public float HealInterval = 500;//次に回復可能になるまでの時間（フレーム数）

    public bool HandHaving = false;//何かアイテムを持ってるか

    public GameObject AttackHantei;//攻撃判定
    bool CanAttack = true;//攻撃可能か

    public GameObject DeadEffect;//死亡エフェクト

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
        if (Hp >= 100)//hp100だとヒール不可
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



    void FlowerHeal()//回復
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

    void HealBarPlus()//回復バー（丸いやつ）
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


    void HealthBar()//HPバー
    {
        HPbar.value = Hp;
    }

    public void SwitchCanHeal()
    {
        CanHeal = true;   
    }

    void HealStartPushKey()//花の横に立ってFキーを押すと回復
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

    void Attack()//攻撃
    {
        if (Input.GetMouseButtonDown(0))//マウスクリックで攻撃
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

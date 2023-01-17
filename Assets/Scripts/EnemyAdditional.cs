using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�G�l�~�[�̒ǉ��X�N���v�g
public class EnemyAdditional : MonoBehaviour
{
    Enemy enemy;

    private void Start()
    {
        enemy = this.GetComponent<Enemy>();
    }

    private void Update()
    {
        
        if (enemy.hp <= 0)
        {
            //this.gameObject.SetActive(false);
            enemy.isDes = true;
        }
        
        
    }

    void Damage()//�U������ɐG����hp-10
    {
        enemy.hp -= 10;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            Damage();
        }
    }
}

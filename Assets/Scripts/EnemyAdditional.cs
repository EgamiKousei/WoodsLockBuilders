using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//エネミーの追加スクリプト
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

    void Damage()//攻撃判定に触れるとhp-10
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

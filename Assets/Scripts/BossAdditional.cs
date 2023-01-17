using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//ボスの追加スクリプト
public class BossAdditional : MonoBehaviour
{
    Vector3 JumpPos;

    Enemy enemy;

    GameObject JumpTarget;

    public GameObject Tresure;
    // Start is called before the first frame update
    void Start()
    {
        enemy = this.GetComponent<Enemy>();
        this.tag = "Enemy";
        //JumpTarget = enemy.m_Target[0];
        JumpTarget = GameObject.FindGameObjectWithTag("Player");
        Invoke("MiddleBossActive", 1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (enemy.hp <= 0)
        {
            Vector3 Pos = this.transform.position;
            Instantiate(Tresure, Pos, Quaternion.identity);
            enemy.isDes = true;
            //this.gameObject.SetActive(false);
        }
    }

   /* public void MiddleBossActive()
    {
        //Debug.Log("jump");
        JumpPos = JumpTarget.transform.position;
        transform.DOJump(
    JumpPos,5f,1,1f);
    }*/

    void Damage()
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
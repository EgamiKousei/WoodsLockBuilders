using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float x;
    private float z;
    public float Speed = 1.0f;
    float smooth = 10f;
    private Rigidbody rb;
    private Animator animator;
    public Collider attackCollider;

    void Start()
    {
        animator = GetComponent<Animator>();   //アニメーションを取得する
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");


        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.velocity = new Vector3(x, 0, z) * Speed;       //歩く速度
        animator.SetFloat("Walk", rb.velocity.magnitude);        //歩くアニメーションに切り替える

        if (target_dir.magnitude > 0.1)
        {
            //キーを押し方向転換
            Quaternion rotation = Quaternion.LookRotation(target_dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");    //マウスクリックで攻撃モーション
        }

    }


    //武器の判定を有効or無効切り替える
    public void OffColliderAttack()
    {
        attackCollider.enabled = false;
    }
    public void OnColliderAttack()
    {
        attackCollider.enabled = true;
    }


    //被ダメージアニメーションを発生させる
   /* private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent();
        if (damager != null)
        {
            //敵の剣に当たったら被ダメアニメーション発生
            animator.SetTrigger("Gethit");
        }
    }*/
}
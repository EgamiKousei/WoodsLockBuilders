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
        animator = GetComponent<Animator>();   //�A�j���[�V�������擾����
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");


        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.velocity = new Vector3(x, 0, z) * Speed;       //�������x
        animator.SetFloat("Walk", rb.velocity.magnitude);        //�����A�j���[�V�����ɐ؂�ւ���

        if (target_dir.magnitude > 0.1)
        {
            //�L�[�����������]��
            Quaternion rotation = Quaternion.LookRotation(target_dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smooth);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");    //�}�E�X�N���b�N�ōU�����[�V����
        }

    }


    //����̔����L��or�����؂�ւ���
    public void OffColliderAttack()
    {
        attackCollider.enabled = false;
    }
    public void OnColliderAttack()
    {
        attackCollider.enabled = true;
    }


    //��_���[�W�A�j���[�V�����𔭐�������
   /* private void OnTriggerEnter(Collider other)
    {
        Damager damager = other.GetComponent();
        if (damager != null)
        {
            //�G�̌��ɓ����������_���A�j���[�V��������
            animator.SetTrigger("Gethit");
        }
    }*/
}
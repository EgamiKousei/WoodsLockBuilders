using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satuei : MonoBehaviour
{
    public float speed; //�v���C���[�̓����X�s�[�h

    private Vector3 Player_pos; //�v���C���[�̃|�W�V����
    private float x; //x������Imput�̒l
    private float z; //z������Input�̒l
    private Rigidbody rigd;
    private Vector3 latestPos;  //�O���Position
    public Animator animator;  // �A�j���[�^�[�R���|�[�l���g�擾�p

    void Start()
	{
        Player_pos = GetComponent<Transform>().position; //�ŏ��̎��_�ł̃v���C���[�̃|�W�V�������擾
        rigd = GetComponent<Rigidbody>(); //�v���C���[��Rigidbody���擾
        animator.SetFloat("Walk", 1);
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal"); //x������Input�̒l���擾
        z = Input.GetAxis("Vertical"); //z������Input�̒l���擾

        rigd.velocity = new Vector3(x * speed, 0, z * speed); //�v���C���[��Rigidbody�ɑ΂���Input��speed���|�����l�ōX�V���ړ�

        Vector3 diff = transform.position - Player_pos; //�v���C���[���ǂ̕����ɐi��ł��邩���킩��悤�ɁA�����ʒu�ƌ��ݒn�̍��W�������擾

        if (diff.magnitude > 0.01f) //�x�N�g���̒�����0.01f���傫���ꍇ�Ƀv���C���[�̌�����ς��鏈��������(0�ł͓���Ȃ��̂Łj
        {
            transform.rotation = Quaternion.LookRotation(diff);  //�x�N�g���̏���Quaternion.LookRotation�Ɉ����n����]�ʂ��擾���v���C���[����]������
        }

        Player_pos = transform.position; //�v���C���[�̈ʒu���X�V

        if(Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("Attack");
        }

    }
}

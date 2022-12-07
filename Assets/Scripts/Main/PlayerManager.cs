using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //�ړ����x�̒�`
     float NomalSpeed = 300f;
     float SprintSpeed = 400f;
    float PlayerSpeed;
    public static float Gravi=100f;
    public static float JumpGravi = 100f;

    Rigidbody rb;//���M�b�h�{�f�B

    private Animator _animator;

    private Vector3 latestPos;
    float inputHorizontal;
    float inputVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& _animator.GetBool("Jump")==false)
        {
            _animator.SetBool("Jump", true);
            rb.AddForce(transform.up * JumpGravi, ForceMode.Impulse);
            Multicast.SendPlayerAction("Jump", transform.position, transform.rotation.y);
            Invoke("JumpEnd", 0.8f);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A))
        {
            _animator.SetBool("Move", true);
            Multicast.SendPlayerAction("Move", transform.position, transform.rotation.y);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A))
        {
            _animator.SetBool("Move", false);
            Multicast.SendPlayerAction("MoveEnd", transform.position, transform.rotation.y);
        }
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }
    void JumpEnd()
    {
        _animator.SetBool("Jump", false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, -Gravi, 0));
        if (Input.GetKey(KeyCode.LeftShift))
            PlayerSpeed = SprintSpeed;
        else
            PlayerSpeed = NomalSpeed;

        if (rb.velocity.magnitude > PlayerSpeed)
            rb.velocity = new Vector3(rb.velocity.x / 1.1f, rb.velocity.y, rb.velocity.z / 1.1f);
        else
        {
            // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

            rb.AddForce(moveForward * PlayerSpeed, ForceMode.Force);

            Vector3 differenceDis = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(latestPos.x, 0, latestPos.z);
            latestPos = transform.position;
            if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(differenceDis);
                rot = Quaternion.Slerp(rb.transform.rotation, rot, 0.1f);
                transform.rotation = rot;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //移動速度の定義
     float NomalSpeed = 300f;
     float SprintSpeed = 400f;
    float PlayerSpeed;
    public static float Gravi=100f;
    public static float JumpGravi = 100f;

    Rigidbody rb;//リギッドボディ

    private Animator _animator;

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

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A))
        {
            _animator.SetBool("Move", false);
            Multicast.SendPlayerAction("MoveEnd", transform.position, transform.rotation.y);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A))
        {
            _animator.SetBool("Move", true);
            Multicast.SendPlayerAction("Move", transform.position, transform.rotation.y);
        }
    }
    void JumpEnd()
    {
        _animator.SetBool("Jump", false);
        Multicast.SendPlayerAction("JumpEnd", transform.position, transform.rotation.y);
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
            if (Input.GetKey(KeyCode.W)) 
                rb.AddForce(transform.forward* PlayerSpeed, ForceMode.Force);  // 前   
            if (Input.GetKey(KeyCode.D))
                rb.AddForce(transform.right * PlayerSpeed, ForceMode.Force);  // 右
            if (Input.GetKey(KeyCode.S))
                rb.AddForce(-transform.forward * PlayerSpeed, ForceMode.Force);  // 後
            if (Input.GetKey(KeyCode.A))
                rb.AddForce(-transform.right * PlayerSpeed, ForceMode.Force);  // 左
        }
    }
}

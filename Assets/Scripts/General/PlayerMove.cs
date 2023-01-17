using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの移動スクリプト（大多和作）
public class PlayerMove : MonoBehaviour
{
    private Vector3 latestPos;
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;

    float NomalSpeed = 300f;
    float SprintSpeed = 400f;
    float PlayerSpeed;
    public static float Gravi = 100f;

    private int upForce = 2000;
    private bool isGround = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

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
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とカメラの向きから、移動方向を決定
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

        if (Input.GetKeyDown("space") && isGround)
            rb.AddForce(new Vector3(0, upForce, 0));
    }

   
}


using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Transform _Transform;

    //移動速度の定義
    float NomalSpeed = 300f;
     float SprintSpeed = 400f;
    float PlayerSpeed;
    public static float Gravi=120f;
    public static float JumpGravi = 100f;

    Rigidbody rb;//リギッドボディ

    private Animator _animator;

    private Vector3 latestPos;
    float inputHorizontal;
    float inputVertical;

    public Material PlayerMate;

    Transform CameraTrans;

    public static int jumpParamHash, moveParamHash;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        //プレイヤーの色の設定
        float r = (Convert.ToInt32(PlayerData.SaveData["color"], 16) >> 16) & 0xff;
        float g = (Convert.ToInt32(PlayerData.SaveData["color"], 16) >> 8) & 0xff;
        float b = Convert.ToInt32(PlayerData.SaveData["color"], 16) & 0xff; new Color(r / 255, g / 255, b / 255);
        PlayerMate.color= new Color(r / 255, g / 255, b / 255);

        _Transform = transform;
        CameraTrans = Camera.main.transform;

        jumpParamHash = Animator.StringToHash("Jump");
        moveParamHash= Animator.StringToHash("Move");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space)&& _animator.GetBool("Jump") == false)
        {
                _animator.SetBool(jumpParamHash, true);
                rb.AddForce(_Transform.up * JumpGravi, ForceMode.Impulse);
                Multicast.SendPlayerAction("Jump", _Transform.position, _Transform.rotation.y);
                StartCoroutine(JumpEnd());
        }
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }
    private IEnumerator JumpEnd()
    {
        yield return new WaitForSeconds(0.7f);
        _animator.SetBool(jumpParamHash, false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, -Gravi, 0));
        if (Input.GetKey(KeyCode.LeftShift))
            PlayerSpeed = SprintSpeed;
        else
            PlayerSpeed = NomalSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A))
        {
            _animator.SetBool(moveParamHash, true);
            Multicast.SendPlayerAction("Move", _Transform.position, _Transform.rotation.y);
        }
        else { 
            _animator.SetBool(moveParamHash, false);
            Multicast.SendPlayerAction("MoveEnd", _Transform.position, _Transform.rotation.y);
        }

        if (rb.velocity.magnitude > PlayerSpeed)
            rb.velocity = new Vector3(rb.velocity.x / 1.1f, rb.velocity.y, rb.velocity.z / 1.1f);
        else {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(CameraTrans.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + CameraTrans.right * inputHorizontal;

            rb.AddForce(moveForward * PlayerSpeed, ForceMode.Force);

            Vector3 differenceDis = new Vector3(_Transform.position.x, 0, _Transform.position.z) - new Vector3(latestPos.x, 0, latestPos.z);
            latestPos = _Transform.position;
            if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(differenceDis);
                rot = Quaternion.Slerp(_Transform.rotation, rot, 0.1f);
                _Transform.rotation = rot;
            }
        }
    }
}

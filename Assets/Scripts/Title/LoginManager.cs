using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    Vector3 camPos;
    public GameObject Cam;
    public GameObject woodsPrefab;
    public GameObject doorPrefab;

    GameObject LeftDoor;
    GameObject RightDoor;

    public Image FadeLight;
    bool Logined = false;
    bool CanGamestart = false;
    float posZ = 0.1f;//�ړ����x
    int CurrentBlock = 1;

    // Start is called before the first frame update
    void Start()
    {
        camPos = Cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        PushDoor();
    }
    private void FixedUpdate()
    {
        camPos.z += posZ;//�J�����𓮂�������
        Cam.transform.position = camPos;
        if (Time.frameCount % 300 == 0 && Logined == false)//���b���Ƃ��ɐX��ǉ����Ă���
        {
            Vector3 pos = new Vector3(0.0f, 0.0f, 60.0f * CurrentBlock);
            Instantiate(woodsPrefab, pos, Quaternion.identity);
            CurrentBlock += 1;
        }
    }

    public void PushLoginButton()
    {
        SpawnDoor();
    }
    void SpawnDoor()
    {
        Logined = true;
        Vector3 doorPos = Cam.transform.position += new Vector3(0.0f, 0, 50.0f);
        Instantiate(doorPrefab, doorPos, Quaternion.identity);
        Invoke("CamStop", 8);

        
    }

    void PushDoor()
    {
        if (Input.GetMouseButtonDown(0) && CanGamestart == true)//���O�C���ς݂��J�������~�܂��Ă���ƃQ�[���X�^�[�g
        {
            RightDoor = GameObject.Find("RightDoor");
            LeftDoor = GameObject.Find("LeftDoor");

            RightDoor.transform.DORotate(new Vector3(0, -95, 0f), 4f);
            LeftDoor.transform.DORotate(new Vector3(0, 95, 0f), 4f);
            FadeLight.DOFade(endValue: 1f, duration: 2.5f);
        }

        
    }

    void CamStop()
    {
        posZ = 0;
        CanGamestart = true;
    }
}

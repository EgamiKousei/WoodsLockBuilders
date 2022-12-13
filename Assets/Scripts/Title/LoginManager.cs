using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    Vector3 camPos;
    public GameObject Cam;
    public GameObject woodsPrefab;
    public GameObject doorPrefab;

    GameObject LeftDoor;
    GameObject RightDoor;

    public Camera Cams;

    public Image FadeLight;
    bool Logined = false;
    bool CanGamestart = false;
    bool ZoomStart = false;
    bool DoorPushed = false;
    float posZ = 0.1f;//移動速度
    int CurrentBlock = 1;

    public GameObject Rogin;

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
        camPos.z += posZ;//カメラを動かす処理
        Cam.transform.position = camPos;
        if (Time.frameCount % 300 == 0 && Logined == false)//何秒ごとかに森を追加していく
        {
            Vector3 pos = new Vector3(0.0f, 0.0f, 60.0f * CurrentBlock);
            Instantiate(woodsPrefab, pos, Quaternion.identity);
            CurrentBlock += 1;
        }
        ZoomCam();
    }

    public void PushLoginButton()
    {
        SpawnDoor();
    }

    public void SpawnDoor()
    {
        Rogin.SetActive(false);
        Logined = true;
        Vector3 doorPos = Cam.transform.position += new Vector3(0.0f, 0, 50.0f);
        Instantiate(doorPrefab, doorPos, Quaternion.identity);
        Invoke("CamStop", 8);
    }

    void PushDoor()
    {
        if (Input.GetMouseButtonDown(0) && CanGamestart == true && DoorPushed == false)//ログイン済みかつカメラが止まっているとゲームスタート
        {
            RightDoor = GameObject.Find("RightDoor");
            LeftDoor = GameObject.Find("LeftDoor");
            RightDoor.transform.DORotate(new Vector3(0, -95, 0f), 4f);
            LeftDoor.transform.DORotate(new Vector3(0, 95, 0f), 4f);
            FadeLight.DOFade(endValue: 1f, duration: 2.5f);
            ZoomStart = true;
            DoorPushed = true;
            Invoke("Login", 5);
        }
    }

    public void Login()
    {
        SceneManager.LoadScene("Main");
    }

    void CamStop()
    {
        posZ = 0;
        CanGamestart = true;
    }


    void ZoomCam()
    {
        if (ZoomStart == true)
        {
            Cams.fieldOfView -= 0.3f;
            if (Cams.fieldOfView <= 5)
                ZoomStart = false;
        }
    }

}

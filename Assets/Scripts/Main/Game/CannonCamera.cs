using UnityEngine;

public class CannonCamera : MonoBehaviour
{
    Transform _Transform,_player;
    Vector3 startAngle;

    //カメラの操作状態
    public enum Camera
    {
        nomal,
        move
    }
    Camera CameraFlag;

    private void Awake()
    {
        CameraFlag = Camera.nomal;
        _Transform = transform.parent.gameObject.transform;
        startAngle = _Transform.eulerAngles;
        _player = GameObject.Find("Player").transform;
    }

    public void setCannon()
    {
        _player.parent = _Transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            CameraFlag = Camera.move;
        }
        else { CameraFlag = Camera.nomal; }

        switch (CameraFlag)
        {
            case Camera.nomal:
                // カーソルを表示にして固定解除
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case Camera.move:
                // カーソルを非表示にして中央に固定
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                //マウスのX,Y軸移動の取得
                float X_Rotation = Input.GetAxis("Mouse X");

                // 回転させる
                _Transform.eulerAngles += new Vector3(0, X_Rotation * 3, 0);
                break;

        }
    }

    public void endCannon()
    {
        _Transform.eulerAngles = startAngle;
        _player.parent = null;
    }
}

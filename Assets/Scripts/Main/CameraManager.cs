using UnityEngine;

public class CameraManager : MonoBehaviour
{

    //カメラ上下移動の最大、最小角度
    private float maxYAngle = 90;
    private float minYAngle = 0;

    public GameObject player;
    Transform _Transform, _PlayerTransform;

    //カメラの操作状態
    public enum Camera
    {
        nomal,
        move
    }
    Camera CameraFlag;

    private void Start()
    {
        CameraFlag = Camera.nomal;
        _Transform = transform;
        _PlayerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _Transform.position = new Vector3(_PlayerTransform.localPosition.x,0, _PlayerTransform.localPosition.z);

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
                float Y_Rotation = Input.GetAxis("Mouse Y");

                // 回転させる
                _Transform.eulerAngles += new Vector3(0, X_Rotation*3, 0);

                float nowAngle = _Transform.localEulerAngles.x;
                if (-Y_Rotation != 0)
                {
                    if(maxYAngle+ Y_Rotation > nowAngle&&nowAngle- Y_Rotation > minYAngle)
                        _Transform.eulerAngles += new Vector3(-Y_Rotation, 0, 0);
                    else if (maxYAngle+ Y_Rotation<= nowAngle)
                        _Transform.eulerAngles = new Vector3(maxYAngle, _Transform.localEulerAngles.y, 0);
                    else if (minYAngle- Y_Rotation >= nowAngle)
                        _Transform.eulerAngles = new Vector3(minYAngle, _Transform.localEulerAngles.y, 0);
                }
               break;

        }
    }

}

using UnityEngine;

public class CameraManager : MonoBehaviour
{

    //カメラ上下移動の最大、最小角度
    private float maxYAngle = 90;
    private float minYAngle = 0;

    public GameObject player;

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
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.localPosition.x,0, player.transform.localPosition.z);

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
                transform.eulerAngles += new Vector3(0, X_Rotation*3, 0);

                float nowAngle = transform.localEulerAngles.x;
                if (-Y_Rotation != 0)
                {
                    if(maxYAngle+ Y_Rotation > nowAngle&&nowAngle- Y_Rotation > minYAngle)
                        transform.eulerAngles += new Vector3(-Y_Rotation, 0, 0);
                    else if (maxYAngle+ Y_Rotation<= nowAngle)
                        transform.eulerAngles = new Vector3(maxYAngle, transform.localEulerAngles.y, 0);
                    else if (minYAngle- Y_Rotation >= nowAngle)
                        transform.eulerAngles = new Vector3(minYAngle, transform.localEulerAngles.y, 0);
                }
               break;

        }
    }

}

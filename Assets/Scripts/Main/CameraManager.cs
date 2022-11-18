using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform pivot, character;    //キャラクター

    //private float zoomSpeed = 1; // カメラ移動スピード

    //カメラ上下移動の最大、最小角度
    private float maxYAngle = -0.08f;
    private float minYAngle = 0.08f;

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

                //X軸を更新
                character.transform.Rotate(0, X_Rotation * 3, 0);

                //Y軸を更新
                float nowAngle = pivot.transform.localRotation.x;
                //最大値、または最小値を超えた場合、カメラをそれ以上動かさない
                if (-Y_Rotation != 0)
                {
                    if (maxYAngle <= nowAngle && nowAngle <= minYAngle)
                    {
                        pivot.transform.Rotate(-Y_Rotation, 0, 0);
                    }
                    else if ((maxYAngle <= nowAngle && minYAngle < nowAngle) && 0 < Y_Rotation)
                    {
                        pivot.transform.Rotate(-Y_Rotation, 0, 0);
                    }
                    else if ((nowAngle < maxYAngle && nowAngle <= minYAngle) && Y_Rotation < 0)
                    {
                        pivot.transform.Rotate(-Y_Rotation, 0, 0);
                    }
                }
                break;

        }


        //マウススクロール処理
        /*var scroll = Input.mouseScrollDelta.y;
        transform.position += -transform.forward * scroll * zoomSpeed;*/
    }

}

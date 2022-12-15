using UnityEngine;

public class CameraManager : MonoBehaviour
{

    //�J�����㉺�ړ��̍ő�A�ŏ��p�x
    private float maxYAngle = 90;
    private float minYAngle = 0;

    public GameObject player;
    Transform _Transform, _PlayerTransform;

    //�J�����̑�����
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
                // �J�[�\����\���ɂ��ČŒ����
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case Camera.move:
                // �J�[�\�����\���ɂ��Ē����ɌŒ�
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                //�}�E�X��X,Y���ړ��̎擾
                float X_Rotation = Input.GetAxis("Mouse X");
                float Y_Rotation = Input.GetAxis("Mouse Y");

                // ��]������
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

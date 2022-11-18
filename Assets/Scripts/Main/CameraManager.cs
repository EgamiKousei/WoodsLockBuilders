using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform pivot, character;    //�L�����N�^�[

    //private float zoomSpeed = 1; // �J�����ړ��X�s�[�h

    //�J�����㉺�ړ��̍ő�A�ŏ��p�x
    private float maxYAngle = -0.08f;
    private float minYAngle = 0.08f;

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

                //X�����X�V
                character.transform.Rotate(0, X_Rotation * 3, 0);

                //Y�����X�V
                float nowAngle = pivot.transform.localRotation.x;
                //�ő�l�A�܂��͍ŏ��l�𒴂����ꍇ�A�J����������ȏ㓮�����Ȃ�
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


        //�}�E�X�X�N���[������
        /*var scroll = Input.mouseScrollDelta.y;
        transform.position += -transform.forward * scroll * zoomSpeed;*/
    }

}

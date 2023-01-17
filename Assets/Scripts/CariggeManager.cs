using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//�n�ԂƃQ�[���̐i�s�Ɋւ���X�N���v�g
public class CariggeManager : MonoBehaviour
{
    public static int HaveWood = 0;//�e�A�C�e��������
    public static int HaveCoin = 0;
    public int HaveFlower = 0;
    public static int HaveMetal = 0;

    public bool GameStart = false;//�W�����ăQ�[�����n�܂��Ă邩

    public GameObject WoodBlock;//�h���b�v����؃A�C�e��
    public GameObject MainCam;//���C���J����

    //float CariggeSpeed = 270;
    float CariggeSpeed = 270;//�ŏ��̒����̃X�s�[�h
    float CurveSpeed = 270;//�P��ڂ̃J�[�u�ȍ~�̃X�s�[�h


    Rigidbody rb;

    float P2 = 0; //�t�F�[�Y���ƂɉE���ǂ����I�񂾂� 0���E1����
    float P3 = 0;

    float ThisPhase = 1;//���݂̃t�F�[�Y

    //bool CanCurve = false;
    bool EndAnim = false;//�ړ��A�j���[�V�������I��������
    bool SelectP2End = false;//���E�����肳�ꂽ��
    bool SelectP3End = false;

    bool DntSelect = false;//���E�̑I���\��

    public GameObject CoinUI;//�eUI
    public GameObject WoodUI;
    public GameObject FlowerUI;
    public GameObject MetalUI;

    public GameObject Player;

    public GameObject[] RoadUI;

    public bool CariggeStop = false;//�n�Ԃ��~�܂��Ă��邩
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (EndAnim == true && SelectP2End == true)//�ړ������������E�̑I�����������Ă���Ǝ��s
        {
            Invoke("Phase2", 1);
            Invoke("DropArrowBoard2", 1);

            EndAnim = false;
            SelectP2End = false;
        }
        if (EndAnim == true && SelectP3End == true)//�ړ������������E�̑I�����������Ă���Ǝ��s�i�Q��ځj
        {
            Invoke("Phase3", 1);
            Invoke("DropArrowBoard3R", 1);
            Invoke("DropArrowBoard3L", 1);
            EndAnim = false;
            SelectP3End = false;
        }
    }

  void movepl()//�f�o�b�O�p����n�_�ɗ���ƃv���C���[����[��
    {
        Player.transform.position = this.transform.position;
    }

    public void Phase1()//�t�F�[�Y�P
    {
        
        transform.DOMove(new Vector3(430f, 0f, 0f), CariggeSpeed)//�ŏ��̕���n�_�Ɉړ�
        .SetEase(Ease.Linear)
        .OnComplete(() =>
         {
             EndAnim = true;
             ThisPhase = 2;
             //Invoke("Phase2", 10f);
             movepl();
         });
    }


    void Phase2()//�t�F�[�Y2���E��I����Ɏ��s�����
    {
        if (P2 == 0)//�E��I�񂾏ꍇ�̏���,vector3�ňړ��n�_���ݒ肳�ꂻ��Ɍ������đO�i����
        {

            Vector3[] path = {
                new Vector3(435.2841f,0f,0f),
                new Vector3(440.893f,0f,-0.4182539f),
                new Vector3(445.7401f,0f,-2.756904f),
                new Vector3(448.7082f,0f,-8.002534f),
                new Vector3(685f,0f,-411f) 

        };

             transform.DOPath(path, CurveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.forward)
                    .SetOptions(false, AxisConstraint.Y)
                    .OnComplete(() =>
                    {
                        EndAnim = true;
                        ThisPhase = 3;
                        //Invoke("Phase3", 10f);
                        movepl();
                    });
        }
        else//����I�������ꍇ�̏���
        {
            Vector3[] path = {
                new Vector3(435.2841f,0f,0f),
                new Vector3(440.893f,0f,0.4182539f),
                new Vector3(445.7401f,0f,2.756904f),
                new Vector3(448.7082f,0f,8.002534f),
                new Vector3(685f,0f,411f)
         };

            transform.DOPath(path, CurveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.forward)
                    .SetOptions(false, AxisConstraint.Y)
                    .SetAutoKill(true)
                    .OnComplete(() =>
                    {
                        EndAnim = true;
                        ThisPhase = 3;
                        //Invoke("Phase3", 10f);
                        movepl();
                    });
        }
    }
    void Phase3()//��Ɠ��l
    {
        
        if (P2 == 0 &&P3 == 0)//�P��ڂ��E���Q��ڂ��E
        {

            Vector3[] path = {
               new Vector3(686.0397f,0f,-412.8144f),
                new Vector3(687.5241f,0f,-416.5761f),
                new Vector3(687.5666f,0f,-419.4821f),
                new Vector3(686.1626f,0f,-424.315f),
                new Vector3(682.203f,0f,-433.0391f),
                new Vector3(437.5692f,0f,-863.125f)

             };

            transform.DOPath(path, CurveSpeed, PathType.Linear)
                   .SetEase(Ease.Linear)
                   .SetLookAt(0.01f, Vector3.forward)
                   .SetOptions(false, AxisConstraint.Y)
                   .OnComplete(() =>
                   {
                       movepl();


                   });
        }
        else if(P2 == 0&& P3 == 1)
        {
            Vector3[] path = {
                new Vector3(686.0397f,0f,-412.8144f),
                new Vector3(688.9564f,0f,-416.6613f),
                new Vector3(693.5315f,0f,-420.2763f),
                new Vector3(700.3274f,0f,-423.0587f),
                new Vector3(709.9006f,0f,-424.988f),
                new Vector3(1180f,0f,-424.8311f)
        };

            transform.DOPath(path, CurveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.forward)
                    .SetOptions(false, AxisConstraint.Y)
                    .SetAutoKill(true)
                    .OnComplete(() =>
                    {

                        movepl();
                    });
        }
        else if (P2 == 1 && P3 == 0)
        {

            Vector3[] path = {

                new Vector3(686.0397f,0f,412.8144f),
                new Vector3(688.9564f,0f,416.6613f),
                new Vector3(693.5315f,0f,420.2763f),
                new Vector3(700.3274f,0f,423.0587f),
                new Vector3(709.9006f,0f,424.988f),
                new Vector3(1180f,0f,424.8311f)

               

             };

            transform.DOPath(path, CurveSpeed, PathType.Linear)
                   .SetEase(Ease.Linear)
                   .SetLookAt(0.01f, Vector3.forward)
                   .SetOptions(false, AxisConstraint.Y)
                   .OnComplete(() =>
                   {
                       movepl();


                   });
        }
        else if (P2 == 1 && P3 == 1)
        {
            Vector3[] path = {
                new Vector3(686.0397f,0f,412.8144f),
                new Vector3(687.5241f,0f,416.5761f),
                new Vector3(687.5666f,0f,419.4821f),
                new Vector3(686.1626f,0f,424.315f),
                new Vector3(682.203f,0f,433.0391f),
                new Vector3(437.5692f,0f,863.125f)
        };

            transform.DOPath(path, CurveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.forward)
                    .SetOptions(false, AxisConstraint.Y)
                    .SetAutoKill(true)
                    .OnComplete(() =>
                    {
                        movepl();


                    });
        }
    }



    public void SetPhase2R()//�t�F�[�Y���Ƃ̐i�s�����̌���@�ȉ����l
    {
        P2 = 0;
        SelectP2End = true;
        //Debug.Log("p2SetR");
    }

    public void SetPhase2L()
    {
        P2 = 1;
        SelectP2End = true;
        //Debug.Log("p2SetL");
    }

    public void SetPhase3R()
    {
        P3 = 0;
        SelectP3End = true;
        //Debug.Log("p2SetR");
    }

    public void SetPhase3L()
    {
        P3 = 1;
        SelectP3End = true;
        //Debug.Log("p2SetL");
    }

    public void SetCanCurve()
    {
       // CanCurve = true;
    }

    void DropArrowBoard2()//�{�[�h�̖��𗎂Ƃ����o �ȉ����l
    {
        GameObject NotBoard2;
        RoadUI[0].SetActive(false);
        if (P2 == 0)
        {
            NotBoard2 = GameObject.Find("MeP2L");
            Rigidbody rb = NotBoard2.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
        else
        {
            NotBoard2 = GameObject.Find("MeP2R");
            Rigidbody rb = NotBoard2.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }

    void DropArrowBoard3R()
    {
        GameObject NotBoard3R;
        if (P3 == 0)
        {
            NotBoard3R = GameObject.Find("MeP3RL");
            Rigidbody rb = NotBoard3R.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
        else
        {
            NotBoard3R = GameObject.Find("MeP3RR");
            Rigidbody rb = NotBoard3R.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }
    void DropArrowBoard3L()
    {
        GameObject NotBoard3L;
        if (P3 == 0)
        {
            NotBoard3L = GameObject.Find("MeP3LL");
            Rigidbody rb = NotBoard3L.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
        else
        {
            NotBoard3L = GameObject.Find("MeP3LR");
            Rigidbody rb = NotBoard3L.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }

    public void AsembStart()//�v���C���[�S�����W�������ꍇ�ɌĂяo�����
    {
        GameStart = true;
        Phase1();
    }

    public void GetCoin()//�����R�C���𑝉��@�ȉ����l
    {
        HaveCoin+=1;
        CoinUI.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.05f)
        .OnComplete(() =>
         {
             CoinUI.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.05f);
         });
    }
    public void GetWood()
    {
        HaveWood += 1;
        WoodUI.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.05f)
        .OnComplete(() =>
        {
            WoodUI.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.05f);
        });
    }

    public void GetFlower()
    {
        HaveFlower += 1;
        FlowerUI.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.05f)
        .OnComplete(() =>
        {
            FlowerUI.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.05f);
        });
    }
    public void GetMetal()
    {
        HaveMetal += 1;
        MetalUI.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.05f)
        .OnComplete(() =>
        {
            MetalUI.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.05f);
        });
    }

    public void CoinMinus()//�����R�C�����}�C�i�X����@�ȉ����l
    {
        HaveCoin -= 1;
    }
    public void WoodMinus()
    {
        HaveWood -= 1;
    }
    public void FlowerMinus()
    {
        HaveFlower -= 1;
    }

    

    public void WoodClash()//�|�؂ɂԂ������ۂ̏����@pos���w�肵�Ė؃u���b�N�̃v���n�u���o�������Ė؂��}�C�i�X����
    {
       Debug.Log("clash");

       HaveWood -= 4;

       Vector3 Pos1 = this.gameObject.transform.position;
       Vector3 Pos2 = this.gameObject.transform.position;
       Vector3 Pos3 = this.gameObject.transform.position;
       Vector3 Pos4 = this.gameObject.transform.position;

       Pos1.x -= 4; Pos1.z -= 2; Pos1.y += 0.5f;
       Pos2.x -= 6; Pos2.z -= 1; Pos2.y += 0.5f;
       Pos3.x -= 4; Pos3.z += 2; Pos3.y += 0.5f;
       Pos4.x -= 6; Pos4.z += 1; Pos4.y += 0.5f;

       Instantiate(WoodBlock, Pos1, Quaternion.identity);
       Instantiate(WoodBlock, Pos2, Quaternion.identity);
       Instantiate(WoodBlock, Pos3, Quaternion.identity);
       Instantiate(WoodBlock, Pos4, Quaternion.identity);

        MainCam.transform.DOPunchRotation(new Vector3(0, 0, 5), 0.5f);

        transform.DOPause();//�i�s���ꎞ��~

        CariggeStop = true;//�n�Ԃ��~�܂�

    }

    public void BrokeFallenTree()//�n�Ԃ��~�܂�����Ԃ��|�؂��󂵂���Ăяo�����
    {
        CariggeStop = false;
        transform.DOPlay();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FallenTreeCol")
        {
            WoodClash();
        }
        if (other.gameObject.name == "BossArea")
        {
            MainCam.transform.DOLocalMove(new Vector3(16, 47, 12), 3f);
            Debug.Log("boss");
        }
    }
}

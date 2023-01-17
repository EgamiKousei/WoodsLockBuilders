using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//馬車とゲームの進行に関するスクリプト
public class CariggeManager : MonoBehaviour
{
    public static int HaveWood = 0;//各アイテム所持数
    public static int HaveCoin = 0;
    public int HaveFlower = 0;
    public static int HaveMetal = 0;

    public bool GameStart = false;//集合してゲームが始まってるか

    public GameObject WoodBlock;//ドロップする木アイテム
    public GameObject MainCam;//メインカメラ

    //float CariggeSpeed = 270;
    float CariggeSpeed = 270;//最初の直線のスピード
    float CurveSpeed = 270;//１回目のカーブ以降のスピード


    Rigidbody rb;

    float P2 = 0; //フェーズごとに右左どっち選んだか 0が右1が左
    float P3 = 0;

    float ThisPhase = 1;//現在のフェーズ

    //bool CanCurve = false;
    bool EndAnim = false;//移動アニメーションが終了したか
    bool SelectP2End = false;//左右が決定されたか
    bool SelectP3End = false;

    bool DntSelect = false;//左右の選択可能か

    public GameObject CoinUI;//各UI
    public GameObject WoodUI;
    public GameObject FlowerUI;
    public GameObject MetalUI;

    public GameObject Player;

    public GameObject[] RoadUI;

    public bool CariggeStop = false;//馬車が止まっているか
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (EndAnim == true && SelectP2End == true)//移動が完了し左右の選択が完了していると実行
        {
            Invoke("Phase2", 1);
            Invoke("DropArrowBoard2", 1);

            EndAnim = false;
            SelectP2End = false;
        }
        if (EndAnim == true && SelectP3End == true)//移動が完了し左右の選択が完了していると実行（２回目）
        {
            Invoke("Phase3", 1);
            Invoke("DropArrowBoard3R", 1);
            Invoke("DropArrowBoard3L", 1);
            EndAnim = false;
            SelectP3End = false;
        }
    }

  void movepl()//デバッグ用分岐地点に来るとプレイヤーをわーぷ
    {
        Player.transform.position = this.transform.position;
    }

    public void Phase1()//フェーズ１
    {
        
        transform.DOMove(new Vector3(430f, 0f, 0f), CariggeSpeed)//最初の分岐地点に移動
        .SetEase(Ease.Linear)
        .OnComplete(() =>
         {
             EndAnim = true;
             ThisPhase = 2;
             //Invoke("Phase2", 10f);
             movepl();
         });
    }


    void Phase2()//フェーズ2左右を選択後に実行される
    {
        if (P2 == 0)//右を選んだ場合の処理,vector3で移動地点が設定されそれに向かって前進する
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
        else//左を選択した場合の処理
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
    void Phase3()//上と同様
    {
        
        if (P2 == 0 &&P3 == 0)//１回目が右かつ２回目が右
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



    public void SetPhase2R()//フェーズごとの進行方向の決定　以下同様
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

    void DropArrowBoard2()//ボードの矢印を落とす演出 以下同様
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

    public void AsembStart()//プレイヤー全員が集合した場合に呼び出される
    {
        GameStart = true;
        Phase1();
    }

    public void GetCoin()//所持コインを増加　以下同様
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

    public void CoinMinus()//所持コインをマイナスする　以下同様
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

    

    public void WoodClash()//倒木にぶつかった際の処理　posを指定して木ブロックのプレハブを出現させて木をマイナスする
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

        transform.DOPause();//進行を一時停止

        CariggeStop = true;//馬車が止まる

    }

    public void BrokeFallenTree()//馬車が止まった状態かつ倒木を壊したら呼び出される
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

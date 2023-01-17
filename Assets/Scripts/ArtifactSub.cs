using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アーティファクト、バフに関するスクリプト2
public class ArtifactSub : MonoBehaviour
{
    public int ArtiNum = 1;//アーティファクト番号1木2花3ドロップ4倒木
    bool CanBuff = false;//使えるかどうか

    GameObject Carigge;
    CariggeManager cariggeManager;

    GameObject ArtiMain;//アーティファクトのメインスクリプト
    ArtifactMain artiMain;

    int CoinQuantity;//コイン所持数

    BoxCollider col;

    public GameObject[] Deltree;//削除される倒木

    int count = 0;//giveカウント

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        Carigge = GameObject.Find("Carigge");
        ArtiMain = GameObject.Find("Artifact");
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        artiMain = ArtiMain.GetComponent<ArtifactMain>();

        col = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCoinQuantity();//ここで所持コインを判定

        if (Input.GetKeyDown(KeyCode.F) && CanBuff == true && CoinQuantity >= 400)//Fキーを押したときにコインが400以上だと使用できる
        {
            CariggeManager.HaveCoin -= 400;//所持コインから400引く
            col.enabled = false;//コライダーを無効化
            canvas.SetActive(false);
            if (ArtiNum == 1)//それぞれのタイプに応じた処理
            {
                GiveWood();
            }
            else if (ArtiNum == 1)
            {
                GiveFlower();
            }
            else if (ArtiNum == 3)
            {
                DropUp();
            }
            else if (ArtiNum == 4)
            {
                Obstacle();
            }
        }
    }

    void GiveWood()//木を25個追加
    {
        CanBuff = false;
        if (count <= 24)
        {
        cariggeManager.GetWood();
        count++;
        Invoke("GiveWood", 0.05f);
        }
    }

    void GiveFlower()//花を５個追加
    {       
        CanBuff = false;
        if (count <= 4)
        {
            cariggeManager.GetFlower();
            count++;
            Invoke("GiveFlower", 0.05f);
        }
    }

    void DropUp()//木のドロップ量が一定時間２倍に
    {
        artiMain.DropMultiply = 2;
        CanBuff = false;
    }

    void Obstacle()//付近の倒木を消す
    {
        Deltree[0].SetActive(false);
        Deltree[1].SetActive(false);
        CanBuff = false;
    }

    void GetCoinQuantity()
    {
        CoinQuantity = CariggeManager.HaveCoin;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanBuff = true;
            canvas.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanBuff = false;
            canvas.SetActive(false);
        }
    }
}

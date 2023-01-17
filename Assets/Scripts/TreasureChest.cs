using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//宝箱に関するスクリプト
public class TreasureChest : MonoBehaviour
{
    int life = 2;//耐久

    int WoodPrice = 0;//ドロップする木の量
    int CoinPrice = 0;//ドロップするコインの量

    bool Opened = false;//開いたか

    public GameObject WoodBlock;
    public GameObject Coin;

    public GameObject WoodDropPoint1;
    public GameObject WoodDropPoint2;
    public GameObject WoodDropPoint3;

    public GameObject CoinDropPoint;

    public GameObject close;//閉じたモデル
    public GameObject open;//開いたモデル

    int  CoinDropPos = 1;

    bool DropStart = false;
    int Droped = 0;
    // Start is called before the first frame update
    void Start()
    {
        WoodPrice = Random.Range(2, 3);
        CoinPrice = Random.Range(12,18);
        //CoinPrice = 1200000000;
    }

    // Update is called once per frame
    void Update()
    {
        if(life <=0&&Opened == false)//２回たたかれると開く
        {
            Open();
            Opened = true;
            DropStart = true;
            close.SetActive(false);
            open.SetActive(true);
        }

        MoveDropPoint();
        
    }

    private void FixedUpdate()
    {
        DropCoin();
    }

    void Damage()//攻撃を受けると揺らす
    {
        life -= 1;
        close.transform.DOPunchRotation(new Vector3(0, 0, 10),0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            Damage();
        }
    }

    void Open()
    {
        DropWood();
        open.transform.DOLocalJump(new Vector3(0f, 0.8f, 0f), jumpPower: 1f, numJumps: 1, duration: 0.5f)
            .OnComplete(() =>
            {
                open.transform.DOLocalMoveY(0.4f,0.2f);
            });
    }

    void DropWood()//木をドロップ
    {
        Vector3 Pos1 = WoodDropPoint1.gameObject.transform.position;
        Instantiate(WoodBlock, Pos1, Quaternion.identity);

        Vector3 Pos2 = WoodDropPoint2.gameObject.transform.position;
        Instantiate(WoodBlock, Pos2, Quaternion.identity);

        if (WoodPrice == 3)
        {
            Vector3 Pos3 = WoodDropPoint3.gameObject.transform.position;
            Instantiate(WoodBlock, Pos3, Quaternion.identity);
        }
    }

    void DropCoin()//コインをドロップ
    {
        if (DropStart == true&&Droped <=CoinPrice)
        {
            Vector3 Pos = CoinDropPoint.gameObject.transform.position;
            Instantiate(Coin, Pos, Quaternion.identity);
            Droped++;
        }
         
    }

    void MoveDropPoint()//コインが重ならないように毎フレームドロップ場所を変更
    {
        if(CoinDropPos == 1)
        {
            CoinDropPoint.transform.localPosition = new Vector3(0.6f,1.5f,0.7f);
            CoinDropPos = 2;
        }
        else if (CoinDropPos == 2)
        {
            CoinDropPoint.transform.localPosition = new Vector3(0.6f, 1.5f, -0.7f);
            CoinDropPos = 3;
        }
        else if (CoinDropPos == 3)
        {
            CoinDropPoint.transform.localPosition = new Vector3(-0.6f, 1.5f, -0.7f);
            CoinDropPos = 4;
        }
        else if (CoinDropPos == 4)
        {
            CoinDropPoint.transform.localPosition = new Vector3(-0.6f, 1.5f, 0.7f);
            CoinDropPos = 1;
        }
    }
}

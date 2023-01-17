using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//トロッコとゲームの進行に関するスクリプト
public class MinecartManager : MonoBehaviour
{
    public int HaveCoal = 0;
    public int HaveMetal = 0;
    public int HaveCoin = 0;
    public int HaveFlower = 0;

    bool GameStart = false;

    public GameObject CoalItem;
    public GameObject MetalItem;
    public GameObject MainCam;

    float P2 = 0; //フェーズごとに右左どっち選んだか 0が右1が左
    float P3 = 0;

    float ThisPhase = 1;

    public GameObject CoalUI;
    public GameObject MetalUI;
    public GameObject HealUI;
    public GameObject CoinUI;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetFlower", 30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetCoin()
    {
        HaveCoin += 1;
        CoinUI.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.05f)
        .OnComplete(() =>
        {
            CoinUI.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.05f);
        });
    }
    public void GetCoal()
    {
        HaveCoal += 1;
        CoalUI.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.05f)
        .OnComplete(() =>
        {
            CoalUI.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.05f);
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
    public void CoalMinus()
    {
        HaveCoal -= 1;
    }
    public void FlowerMinus()
    {
        HaveFlower -= 1;
    }

    public void CoinMinus()
    {
        HaveCoin -= 1;
    }
    
    void GetFlower()
    {
        HaveFlower += 1;
        Invoke("GetFlower",30);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestObjectGenerate : MonoBehaviour
{
    public GameObject[] Sec1;
    public GameObject[] Sec2;
    public GameObject[] Sec3;

    int Sec1Num;
    int Sec2Num;
    int Sec3Num;
    // Start is called before the first frame update
    void Start()
    {
        DecisionSectorNum();
        TerraGenerate();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DecisionSectorNum()//地形のパターンを決める
    {
        Sec1Num = Random.Range(0, 7);

        for (; ; )
        {
            Sec2Num = Random.Range(0, 7);
            if (Sec1Num != Sec2Num)
                break;
        }

        for (; ; )
        {
            Sec3Num = Random.Range(0, 7);
            if (Sec1Num != Sec3Num && Sec2Num != Sec3Num)
                break;
        }
    }

    void TerraGenerate()//地形の生成
    {
        Sec1[Sec1Num].gameObject.SetActive(true);
        Sec2[Sec2Num].gameObject.SetActive(true);
        Sec3[Sec3Num].gameObject.SetActive(true);
    }
}
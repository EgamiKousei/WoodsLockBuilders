using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//木の自動生成に関する処理
public class ForestMetalSpawn : MonoBehaviour
{
    public GameObject MetalPrefab;//木のプレハブ
    float TimeSpan = 0;//生成スパン
    private float currentTime = 0f;
    public bool thisRight = true;//右の場合はチェック
    public bool stop = false;//木の生成を停止するか

    GameObject Carigge;//馬車のスクリプト
    public CariggeManager cariggeManager;

    public bool CariggeStoped = false;//馬車が止まっているか

    // Start is called before the first frame update
    void Start()
    {
        Carigge = GameObject.Find("Carigge");
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        TimeSpan = Random.Range(6.5f, 12);
    }

    // Update is called once per frame
    void Update()
    {
        CariggeStoped = cariggeManager.CariggeStop;

        currentTime += Time.deltaTime;

        if (currentTime > TimeSpan)
        {
            TreeSpawner();
            if (thisRight == true)
            {
                TimeSpan = Random.Range(30f, 40);
            }
            else if (thisRight == false)
            {
                TimeSpan = Random.Range(30f, 40);
            }
            currentTime = 0f;
        }
    }

    void TreeSpawner()//posを指定して木をスポーンさせる posはオブジェクトの場所
    {
        if (stop == false && CariggeStoped == false && cariggeManager.GameStart == true)
        {
            Vector3 Pos = this.gameObject.transform.position;
            Instantiate(MetalPrefab, Pos, Quaternion.identity);
        }

    }
    private void OnTriggerStay(Collider other)//木の生成をストップする場所に近づくと生成がストップ
    {
        if (other.gameObject.name == "TreeSpawnStop")
        {
            stop = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "TreeSpawnStop")
        {
            stop = false;
            //     Debug.Log("exittt");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    int enemyTime=0;
    // Use this for initialization
    void Start()
    {
        //ルームマスターだったら
        SetEnemTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > enemyTime)
        {
            //メインシーン中のみ稼働
            //地点決めて敵出現
            Debug.Log("経過時間(秒)" + enemyTime);
            SetEnemTime();
        }
    }
    public void SetEnemTime()
    {
        enemyTime = (int)Time.time;
        enemyTime = enemyTime + Random.Range(300, 600);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エネミースポーンスクリプト
public class EnemySpawn : MonoBehaviour
{
    //20秒ごとに3体スポーン

    [SerializeField]
    GameObject[] SpawnPoint;//スポーン場所候補　６か所

    [SerializeField]
    GameObject Enemy;//エネミーのプレハブ

    float Span = 20f;//スポーン間隔
    private float currentTime = 0f;

    bool isDay = true;//昼かどうか

    Vector3 Pos;//スポーン場所

    SwitchDayNight dayNight;//昼夜切り替えのスクリプト
    public GameObject Light;

    CariggeManager cariggeManager;//馬車
    public GameObject Carigge;

    // Start is called before the first frame update
    void Start()
    {
        dayNight = Light.GetComponent<SwitchDayNight>();
        cariggeManager = Carigge.GetComponent<CariggeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cariggeManager.GameStart == true)//馬車が発車している場合のみスポーンする
            Spawn();

        if (dayNight.DayNight == 1)//夜になると10秒ごとにスポーンする
        {
            Span = 20f;
        }
        else
        {
            Span = 10f;
        }
        //MidiumBossSpawn();
    }

    void Spawn()
    {
        currentTime += Time.deltaTime;

        if (currentTime > Span)
        {
            if (dayNight.DayNight == 1)//昼
            {
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
            }
            else//夜
            {
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
                Pos = SpawnPoint[Random.Range(0, 5)].transform.position;
                Instantiate(Enemy, Pos, Quaternion.identity);
            }

            currentTime = 0f;
        }
    }
}

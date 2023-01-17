using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NpcManager : MonoBehaviour
{
    string datapath;                        // ダッシュデータまでのパス
    NpcData npc;                            // NPCデータ格納変数

    // npcオブジェクト格納用配列
    [SerializeField]
    GameObject[] npcObject;

    // Enemyオブジェクト格納用配列
    [SerializeField]
    GameObject[] enemyObject;

    // Itemオブジェクト格納用配列
    [SerializeField]
    GameObject[] ItemObject;

    // npcオブジェクト格納用配列
    [SerializeField]
    float[] npcHp;

    // 護衛物オブジェクト
    [SerializeField]
    GameObject car;

    // npcとそれぞれの敵の距離
    [SerializeField]
    float[] npcDis;

    // npc1とそれぞれの敵の距離
    [SerializeField]
    float[] npc1Dis;

    // npc2とそれぞれの敵の距離
    [SerializeField]
    float[] npc2Dis;

    // npc3とそれぞれの敵の距離
    [SerializeField]
    float[] npc3Dis;

    // それぞれのNPCのターゲット基準
    string[] npcTps;

    // npcの数
    int npcNum;

    // npcがTaregtとしているオブジェクト
    [SerializeField]
    GameObject[] targetObject;

    // トロッコ役を決定するための変数
    public bool isTro = false;
    public int troNo;

    // それぞれの一番近い敵を確認するための変数
    int close1 = 0;
    int close2 = 0;
    int close3 = 0;
    float closeDis1 = 1000;
    float closeDis2 = 1000;
    float closeDis3 = 1000;
    bool isChange1 = false;
    bool isChange2 = false;
    bool isChange3 = false;

    // ターゲット設定処理を行う条件
    public bool isChange = true;

    GameObject closeCarigge;

    public GameObject obstacle; // 障害物
    GameObject closeObstacle;


    private void Awake()
    {
        datapath = Application.dataPath + "/Npc.json"; // NPCデータまでのパス

    }

    // Start is called before the first frame update
    void Start()
    {
        // npc格納処理
        npcObject = GameObject.FindGameObjectsWithTag("Npc");
        npcNum = npcObject.Length;

        // 初期化処理
        targetObject = new GameObject[npcNum];
        npcDis = new float[npcNum];
        npcHp = new float[npcNum];
        npcTps = new string[npcNum];

        for (int i = 0; i < npcNum; i++) 
        {
            npcHp[i] = npcObject[i].GetComponent<Nav>().hp;
            npcTps[i] = npcObject[i].GetComponent<Nav>().tps;
        }

        npc = new NpcData();        // NPCデータ取得

        npc = LoadNPC(datapath);   // NPCデータロード

        SaveNPC(npc);               // NPCデータセーブ

        //ChangeEnemy();

    }

    // NPCデータロード関数
    public NpcData LoadNPC(string dataPath)
    {
        StreamReader reader = new StreamReader(dataPath);
        string datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<NpcData>(datastr);
    }

    // NPCデータセーブ関数
    public void SaveNPC(NpcData npc)
    {
        string jsonstr = JsonUtility.ToJson(npc);
        StreamWriter wreiter = new StreamWriter(datapath, false);
        wreiter.WriteLine(jsonstr);
        wreiter.Flush();
        wreiter.Close();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // 敵出現時の処理
        enemyObject = GameObject.FindGameObjectsWithTag("Enemy");
        // アイテムオブジェクト取得
        ItemObject = GameObject.FindGameObjectsWithTag("Item");
        if (ItemObject.Length != 0 && isChange == true)
        {
            for (int i = 0; i < npcNum; i++)
            {
                targetObject[i] = npcObject[i].GetComponent<Nav>().mainTarget;
                if (targetObject[i].GetComponent<ItemTarget>().isTarget == false)
                {
                    targetObject[i].GetComponent<ItemTarget>().isTarget = true;
                    npcDis[i] = npcObject[i].GetComponent<Nav>().closeDist;
                }
                else
                {
                    if (npcObject[i].GetComponent<Nav>().lv == 1)
                    {
                        npcObject[i].GetComponent<Nav>().Switch();
                    }
                    else if (npcObject[i].GetComponent<Nav>().lv == 2)
                    {
                        npcObject[i].GetComponent<Nav>().Switch2();
                    }
                    else if (npcObject[i].GetComponent<Nav>().lv == 3)
                    {
                        npcObject[i].GetComponent<Nav>().Switch3();
                    }
                    targetObject[i] = npcObject[i].GetComponent<Nav>().mainTarget;
                    targetObject[i].GetComponent<ItemTarget>().isTarget = true;
                    npcDis[i] = npcObject[i].GetComponent<Nav>().closeDist;
                    //i--;
                }

            }
            isChange = false;
        }

        if (enemyObject.Length != 0 && isChange == true)
        {
            for (int i = 0; i < npcNum; i++)
            {
                targetObject[i] = npcObject[i].GetComponent<Nav>().mainTarget;
                if (targetObject[i].GetComponent<Enemy>().isTarget == false)
                {
                    targetObject[i].GetComponent<Enemy>().isTarget = true;
                    npcDis[i] = npcObject[i].GetComponent<Nav>().closeDist;
                }
                else
                {
                    if (npcObject[i].GetComponent<Nav>().lv == 1)
                    {
                        npcObject[i].GetComponent<Nav>().Switch();
                    }
                    else if (npcObject[i].GetComponent<Nav>().lv == 2)
                    {
                        npcObject[i].GetComponent<Nav>().Switch2();
                    }
                    else if (npcObject[i].GetComponent<Nav>().lv == 3)
                    {
                        npcObject[i].GetComponent<Nav>().Switch3();
                    }
                    targetObject[i] = npcObject[i].GetComponent<Nav>().mainTarget;
                    targetObject[i].GetComponent<Enemy>().isTarget = true;
                    npcDis[i] = npcObject[i].GetComponent<Nav>().closeDist;
                    //i--;
                }

            }

            //TargetEnemy();
            isChange = false;
        }
        
        
        /*
        // 配列内にそれぞれのNPCのターゲットを格納
        for(int i = 0; i < npcNum; i++)
        {
            targetObject[npcObject[i].GetComponent<NpcCode>().npcNo] = npcObject[i].GetComponent<Nav>().mainTarget;
            // ターゲットがトロッコだった場合
            if(npcObject[i].GetComponent<Nav>().isSwitch == false && npcObject[i].GetComponent<Nav>().mainTarget.tag == "Target" && isTro == false)
            {
                isTro = true;
                troNo = i;
            }

            // トロッコをターゲットとするNPCを1体に限定
            if(isTro == true && troNo != i && npcObject[i].GetComponent<Nav>().mainTarget.tag == "Target")
            {
                npcObject[i].GetComponent<Nav>().Switch();
            }
        }*/
    }

    // いずれかのHPが変化した時の処理
    public void changeHp()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcHp[i] = npcObject[i].GetComponent<Nav>().hp;
        }
    }

    public void ChangeEnemy()
    {
        for (int i = 0; i < npcNum; i++)
        {
            if (npcObject[i].GetComponent<Nav>().lv == 1)
            {
                npcObject[i].GetComponent<Nav>().Switch();
            }
            else if (npcObject[i].GetComponent<Nav>().lv == 2)
            {
                npcObject[i].GetComponent<Nav>().Switch2();
            }
            else if (npcObject[i].GetComponent<Nav>().lv == 3)
            {
                npcObject[i].GetComponent<Nav>().Switch3();
            }
            npcObject[i].GetComponent<Nav>().isChange = false;
        }
    }

    // ChangeEnemy発生までの時間差
    public IEnumerator ChangeDelay()
    {
        // 1秒処理を待ちます。
        yield return new WaitForSeconds(0.1f);
        ChangeEnemy();
    }

    public void BoolChange()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcObject[i].GetComponent<Nav>().isChange = true;
        }
    }

    public void CloseCarigge()
    {
        float closeDist = 1000;
        foreach (GameObject y in npcObject)
        {
            // コンソール画面での確認用コード
            //print(Vector3.Distance(transform.position, t.transform.position));

            // このオブジェクト（砲弾）と敵までの距離を計測
            float tDist = Vector3.Distance(car.transform.position, y.transform.position);

            // もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
            if (closeDist > tDist)
            {
                // 「closeDist」を「tDist（その敵までの距離）」に置き換える。
                // これを繰り返すことで、一番近い敵を見つけ出すことができる。
                closeDist = tDist;

                // 一番近い敵の情報をcloseEnemyという変数に格納する（★）
                closeCarigge = y;

            }

            // 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
            //Invoke("SwitchOn", 0.5f);


        }
        closeCarigge.GetComponent<Nav>().isFire = true;
    }

    public void CloseObstacle()
    {
        float closeDist = 1000;
        foreach (GameObject y in npcObject)
        {
            // コンソール画面での確認用コード
            //print(Vector3.Distance(transform.position, t.transform.position));

            // このオブジェクト（砲弾）と敵までの距離を計測
            float tDist = Vector3.Distance(obstacle.transform.position, y.transform.position);

            // もしも「初期値」よりも「計測した敵までの距離」の方が近いならば、
            if (closeDist > tDist)
            {
                // 「closeDist」を「tDist（その敵までの距離）」に置き換える。
                // これを繰り返すことで、一番近い敵を見つけ出すことができる。
                closeDist = tDist;

                // 一番近い敵の情報をcloseEnemyという変数に格納する（★）
                closeObstacle = y;

            }

            // 砲弾が生成されて0.5秒後に、一番近い敵に向かって移動を開始する。
            //Invoke("SwitchOn", 0.5f);


        }
        closeObstacle.GetComponent<Nav>().isObstacle = true;
    }

    public void ResetObstacle()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcObject[i].GetComponent<Nav>().isObstacle = false;
            npcObject[i].GetComponent<Nav>().Switch();
        }
    }

    public void HealHp(float hp)
    {
        npc.healHp += hp;
        npc.healCnt++;
        //SaveNPC(npc);
    }

    public void HealFire(float fire)
    {
        npc.torch += fire;
        npc.torchCnt++;
        //SaveNPC(npc);
    }

    public void ActiveNav()
    {
        for (int i = 0; i < npcNum; i++)
        {
            npcObject[i].GetComponent<Nav>().enabled = true;
        }
    }
}

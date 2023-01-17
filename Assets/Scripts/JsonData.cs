using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPCデータ
[System.Serializable]
public class NpcData
{
    // 松明の点灯時間
    public float torch;
    // 松明点灯時間を求めるためのカウントデータ
    public int torchCnt;
    // HP回復行動基準値
    public float healHp;
    // HP回復行動基準に必要なカウント
    public int healCnt;
}

// NPCデータ
[System.Serializable]
public class Npc1Data
{
    // レベル
    public int lv;
    // 敵の位置による優先度
    public string tpsEnemy;
    // 松明の点灯時間
    public float torch;
    // 馬車・トロッコの耐久値回復の基準
    public int endu;
    // HP回復行動基準値
    public float healHp;
    // 経験値
    public int point;
}

// NPCデータ
[System.Serializable]
public class Npc2Data
{
    // レベル
    public int lv;
    // 敵の位置による優先度
    public string tpsEnemy;
    // 松明の点灯時間
    public float torch;
    // 馬車・トロッコの耐久値回復の基準
    public int endu;
    // HP回復行動基準値
    public float healHp;
    // 経験値
    public int point;
}

// NPCデータ
[System.Serializable]
public class Npc3Data
{
    // レベル
    public int lv;
    // 敵の位置による優先度
    public string tpsEnemy;
    // 松明の点灯時間
    public float torch;
    // 馬車・トロッコの耐久値回復の基準
    public int endu;
    // HP回復行動基準値
    public float healHp;
    // 経験値
    public int point;
}

// NPCマイセット（配列）
[System.Serializable]
public class SetData
{
    public setData[] data;
}

// マイセットデータ（変数）
[System.Serializable]
public class setData
{
    // 敵の位置による優先度
    public string tpsEnemy;
    // 松明の点灯時間
    public int torch;
    // 馬車・トロッコの耐久値回復の基準
    public int endu;
    // HP回復行動基準値
    public int healHp;
}



public class JsonData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

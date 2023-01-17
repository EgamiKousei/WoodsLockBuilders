using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "CreateEnemy")]

public class EnemyData : ScriptableObject
{
    //@“G‚Ìí—Ş
    [SerializeField]
    private int kindEnemy;

    //@“G‚Ì–¼‘O
    [SerializeField]
    private string enemyName;

    //@“G‚ÌHP
    [SerializeField]
    private int enemyHp;

    //@“G‚ÌUŒ‚—Í
    [SerializeField]
    private int enemyAtc;

    //@“G‚Ì–hŒä—Í
    [SerializeField]
    private int enemyDef;


    public int GetKindEnemy()
    {
        return kindEnemy;
    }

    public string GetenemyName()
    {
        return enemyName;
    }

    public int GetEnemyHp()
    {
        return enemyHp;
    }

    public int GetEnemyAtc()
    {
        return enemyAtc;
    }

    public int GetEnemyDef()
    {
        return enemyDef;
    }
}

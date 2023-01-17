using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Enemy", menuName = "CreateEnemy")]

public class EnemyData : ScriptableObject
{
    //�@�G�̎��
    [SerializeField]
    private int kindEnemy;

    //�@�G�̖��O
    [SerializeField]
    private string enemyName;

    //�@�G��HP
    [SerializeField]
    private int enemyHp;

    //�@�G�̍U����
    [SerializeField]
    private int enemyAtc;

    //�@�G�̖h���
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

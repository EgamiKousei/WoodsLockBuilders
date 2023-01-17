using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBossAdd : MonoBehaviour
{
    Enemy enemy;
    public GameObject Result;
    EndForest result;
    // Start is called before the first frame update
    void Start()
    {
        Result = GameObject.Find("Result");
        enemy = this.GetComponent<Enemy>();
        result = Result.GetComponent<EndForest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.hp <= 0)
        {
            result.GameEnd();
            this.gameObject.SetActive(false);
        }
    }
    void Damage()
    {
        enemy.hp -= 10;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            Damage();
        }
    }
}

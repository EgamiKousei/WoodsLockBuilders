using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OtherObjectSpawn : MonoBehaviour
{
    public GameObject[] ObjPrefab;
    float TimeSpan = 4.75f;
    private float currentTime = 0f;
    public bool thisRight = true;
    bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        if (thisRight == true && stop == false)
        {
            RightSpawn();
        }
        else if (thisRight == false && stop == false)
        {
            LeftSpawn();
        }

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > TimeSpan)
        {
            TreeSpawner();
            TimeSpan = Random.Range(6.5f, 12);
            currentTime = 0f;
        }


    }


    void LeftSpawn()
    {
        //this.transform.DOLocalMove(new Vector3(0f, 0f, -9f), 1f).SetLoops(-1, LoopType.Restart);
        this.transform.DOLocalMoveX(-9, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear); ;
    }

    void RightSpawn()
    {
        this.transform.DOLocalMoveX(9, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear); ;
    }

    void TreeSpawner()
    {
        Vector3 Pos = this.gameObject.transform.position;
        Instantiate(ObjPrefab[0], Pos, Quaternion.identity);
    }
    private void OnTriggerStay(Collider other)
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
        }
    }
}

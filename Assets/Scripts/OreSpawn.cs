using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OreSpawn : MonoBehaviour
{
    public GameObject[] MetalPrefab;
    public GameObject[] CoalPrefab;
    float TimeSpan = 0;
    public int MetalorCoal = 0;
    private float currentTime = 0f;
    public bool thisRight = true;
    public bool stop = false;
    // Start is called before the first frame update
    void Start()
    {
        TimeSpan = Random.Range(6.5f, 12);
      
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        //MetalorCoal = Random.Range(0, 1);
        if (currentTime > TimeSpan)
        {
            
            if (thisRight == true)
            {
                MetalorCoal = Random.Range(0, 10);
                TimeSpan = Random.Range(12f, 20);
                if (MetalorCoal >=6)
                {
                    MetalSpawner();
                }
                else if(MetalorCoal <= 5)
                {
                    CoalSpawner();
                }
            }
            else if (thisRight == false)
            {
                MetalorCoal = Random.Range(0, 10);
                TimeSpan = Random.Range(17, 30);
                if (MetalorCoal >= 6)
                {
                    MetalSpawner();
                }
                else if (MetalorCoal <= 5)
                {
                    CoalSpawner();
                }
            }
            currentTime = 0f;
        }
    }
    

    void MetalSpawner()
    {
        if (stop == false)
        {
            Vector3 Pos = this.gameObject.transform.position;
            Instantiate(MetalPrefab[Random.Range(0, 4)], Pos, Quaternion.identity);
        }

    }
    void CoalSpawner()
    {
        if (stop == false)
        {
            Vector3 Pos = this.gameObject.transform.position;
            Instantiate(CoalPrefab[Random.Range(0, 4)], Pos, Quaternion.identity);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "TreeSpawnStop")
        {
            stop = true;
            //Debug.Log("stope!!!!");
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

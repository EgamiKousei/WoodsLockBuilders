using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndForest : MonoBehaviour
{
    public GameObject Carigge;

    int Coin;
    int Wood;
    int Metal;

    [SerializeField]
    GameObject ResultUI;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Coin = CariggeManager.HaveCoin;
        Wood = CariggeManager.HaveWood;
        Metal = CariggeManager.HaveMetal;
    }

    public void GameEnd()
    {
        ResultUI.SetActive(true);
    }
}

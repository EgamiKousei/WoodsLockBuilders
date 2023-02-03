using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndForest : MonoBehaviour
{
    public GameObject Carigge;
    CariggeManager cariggeManager;

    int Coin;
    int Wood;
    int Metal;

    NpcManager npcManager;

    [SerializeField]
    GameObject ResultUI;
    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();

        npcManager = GameObject.Find("GameManager").GetComponent<NpcManager>();
    }

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
        npcManager.DataSave();
    }
}

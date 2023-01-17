using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public GameObject Carigge;
    CariggeManager cariggeManager;

    int WoodQuantity = 0;
    int CoinQuantity = 0;
    int MetalQuantity = 0;

    public Text WoodText;
    public Text CoinText;
    public Text MetalText;
    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        WoodQuantity = CariggeManager.HaveWood;
        CoinQuantity = CariggeManager.HaveCoin;
        MetalQuantity = CariggeManager.HaveMetal;

        WoodText.text = string.Format("{00}", WoodQuantity);
        CoinText.text = string.Format("{00}", CoinQuantity);
        MetalText.text = string.Format("{00}", MetalQuantity);
    }
}

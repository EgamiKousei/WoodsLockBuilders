using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartUI : MonoBehaviour
{
    public GameObject Cart;
    public MinecartManager minecartManager;

    int CoalQuantity = 0;
    int MetalQuantity = 0;
    int CoinQuantity = 0;
    int FlowerQuantity = 0;

    public Text CoalText;
    public Text MetalText;
    public Text CoinText;
    public Text FlowerText;

    // Start is called before the first frame update
    void Start()
    {
        minecartManager = Cart.GetComponent<MinecartManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CoalQuantity = minecartManager.HaveCoal;
        MetalQuantity = minecartManager.HaveMetal;
        CoinQuantity = minecartManager.HaveCoin;
        FlowerQuantity = minecartManager.HaveMetal;

        CoalText.text = string.Format("{00}", CoalQuantity);
        MetalText.text = string.Format("{00}", MetalQuantity);
        CoinText.text = string.Format("{00}", CoinQuantity);
        FlowerText.text = string.Format("{00}", FlowerQuantity);
    }
}

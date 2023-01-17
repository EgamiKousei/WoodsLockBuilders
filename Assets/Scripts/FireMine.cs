using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireMine : MonoBehaviour
{
    public bool CanIgni = false;
    bool OnFire = false;
    float FireTimer = 0;
    float TimerView = 0;
    public int CoalQuantity;

    public GameObject MineCart;
    MinecartManager minecartManager;

    public GameObject Fire_UI;
    FireUI fireui;

    public GameObject FireMesh;

    float span = 9f;
    private float currentTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        minecartManager = MineCart.GetComponent<MinecartManager>();
        fireui = Fire_UI.GetComponent<FireUI>();

        for (int i = 0;i <= 4;i++)
        {
            FireMesh.SetActive(true);
            FireTimer += 1;
            minecartManager.CoalMinus();
            fireui.FireTimePlusMine();
            OnFire = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetWoodQuantity();

        if (Input.GetKeyDown(KeyCode.F) && CanIgni == true && CoalQuantity >= 1 && FireTimer < 5)
        {
            FireMesh.SetActive(true);
            FireTimer += 1;
            minecartManager.CoalMinus();
            fireui.FireTimePlusMine();
            OnFire = true;
        }

        ActiveUI();
        MinusFireTimer();
    }

    void GetWoodQuantity()
    {
        CoalQuantity = minecartManager.HaveCoal;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanIgni = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanIgni = false;
        }
    }

    void MinusFireTimer()
    {

        if (FireTimer >= 1)
        {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {
                FireTimer -= 1;
                currentTime = 0f;
            }
        }

        if (FireTimer <= 0 && OnFire == true)
        {
            FireMesh.SetActive(false);
            OnFire = false;
        }

        TimerView = FireTimer * 2;
    }

    void ActiveUI()
    {
        if (FireTimer >= 1)
        {
            fireui.FireUITrue();
        }
        if (FireTimer <= 0)
        {
            fireui.FireUIFalse();
        }
    }
}

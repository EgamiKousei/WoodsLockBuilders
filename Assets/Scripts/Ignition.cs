using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//馬車の火に関するスクリプト　※スパゲッティコード化
public class Ignition : MonoBehaviour
{
    public bool CanIgni = false;//着火できるか
    bool OnFire = false;//火が付いたか
    float FireTimer = 0;//火の残り時間
    float TimerView = 0;//UIの残り時間
    public int WoodQuantity;//木の所持数

    public GameObject Carigge;
    CariggeManager cariggeManager;

    public GameObject Fire_UI;//火のUI
    FireUI fireui;

    public GameObject FireMesh;//火のモデル、ライト

    float span = 3f;
    private float currentTime = 0f;

    GameObject npcObject;   // Npcオブジェクト
    bool isNpcHit = false;  // Npcにヒットしているか
    public GameObject Light;//directinallight
    NpcManager npcManager;

    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        fireui = Fire_UI.GetComponent<FireUI>();

        // Npc用
        npcManager = GameObject.Find("GameManager").GetComponent<NpcManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetWoodQuantity();

        if (Input.GetKeyDown(KeyCode.F)&&CanIgni == true&&WoodQuantity >=1&&FireTimer <5)
        {
          FireMesh.SetActive(true);
          FireTimer += 1;
          npcManager.HealFire(FireTimer);
          cariggeManager.WoodMinus();
          fireui.FireTimePlus();
          OnFire = true;
        }

        // Npc処理
        if(FireTimer < 2 && WoodQuantity >= 1 && Light.GetComponent<SwitchDayNight>().DayNight == 2)
        {
            npcManager.CloseCarigge();
        }
        else if(FireTimer >= 2 || WoodQuantity <= 0 || Light.GetComponent<SwitchDayNight>().DayNight == 1)
        {
            if (npcObject != null)
            {
                npcObject.GetComponent<Nav>().isFire = false;
            }
        }

        if(isNpcHit == true && WoodQuantity >= 1 && FireTimer < 5 && npcObject.GetComponent<Nav>().isFire == true)
        {
            FireMesh.SetActive(true);

            while (FireTimer < 5)
            {
                FireTimer += 1;
                cariggeManager.WoodMinus();
                fireui.FireTimePlus();
                if(WoodQuantity <= 0)
                {
                    break;
                }
            }
            OnFire = true;
        }

        ActiveUI();
        MinusFireTimer();
    }

    void GetWoodQuantity()
    {
        WoodQuantity = CariggeManager.HaveWood;
        
    }  

private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanIgni = true;
        }
        
        if(other.gameObject.tag=="Npc")
        {
            isNpcHit = true;
            npcObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            CanIgni = false;
        }

        if (other.gameObject.tag == "Npc")
        {
            isNpcHit = false;
        }
    }

    void MinusFireTimer()
    {
        
        if (FireTimer >= 1) {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {              
                FireTimer -= 1;
                currentTime = 0f;
            }
        }

        if(FireTimer <= 0&&OnFire == true)
        {
            FireMesh.SetActive(false);
            OnFire = false;
        }

        TimerView = FireTimer * 6;
    }

    void ActiveUI()
    {
        if(FireTimer >= 1)
        {
            fireui.FireUITrue();
        }
        if (FireTimer <= 0)
        {
            fireui.FireUIFalse();
        }
    }
}

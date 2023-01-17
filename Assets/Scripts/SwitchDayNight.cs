using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

//昼夜切り替えのスクリプト
public class SwitchDayNight : MonoBehaviour
{
    public int DayNight = 1;//1=ひる 2=よる

    public GameObject Light;//directinallight

    float Span = 60f;//スパン
    //float Span = 20f;//スパン
    private float currentTime = 0f;

    private Animator anim;//アニメーター

    public GameObject Carigge;//馬車
    CariggeManager cariggeManager;
    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        anim = gameObject.GetComponent<Animator>();
    }

   
    void Update()//アニメーターを使用してlightの明度を下げる
    {
        if (cariggeManager.GameStart == true)
        {
            currentTime += Time.deltaTime;

            if (currentTime > Span)
            {
                if (DayNight == 1)//夜になる
                {
                    DayNight = 2;
                    anim.SetBool("SetNight", true);
                    anim.SetBool("SetDay", false);
                    Span = 30;
                }
                else if (DayNight == 2)//昼になる
                {
                    DayNight = 1;
                    anim.SetBool("SetNight", false);
                    anim.SetBool("SetDay", true);
                    Span = 60;
                }
                currentTime = 0f;
            }
        }
        
      
    }

     
}


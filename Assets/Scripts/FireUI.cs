using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//火のUIに関するスクリプト
public class FireUI : MonoBehaviour
{
    public Slider SliderR;
    public Slider SliderL;

    float span = 0.1f;
    private float currentTime = 0f;

    //float maxtime = 15;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        //time = 15;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 0.1) {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {
                time-=0.1f;
                currentTime = 0f;
            }
        }
        SliderR.value = time;
        SliderL.value = time;
    }

    public void FireTimePlus()
    {
        time += 3;
    }
    public void FireTimePlusMine()
    {
        time += 9;
    }


    public void FireUITrue()
    {
        transform.DOScaleY(1,0.5f);
    }

    public void FireUIFalse()
    {
        transform.DOScaleY(0, 0.5f);
    }
}

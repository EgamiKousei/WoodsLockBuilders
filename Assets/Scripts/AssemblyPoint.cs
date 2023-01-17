using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//集合場所、ゲームスタートに関するスクリプト
public class AssemblyPoint : MonoBehaviour
{
    public Slider slider;//スライダー
    bool GetOnChara = false;//キャラが上に載ってるか
    public GameObject Carigge;
    CariggeManager cariggeManager;

    public GameObject cnv;//UIキャンバス
    BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        cariggeManager = Carigge.GetComponent<CariggeManager>();
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        sliderplus();
        if(slider.value >= 1)//ゲージマックスで開始
        {
            Invoke("EndStartAnim", 3);
            cnv.SetActive(false);
            col.enabled = false;
        }
    }

    void EndStartAnim()//3秒間くらいの開始アニメーションが入る（予定）
    {
        this.gameObject.SetActive(false);
        cariggeManager.AsembStart();
    }

    void sliderplus()//キャラが上に載ってるときゲージが増えていく
    {
        if(GetOnChara == true)
        {
            slider.value += 0.006f;
        }
        else
        {
            slider.value = 0f;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            GetOnChara = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            GetOnChara = false;
        }
    }
}

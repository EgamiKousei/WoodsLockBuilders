using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//シーン開始時に黒からフェードアウトするスクリプト
public class CameraStartFade : MonoBehaviour
{
    public GameObject FadeBlack;//黒い前景
    public Image fadeblack;//イメージ
    public float CamPos = 26.5f;//カメラのY座標
    // Start is called before the first frame update
    void Start()
    {
        FadeBlack.SetActive(true);
        Invoke("FadeStart", 1.5f);
    }
        

    // Update is called once per frame
    void Update()
    {

    }

    void FadeStart()//フェードアウトしながらY座標を下げる
    {
        fadeblack.DOFade(endValue: 0f, duration: 4f);
     
        transform.DOLocalMoveY(CamPos,4f);

        GameObject.Find("GameManager").GetComponent<NpcManager>().Invoke("ActiveNav", 3f);
        
    }

}

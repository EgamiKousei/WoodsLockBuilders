using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//森の分岐の決定に関するスクリプト
public class RoadSignArrow : MonoBehaviour
{
    public GameObject obj;//オブジェクト
    public Material DefMat;//デフォルトのマテリアル
    public Material ShineMat;//アクティブなマテリアル

    GameObject Carigge;

    public bool ThisRight = true;//右の場合チェック
    public bool ThisP2 = true;//フェーズ2の場合チェック
    bool CanSelect = false;//セレクトできるか

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanSelect == true)//セレクトできる場所の上に乗った状態でFを押すと実行される
        {
            PhaseSet();
        }
    }

    private void OnTriggerEnter(Collider other)//明るいマテリアルへの変更
    {
        if(other.gameObject.name == "Player")
        {
            obj.GetComponent<MeshRenderer>().material = ShineMat;
            CanSelect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            obj.GetComponent<MeshRenderer>().material = DefMat;
            CanSelect = false;
        }
    }

    void PhaseSet()//それぞれの分岐をセットする
    {
        Carigge = GameObject.Find("Carigge");
       
        if (ThisRight == true&&ThisP2 == true)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase2R();
        }
        else if(ThisRight == false&&ThisP2 == true)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase2L();
        }
        else if(ThisRight == true&&ThisP2 == false)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase3R();
        }
        else if (ThisRight == false && ThisP2 == false)
        {
            Carigge.GetComponent<CariggeManager>().SetPhase3L();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RailSwitch : MonoBehaviour
{
    public GameObject RailR;
    public GameObject RailL;

    public GameObject AGR;
    public GameObject AGL;

    public GameObject Lever;

    bool Canchange = false;
    bool Canchange2 = true;

    bool SetR = true;
    bool SetL = false;
    
    Renderer rendererR;
    Renderer rendererL;

    [SerializeField]
    Renderer rendererComponent;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Canchange == true && Canchange2 == true)
        {
            if (SetR == true)
            {
                RailL.SetActive(true);
                RailR.SetActive(false);
                Canchange2 = false;
                SetR = false;
                SetL = true;
                Debug.Log("setr");
                Invoke("setCanchange2", 0.5f);
                Lever.transform.DORotate(new Vector3(0, 0, -25), 0.4f);
            }
            else if (SetL == true)
            {
                RailR.SetActive(true);
                RailL.SetActive(false);
                Canchange2 = false;
                SetL = false;
                SetR = true;
                Debug.Log("setl");
                Invoke("setCanchange2", 0.5f);
                Lever.transform.DORotate(new Vector3(0, 0, 25), 0.4f);
            }

        }
    }

    void setCanchange2()
    {
        Canchange2 = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            Canchange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "ActivatePoint")
        {
            Canchange = false;
        }
    }
}

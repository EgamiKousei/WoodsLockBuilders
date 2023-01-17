using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI周りのスクリプト
public class UiActive : MonoBehaviour
{
    public GameObject FireOn;
    public GameObject HealOn;
    public GameObject GrabOn;
    public GameObject TrackOn;

    public PlayerManager2 playermanager;

    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "FireZone")
        {
            FireOn.SetActive(true);
        }
        if (other.gameObject.name == "HealZone")
        {
            HealOn.SetActive(true);
            playermanager.SwitchCanHeal();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FireZone")
        {
            FireOn.SetActive(false);
        }
        if (other.gameObject.name == "HealZone")
        {
            HealOn.SetActive(false);
            playermanager.SwitchCanHealFalse();
        }
    }

}

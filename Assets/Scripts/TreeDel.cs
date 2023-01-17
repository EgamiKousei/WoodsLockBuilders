using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDel : MonoBehaviour
{

    public GameObject TreeMain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Artifact")
        {
            TreeMain.SetActive(false);
        }
        if (other.gameObject.tag == "Bossobj")
        {
            TreeMain.SetActive(false);
        }
        if (other.gameObject.tag == "Metal")
        {
            TreeMain.SetActive(false);
        }
    }
}

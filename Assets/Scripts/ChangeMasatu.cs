using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMasatu : MonoBehaviour
{
    public PhysicMaterial matnrm;
    public PhysicMaterial matlow;

    public GameObject OyaWheel;

    SphereCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = OyaWheel.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Masatu")
        {
            col.material = matlow;
            Debug.Log("on");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Masatu")
        {
            col.material = matnrm;
            Debug.Log("of");
        }
    }
}

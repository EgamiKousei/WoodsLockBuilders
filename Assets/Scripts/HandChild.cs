using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandChild : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform children = this.gameObject.GetComponentInChildren<Transform>();
        //�q�v�f�����Ȃ���ΏI��
        if (children.childCount == 0)
        {
            parent.gameObject.GetComponent<Nav>().HandHaving = false;
        }
        else
        {
            parent.gameObject.GetComponent<Nav>().HandHaving = true;
        }
    }
}
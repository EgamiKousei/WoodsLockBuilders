using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandChild : MonoBehaviour
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
        //éqóvëfÇ™Ç¢Ç»ÇØÇÍÇŒèIóπ
        if (children.childCount == 0)
        {
            parent.gameObject.GetComponent<PlayerManager2>().HandHaving = false;
        }
        else
        {
            parent.gameObject.GetComponent<PlayerManager2>().HandHaving = true;
        }
    }
}

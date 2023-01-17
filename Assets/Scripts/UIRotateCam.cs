using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        //@ƒJƒƒ‰‚Æ“¯‚¶Œü‚«‚Éİ’è
        transform.rotation = Camera.main.transform.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    //public Animator anim;
    void Start()
    {
        //anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        Animator anim = GetComponent<Animator>();
        if (Input.GetKey("w"))
        {
            anim.SetBool("is_running", true);
        }
        if (Input.GetKey("a"))
        {
            anim.SetBool("is_running", true);
        }
        if (Input.GetKey("s"))
        {
            anim.SetBool("is_running", true);
        }
        if (Input.GetKey("d"))
        {
            anim.SetBool("is_running", true);
        }
        else
        {
            anim.SetBool("is_running", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("atk", true);
        }
        else
        {
            anim.SetBool("atk", false);

        }
    }
}

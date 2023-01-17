using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//トロッコ移動に関するスクリプト
public class MinecartMove : MonoBehaviour
{
    public GameObject Cart;

    void CartMove()//攻撃されたらaddforceで移動
    {
        Rigidbody rb = Cart.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * 800000);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AttackHantei")
        {
            CartMove();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�g���b�R�ړ��Ɋւ���X�N���v�g
public class MinecartMove : MonoBehaviour
{
    public GameObject Cart;

    void CartMove()//�U�����ꂽ��addforce�ňړ�
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

using UnityEngine;

public class OtherPlayer : MonoBehaviour
{

    Rigidbody rb;//リギッドボディ

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, -PlayerManager.Gravi, 0));
    }
    }

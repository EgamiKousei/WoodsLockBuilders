using UnityEngine;

public class OtherPlayer : MonoBehaviour
{

    Rigidbody rb;//リギッドボディ
    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, -PlayerManager.Gravi, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag!="Player")
            _animator.SetBool(PlayerManager.jumpParamHash, false);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag != "Player")
            _animator.SetBool(PlayerManager.jumpParamHash, true);
    }
}

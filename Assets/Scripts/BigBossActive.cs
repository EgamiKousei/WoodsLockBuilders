using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigBossActive : MonoBehaviour
{
    public GameObject Base;//–{‘Ì
    NavMeshAgent nav;
    ChangeTag change;

    // Start is called before the first frame update
    void Start()
    {
        change = Base.GetComponent<ChangeTag>();
        nav = Base.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            nav.enabled = true;
            change.enabled = true;

        }
    }
}

using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject chairText;

    public GameObject mainCamera, aimPlace;
    GameObject cannonCamera;
    bool Attack=false;

    Transform _Transform;

    private void Start()
    {
        _Transform = transform;
    }

    private void Update()
    {
        if (Attack == true)
        {
            chairText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                ActionManager.playerScean = ActionManager.Player.Cannon;
                mainCamera.SetActive(false);
                cannonCamera.SetActive(true);
                cannonCamera.GetComponent<CannonCamera>().setCannon();
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                cannonCamera.GetComponent<CannonCamera>().endCannon();
                mainCamera.SetActive(true);
                cannonCamera.SetActive(false);
                ActionManager.playerScean = ActionManager.Player.Attack;
            }
            if (Input.GetMouseButton(0)&& Input.GetKey(KeyCode.F))
                {
                    aimPlace.SetActive(true);
                    Debug.Log("ó≠Çﬂ");
                }
                // î≠éÀ
                else if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.F))
                {
                    Debug.Log("âìäuçUåÇ");
                    aimPlace.SetActive(false);
                }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"&&ActionManager.playerScean== ActionManager.Player.Attack)
        {
            Attack = true;
            cannonCamera = _Transform.GetChild(0).gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Attack = false;
            chairText.SetActive(false);
        }
    }
}

using UnityEngine;

public class FieldManager : MonoBehaviour
{
    // Start is called before the first frame update
    int plant_1, plant_2, plant_3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Plant()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && ActionManager.playerScean == ActionManager.Player.Field)
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
        }
    }
}

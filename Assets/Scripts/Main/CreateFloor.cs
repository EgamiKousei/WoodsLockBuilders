using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{
    public GameObject FloorPrefab, Plane;
    private GameObject obj;
    // Start is called before the first frame update
    private void Awake()
    {
        for (int x = -28; x <= 30; x = x + 2)
        {
            for(int z = -30; z <= 28; z = z + 2)
            {
                Vector3 pos = new Vector3(x, 0, z);
                obj=Instantiate(FloorPrefab, pos, Quaternion.Euler(-90, 0, 0));
                obj.transform.parent = Plane.transform;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSet : MonoBehaviour
{
    public GameObject CreateMap;
    GameObject itemObject = null;
    public static int objNum=0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var i in PlayerData.PlayMap.Values)
        {
            itemObject = (GameObject)Resources.Load(i.name);
            Vector3 arPoint = new Vector3(i.x, i.y,i.z);
            GameObject obj = Instantiate(itemObject, arPoint, Quaternion.identity);
            obj.name = i.num.ToString();
            Vector3 rotationAngles = new Vector3(i.xr, i.yr, i.zr);
            obj.transform.rotation= Quaternion.Euler(rotationAngles);
            obj.transform.parent = CreateMap.transform;
            if (objNum <= i.num)
                objNum = i.num;
        }
    }
}

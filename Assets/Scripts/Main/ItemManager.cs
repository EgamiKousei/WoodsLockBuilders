using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.ItemBox.ContainsKey("装備"))
        {
            Debug.Log(PlayerData.ItemBox["装備"].category);
        }
        foreach (var i in PlayerData.ItemBox.Values)
        {
            if (i.invno > 0)
            {
                Debug.Log("インベントリ" + i.invno + ":" + i.name + i.invnum);
            }
            if (i.bagnum > 0)
            {
                //バッグにセット
                Debug.Log("バッグ:" + i.name + i.bagnum);
            }
        }
    }
}

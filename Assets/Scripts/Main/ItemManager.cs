using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.ItemBox.ContainsKey("����"))
        {
            Debug.Log(PlayerData.ItemBox["����"].category);
        }
        foreach (var i in PlayerData.ItemBox.Values)
        {
            if (i.invno > 0)
            {
                Debug.Log("�C���x���g��" + i.invno + ":" + i.name + i.invnum);
            }
            if (i.bagnum > 0)
            {
                //�o�b�O�ɃZ�b�g
                Debug.Log("�o�b�O:" + i.name + i.bagnum);
            }
        }
    }
}

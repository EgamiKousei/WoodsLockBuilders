using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoForest()
    {
        SceneManager.LoadScene("Forest");
    }

    public void GoMain()
    {
        ItemSet("Coin", CariggeManager.HaveCoin);
        ItemSet("Box", CariggeManager.HaveWood);
        ItemSet("Stone", CariggeManager.HaveMetal);
        SceneManager.LoadScene("Main");
    }
    public void ItemSet(string ItemName,int data)
    {
        if (PlayerData.ItemBox[ItemName].invnum > 0)
            PlayerData.ItemBox[ItemName].invnum+= data;
        else if (PlayerData.ItemBox[ItemName].bagnum > 0)
            PlayerData.ItemBox[ItemName].bagnum+= data;
        else
        {
            int bagno = 1;
            foreach (var x in PlayerData.ItemBox.Values)
            {
                if (x.bagno > bagno)
                    bagno = x.bagno;
            }
            PlayerData.ItemBox[ItemName].bagnum+=data;
            PlayerData.ItemBox[ItemName].bagno = bagno + 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject ColorMana;
    bool Color = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)&& Color==false)
            SetColor();
        else if (Input.GetKeyDown(KeyCode.X) && Color == true)
            EndColor();
    }
    public void SetColor()
    {
        ColorMana.SetActive(true);
        Color = true;
    }
    public void EndColor() {
        ColorMana.SetActive(false);
        ColorMana.GetComponent<ColorManager>().SetEnd();
        Color = false;
    }
}

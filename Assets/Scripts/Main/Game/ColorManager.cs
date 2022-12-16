using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public Material PlayerMate;
    public Slider H, S, V;
    public Image Simage;
    private float H_flort, S_flort, V_flort;
    //–{—ˆ‚ÌƒJƒ‰[‚Í3059AE

    private void Awake()
    {
        float r = (Convert.ToInt32(PlayerData.SaveData["color"], 16) >> 16) & 0xff;
        float g = (Convert.ToInt32(PlayerData.SaveData["color"], 16) >> 8) & 0xff;
        float b = Convert.ToInt32(PlayerData.SaveData["color"], 16) & 0xff;
        //PlayerMate.color =new Color(r / 255, g / 255, b / 255);
        Color.RGBToHSV(new Color(r / 255, g / 255, b / 255), out H_flort, out S_flort, out V_flort);

        H.value = H_flort;
        S.value = S_flort;
        V.value = V_flort;
    }
    void Update()
    {
        Simage.color = Color.HSVToRGB(H.value, S.value, V.value);
        PlayerMate.color = Color.HSVToRGB(H.value, S.value, V.value);
    }
    public void SetEnd()
    {
        int r = (int)(PlayerMate.color.r * 255);
        int g = (int)(PlayerMate.color.g * 255);
        int b = (int)(PlayerMate.color.b * 255);
        string R = r.ToString("x");
        string G = g.ToString("x");
        string B = b.ToString("x");
        string RGB = R + G + B;
        PlayerData.SaveData["color"] = RGB;
    }
}

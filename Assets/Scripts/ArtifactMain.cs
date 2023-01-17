using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アーティファクトのメイン　30秒間ドロップ数を2倍にする処理のみ
public class ArtifactMain : MonoBehaviour
{
    public int DropMultiply = 1;//ドロップ倍率

    public GameObject[] DelTree;

    float span = 30f;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DropMultiply >=2)
        {
            currentTime += Time.deltaTime;
            if (currentTime > span)
            {
                DropMultiply = 1;
                currentTime = 0f;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�A�[�e�B�t�@�N�g�̃��C���@30�b�ԃh���b�v����2�{�ɂ��鏈���̂�
public class ArtifactMain : MonoBehaviour
{
    public int DropMultiply = 1;//�h���b�v�{��

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���Ɋւ���X�N���v�g1�@���̌����������_���ɂ��邾���@���C���̏�����weedsway
public class Weed : MonoBehaviour
{
    public GameObject mesh;//���f��
    int randomY;
    float meshY;

    
    // Start is called before the first frame update
    void Start()
    {
        RotateRanomizer();
        mesh.transform.Rotate(0.0f, meshY, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RotateRanomizer()//��]��4�p�^�[�����烉���_���Őݒ�
    {
        randomY = Random.Range(0, 3);
        if (randomY== 0)
        {
            meshY = 0;
        }
        else if (randomY == 1)
        {
            meshY = 90;
        }
        else if (randomY == 2)
        {
            meshY = 180;
        }
        else if (randomY == 3)
        {
            meshY = 270;
        }
    }
}

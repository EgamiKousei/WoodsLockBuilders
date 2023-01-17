using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//�V�[���J�n���ɍ�����t�F�[�h�A�E�g����X�N���v�g
public class CameraStartFade : MonoBehaviour
{
    public GameObject FadeBlack;//�����O�i
    public Image fadeblack;//�C���[�W
    public float CamPos = 26.5f;//�J������Y���W
    // Start is called before the first frame update
    void Start()
    {
        FadeBlack.SetActive(true);
        Invoke("FadeStart", 1.5f);
    }
        

    // Update is called once per frame
    void Update()
    {

    }

    void FadeStart()//�t�F�[�h�A�E�g���Ȃ���Y���W��������
    {
        fadeblack.DOFade(endValue: 0f, duration: 4f);
     
        transform.DOLocalMoveY(CamPos,4f);

        GameObject.Find("GameManager").GetComponent<NpcManager>().Invoke("ActiveNav", 3f);
        
    }

}

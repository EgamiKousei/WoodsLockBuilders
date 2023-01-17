using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MineCamMove : MonoBehaviour
{
    //float MoveSpeed = 270;
    float MoveSpeed = 20;
    public GameObject Player;

    int P2 = 0;
    int P3 = 0;
    // Start is called before the first frame update
    void Start()
    {
        Phase1();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Phase1()
    {
        transform.DOMove(new Vector3(430f, 0f, 0f), MoveSpeed)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
           // EndAnim = true;
            //ThisPhase = 2;
            Invoke("Phase2", 5f);
            movepl();
        });
    }

    void Phase2()
    {
        if (P2 == 0)
        {

            Vector3[] path = {
                new Vector3(435.2841f,0f,0f),
                new Vector3(440.893f,0f,-0.4182539f),
                new Vector3(445.7401f,0f,-2.756904f),
                new Vector3(448.7082f,0f,-8.002534f),
                new Vector3(685f,0f,-411f)

        };

            transform.DOPath(path, MoveSpeed, PathType.Linear)
                   .SetEase(Ease.Linear)
                   .SetLookAt(0.01f, Vector3.left)
                   .SetOptions(false, AxisConstraint.Y)
                   .OnComplete(() =>
                   {
                       Invoke("Phase3", 5f);
                   });
        }
        else
        {
            Vector3[] path = {
                new Vector3(435.2841f,0f,0f),
                new Vector3(440.893f,0f,0.4182539f),
                new Vector3(445.7401f,0f,2.756904f),
                new Vector3(448.7082f,0f,8.002534f),
                new Vector3(685f,0f,411f)
         };

            transform.DOPath(path, MoveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.left)
                    .SetOptions(false, AxisConstraint.Y)
                    .SetAutoKill(true)
                    .OnComplete(() =>
                    {
                        
                    });
        }
    }

    void Phase3()
    {

        if (P2 == 0 && P3 == 0)
        {

            Vector3[] path = {
               new Vector3(686.0397f,0f,-412.8144f),
                new Vector3(687.5241f,0f,-416.5761f),
                new Vector3(688.2f,0f,-419.4f),
                new Vector3(688.5f,0f,-426.7f),
                new Vector3(685.6f,0f,-437.75f),
                new Vector3(443.8f,0f,-860.3f)

             };

            transform.DOPath(path, MoveSpeed, PathType.Linear)
                   .SetEase(Ease.Linear)
                   .SetLookAt(0.01f, Vector3.left)
                   .SetOptions(false, AxisConstraint.Y)
                   .OnComplete(() =>
                   {
                       movepl();


                   });
        }
        else if (P2 == 0 && P3 == 1)
        {
            Vector3[] path = {
                new Vector3(686.0397f,0f,-412.8144f),
                new Vector3(688.9564f,0f,-416.6613f),
                new Vector3(693.5315f,0f,-420.2763f),
                new Vector3(700.3274f,0f,-423.0587f),
                new Vector3(709.9006f,0f,-424.988f),
                new Vector3(1180f,0f,-424.8311f)
        };

            transform.DOPath(path, MoveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.left)
                    .SetOptions(false, AxisConstraint.Y)
                    .SetAutoKill(true)
                    .OnComplete(() =>
                    {

                        movepl();
                    });
        }
        else if (P2 == 1 && P3 == 0)
        {

            Vector3[] path = {

                new Vector3(686.0397f,0f,412.8144f),
                new Vector3(688.9564f,0f,416.6613f),
                new Vector3(693.5315f,0f,420.2763f),
                new Vector3(700.3274f,0f,423.0587f),
                new Vector3(709.9006f,0f,424.988f),
                new Vector3(1180f,0f,424.8311f)



             };

            transform.DOPath(path, MoveSpeed, PathType.Linear)
                   .SetEase(Ease.Linear)
                   .SetLookAt(0.01f, Vector3.left)
                   .SetOptions(false, AxisConstraint.Y)
                   .OnComplete(() =>
                   {
                       movepl();


                   });
        }
        else if (P2 == 1 && P3 == 1)
        {
            Vector3[] path = {
                new Vector3(686.0397f,0f,412.8144f),
                new Vector3(687.5241f,0f,416.5761f),
                new Vector3(687.5666f,0f,419.4821f),
                new Vector3(686.1626f,0f,424.315f),
                new Vector3(682.203f,0f,433.0391f),
                new Vector3(437.5692f,0f,863.125f)
        };

            transform.DOPath(path, MoveSpeed, PathType.Linear)
                    .SetEase(Ease.Linear)
                    .SetLookAt(0.01f, Vector3.left)
                    .SetOptions(false, AxisConstraint.Y)
                    .SetAutoKill(true)
                    .OnComplete(() =>
                    {
                        movepl();


                    });
        }
    }
    void movepl()
    {
        Player.transform.position = this.transform.position;
    }
}

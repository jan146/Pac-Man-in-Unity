using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDir : MonoBehaviour
{

    public GameObject GO;
    public Animator Animator;
    public string AnimatorParameter;
    public int Direction;

    float Currentx;
    float Currenty;
    float Prevx;
    float Prevy;

    void Update()
    {

        Currentx = GO.transform.position.x;
        Currenty = GO.transform.position.y;

        if (Currentx > Prevx) Direction = 0;        // desno
        else if (Currentx < Prevx) Direction = 1;   // levo
        else if (Currenty > Prevy) Direction = 2;   // gor
        else if (Currenty < Prevy) Direction = 3;   // dol

        Prevx = Currentx;
        Prevy = Currenty;

        Animator.SetInteger(AnimatorParameter, Direction);
        //DirPrint();

    }

    void DirPrint() {

        if (Direction == 0) Debug.Log("desno");
        else if (Direction == 1) Debug.Log("levo");
        else if (Direction == 2) Debug.Log("gor");
        else if (Direction == 3) Debug.Log("dol");

    }

}
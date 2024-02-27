using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float DefaultSpeed;
    float ActualSpeed;

    public GameObject Player1;
    public Rigidbody2D RB1;
    public GameObject Player2;
    public Rigidbody2D RB2;
    public GameController GameController;

    Vector2 Dest1 = Vector2.zero;
    Vector2 MainDir1 = Vector2.zero;
    Vector2 AltDir1 = Vector2.zero;
    Vector2 Dest2 = Vector2.zero;
    Vector2 MainDir2 = Vector2.zero;
    Vector2 AltDir2 = Vector2.zero;

    void Awake()
    {

        Dest1 = (Vector2)Player1.transform.position;
        if (OptionsController.Multiplayer)
            Dest2 = (Vector2)Player2.transform.position;

        switch (OptionsController.Difficulty)
        {

            case 0: DefaultSpeed = 0.1f;
                break;
            case 1: DefaultSpeed = 0.09f;
                break;
            case 2: DefaultSpeed = 0.08f;
                break;

        }

    }

    void Update()
    {

        if (GameController.GetComponent<GameController>().Freeze) ActualSpeed = 0f;
        else ActualSpeed = DefaultSpeed;

        RB1.MovePosition(Vector2.MoveTowards((Vector2)Player1.transform.position, Dest1, ActualSpeed));
        CheckInput1();
        Dest1 = WorldLoop(Player1, Dest1);
        if (Dest1 == (Vector2)Player1.transform.position)
            ChangeDirection1();

        if (OptionsController.Multiplayer)
        {

            RB2.MovePosition(Vector2.MoveTowards((Vector2)Player2.transform.position, Dest2, ActualSpeed));
            CheckInput2();
            Dest2 = WorldLoop(Player2, Dest2);
            if (Dest2 == (Vector2)Player2.transform.position)
                ChangeDirection2();

        }
        //DirPrint();

    }

    void CheckInput1() {

        if (Input.GetKey("w")) AltDir1 = Vector2.up;
        else if (Input.GetKey("a")) AltDir1 = Vector2.left;
        else if (Input.GetKey("s")) AltDir1 = Vector2.down;
        else if (Input.GetKey("d")) AltDir1 = Vector2.right;

    }

    void CheckInput2()
    {

        if (Input.GetKey("up")) AltDir2 = Vector2.up;
        else if (Input.GetKey("left")) AltDir2 = Vector2.left;
        else if (Input.GetKey("down")) AltDir2 = Vector2.down;
        else if (Input.GetKey("right")) AltDir2 = Vector2.right;

    }

    void ChangeDirection1() {

        if (CheckRay(Player1.transform.position, AltDir1)) MainDir1 = AltDir1;
        if (CheckRay(Player1.transform.position, MainDir1)) Dest1 += MainDir1;

    }

    void ChangeDirection2() {

        if (CheckRay(Player2.transform.position, AltDir2)) MainDir2 = AltDir2;
        if (CheckRay(Player2.transform.position, MainDir2)) Dest2 += MainDir2;

    }

    bool CheckRay(Vector2 Source, Vector2 Direction)
    {


        //int t = Physics2D.Linecast((Vector2) transform.position + Dir, (Vector2)transform.position);
        RaycastHit2D t = Physics2D.Linecast(Source + Direction, Source);
        //Debug.Log(Physics2D.Linecast((Vector2) transform.position + Dir, (Vector2)transform.position).collider);

        //Debug.Log(t);
        //Debug.Log(t.distance);

        if (t.collider != null) return (t.collider.tag != "Wall");
        return true;

    }

    void DirPrint()
    {

        Debug.Log("Dest1: " + Dest1);
        Debug.Log("Player1: " + (Vector2)Player1.transform.position);
        Debug.Log("Dest2: " + Dest2);
        Debug.Log("Player2: " + (Vector2)Player2.transform.position);

    }

    Vector2 WorldLoop(GameObject GO, Vector2 Dest)
    {

        if (GO.transform.position.x > GameController.WorldWidth)
        {

            GO.transform.Translate(new Vector2(-2*GameController.WorldWidth, 0f));
            Dest.x = -GameController.WorldWidth + 0.5f;

        }

        else if (GO.transform.position.x < -GameController.WorldWidth)
        {

            GO.transform.Translate(new Vector2(2*GameController.WorldWidth, 0f));
            Dest.x = GameController.WorldWidth - 0.5f;

        }
        return Dest;

    }

    public void ResetDestination()
    {

        Dest1 = (Vector2)Player1.transform.position;
        MainDir1 = AltDir1 = Vector2.zero;
        if (OptionsController.Multiplayer) {

            Dest2 = (Vector2)Player2.transform.position;
            MainDir2 = AltDir2 = Vector2.zero;

        }

    }

}
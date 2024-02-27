using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pink : GhostManager
{

    Vector2 AlternativeTarget;
    public GameObject PinkPointer;

    void Update()
    {

        CheckDeath();   // za freeze oz. ce umre
        CheckFrightened();  // za animacijo (bigpellet)
        if (!IsDead) Alive();
        else Dead();

    }

    void Alive()
    {

        if (Frightened != 0)
        {

            if (!GameController.GetComponent<GameController>().Freeze) ActualSpeed = 0.03f;
            if ((Vector2)Ghost.transform.position == Dest)
            {
                if (OptionsController.Multiplayer && Player2Closer()) MoveFrightened(Player2);
                else MoveFrightened(Player1);
            }

        }

        else if ((Vector2)Ghost.transform.position == Dest)
        {
            ActualSpeed = DefaultSpeed;
            if (OptionsController.Multiplayer && Player2Closer())
                Move(Player2);
            else
                Move(Player1);
        }

        RB.MovePosition(Vector2.MoveTowards((Vector2)Ghost.transform.position, Dest, ActualSpeed));

        WorldLoop();

    }

    void Move(GameObject Player)
    {

        if (CheckForPlayer(Vector2.up) || CheckForPlayer(Vector2.down) || CheckForPlayer(Vector2.left) || CheckForPlayer(Vector2.right))
        {

            PinkPointer.transform.position = Player.transform.position;

            if (CheckForPlayer(Vector2.up)) Dir = Vector2.up;
            else if (CheckForPlayer(Vector2.down)) Dir = Vector2.down;
            else if (CheckForPlayer(Vector2.left)) Dir = Vector2.left;
            else if (CheckForPlayer(Vector2.right)) Dir = Vector2.right;
            Dest += Dir;

        }

        else    // ce ni pacmana 2 enoti stran
        {

            if (Player.GetComponent<TrackDir>().Direction == 0 && CheckForWall(Player, Vector2.right)) AlternativeTarget = (Vector2)Player.transform.position + (Vector2.right * 2);
            else if (Player.GetComponent<TrackDir>().Direction == 1 && CheckForWall(Player, Vector2.left)) AlternativeTarget = (Vector2)Player.transform.position + (Vector2.left * 2);
            else if (Player.GetComponent<TrackDir>().Direction == 2 && CheckForWall(Player, Vector2.up)) AlternativeTarget = (Vector2)Player.transform.position + (Vector2.up * 2);
            else if (Player.GetComponent<TrackDir>().Direction == 3 && CheckForWall(Player, Vector2.down)) AlternativeTarget = (Vector2)Player.transform.position + (Vector2.down * 2);

            PinkPointer.transform.position = AlternativeTarget;

            Chase(AlternativeTarget);

        }

    }

}
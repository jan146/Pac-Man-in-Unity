using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : GhostManager
{

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

        if (PlayerWithinXBlocks(Player, 8)) OrangeToCorner();
        else Chase(Player.transform.position);

    }

    bool PlayerWithinXBlocks(GameObject Player, float Distance)
    {

        if ((Vector2.Distance((Vector2)Ghost.transform.position, (Vector2)Player.transform.position)) <= Distance) return true;
        return false;       // true = stran je manj (ali tocno) kot X enoti, false = stran je vec kot X enot

    }

    void OrangeToCorner()
    {

        Vector2 CurrentDir = Dir;

        if (CheckRay(Vector2.up) && CurrentDir != Vector2.down)
        {

            Dir = Vector2.up;

        }
        if (CheckRay(Vector2.right) && CurrentDir != Vector2.left)
        {

            Dir = Vector2.right;

        }
        if (CheckRay(Vector2.left) && CurrentDir != Vector2.right)
        {

            Dir = Vector2.left;

        }
        if (CheckRay(Vector2.down) && CurrentDir != Vector2.up)
        {

            Dir = Vector2.down;

        }
        Dest += Dir;

    }

}
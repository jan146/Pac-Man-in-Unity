using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : GhostManager
{

    float AdditionalSpeed = 1f;

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
                Chase(Player2.transform.position);
            else
                Chase(Player1.transform.position);
        }

        AdditionalSpeed = (1f + (GameController.CurrentPellets / GameController.TotalPellets) * 0.15f);

        RB.MovePosition(Vector2.MoveTowards((Vector2)Ghost.transform.position, Dest, ActualSpeed * AdditionalSpeed));

        WorldLoop();

    }

}
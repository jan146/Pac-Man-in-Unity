using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{

    public GameObject Player;
    public AudioController AudioController;
    public GameController GameController;
    public GameObject GhostRed;
    public GameObject GhostPink;
    public GameObject GhostOrange;
    public GameObject GhostBlue;

    public float Delay1 = 5f;
    public float Delay2 = 2.5f;
    float CurrentTime = 0;

    int BigPelletCounter = 0;
    int GhostsKilled = 0;

    int RedDeaths = 0;
    int PinkDeaths = 0;
    int OrangeDeaths = 0;
    int BlueDeaths = 0;

    void Update()
    {

        if (GameController.CurrentPellets >= GameController.TotalPellets) GameController.GetComponent<GameController>().GameWin();
        if (GameController.GetComponent<GameController>().Freeze) CurrentTime = 0;

        if (!GameController.GetComponent<GameController>().Freeze && BigPelletCounter > 0)
        {

            CurrentTime -= Time.deltaTime;
            if (CurrentTime > Delay2)
            {

                if (RedDeaths < BigPelletCounter) GhostRed.GetComponent<GhostManager>().Frightened = 1;
                if (PinkDeaths < BigPelletCounter) GhostPink.GetComponent<GhostManager>().Frightened = 1;
                if (OrangeDeaths < BigPelletCounter) GhostOrange.GetComponent<GhostManager>().Frightened = 1;
                if (BlueDeaths < BigPelletCounter) GhostBlue.GetComponent<GhostManager>().Frightened = 1;

            }

            else if (CurrentTime > 0 && CurrentTime < Delay2)
            {

                if (GhostRed.GetComponent<GhostManager>().Frightened == 1) GhostRed.GetComponent<GhostManager>().Frightened = 2;
                if (GhostPink.GetComponent<GhostManager>().Frightened == 1) GhostPink.GetComponent<GhostManager>().Frightened = 2;
                if (GhostOrange.GetComponent<GhostManager>().Frightened == 1) GhostOrange.GetComponent<GhostManager>().Frightened = 2;
                if (GhostBlue.GetComponent<GhostManager>().Frightened == 1) GhostBlue.GetComponent<GhostManager>().Frightened = 2;

            }

            else if (CurrentTime <= 0)
            {

                GhostRed.GetComponent<GhostManager>().Frightened = GhostPink.GetComponent<GhostManager>().Frightened = GhostOrange.GetComponent<GhostManager>().Frightened = GhostBlue.GetComponent<GhostManager>().Frightened = 0;
                CurrentTime = 0;
                GhostsKilled = 0;

            }

        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Pellet")
        {

            Destroy(col.gameObject);
            GameController.Score += 10;
            GameController.CurrentPellets++;
            AudioController.PlaySound("Chomp");

        }

        else if (col.gameObject.tag == "BigPellet")
        {

            Destroy(col.gameObject);
            GameController.CurrentPellets++;
            BigPelletCounter++;
            CurrentTime = Delay1 + Delay2;
            GameController.Score += 10;
            AudioController.PlaySound("Chomp");

        }

        else if (col.gameObject.tag == "Red")
        {

            if (GhostRed.GetComponent<GhostManager>().Frightened != 0)
            {
                AudioController.PlaySound("GhostDeath");
                GhostRed.GetComponent<GhostManager>().IsDead = true;
                RedDeaths++;
                GhostsKilled++;
                KillGhost();
            }

            else GameController.LoseLife();

        }

        else if (col.gameObject.tag == "Pink")
        {

            if (GhostPink.GetComponent<GhostManager>().Frightened != 0)
            {
                AudioController.PlaySound("GhostDeath");
                GhostPink.GetComponent<GhostManager>().IsDead = true;
                PinkDeaths++;
                GhostsKilled++;
                KillGhost();
            }

            else GameController.LoseLife();

        }

        else if (col.gameObject.tag == "Orange")
        {

            if (GhostOrange.GetComponent<GhostManager>().Frightened != 0)
            {
                AudioController.PlaySound("GhostDeath");
                GhostOrange.GetComponent<GhostManager>().IsDead = true;
                OrangeDeaths++;
                GhostsKilled++;
                KillGhost();
            }

            else GameController.LoseLife();

        }

        else if (col.gameObject.tag == "Blue")
        {

            if (GhostBlue.GetComponent<GhostManager>().Frightened != 0)
            {
                AudioController.PlaySound("GhostDeath");
                GhostBlue.GetComponent<GhostManager>().IsDead = true;
                BlueDeaths++;
                GhostsKilled++;
                KillGhost();
            }

            else GameController.LoseLife();

        }

        else if (col.gameObject.tag == "Fruit") {

            Destroy(col.gameObject);
            AudioController.PlaySound("Fruit");
            GameController.Score += 100;

        }

    }

    void KillGhost() {

        if (GhostsKilled > 4) GhostsKilled = 4;
        switch (GhostsKilled) {

            case 1: GameController.Score += 200;
                break;
            case 2: GameController.Score += 400;
                break;
            case 3: GameController.Score += 800;
                break;
            case 4: GameController.Score += 1600;
                break;
            default: break;

        }

    }

}
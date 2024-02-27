using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{

    public AudioController AudioController;
    public GameObject GhostRed;
    public GameObject GhostPink;
    public GameObject GhostOrange;
    public GameObject GhostBlue;

    bool FrightenedNew;
    bool FrightenedOld;

    void Update()
    {

        if (GhostRed.GetComponent<GhostManager>().Frightened != 0 || GhostPink.GetComponent<GhostManager>().Frightened != 0 || GhostOrange.GetComponent<GhostManager>().Frightened != 0 || GhostBlue.GetComponent<GhostManager>().Frightened != 0)
        {

            FrightenedNew = true;

        }

        else FrightenedNew = false;

        if (FrightenedNew && !FrightenedOld) {

            AudioController.StopSound("Ambient1");
            AudioController.PlaySound("Ambient2");

        }

        else if (FrightenedOld && !FrightenedNew)   //niso vec frightened
        {  

            AudioController.StopSound("Ambient2");
            AudioController.PlaySound("Ambient1");

        }

        FrightenedOld = FrightenedNew;

    }
}
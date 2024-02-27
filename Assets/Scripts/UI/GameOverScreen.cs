using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{

    public GameObject Background;
    public Canvas Canvas;
    public GameController GameController;
    public AudioController AudioController;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;

    void Start()
    {

        Background.GetComponent<SpriteRenderer>().enabled = false;
        Canvas.enabled = false;

    }

    public void OpenGameOverScreen() {

        Background.GetComponent<SpriteRenderer>().enabled = true;
        Canvas.enabled = true;
        AudioController.StopSound("Ambient1");
        AudioController.StopSound("Ambient2");
        Score.text = "Score: " + GameController.Score;
        HighScore.text = "High Score: " + GameController.HighScore;

    }

}
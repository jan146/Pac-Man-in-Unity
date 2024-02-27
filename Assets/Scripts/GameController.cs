using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Text ScoreText;
    public Text LivesText;

    public GameObject Player1;
    public GameObject Player2;
    public AudioController AudioController;
    public GameObject GhostRed;
    public GameObject GhostPink;
    public GameObject GhostOrange;
    public GameObject GhostBlue;

    public static int Score = 0;
    public static int HighScore = 0;
    public static int Lives = 3;
    public static int TotalPellets;
    public static int CurrentPellets = 0;
    public static int WorldWidth;
    public bool Freeze = false;
    public bool GameOver = false;
    Vector3 SpawnPoint1; Vector3 SpawnPoint2;

    void Start()
    {

        TotalPellets = GameObject.FindGameObjectsWithTag("Pellet").Length + GameObject.FindGameObjectsWithTag("BigPellet").Length;
        Freeze = true;
        AudioController.PlaySound("Start");
        SpawnPoint1 = Player1.transform.position;
        if (OptionsController.Multiplayer) SpawnPoint2 = Player2.transform.position;
        SetWorldWidth();
        Invoke("Begin", 4.3f);

    }

    void Update()
    {
        ScoreText.text = "Score: " + Score.ToString();
        LivesText.text = "Lives: " + Lives.ToString();
    }

    public void Restart() {

        GameOver = false;
        Freeze = false;

        Lives = 3;
        Score = 0;
        CurrentPellets = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void LoadMainMenu() {

        GameOver = false;
        Freeze = false;

        Lives = 3;
        Score = 0;

        SceneManager.LoadScene("MainMenu");

    }

    void Begin() {

        Freeze = false;
        AudioController.PlaySound("Ambient1");

    }

    public void LoseLife() {

        if (Lives - 1 < 1)
        {

            Lives = 0;
            if (Score > HighScore) HighScore = Score;
            Freeze = true;
            GameOver = true;    // pol klici Restart
            AudioController.PlaySound("PlayerDeath");
            FindObjectOfType<GameOverScreen>().OpenGameOverScreen();

        }

        else
        {
            Freeze = true;
            AudioController.PlaySound("PlayerDeath");
            Invoke("Respawn", 1);

        }

    }

    void Respawn() {

        ResetPositions();
        Lives--;
        Invoke("StartAgain", 1);

    }

    void StartAgain() {

        GhostRed.GetComponent<GhostManager>().TurnAlive();
        GhostPink.GetComponent<GhostManager>().TurnAlive();
        GhostOrange.GetComponent<GhostManager>().TurnAlive();
        GhostBlue.GetComponent<GhostManager>().TurnAlive();
        GhostRed.GetComponent<GhostManager>().IsDead = GhostPink.GetComponent<GhostManager>().IsDead = GhostOrange.GetComponent<GhostManager>().IsDead = GhostBlue.GetComponent<GhostManager>().IsDead = false;

        CurrentPellets = 0;
        Freeze = false;

    }

    public void GameWin() {

        Freeze = true;
        Invoke("NextLevel", 1);

    }

    void NextLevel() {

        ResetPositions();
        Invoke("ResetScene", 1);

    }

    void ResetPositions() {

        Player1.transform.position = SpawnPoint1;
        if (OptionsController.Multiplayer) Player2.transform.position = SpawnPoint2;

        FindObjectOfType<Movement>().ResetDestination();
        GhostRed.GetComponent<GhostManager>().Frightened = GhostPink.GetComponent<GhostManager>().Frightened = GhostOrange.GetComponent<GhostManager>().Frightened = GhostBlue.GetComponent<GhostManager>().Frightened = 0;
        GhostRed.GetComponent<GhostManager>().IsDead = GhostPink.GetComponent<GhostManager>().IsDead = GhostOrange.GetComponent<GhostManager>().IsDead = GhostBlue.GetComponent<GhostManager>().IsDead = true;

    }

    void ResetScene() {

        StartAgain();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void SetWorldWidth() {

        switch (OptionsController.MapSize) {

            case 0: WorldWidth = 13;
                break;
            case 1: WorldWidth = 14;
                break;
            case 2: WorldWidth = 29;
                break;

        }

    }

}
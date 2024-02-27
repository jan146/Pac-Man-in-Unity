using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{

    public static int Difficulty = 1;           // easy = 0; normal = 1; hard = 2;
    public static float Volume = 0.5f;          // glasnost zvoka, od 0 do 1
    public static bool Multiplayer = false;     // true = 2-player false = 1-player
    public static int MapSize = 1;              // small = 0; medium = 1; large = 2;
    public Slider Slider;

    void Start() {

        if (Slider != null) Slider.value = Volume;

    }

    public void SetDifficulty(int value) {

        Difficulty = value;

    }

    public void SetVolume(float value) {

        Volume = value;

    }

    public void SetMapSize(int value) {

        MapSize = value;

    }

    public void SetMode(bool value) {

        Multiplayer = value;

    }

    public void GoBack() {

        SceneManager.LoadScene("MainMenu");

    }

}
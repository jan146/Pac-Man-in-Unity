using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void StartGame() {

        if (!OptionsController.Multiplayer)
        {

            if (OptionsController.MapSize == 0)
                SceneManager.LoadScene("Small1");
            if (OptionsController.MapSize == 1)
                SceneManager.LoadScene("Medium1");
            if (OptionsController.MapSize == 2)
                SceneManager.LoadScene("Large1");

        }
        else {

            if (OptionsController.MapSize == 0)
                SceneManager.LoadScene("Small2");
            if (OptionsController.MapSize == 1)
                SceneManager.LoadScene("Medium2");
            if (OptionsController.MapSize == 2)
                SceneManager.LoadScene("Large2");

        }

    }

    public void ExitGame() {

        Application.Quit();

    }

    public void OpenOptions() {

        SceneManager.LoadScene("OptionsMenu");

    }

    public void MainMenu() {

        SceneManager.LoadScene("MainMenu");

    }

}
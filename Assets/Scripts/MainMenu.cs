using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayGame() {
        SceneManager.LoadScene("Level");
    }

    public void Daily()
    {
        SceneManager.LoadScene("DailyRewards");
    }

    public void Cheat()
    {
        SceneManager.LoadScene("Level cheat");
    }

    public void QuitGame() {
        Application.Quit();
    }
}

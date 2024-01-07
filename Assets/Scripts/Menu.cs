using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
public void PlayGame()
    {
        SceneManager.LoadScene("Character Shop");
    }
public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}

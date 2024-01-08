using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{

    public GameObject Camera1;
    public GameObject Camera2;

    public GameObject character1;
    public GameObject character2;


public void Character1()
    {
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        character1.SetActive(true);
        character2.SetActive(false);
    }
    public void Character2()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        character1.SetActive(false);
        character2.SetActive(true);    
    }
public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

}

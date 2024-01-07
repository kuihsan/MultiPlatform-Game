using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
public void Character1()
    {
        SceneManager.LoadScene("Character 1");
    }
public void Character2()
    {
        SceneManager.LoadScene("Character 2");
    }
public void Character3()
    {
        SceneManager.LoadScene("Character 3");
    }
public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

}

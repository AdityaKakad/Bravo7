using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text Instructions;
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ShowInstructions()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Instructions");
    }
    public void BackToMain()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

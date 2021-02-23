using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
}

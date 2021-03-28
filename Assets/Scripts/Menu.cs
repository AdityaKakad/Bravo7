using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text Instructions;
    public Text highScoreText;
    public Text playerName;
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ShowInstructions()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Instructions");
    }

    public void ShowLeaderboard()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Leaderboard");
    }

    public void ShowSettings()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Settings");
    }

    public void BackToMain()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    private void Awake() 
    {
        Text high = GameObject.Find("HighScoreCard").GetComponent<Text>();
        string asd = PlayerPrefs.GetInt("HighScore", 0).ToString();
        string name = PlayerPrefs.GetString("PlayerName", "Anonymous").ToString();
        high.text = "High Score: " + asd;
        playerName.text = "Name: " + name;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

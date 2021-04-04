using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text highScoreText;
    public Text playerName;
    public Text totalCoins;
    public AudioSource source;

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

    public void ShowStore()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Store");
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
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        float audioVal = PlayerPrefs.GetFloat("AudioValue", 0.5f);
        if (source != null)
        {
            source.volume = audioVal;
        }
        high.text = "High Score: " + asd;
        playerName.text = "Name: " + name;
        totalCoins.text = "Total Coins: " + coins.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

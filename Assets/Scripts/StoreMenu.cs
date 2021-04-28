using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public Text totalCoins;
    public Text highScore;
    public AudioSource audioSrc;

    public void BackToMain()
    {
        SceneManager.LoadScene("Menu");
    }

    private void Awake()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins.text = "Total Coins: " + coins.ToString();
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        highScore.text = "High Score: " + highscore.ToString();
        float volume = PlayerPrefs.GetFloat("SFXValue", 0.5f);
        audioSrc.volume = volume;
    }

    private void Update()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins.text = "Total Coins: " + coins.ToString();
    }
}

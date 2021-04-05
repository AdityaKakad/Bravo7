using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public Text totalCoins;
    public Text highScore;

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
    }

    private void Update()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins.text = "Total Coins: " + coins.ToString();
    }
}

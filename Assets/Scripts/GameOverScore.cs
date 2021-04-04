using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    public Text finalScoreText;
    public Text highScoreText;

    private void Awake()
    {
        //final score = score + 2*mask + 2*syringes
        int score = PlayerPrefs.GetInt("CurrentScore", 0) + 2*PlayerPrefs.GetInt("CurrentMaskCount", 0) + 2*PlayerPrefs.GetInt("CurrentSyringeCount", 0);
        finalScoreText.text = "Final Score: " + score + "!";
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        int coinsCollected = PlayerPrefs.GetInt("CurrentCoins", 0);
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins += coinsCollected;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);

        if(highScore <= score) {
            highScore = score; 
            PlayerPrefs.SetInt("HighScore", score);
        }
        highScoreText.text = "High Score: " + highScore + "!";
        string name = PlayerPrefs.GetString("PlayerName", "Anonymous").ToString();
        if(name != "Anonymous")
        {
            Highscores.AddNewHighscore(name, score);
        }
    }
}

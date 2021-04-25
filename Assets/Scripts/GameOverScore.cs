using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    public Text finalScoreText;
    public Text highScoreText;
    public Text gameOverText;

    private void Awake()
    {
        //final score = score + 2*mask + 2*syringes
        int score = PlayerPrefs.GetInt("CurrentScore", 0) + 2*PlayerPrefs.GetInt("CurrentMaskCount", 0) + 2*PlayerPrefs.GetInt("CurrentSyringeCount", 0);
        finalScoreText.text = "Final Score: " + score + "!";
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        string gameOver = "You have been infected! Game Over!";
        int coinsCollected = PlayerPrefs.GetInt("CurrentCoins", 0);
        int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        totalCoins += coinsCollected;

        if (score - highScore >= 50)
        {
            gameOver += "\nYou made progress! +20 coins!";
            totalCoins += 20;
        }

        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        gameOverText.text = gameOver;

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

        // Daily tasks logic
        int virusKilled = PlayerPrefs.GetInt("VirusKilled", 0);
        int peopleSaved = PlayerPrefs.GetInt("PeopleSaved", 0);
        string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
        string lastLogin = PlayerPrefs.GetString("LastLogin", DateTime.MinValue.ToString("dd-MM-yyyy"));
        if (currentDate.Equals(lastLogin))
        {
            int totalVirusKilled = PlayerPrefs.GetInt("TotalVirusKilled", 0);
            int totalPeopleSaved = PlayerPrefs.GetInt("TotalPeopleSaved", 0);
            int totalGames = PlayerPrefs.GetInt("TotalGames", 0);
            if(score >= 100) totalGames++;   // hardcoded, change later
            totalVirusKilled += virusKilled;
            totalPeopleSaved += peopleSaved;

            PlayerPrefs.SetInt("TotalVirusKilled", totalVirusKilled);
            PlayerPrefs.SetInt("TotalPeopleSaved", totalPeopleSaved);
            PlayerPrefs.SetInt("TotalGames", totalGames);
        } else
        {
            PlayerPrefs.SetInt("TotalVirusKilled", virusKilled);
            PlayerPrefs.SetInt("TotalPeopleSaved", peopleSaved);
            PlayerPrefs.SetInt("TotalGames", 1);
            PlayerPrefs.SetString("LastLogin", DateTime.Today.ToString("dd-MM-yyyy"));
        }
    }
}

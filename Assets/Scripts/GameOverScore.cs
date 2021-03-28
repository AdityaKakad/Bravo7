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
        int score = PlayerPrefs.GetInt("CurrentScore", 0);
        finalScoreText.text = "Final Score: " + score + "!";
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
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

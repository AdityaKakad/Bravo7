using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScore : MonoBehaviour
{
    public Text finalScoreText;
    public Text highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }
}

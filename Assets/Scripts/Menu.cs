using System;
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
    public Text dailyLogin;
    public AudioSource source;
    public Canvas dailyTasks;
    public Canvas mainMenu;
    public DateTime loginBonusFlashStamp;

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

        //daily and weekly login bonuses
        long lastLoginStamp = Convert.ToInt64(PlayerPrefs.GetString("dailyBonusTimestamp", "-1"));
        int dayCount = PlayerPrefs.GetInt("dailyLoginCount", 0);
        if (lastLoginStamp == -1) {
            coins++;
            PlayerPrefs.SetInt("TotalCoins", coins);
            dailyLogin.text = "Login Bonus: +1";
            loginBonusFlashStamp = DateTime.Now.AddSeconds(5);
            dayCount++;
            PlayerPrefs.SetInt("dailyLoginCount", dayCount);
        }
        else {
            DateTime oldDate = DateTime.FromBinary(lastLoginStamp);
            DateTime currentDate = System.DateTime.Now;
            TimeSpan difference = currentDate.Subtract(oldDate);
            if (difference.Hours > 24) {
                if (difference.Hours <= 48) {
                    dayCount++;
                }
                else {
                    dayCount = 0;
                }
                if (dayCount == 7) {
                    coins += 10;
                    dayCount = 0;
                    dailyLogin.text = "Weekly Login Bonus: +10";
                }
                else {
                    coins++;
                    dailyLogin.text = "Daily Login Bonus: +1";
                }
                PlayerPrefs.SetInt("dailyLoginCount", dayCount);
                PlayerPrefs.SetInt("TotalCoins", coins);
                loginBonusFlashStamp = DateTime.Now.AddSeconds(5);
            }
        }
        PlayerPrefs.SetString("dailyBonusTimestamp", System.DateTime.Now.ToBinary().ToString());
        //string lastLogin = PlayerPrefs.GetString("LastLogin", DateTime.Today.ToString("dd-MM-yyyy"));
        // daily login reward logic
        // update consecutive days counter

    }

    private void Update()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins.text = "Total Coins: " + coins.ToString();
        if (loginBonusFlashStamp < DateTime.Now) {
            dailyLogin.text = "";
        }
    }

    private void Start()
    {
        mainMenu.gameObject.SetActive(true);
        dailyTasks.gameObject.SetActive(false);
        string lastLogin = PlayerPrefs.GetString("LastLogin", DateTime.Today.ToString("dd-MM-yyyy"));
        string now = DateTime.Today.ToString("dd-MM-yyyy");
        if (!lastLogin.Equals(now))
        {
            PlayerPrefs.SetInt("TotalVirusKilled", 0);
            PlayerPrefs.SetInt("TotalPeopleSaved", 0);
            PlayerPrefs.SetInt("TotalGames", 0);
        }
        PlayerPrefs.SetString("LastLogin", DateTime.Today.ToString("dd-MM-yyyy"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ShowDailyTasks()
    {
        dailyTasks.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }
}

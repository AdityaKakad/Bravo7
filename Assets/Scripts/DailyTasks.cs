using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyTasks : MonoBehaviour
{
    public Canvas dailyTasks;
    public Canvas mainMenu;
    public Text coinsText;

    public void BackToMain()
    {
        mainMenu.gameObject.SetActive(true);
        dailyTasks.gameObject.SetActive(false);
    }

    public void Update()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        coinsText.text = "Total Coins: " + coins.ToString();
    }
}

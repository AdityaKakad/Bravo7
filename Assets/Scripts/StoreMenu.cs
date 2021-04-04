using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    public Text totalCoins;

    public void BackToMain()
    {
        SceneManager.LoadScene("Menu");
    }

    private void Awake()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoins.text = "Total Coins: " + coins.ToString();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public Text status;
    public string key;
    public int taskLimit;
    public int coinsReward;
    public Button collectBtn;
    private bool isCollected;

    private void Awake()
    {
        string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
        string lastLogin = PlayerPrefs.GetString("LastLogin", DateTime.MinValue.ToString("dd-MM-yyyy"));
        int value = PlayerPrefs.GetInt(key, 0);
        isCollected = bool.Parse(PlayerPrefs.GetString(key + "Collected", "False"));
        if (lastLogin.Equals(currentDate) && value >= taskLimit)
        {
            status.text = "COMPLETE";
            //collectBtn.gameObject.SetActive(true);
            collectBtn.GetComponent<Button>().interactable = true;
        } else {
            status.text = "INCOMPLETE";
            //collectBtn.gameObject.SetActive(false);
            collectBtn.GetComponent<Button>().interactable = false;
            if (!lastLogin.Equals(currentDate))
            {
                PlayerPrefs.SetString("LastLogin", DateTime.Today.ToString("dd-MM-yyyy"));
                PlayerPrefs.SetInt(key, 0);
            }

            PlayerPrefs.SetString(key + "Collected", "False");
        }

        if(isCollected) collectBtn.GetComponent<Button>().interactable = false;
    }

    public void CollectCoins()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        coins += coinsReward;
        PlayerPrefs.SetInt("TotalCoins", coins);
        collectBtn.GetComponent<Button>().interactable = false;
        isCollected = true;
        PlayerPrefs.SetString(key + "Collected", "True");
        //collectBtn.gameObject.SetActive(false);
    }
}

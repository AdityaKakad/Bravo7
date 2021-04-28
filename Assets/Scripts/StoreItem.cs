using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;


public class StoreItem : MonoBehaviour
{
    public string key;
    public int cost;
    int own = 0;
    public Text ownText;
    public Text costText;
    public Button applyBtn;
    public Button buyBtn;
    bool isApplied = false;
    public string hoverText;
    public Text descriptionText;
    public int scoreUnlockLimit;
    public AudioSource audioSrc;
    public AudioClip applyClip;
    public AudioClip buyClip;

    private void Awake()
    {
        own = PlayerPrefs.GetInt(key+"Own", 0);
        isApplied = bool.Parse(PlayerPrefs.GetString(key + "Applied", "False")); 
        descriptionText.text = hoverText;
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScore>=scoreUnlockLimit)
        {
            ownText.text = "Own: " + own.ToString();
            costText.text = "Cost: " + cost.ToString();
            buyBtn.GetComponent<Button>().interactable = true;
            applyBtn.GetComponent<Button>().interactable = true;
            
        } else
        {
            ownText.text = "Unlock at";
            costText.text = "score: "+scoreUnlockLimit;
            buyBtn.GetComponent<Button>().interactable = false;
            applyBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void ToggleApply()
    {
        if (own <= 0 && !isApplied) return;
        if (audioSrc != null && applyClip != null)
        {
            audioSrc.clip = applyClip;
            audioSrc.Play();
        }
        isApplied = !isApplied;
        PlayerPrefs.SetString(key + "Applied", isApplied.ToString());
        if (isApplied)
        {
            applyBtn.GetComponentInChildren<Text>().text = "Remove";
            own--;
        } else
        {
            applyBtn.GetComponentInChildren<Text>().text = "Apply";
            own++;
        }
        ownText.text = "Own: " + own.ToString();
        PlayerPrefs.SetInt(key + "Own", own);
        print("Applied: " + isApplied);
        print("Own: " + own);
    }

    public void Buy()
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coins >= cost)
        {
            if (audioSrc != null && buyClip != null) {
                audioSrc.clip = buyClip;
                audioSrc.Play();
            }
            coins -= cost;
            own++;
            PlayerPrefs.SetInt(key + "Own", own);
            PlayerPrefs.SetInt("TotalCoins", coins);
            ownText.text = "Own: " + own.ToString();
            Analytics.CustomEvent("Store item bought", new Dictionary<string, object>
            {
                { key, 1} 
            });
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;


public class SettingsMenu : MonoBehaviour
{
    public InputField playerName;
    public Slider audioSlider;
    public Slider sensitivitySlider;
    public Slider sfxSlider;
    public ScoreSheet scoreMgr;

    public void BackToMain()
    {
        if (playerName.text != "Anonymous")
        {
            string prevName = PlayerPrefs.GetString("PlayerName", "Anonymous");
            if(playerName.text != prevName)
            {
                int score = 0;
                if (ScoreSheet.highscores.ContainsKey(playerName.text))
                    score = ScoreSheet.highscores[playerName.text].score;
                PlayerPrefs.SetInt("HighScore", score);
            }
            PlayerPrefs.SetString("PlayerName", playerName.text);
        }

        PlayerPrefs.SetFloat("AudioValue", audioSlider.value);
        PlayerPrefs.SetFloat("SFXValue", sfxSlider.value);
        PlayerPrefs.SetFloat("SensitivityValue", sensitivitySlider.value);
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Menu");
        Analytics.CustomEvent("Audio value", new Dictionary<string, object>
        {
            { "AudioValue", audioSlider.value }
        });
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("AudioValue", audioSlider.value);
        PlayerPrefs.SetFloat("SFXValue", sfxSlider.value);
    }

    private void Awake()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Anonymous").ToString();
        float audioVal = PlayerPrefs.GetFloat("AudioValue", 0.5f);
        float sensitivityVal = PlayerPrefs.GetFloat("SensitivityValue", 1.7f);
        float sfxVal = PlayerPrefs.GetFloat("SFXValue", 0.5f);
        audioSlider.value = audioVal;
        sensitivitySlider.value = sensitivityVal;
        sfxSlider.value = sfxVal;
        playerName.text = name;
    }

}

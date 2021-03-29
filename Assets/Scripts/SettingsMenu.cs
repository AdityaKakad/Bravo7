using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public InputField playerName;
    public Slider audioSlider;

    public void BackToMain()
    {
        if(playerName.text != "Anonymous")
            PlayerPrefs.SetString("PlayerName", playerName.text);

        PlayerPrefs.SetFloat("AudioValue", audioSlider.value);
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    private void Awake()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Anonymous").ToString();
        float audioVal = PlayerPrefs.GetFloat("AudioValue", 0.5f);
        audioSlider.value = audioVal;
        playerName.text = name;
    }

}

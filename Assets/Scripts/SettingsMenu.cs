using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public InputField playerName;

    public void BackToMain()
    {
        if(playerName.text != "Anonymous")
            PlayerPrefs.SetString("PlayerName", playerName.text);
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    private void Awake()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Anonymous").ToString();
        playerName.text = name;
    }

}

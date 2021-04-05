using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public AudioSource source;
    public string[] keys = { };
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }

    private void Awake()
    {
        float audioVal = PlayerPrefs.GetFloat("AudioValue", 0.5f);
        if (source != null)
        {
            source.volume = audioVal;
        }

        foreach(string key in keys){
            PlayerPrefs.SetString(key + "Applied", "False");
        }

    }
}

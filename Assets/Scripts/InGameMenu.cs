using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public MovePlayer playerMovement;
    public AudioSource audioSource;
    public AudioSource fxSource;
    private static float audioVal;
    private static float fxVal;
    public Button muteBtn;
    public Button muteFXBtn;

    // Start is called before the first frame update
    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }

    // Update is called once per frame
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }

    public void EndGame()
    {
        GameManager.inst.DecrementLives();
        while (GameManager.inst.DecrementLives(true)) { }
        Time.timeScale = 1;
        playerMovement.Die();
    }

    public void ToggleAudio()
    {
        if (audioSource.volume > 0)
        {
            Time.timeScale = 1;
            audioVal = audioSource.volume;
            audioSource.volume = 0f;
            Time.timeScale = 0;
            muteBtn.GetComponentInChildren<Text>().text = "Unmute Audio";
        } 
        else
        {
            audioSource.volume = audioVal;
            muteBtn.GetComponentInChildren<Text>().text = "Mute Audio";
        }
    }

    public void ToggleFX()
    {
        if (fxSource.volume > 0)
        {
            fxVal = fxSource.volume;
            fxSource.volume = 0f;
            muteFXBtn.GetComponentInChildren<Text>().text = "Unmute FX Audio";
        }
        else
        {
            fxSource.volume = fxVal;
            muteFXBtn.GetComponentInChildren<Text>().text = "Mute FX Audio";
        }
    }
}

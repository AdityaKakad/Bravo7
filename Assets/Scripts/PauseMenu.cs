using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    void Start()
    {
        pausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }
    }
    private void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        //Disable scripts that still work while timescale is set to 0
    }

    private void ContinueGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        //enable the scripts again
    }
}

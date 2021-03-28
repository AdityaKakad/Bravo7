using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardMenu : MonoBehaviour
{
    public void BackToMain()
    {
        //Instructions.gameObject.SetActive(true);
        SceneManager.LoadScene("Menu");
    }
}

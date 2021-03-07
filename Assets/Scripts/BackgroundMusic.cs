using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic background;

    void Awake()
    {
        if (background == null)
        {
            background = this;
            DontDestroyOnLoad(background);
        }
        else
        {
            Destroy(gameObject);
        }

    }



}

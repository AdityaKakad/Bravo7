using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic background;
    public AudioSource source;

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

        float audioVal = PlayerPrefs.GetFloat("AudioValue", 0.5f);
        if (source != null)
        {
            source.volume = audioVal;
        }
    }



}

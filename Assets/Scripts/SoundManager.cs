using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    public Image soundOnIcon;
    public Image soundOffIcon;
    private bool muted = false;
    // Start is called before the first frame update

    public void start()
    {
        soundOffIcon.enabled = false;
        soundOnIcon.enabled = true;

    }

    public void onButtonPress()
    {
        if (muted == false)
        {

            muted = true;
            AudioListener.pause = true;
            UpdateButtonIcon();

        }
        else
        {

            muted = false;
            AudioListener.pause = false;
            UpdateButtonIcon();
        }


    }



    public void UpdateButtonIcon()
    {
        if (muted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;

        }

        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;

        }

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MysteryBox : MonoBehaviour
{
    public float turnSpeed = 90f;
    public static string[] powerUp = { "+5 masks", "+5 syringes", "+30 points", "Role switch!", "Oh no! -20 points!", 
        "Superman Drive", "Vaccinated! +1 life!", "Oh no! -5 masks!", "Oh no! -5 syringes!", "No Jump!", "5x Coins!"};
    public string[] paidItems = { };
    public AudioClip effectClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Virus>() != null ||
            other.gameObject.GetComponent<Bat>() != null ||
            other.gameObject.GetComponent<People>() != null ||
            other.gameObject.GetComponent<Arch>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // check the object we collide with is the player
        if (other.gameObject.name != "Player") return;

        GameManager.inst.SetMysteryBoxStamp();
        GameManager.inst.audioSrc.clip = effectClip;
        GameManager.inst.audioSrc.Play();

        // Logic for store bought power up
        bool coinMultiplierApplied = bool.Parse(PlayerPrefs.GetString("CoinMultiplier" + "Applied", "False"));
        float upper = 10f;
        if (coinMultiplierApplied)
            upper = 11f;

        float random = Random.Range(0f, upper);
        int idx = (int) Mathf.Floor(random);

        //alter mystery box probability based on in-game situation
        if (GameManager.inst.livesLeft == 1) { 
            //if only one life left, increase probability of superman drive and vaccination
            if (idx != 5 && idx != 6) {
                if (random-idx >= 0.8) {
                    idx = 6;
                }
                else if (random-idx >= 0.7) {
                    idx = 5;
                }
            }
        }
        else if (!GameManager.inst.isDoctor && GameManager.inst.maskCount+GameManager.inst.syringeCount >= 20) { 
            //increase doctor mode probability if mask and syringe count is high
            if (idx != 3) {
                if (random-idx >= 0.75) {
                    idx = 3;
                }
            }
        }
        else if (!GameManager.inst.isDoctor && GameManager.inst.maskCount+GameManager.inst.syringeCount < 10) { 
            //increase +5 masks/syringe prob if low masks/syringes
            if (idx != 0 && idx != 1) {
                if (random-idx >= 0.9) {
                    idx = 1;
                }
                else if (random-idx >= 0.8) {
                    idx = 0;
                }
            }
        }
        else if (GameManager.inst.isDoctor) { 
            //increase +5 masks/syringe prob & reduce role change prob when in doc mode.
            if (idx != 0 && idx != 1) {
                if (random-idx >= 0.9 || (idx == 3 && random-idx >=0.75 && random-idx < 0.8)) {
                    idx = 1;
                }
                else if (random-idx >= 0.8 || (idx == 3 && random-idx >=0.7)) {
                    idx = 0;
                }
            }
        }

        
        idx = MysteryBoxLogic(idx, coinMultiplierApplied);
        string power = powerUp[idx];

        if (other.gameObject.GetComponent<MovePlayer>() != null)
        {
            other.gameObject.GetComponent<MovePlayer>().ChangeColor();
        }

        // Calculate effect and apply changes
        GameManager.inst.SetMessage("Mystery Box picked! \n "+ power);

        // Destroy the mask object
        Destroy(gameObject);
    }

    private int MysteryBoxLogic(int choice, bool apply)
    {
        //Mystery logic - Secret Sauce
        switch (choice)
        {
            case 0:
                GameManager.inst.IncrementMask(5);
                break;
            case 1:
                GameManager.inst.IncrementSyringe(5);
                break;
            case 2:
                GameManager.inst.IncrementScore(30);
                break;
            case 3:
                if(GameManager.inst.maskCount>0 && GameManager.inst.syringeCount>0) // human -> doctor 0M & 0SY
                    GameManager.inst.ChangeRole();
                break;
            case 4:
                GameManager.inst.DecrementScore(20);
                break;
            case 5:
                GameManager.inst.SetSuperManStamp();
                break;
            case 6:
                GameManager.inst.IncrementLives();
                break;
            case 7:
                GameManager.inst.DecrementMask(5);
                break;
            case 8:
                GameManager.inst.DecrementSyringe(5);
                break;
            case 9:
                if(GameManager.inst.score > 1000)
                {
                    float random = Random.Range(0f, 25f);
                    if (random < 2f)
                    {
                        GameManager.inst.SetNoJumpStamp();
                    }
                    else
                    {
                        GameManager.inst.SetSuperManStamp();
                        return 5;
                    }
                }
                else
                    GameManager.inst.SetNoJumpStamp();
                break;
            case 10:
                if(apply)
                    GameManager.inst.SetCoinMultiplierStamp();
                break;
        }
        return choice;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}

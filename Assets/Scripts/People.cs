using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{
    MovePlayer playerMovement;
    public AudioClip savedClip;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<MovePlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.inst.GetSuperManStamp() < DateTime.Now)
        {
            if (collision.gameObject.name == "Player")
            {
                // kill the player
                if (!GameManager.inst.isDoctor)
                {
                    if (!GameManager.inst.DecrementLives())
                        playerMovement.Die();
                }
                else
                {
                    if (!GameManager.inst.DecrementMask())
                    {
                        GameManager.inst.ChangeRole();

                        if (collision.gameObject.GetComponent<MovePlayer>() != null)
                        {
                            collision.gameObject.GetComponent<MovePlayer>().ChangeColor();
                        }
                    }
                    else
                    {
                        GameManager.inst.IncrementScore(GameManager.inst.DOCTOR_POWER_POINT);
                        GameManager.inst.audioSrc.clip = savedClip;
                        GameManager.inst.audioSrc.Play();
                        GameManager.inst.peopleSaved++;
                    }
                }

            }
        } else
        {
            if (GameManager.inst.isDoctor)
            {
                GameManager.inst.IncrementScore(GameManager.inst.DOCTOR_POWER_POINT);
                GameManager.inst.audioSrc.clip = savedClip;
                GameManager.inst.audioSrc.Play();
                GameManager.inst.peopleSaved++;
            }
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

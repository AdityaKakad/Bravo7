using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    MovePlayer playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<MovePlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.inst.GetSuperManStamp() < DateTime.Now)
        {
            // kill the player
            if (!GameManager.inst.isDoctor)
            {
                if (!GameManager.inst.DecrementLives())
                    playerMovement.Die();
            }
            else
            {
                if (!GameManager.inst.DecrementSyringe())
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
                    GameManager.inst.virusKilled++;
                }
            }
        } else
        {
            if (GameManager.inst.isDoctor)
            {
                GameManager.inst.IncrementScore(GameManager.inst.DOCTOR_POWER_POINT);
                GameManager.inst.virusKilled++;
            }
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

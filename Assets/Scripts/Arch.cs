using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arch : MonoBehaviour
{
    MovePlayer playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<MovePlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GameManager.inst.GetSuperManStamp() < DateTime.Now)
        {
            if(collision.gameObject.name == "Player")
            {
                if (!GameManager.inst.DecrementLives())
                    playerMovement.Die();
            }
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

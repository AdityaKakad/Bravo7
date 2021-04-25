using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Virus>() != null ||
            other.gameObject.GetComponent<Bat>() != null ||
            other.gameObject.GetComponent<People>() != null)
        {
            Destroy(gameObject);
            return;
        }
        // check the object we collide with is the player
        if (other.gameObject.name != "Player") return;

        // Add to the player's score
        if (GameManager.inst.GetCoinMultiplierStamp() < DateTime.Now)
        {
            GameManager.inst.IncrementScore();
            GameManager.inst.coinsCollectedPerGame++;
        } 
        else
        {
            GameManager.inst.IncrementScore(5);
            GameManager.inst.coinsCollectedPerGame+=5;
        }

        // Destroy the mask object
        Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}

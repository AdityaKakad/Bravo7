using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    public float turnSpeed = 90f;
    public string[] powerUp = { "+5 masks", "+5 syringes", "+100 points", "Role switch!", "Oh no! -50 points!", 
        "Superman Drive", "Vaccinated! +1 life!" };

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

        float random = Random.Range(0f, 7f);
        int idx = (int) Mathf.Floor(random);
        string power = powerUp[idx];

        MysteryBoxLogic(idx);

        if (other.gameObject.GetComponent<MovePlayer>() != null)
        {
            other.gameObject.GetComponent<MovePlayer>().ChangeColor();
        }

        // Calculate effect and apply changes
        GameManager.inst.SetMessage("Mystery Box picked! \n "+ power);

        // Destroy the mask object
        Destroy(gameObject);
    }

    private void MysteryBoxLogic(int choice)
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
                GameManager.inst.IncrementScore(100);
                break;
            case 3:
                if(GameManager.inst.maskCount>0 && GameManager.inst.syringeCount>0) // human -> doctor 0M & 0SY
                    GameManager.inst.ChangeRole();
                break;
            case 4:
                GameManager.inst.DecrementScore(50);
                break;
            case 5:
                GameManager.inst.SetSuperManStamp();
                break;
            case 6:
                GameManager.inst.IncrementLives();
                break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}

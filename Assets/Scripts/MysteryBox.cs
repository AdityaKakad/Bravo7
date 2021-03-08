using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour
{
    public float turnSpeed = 90f;
    public static string[] powerUp = { "+5 masks", "+5 syringes", "+30 points", "Role switch!", "Oh no! -20 points!", 
        "Superman Drive", "Vaccinated! +1 life!", "Oh no! -5 masks!", "Oh no! -5 syringes!"};

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

        float random = Random.Range(0f, 9f);
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
        }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}

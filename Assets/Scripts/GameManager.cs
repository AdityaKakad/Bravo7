using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int maskCount = 0;
    public int syringeCount = 0;
    public int livesLeft = 0;
    public bool isDoctor = false;
    public string role = "HUMAN";
    public static GameManager inst;
    public Text scoreText;
    public Text maskText;
    public Text syringeText;
    public Text roleText;
    public Text livesText;
    public Text guiText;
    public MovePlayer playerMovement;
    public DateTime superManEffectStamp = DateTime.MinValue;
    public int DOCTOR_POWER_POINT = 10;

    public void IncrementScore()
    {
        emptyMessage();
        score++;
        //TODO: change point divisor to 100
        playerMovement.speed = playerMovement.startSpeed + (score / 100) * playerMovement.speedIncreasePer100Points;
        scoreText.text = "Score: " + score;
    }

    public void IncrementScore(int value)
    {
        emptyMessage();
        score+= value;
        //TODO: change point divisor to 100
        playerMovement.speed = playerMovement.startSpeed + (score / 100) * playerMovement.speedIncreasePer100Points;
        scoreText.text = "Score: " + score;
    }

    public void DecrementScore(int value)
    {
        if (value < score)
        {
            score -= value;
        }
        else
        {
            score = 0;
        }
        scoreText.text = "Score: " + score;
    }

    public void SetSuperManStamp()
    {
        superManEffectStamp = DateTime.Now.AddSeconds(7);
    }

    public DateTime GetSuperManStamp()
    {
        return superManEffectStamp;
    }

    public void IncrementMask()
    {
        emptyMessage();
        maskCount++;
        
        maskText.text = "Masks: " + maskCount;
    }

    public void IncrementMask(int value)
    {
        emptyMessage();
        maskCount+=value;

        maskText.text = "Masks: " + maskCount;
    }

    public bool DecrementMask()
    {
        if (maskCount > 0)
            maskCount--;
        else
            return false;

        maskText.text = "Masks: " + maskCount;

        if (maskCount > 0) return true;
        else return false;
    }

    public void DecrementMask(int value)
    {
        emptyMessage();
        if (value < maskCount)
        {
            maskCount -= value;
        }
        else
        {
            maskCount = 0;
            if (isDoctor) {
                ChangeRole();
            }
        }

        maskText.text = "Masks: " + maskCount;
    }

    public void IncrementSyringe()
    {
        emptyMessage();
        syringeCount++;

        syringeText.text = "Syringes: " + syringeCount;
    }

    public void IncrementSyringe(int value)
    {
        emptyMessage();
        syringeCount+=value;

        syringeText.text = "Syringes: " + syringeCount;
    }

    public bool DecrementSyringe()
    {
        if (syringeCount > 0)
            syringeCount--;
        else
            return false;

        syringeText.text = "Syringes: " + syringeCount;
        if (syringeCount > 0) return true;
        else return false;
    }

    public void DecrementSyringe(int value)
    {
        emptyMessage();
        if (value < syringeCount)
        {
            syringeCount -= value;
        }
        else
        {
            syringeCount = 0;
            if (isDoctor) {
                ChangeRole();
            }
        }

        syringeText.text = "Syringes: " + syringeCount;
    }

    public void IncrementLives()
    {
        emptyMessage();
        if (livesLeft<3)
            livesLeft++;

        livesText.text = "Lives left: " + livesLeft;
    }

    public void emptyMessage()
    {
        guiText.text = "";
    }

    public bool DecrementLives()
    {
        if (livesLeft > 1)
            livesLeft--;
        else
            return false;

        livesText.text = "Lives left: " + livesLeft;
        return true;
    }

    public void ChangeRole()
    {
        isDoctor = !isDoctor;
        if (isDoctor)
        {
            role = "DOCTOR";
        } else
        {
            role = "HUMAN";
        }

        SetMessage("Role changed to "+role);
        roleText.text = "Role: " + role;
    }

    public void SetMessage(string message)
    {
        guiText.text = message;
    }

    private void Awake()
    {
        inst = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

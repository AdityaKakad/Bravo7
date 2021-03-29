using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System;

public class MovePlayer : MonoBehaviour
{
    bool alive = true;
    public float startSpeed = 5; //initially set to 8
    public float speed = 5; //initially set to 8
    public Rigidbody rb;
    public Renderer renderer;
    int JumpCount = 0;
    public int MaxJumps = 1; //Maximum amount of jumps (i.e. 2 for double jumps)

    float horizontalInput;
    public float horizontalMultiplier = 30;
    public float jumpForce = 7f;
    public float speedIncreasePer100Points = 0.5f; //initially set to 1f
    public LayerMask groundMask;

    private void Start()
    {
        GameManager.inst.gameStartTime = DateTime.Now;
        GameManager.inst.docTimeSeconds = 0;
        GameManager.inst.doctorModePoints = 0;
        GameManager.inst.cumulativeDocPoints = 0;
        GameManager.inst.cumulativeSupermanPoints = 0;
        GameManager.inst.supermanCount = 0;
        renderer = GetComponent<Renderer>();
        JumpCount = MaxJumps;
    }

    private void FixedUpdate()
    {
        if (!alive) return;
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpCount > 0)
            {
                Jump();
            }
        }

        if (transform.position.y < -5) { Die(); }
    }

    public void Die()
    {
        alive = false;
        Debug.Log("Game over. Show final score!");
        PlayerPrefs.SetInt("CurrentScore", GameManager.inst.score);
        PlayerPrefs.SetInt("CurrentMaskCount", GameManager.inst.maskCount);
        PlayerPrefs.SetInt("CurrentSyringeCount", GameManager.inst.syringeCount);

        DateTime endTime = DateTime.Now;
        int seconds = (int)System.Math.Abs((GameManager.inst.gameStartTime - endTime).TotalSeconds);
        Analytics.CustomEvent("Game Statistics", new Dictionary<string, object>
          {
            { "coins", GameManager.inst.coinsCollectedPerGame},
            { "sessionLength", seconds },
            { "total points", GameManager.inst.score},
            { "doctor time", GameManager.inst.docTimeSeconds },
            { "human time", seconds - GameManager.inst.docTimeSeconds },
            { "doctor points", GameManager.inst.cumulativeDocPoints },
            { "human points", GameManager.inst.score - GameManager.inst.cumulativeDocPoints },
            { "superman count", GameManager.inst.supermanCount },
            { "superman points", GameManager.inst.cumulativeSupermanPoints }
          });
        GameManager.inst.coinsCollectedPerGame = 0;
        SceneManager.LoadScene("EndGame");
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
        JumpCount -= 1;
    }

    public void ChangeColor()
    {
        if (GameManager.inst.isDoctor)
            renderer.material.SetColor("_Color", Color.black);
        else
            renderer.material.SetColor("_Color", Color.white);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Plane"))
        {
            JumpCount = MaxJumps;
        }
    }
}
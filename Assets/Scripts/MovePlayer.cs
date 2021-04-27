using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    bool alive = true;
    public float startSpeed = 5; //initially set to 8
    public float speed = 5; //initially set to 8
    public Rigidbody rb;
    public SkinnedMeshRenderer renderer;
    public GameObject docIcon;
    int JumpCount = 0;
    bool dblJumpApplied = false;
    public int MaxJumps = 1; //Maximum amount of jumps (i.e. 2 for double jumps)

    float horizontalInput;
    float verticalInput;
    public float horizontalMultiplier = 1.7f;
    public float jumpForce = 7f;
    public float speedIncreasePer100Points = 0.5f; //initially set to 1f
    public LayerMask groundMask;
    public GameObject extraHeart;
    public AudioClip jumpClip;
    public AudioSource mainSrc;

    private void Start()
    {
        float sfxVal = PlayerPrefs.GetFloat("SFXValue", 0.5f);
        GameManager.inst.audioSrc.volume = sfxVal;
        //if(mainSrc!=null) mainSrc.Play();
        GameManager.inst.gameStartTime = DateTime.Now;
        GameManager.inst.docTimeSeconds = 0;
        GameManager.inst.doctorModePoints = 0;
        GameManager.inst.cumulativeDocPoints = 0;
        GameManager.inst.cumulativeSupermanPoints = 0;
        GameManager.inst.supermanCount = 0;
        docIcon.SetActive(false);
        horizontalMultiplier = PlayerPrefs.GetFloat("SensitivityValue", 1.7f);
        renderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        JumpCount = MaxJumps;
        bool isApplied = bool.Parse(PlayerPrefs.GetString("ExtraLife" + "Applied", "False"));
        dblJumpApplied = bool.Parse(PlayerPrefs.GetString("DoubleJump" + "Applied", "False"));
        if (isApplied)
        {
            extraHeart.SetActive(true);
            GameManager.inst.livesLeft = 4;
        }
        else
        {
            extraHeart.SetActive(false);
            GameManager.inst.livesLeft = 3;
        }
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
        verticalInput = Input.GetAxis("Vertical");
        if (verticalInput > 0)
        {
            if (JumpCount > 0)
            {
                if (GameManager.inst.GetNoJumpStamp() < DateTime.Now) {
                    GameManager.inst.audioSrc.clip = jumpClip;
                    GameManager.inst.audioSrc.Play();
                    Jump();
                }
            }
        }
        else if (dblJumpApplied && Input.GetKeyDown(KeyCode.Space))
        {
            if (JumpCount > 0)
            {
                GameManager.inst.audioSrc.clip = jumpClip;
                GameManager.inst.audioSrc.Play();
                if (GameManager.inst.GetNoJumpStamp() < DateTime.Now) {
                    DoubleJump();
                }
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
        PlayerPrefs.SetInt("CurrentCoins", GameManager.inst.coinsCollectedPerGame);
        PlayerPrefs.SetInt("PeopleSaved", GameManager.inst.peopleSaved);
        PlayerPrefs.SetInt("VirusKilled", GameManager.inst.virusKilled);

        if(GameManager.inst.livesLeft>0)
            Analytics.CustomEvent("Player fell");
        else
            Analytics.CustomEvent("Player lost all lives");

        int score = GameManager.inst.score + 2 * GameManager.inst.maskCount + 2 * GameManager.inst.syringeCount;
        DateTime endTime = DateTime.Now;
        int seconds = (int)System.Math.Abs((GameManager.inst.gameStartTime - endTime).TotalSeconds);
        Analytics.CustomEvent("Game Statistics", new Dictionary<string, object>
          {
            { "coins", GameManager.inst.coinsCollectedPerGame},
            { "sessionLength", seconds },
            { "total points", score},
            { "doctor time", GameManager.inst.docTimeSeconds },
            { "human time", seconds - GameManager.inst.docTimeSeconds },
            { "doctor points", GameManager.inst.cumulativeDocPoints },
            { "human points", GameManager.inst.score - GameManager.inst.cumulativeDocPoints },
            { "superman count", GameManager.inst.supermanCount },
            { "superman points", GameManager.inst.cumulativeSupermanPoints }
          });
        GameManager.inst.coinsCollectedPerGame = 0;
        //if(mainSrc!=null) mainSrc.Stop();
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

    private void DoubleJump()
    {
        rb.AddForce(Vector3.up * 2 * jumpForce, ForceMode.Acceleration);
        JumpCount -= 1;
    }

    public void ChangeColor()
    {
        if (GameManager.inst.isDoctor)
        {
            docIcon.SetActive(true);
            renderer.material.SetColor("_Color", Color.yellow);
        }
        else
        {
            docIcon.SetActive(false);
            renderer.material.SetColor("_Color", Color.white);
        }
           
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Plane"))
        {
            JumpCount = MaxJumps;
        }
    }
}
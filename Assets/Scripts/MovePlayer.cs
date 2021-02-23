using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    bool alive = true;
    public float startSpeed = 8;
    public float speed = 8;
    public Rigidbody rb;
    public Renderer renderer;
    int JumpCount = 0;
    public int MaxJumps = 1; //Maximum amount of jumps (i.e. 2 for double jumps)

    float horizontalInput;
    public float horizontalMultiplier = 30;
    public float jumpForce = 7f;
    public float speedIncreasePer100Points = 1f;
    public LayerMask groundMask;

    private void Start()
    {
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
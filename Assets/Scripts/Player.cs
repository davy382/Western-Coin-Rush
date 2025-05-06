using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 200f;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public string endScene; // Scene to load when player dies

    private Animator animator;
    private Rigidbody rb;
    private Joystick joystick;
    private JoyButton jumpButton;

    private bool isGrounded = true; // looking for ground to make sure can jump

    private bool isDead = false;
    public AudioSource collectSound;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        joystick = FindObjectOfType<Joystick>();
        jumpButton = FindObjectOfType<JoyButton>();

        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetmaxHealth(maxHealth);
    }

    void Update()
    {
        MovePlayer();
        UpdateHealth();

        if ((jumpButton != null && jumpButton.Pressed) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


    }



    private void MovePlayer()
    {
        float moveInputKeyboard = Input.GetAxis("Vertical");
        float rotateInputKeyboard = Input.GetAxis("Horizontal");

        float moveInputJoystick = joystick != null ? joystick.Vertical : 0f;
        float rotateInputJoystick = joystick != null ? joystick.Horizontal : 0f;

        float moveInput = Mathf.Abs(moveInputKeyboard) > Mathf.Abs(moveInputJoystick) ? moveInputKeyboard : moveInputJoystick;
        float rotateInput = Mathf.Abs(rotateInputKeyboard) > Mathf.Abs(rotateInputJoystick) ? rotateInputKeyboard : rotateInputJoystick;

        float move = moveInput * moveSpeed * Time.deltaTime;
        float rotate = rotateInput * rotateSpeed * Time.deltaTime;

        // Rotate using transform
        transform.Rotate(0, rotate, 0);

        // Move using Rigidbody
        Vector3 forward = transform.forward * move;
        rb.MovePosition(rb.position + forward);

        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            isGrounded = false;
            if (animator != null)
                animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    }


    private void UpdateHealth()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Clamp at 0

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            // Cancel GetHit if queued or mid-play
            animator.ResetTrigger("GetHit");

            // Trigger Death animation
            animator.SetTrigger("Death");

            // Lock down movement and input
            Death();
        }
        else
        {
            // Brief flinch animation
            animator.SetTrigger("GetHit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            TakeDamage(15);
        }
        if (other.CompareTag("Bullet"))
            {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Coin"))
        {
            collectSound.Play();
        }
    }



    public void Death()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Death");

        moveSpeed = 0;
        rotateSpeed = 0;
        rb.velocity = Vector3.zero;

        joystick = null;
        jumpButton = null;

        // call for the enemies to stop shooting because player is dead (This will help with redundancies)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetPlayerDead();
            }
        }

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(2f); // Adjust for your animation length
        SceneManager.LoadScene(2);
    }
}


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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(15);
        }

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(endScene);
        }
        if ((jumpButton != null && jumpButton.Pressed) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        // Keyboard input
        float moveInputKeyboard = Input.GetAxis("Vertical");
        float rotateInputKeyboard = Input.GetAxis("Horizontal");

        // Joystick input
        float moveInputJoystick = joystick != null ? joystick.Vertical : 0f;
        float rotateInputJoystick = joystick != null ? joystick.Horizontal : 0f;

        // Combine both inputs
        float moveInput = Mathf.Abs(moveInputKeyboard) > Mathf.Abs(moveInputJoystick) ? moveInputKeyboard : moveInputJoystick;
        float rotateInput = Mathf.Abs(rotateInputKeyboard) > Mathf.Abs(rotateInputJoystick) ? rotateInputKeyboard : rotateInputJoystick;

        float move = moveInput * moveSpeed * Time.deltaTime;
        float rotate = rotateInput * rotateSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, rotate, 0);

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
        currentHealth -= damage;
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(endScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            TakeDamage(15);
        }
    }
}
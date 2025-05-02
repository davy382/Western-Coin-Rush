using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float shootRange = 5f;

    private Animator animator;
    private float shootTimer;
    private bool isShooting;
    private bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shootTimer = shootInterval;
    }

    private void Update()
    {
        if (animator.GetBool("isDead"))
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootRange)
        {
            animator.SetBool("isShooting", true);
            animator.SetBool("isRunning", false);

            LookAtPlayer();

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0f;
            }
        }
        else if (distance <= detectionRange)
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isRunning", true);
            MoveTowardsPlayer();
        }
        else
        {
            animator.SetBool("isShooting", false);
            animator.SetBool("isRunning", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; 
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
    }
}
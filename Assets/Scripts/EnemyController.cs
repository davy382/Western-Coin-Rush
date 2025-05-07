using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 15f;
    public float shootRange = 7f;
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioSource ShootSound;

    public Transform patrolParent; 
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentPatrolIndex = 0;

    private NavMeshAgent agent;
    private Animator animator;
    private float shootTimer;
    private bool isPlayerDead = false;

    private enum State { Patrolling, Chasing, Shooting }
    private State currentState = State.Patrolling;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shootTimer = shootInterval;

        // finds all the patrol points under the parent patrol point
        if (patrolParent != null)
        {
            foreach (Transform point in patrolParent)
            {
                patrolPoints.Add(point);
            }
        }

        if (patrolPoints.Count > 0)
        {
            currentPatrolIndex = Random.Range(0, patrolPoints.Count);
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Update()
    {
        if (isPlayerDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (distance <= detectionRange)
                {
                    currentState = State.Chasing;
                }
                break;

            case State.Chasing:
                agent.SetDestination(player.position);
                animator.SetBool("isRunning", true);
                animator.SetBool("isShooting", false);

                if (distance <= shootRange)
                {
                    currentState = State.Shooting;
                }
                else if (distance > detectionRange)
                {
                    ReturnToPatrol();
                }
                break;

            case State.Shooting:
                agent.ResetPath();
                animator.SetBool("isRunning", false);
                animator.SetBool("isShooting", true); // we have to force the shooting.. because the navmesh causing conflicts

                LookAtPlayer();

                shootTimer += Time.deltaTime;
                if (shootTimer >= shootInterval)
                {
                    Shoot();
                    shootTimer = 0f;
                }

                if (distance > shootRange)
                {
                    animator.SetBool("isShooting", false);
                    currentState = State.Chasing;
                }
                break;
        }
    }

    void Patrol()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isShooting", false);

        if (!agent.pathPending && agent.remainingDistance < 0.5f && patrolPoints.Count > 0)
        {
            currentPatrolIndex = Random.Range(0, patrolPoints.Count);
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void ReturnToPatrol()
    {
        currentState = State.Patrolling;
        if (patrolPoints.Count > 0)
        {
            currentPatrolIndex = Random.Range(0, patrolPoints.Count);
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    void Shoot()
    {
        if (bulletPrefab && firePoint)
        {
            ShootSound?.Play();
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void SetPlayerDead()
    {
        isPlayerDead = true;
        agent.ResetPath();
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
    }

    public void Die()
    {
        isPlayerDead = true;
        agent.enabled = false;
        animator.SetBool("isRunning", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isDead", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ruinseeker
{

    public class Gunner : Enemy
    {
        public GameObject bulletPrefab;
        public float bulletSpeed = 5f;
        public float bulletRange = 10f;
        private bool isShooting;
        private Animator animator;

        public Transform waypointA; 
        public Transform waypointB; 
        public float patrolSpeed = 0.5f;

        private Transform targetWaypoint;
        private bool isPatrolling = true;

        private Vector3 waypointAWorldPos;
        private Vector3 waypointBWorldPos;
        private void Awake()
        {
             animator = GetComponent<Animator>();
            if (waypointA == null || waypointB == null)
            {
                Debug.LogError("Waypoints no asignados!");
                return;
            }
            waypointAWorldPos = waypointA.position;
            waypointBWorldPos = waypointB.position;

            waypointA.SetParent(null);
            waypointB.SetParent(null);
            targetWaypoint = waypointB;
        }
      
        public override void Patrol()
        {
            if (!IsPlayerInRange())
            {
                MoveTowardsWaypoint();
            }
        }
        private void MoveTowardsWaypoint()
        {
            Vector3 targetPosition = targetWaypoint == waypointA ? waypointAWorldPos : waypointBWorldPos;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);
            FlipToWaypoint(targetPosition);
            if (transform.position == targetPosition)
            {
                targetWaypoint = (targetWaypoint == waypointA) ? waypointB : waypointA;
            }
        }
        private void FlipToWaypoint(Vector3 targetPosition)
        {
            if (targetPosition.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else if (targetPosition.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
        }
        public override void OnPlayerDetected()
        {
            if (!isShooting)
            {
                StopPatrolling();
                StartCoroutine(Shoot());
            }
        }
        private void StopPatrolling()
        {
            isPatrolling = false;
        }
        private void ResumePatrolling()
        {
            isPatrolling = true;
        }
        private IEnumerator Shoot()
        {

            isShooting = true;
            animator.SetTrigger("Shoot");
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Initialize(transform.position, player.position, bulletSpeed);

            yield return new WaitUntil(() => bullet == null);

            isShooting = false;
            ResumePatrolling();
        }

        public override void Die()
        {
            Debug.Log("Boomerang Guy defeated!");

            ResetBulletState();

            base.Die();
        }

        private void ResetBulletState()
        {
            isShooting = false;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(Vector2.up, collisionDirection);

                if (angle < 70)
                {
                    Die();
                    GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerMovement>().JumpAfterKillingEnemy();
                }
                else
                {
                    collision.gameObject.GetComponent<PlayerMovement>().CheckDeath();
                }
            }
        }
    }
}


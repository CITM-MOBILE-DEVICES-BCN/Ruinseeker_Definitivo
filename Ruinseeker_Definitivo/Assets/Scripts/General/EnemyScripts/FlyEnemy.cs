using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Ruinseeker {
    public class FlyEnemy : Enemy
    {
        private bool isChasingPlayer = false;
        public Transform waypointA; 
        public Transform waypointB;
        public float patrolSpeed = 0.5f; 

        private Transform targetWaypoint;
        private bool isPatrolling = true; 

        private Vector3 waypointAWorldPos;
        private Vector3 waypointBWorldPos;
        private void Awake()
        {
            if (waypointA == null || waypointB == null)
            {
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
            StopPatrolling();
            isChasingPlayer = true;
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        private void StopPatrolling()
        {
            isPatrolling = false;
        }
        private void ResumePatrolling()
        {
            isPatrolling = true;
        }
        public override void Die()
        {
            Debug.Log("Fly defeated!");
            base.Die();
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;

                float angle = Vector2.Angle(Vector2.up, collisionDirection);

                if (angle < 45)
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


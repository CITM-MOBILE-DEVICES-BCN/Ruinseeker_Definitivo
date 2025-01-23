using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class ShadowEnemy : Enemy
    {
        [SerializeField] private float movementDelay = 1.0f;
        private Queue<Vector3> playerPositions = new Queue<Vector3>();
        private float positionInterval = 0.05f;
        private float elapsedTime = 0f;
        private bool isRecording = false;
        private bool isTrackingPlayer = false;

        protected override void Start()
        {
            base.Start();

            if (player == null)
            {
                Debug.LogError("Player transform not assigned to ShadowEnemy.");
            }
            else
            {
                Debug.Log("ShadowEnemy initialized. Player found at: " + player.position);
            }
        }

        protected override void Update()
        {
            if (IsPlayerInRange())
            {
                OnPlayerDetected();
            }

            if (!isTrackingPlayer)
            {
                return;
            }

            RecordPlayerMovement();
            Patrol();

        }

        public override void Patrol()
        {
            if (playerPositions.Count > 0)
            {
                Vector3 nextPosition = playerPositions.Peek();
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, nextPosition) < 0.05f)
                {
                    playerPositions.Dequeue();
                }
            }
            else
            {
               // Debug.Log("PlayerPositions queue is empty. ShadowEnemy is idle.");
            }
        }

        public override void OnPlayerDetected()
        {
            if (!isTrackingPlayer)
            {
                isTrackingPlayer = true;
            }
            else
            {
               // Debug.Log("ShadowEnemy is already tracking the player.");
            }
        }

        private void RecordPlayerMovement()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= positionInterval)
            {
                if (player != null)
                {
                    playerPositions.Enqueue(player.position);
                    int maxQueueSize = Mathf.CeilToInt(movementDelay / positionInterval);
                    while (playerPositions.Count > maxQueueSize)
                    {
                        playerPositions.Dequeue();
                    }
                }
                else
                {
                   // Debug.LogWarning("Player is null. Can't record position.");
                }

                elapsedTime = 0f;
            }
        }

    
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //Debug.Log("ShadowEnemy collided with player. Stopping tracking.");
                isTrackingPlayer = false;
                collision.gameObject.GetComponent<PlayerMovement>().DeadFunction();
                playerPositions.Clear();
            }
            else
            {
               // Debug.Log("ShadowEnemy collided with non-player object: " + collision.gameObject.name);
            }
        }
    }
}

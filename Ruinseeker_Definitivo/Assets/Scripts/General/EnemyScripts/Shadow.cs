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
              //  Debug.Log("Player detected within range!");
                OnPlayerDetected();
            }
            else
            {
                //Debug.Log("Player not in range.");
            }

            if (!isTrackingPlayer)
            {
                //Debug.Log("ShadowEnemy is not tracking player.");
                return;
            }

            //Debug.Log("ShadowEnemy is tracking the player.");
            RecordPlayerMovement();
            Patrol();

        }

        public override void Patrol()
        {
            if (playerPositions.Count > 0)
            {
                Vector3 nextPosition = playerPositions.Peek();
              //  Debug.Log("Moving ShadowEnemy to position: " + nextPosition);
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, nextPosition) < 0.05f)
                {
                   // Debug.Log("ShadowEnemy reached target position. Removing from queue.");
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
               // Debug.Log("Player detected! Starting tracking.");
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
          //  Debug.Log("Recording player movement. Elapsed time: " + elapsedTime);

            if (elapsedTime >= positionInterval)
            {
                if (player != null)
                {
                    // Solo registrar posiciones si han pasado el tiempo adecuado
                    playerPositions.Enqueue(player.position);
                    //Debug.Log("Recorded player position: " + player.position);

                    int maxQueueSize = Mathf.CeilToInt(movementDelay / positionInterval);
                    //Debug.Log("Max queue size: " + maxQueueSize);

                    // Asegurarse de que la cola no se haga demasiado grande
                    while (playerPositions.Count > maxQueueSize)
                    {
                        playerPositions.Dequeue();
                      //  Debug.Log("Dequeuing old player position to maintain queue size.");
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
                Debug.Log("ShadowEnemy collided with player. Stopping tracking.");
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

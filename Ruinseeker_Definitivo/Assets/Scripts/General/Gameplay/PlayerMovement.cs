using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class PlayerMovement : MonoBehaviour
    {
        public float jumpForce = 10f; // Fuerza del salto
        public float moveSpeed = 2f; // Velocidad de movimiento
        private Rigidbody2D rb;
        private Vector3 moveDirection = Vector3.right; // Dirección inicial de movimiento
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        bool hasJumped = false;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            Move();
        }

        void Jump()
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasJumped = true;
        }

        void Move()
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                moveDirection = moveDirection == Vector3.right ? Vector3.left : Vector3.right;
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                // Restaurar el estado original del personaje
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
                transform.rotation = originalRotation;
                rb.velocity = Vector3.zero;
            }
        }
    }
}


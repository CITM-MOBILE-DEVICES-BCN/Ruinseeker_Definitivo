using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class PlayerMovement : MonoBehaviour
    {
        public float jumpForce = 10f; // Fuerza del salto
        public float moveSpeed = 2f; // Velocidad de movimiento
        public float wallJumpForce = 5f; // Fuerza del salto en la pared
        public float wallJumpForceX = 5f;
        public float wallSlideSpeed = 1f; // Velocidad de deslizamiento en la pared
        private Rigidbody2D rb;
        private Vector3 moveDirection = Vector3.right; // Dirección inicial de movimiento
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        bool hasJumped = false;
        bool isTouchingWall = false;
        bool isGrounded = false;

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
            if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
            {
                if (isTouchingWall)
                {
                    WallJump();
                }
                else
                {
                    Jump();
                }
            }

            if (!isTouchingWall)
            {
                Move();
            }
        }

        void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasJumped = true;
        }

        void WallJump()
        {
            // Aplicar fuerza en la dirección opuesta a la pared y hacia arriba
            rb.velocity = new Vector2(wallJumpForceX * (transform.localScale.x > 0 ? 1 : -1), jumpForce);
            hasJumped = true;
            isTouchingWall = false; // Resetear el estado de contacto con la pared
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
                if (!isGrounded)
                {
                    isTouchingWall = true;
                    hasJumped = false;
                    rb.velocity = new Vector2(0, -wallSlideSpeed); // Deslizarse lentamente hacia abajo
                }
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                // Restaurar el estado original del personaje
                transform.position = new Vector3(transform.position.x, originalPosition.y, transform.position.z);
                transform.rotation = originalRotation;
                rb.velocity = Vector3.zero;
                hasJumped = false; // Resetear el estado de salto
            }
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall") && !isGrounded)
            {
                rb.velocity = new Vector2(0, -wallSlideSpeed); // Deslizarse lentamente hacia abajo
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                isTouchingWall = false;
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}

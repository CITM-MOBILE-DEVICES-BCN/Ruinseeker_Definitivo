using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ruinseeker
{
    public class PlayerMovement : MonoBehaviour
    {
        public float jumpForce = 10f; // Fuerza del salto
        public float moveSpeed = 2f; // Velocidad de movimiento
        public float dashSpeed = 5f; // Velocidad de dash
        public float wallJumpForce = 5f; // Fuerza del salto en la pared
        public float wallJumpForceX = 5f;
        public float wallSlideSpeed = 1f; // Velocidad de deslizamiento en la pared
        private Rigidbody2D rb;
        private Vector3 moveDirection = Vector3.right; // Dirección inicial de movimiento
        private Vector2 dashDirection = Vector2.zero;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        [SerializeField] private bool hasJumped = false;
        [SerializeField] private bool isTouchingWall = false;
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private bool isDashing = false;
        private float velX;
        private float velY;

        enum DashDirection
        {
            Left,
            Right,
            Up,
            Down,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
        }
        private DashDirection dashDir;

        private void Awake()
        {
            SwipeDetection.instance.swipePerformed += context => { DashActivation(context); };
        }

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

            if (!isTouchingWall && !isDashing)
            {
                Move();
            }
            else if (isDashing)
            {
                DashMovement();
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

        private void DashActivation(Vector2 direction)
        {
            Debug.Log("Dash: " + direction);

            if (hasJumped)
            {
                dashDirection = direction;
                isDashing = true;
                velX = rb.velocity.x;
                velY = rb.velocity.y;
                rb.velocity = Vector2.zero;
                if(direction.x != 0)
                {
                    moveDirection.x = direction.x;
                }

                if (direction.x == 1 && direction.y == 0)
                {
                    dashDir = DashDirection.Right;
                }
                else if (direction.x == -1 && direction.y == 0)
                {
                    dashDir = DashDirection.Left;
                }
                else if (direction.x == 0 && direction.y == 1)
                {
                    dashDir = DashDirection.Up;
                }
                else if (direction.x == 0 && direction.y == -1)
                {
                    dashDir = DashDirection.Down;
                }
                else if (direction.x == 1 && direction.y == 1)
                {
                    dashDir = DashDirection.UpRight;
                }
                else if (direction.x == -1 && direction.y == 1)
                {
                    dashDir = DashDirection.UpLeft;
                }
                else if (direction.x == 1 && direction.y == -1)
                {
                    dashDir = DashDirection.DownRight;
                }
                else if (direction.x == -1 && direction.y == -1)
                {
                    dashDir = DashDirection.DownLeft;
                }

                    StartCoroutine(WaitTimeForDash(0.3f));
            }
        }

        private void DashMovement()
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
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

        IEnumerator WaitTimeForDash(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isDashing = false;
            switch(dashDir)
            {
                case DashDirection.Left:
                    rb.velocity = new Vector2(velX, 0);
                    break;
                case DashDirection.Right:
                    rb.velocity = new Vector2(velX, 0);
                    break;
                case DashDirection.Up:
                    rb.velocity = new Vector2(velX, 10);
                    break;
                case DashDirection.Down:
                    rb.velocity = new Vector2(velX, 10);
                    break;
                case DashDirection.UpRight:
                    rb.velocity = new Vector2(velX, 10);
                    break;
                case DashDirection.UpLeft:
                    rb.velocity = new Vector2(velX, 10);
                    break;
                case DashDirection.DownRight:
                    rb.velocity = new Vector2(velX, 10);
                    break;
                case DashDirection.DownLeft:
                    rb.velocity = new Vector2(velX, 10);
                    break;
            }
            

        }
    }
}

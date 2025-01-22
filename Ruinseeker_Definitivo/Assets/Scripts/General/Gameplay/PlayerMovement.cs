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
        private int groundContactCount = 0; //Por si toca varios suelos a la vez
        public bool hasBoots = false;
        public bool hasStar = false;
        public int dashCnt = 1;
        public bool canTP = true;
        [SerializeField] private bool hasJumped = false;
        [SerializeField] private bool isTouchingWall = false;
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private bool isDashing = false;
        [SerializeField] private bool hasDashed = false;
        [SerializeField] private bool dashInterrupted = false;
        [SerializeField] private EnemyManager enemyManager;
        private float velX;
        private float velY;

        private bool invertedControls = false;

        [SerializeField] private GameObject LeftWallTP;
        [SerializeField] private GameObject RightWallTP;


        public SpriteRenderer renderer;
        public Animator playeranimator;

        public ParticleSystem wallSliderParticlesRight;
        public ParticleSystem wallSliderParticlesLeft;
        public TrailRenderer trailRenderer;

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
            
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            SwipeDetection.instance.swipePerformed += context => { DashActivation(context); };
            originalPosition = transform.position;
            originalRotation = transform.rotation;

            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(!isDashing)
            {
                if (moveDirection.x < 0)
                {
                    renderer.flipX = true; // Girar el sprite horizontalmente
                }
                else
                {
                    renderer.flipX = false; // Girar el sprite horizontalmente
                }
            }
           
            if (isGrounded && !isTouchingWall)
            {
                playeranimator.SetBool("Jump", false);
                playeranimator.SetBool("Hanged", false);
                wallSliderParticlesRight.gameObject.SetActive(false);
                wallSliderParticlesLeft.gameObject.SetActive(false);
                playeranimator.SetBool("Running", true);
                
            }

            if (isTouchingWall && !isGrounded)
            {
                playeranimator.SetBool("Jump", false);
                playeranimator.SetBool("Hanged", true);
                if(renderer.flipX == true)
                {
                    wallSliderParticlesLeft.gameObject.SetActive(true);
                    wallSliderParticlesRight.gameObject.SetActive(false);
                }
                else if (renderer.flipX == false)
                {
                    wallSliderParticlesLeft.gameObject.SetActive(false);
                    wallSliderParticlesRight.gameObject.SetActive(true);
                }
                
                playeranimator.SetBool("Running", false);

            }
            if(!isGrounded && !isTouchingWall)
            {
                playeranimator.SetBool("Jump", true);
                playeranimator.SetBool("Hanged", false);
                wallSliderParticlesRight.gameObject.SetActive(false);
                wallSliderParticlesLeft.gameObject.SetActive(false);
                playeranimator.SetBool("Running", false);
            }
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

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        void Jump()
        {
            if (!isGrounded) return;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasJumped = true;

        }

        void WallJump()
        {
            moveDirection.x = -moveDirection.x;
    
            // Aplicar fuerza en la dirección opuesta a la pared y hacia arriba
            rb.velocity = new Vector2(wallJumpForceX * (-moveDirection.x), jumpForce);
            
            hasJumped = true;
            isTouchingWall = false; // Resetear el estado de contacto con la pared
        }

        void Move()
        {
            float movementDirection = invertedControls ? -1 : 1;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        private void DashActivation(Vector2 direction)
        {
            Debug.Log("Dash: " + direction);
          

            if (!isGrounded && !isTouchingWall && dashCnt>0) //dashCcnt>0 (1)
            {
                playeranimator.SetTrigger("Dash");
                if (trailRenderer != null)
                {
                    trailRenderer.emitting = true;
                }
                StartCoroutine(DisableTrailAfterDash());

                dashDirection = invertedControls ? -direction : direction;
                isDashing = true;
                dashCnt--;
                velX = rb.velocity.x;
                velY = rb.velocity.y;
                rb.velocity = Vector2.zero;

                // Establecer moveDirection según el dash
                if (dashDirection.x != 0) // Dash horizontal o diagonal
                {
                    moveDirection = new Vector3(dashDirection.x, 0, 0);
                }
               

                StartCoroutine(WaitTimeForDash(0.2f));
            }
            

        }

        private void DashMovement()
        {
 
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            // Adjust sprite orientation based on dash direction
            if (dashDirection.x < 0)
            {
                renderer.flipX = !invertedControls;  // Voltear sprite según controles invertidos
            }
            else if (dashDirection.x > 0)
            {
                renderer.flipX = invertedControls; // Ajustar para controles invertidos
            }

            float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;  // Compute the angle
            renderer.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        public void InvertControls()
        {
            invertedControls = !invertedControls; // Cambiar el estado de controles invertidos
            Debug.Log("Controles invertidos: " + invertedControls);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
            {
                if (collision.gameObject.CompareTag("Wall"))
                {
                    if (!isGrounded)
                    {
                        isTouchingWall = true;
                        hasJumped = false;
                        hasDashed = false;
                        if (hasBoots == true)
                        {
                            dashCnt = 2;
                        }
                        else
                        {
                            dashCnt = 1;
                        }
                        rb.velocity = new Vector2(0, -wallSlideSpeed); // Deslizarse lentamente hacia abajo
                    }
                    else
                    {
                        moveDirection = moveDirection == Vector3.right ? Vector3.left : Vector3.right;
                    }
                }

                if (collision.gameObject.CompareTag("Ground"))
                {
                    groundContactCount++;
                    isGrounded = true;
                    if (isTouchingWall)
                    {
                        moveDirection.x = -moveDirection.x;
                    }
                    isTouchingWall = false;
                    // Restaurar el estado original del personaje
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    transform.rotation = originalRotation;
                    rb.velocity = Vector3.zero;
                    hasJumped = false; // Resetear el estado de salto
                    hasDashed = false; // Resetear el estado de dash
                    if (hasBoots == true)
                    {
                        dashCnt = 2;
                    }
                    else
                    {
                        dashCnt = 1;
                    }
                }
                InterruptDash();
            }
        }
        void InterruptDash()
        {
            if (isDashing)
            {
                isDashing = false;
                dashInterrupted = true;
                rb.velocity = Vector2.zero;
                if (trailRenderer != null)
                {
                    trailRenderer.emitting = false;
                }
            }
        }
        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Wall") && !isGrounded && isTouchingWall)
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

    if (collision.gameObject.CompareTag("Ground"))
    {
        groundContactCount--;
        if (groundContactCount <= 0)
        {
            isGrounded = false;
        }
    }

    if (collision.gameObject.CompareTag("Dead"))
    {
        CheckDeath();
    }
}        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!canTP) return;

            if (collision.gameObject.CompareTag("LeftWallTP"))
            {
                transform.position = new Vector3(RightWallTP.transform.position.x - 0.45f, transform.position.y, transform.position.z);
                Debug.Log("LeftWallTP");
                StartCoroutine(TeleportCooldown());
            }
            else if (collision.gameObject.CompareTag("RightWallTP"))
            {
                transform.position = new Vector3(LeftWallTP.transform.position.x + 0.45f, transform.position.y, transform.position.z);
                Debug.Log("RightWallTP");
                StartCoroutine(TeleportCooldown());
            }
        }

        private IEnumerator TeleportCooldown()
        {
            canTP = false;
            yield return new WaitForSeconds(0.5f);
            canTP = true;
        }

        public void CheckDeath()
        {
            if (!hasStar)
            {
                playeranimator.SetTrigger("Hurt");
                DeadFunction();
            }
            else
            {
              return;
            }
        }

        public void DeadFunction()
        {
            rb.velocity = Vector3.zero;
            transform.position = GameManager.Instance.checkpointPosition;
            enemyManager.DestroyAllEnemies();
            enemyManager.RespawnAllEnemies();
            invertedControls = false;
        }
        IEnumerator WaitTimeForDash(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            if(dashDirection.x != 0)
            {
                if (!isDashing && !dashInterrupted)
                {
                    // Asegurar que moveDirection respete los controles invertidos
                    if (dashDirection != Vector2.zero)
                    {
                        moveDirection = new Vector3(dashDirection.x, 0, 0);

                        if (invertedControls)
                        {
                            moveDirection.x = -moveDirection.x;
                        }
                    }
                }
            }
            
       
            isDashing = false;
            dashInterrupted = false;


        }

        IEnumerator DisableTrailAfterDash()
        {
            yield return new WaitForSeconds(0.3f);
            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
            }
        }

        public void JumpAfterKillingEnemy()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce / 1.5f);
        
            playeranimator.SetTrigger("LowHit");

        }


    }

}

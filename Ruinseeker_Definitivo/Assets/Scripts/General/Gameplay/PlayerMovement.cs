using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ruinseeker
{
    public class PlayerMovement : MonoBehaviour
    {
        public float jumpForce = 10f;
        public float moveSpeed = 2f;
        public float dashSpeed = 5f;
        public float wallJumpForce = 5f; 
        public float wallJumpForceX = 5f;
        public float wallSlideSpeed = 1f;
        private Rigidbody2D rb;
        private Vector3 moveDirection = Vector3.right;
        private Vector2 dashDirection = Vector2.zero;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private int groundContactCount = 0;
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

        void Update()
        {
            if(!isDashing)
            {
                if (moveDirection.x < 0)
                {
                    renderer.flipX = true;
                }
                else
                {
                    renderer.flipX = false;
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
            AudioManager.instance.PlayJumpSound();

        }

        void WallJump()
        {
            moveDirection.x = -moveDirection.x;
            rb.velocity = new Vector2(wallJumpForceX * (-moveDirection.x), jumpForce);
            
            hasJumped = true;
            isTouchingWall = false;
        }

        void Move()
        {
            float movementDirection = invertedControls ? -1 : 1;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }

        private void DashActivation(Vector2 direction)
        {
            Debug.Log("Dash: " + direction);
          

            if (!isGrounded && !isTouchingWall && dashCnt>0)
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

                if (dashDirection.x != 0)
                {
                    moveDirection = new Vector3(dashDirection.x, 0, 0);
                }
               

                StartCoroutine(WaitTimeForDash(0.2f));
            }
            

        }

        private void DashMovement()
        {
 
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
            AudioManager.instance.PlayDashSound();
            if (dashDirection.x < 0)
            {
                renderer.flipX = !invertedControls; 
            }
            else if (dashDirection.x > 0)
            {
                renderer.flipX = invertedControls;
            }

            float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
            renderer.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        public void InvertControls()
        {
            invertedControls = !invertedControls;
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
                        rb.velocity = new Vector2(0, -wallSlideSpeed);

                        AudioManager.instance.PlayJumpSound();
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
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    transform.rotation = originalRotation;
                    rb.velocity = Vector3.zero;
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
                rb.velocity = new Vector2(0, -wallSlideSpeed);
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
                AudioManager.instance.PlayPortalSound();
            }
            else if (collision.gameObject.CompareTag("RightWallTP"))
            {
                transform.position = new Vector3(LeftWallTP.transform.position.x + 0.45f, transform.position.y, transform.position.z);
                Debug.Log("RightWallTP");
                StartCoroutine(TeleportCooldown());
                AudioManager.instance.PlayPortalSound();
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
                AudioManager.instance.PlayDeathSound();
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
            AudioManager.instance.PlayDamageSound();

        }


    }

}

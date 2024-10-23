using System.Collections;
using System.Collections.Generic;
using IAHealth;
using UnityEngine;

namespace IAScript
{
    public class Script2IA : HealthIA
    {
        public List<GameObject> waypoints;
        private int currentWaypointIndex = 0;
        public float speed = 5.0f;
        private float maxSpeed = 10;
        public float jumpingPower;
        public Vector2 velocity;
        public HealthIA body;
        public Animator animator;
        private Rigidbody2D rb;
        public bool isJumping;
        public bool isDead;
        public bool immoblie;
        public bool isGrounded;
        private bool isjumping;
        private bool isFalling;
        private bool canAttack1;
        private bool canAttack2;
        private bool canAttack3;
        public bool canMoveRight;
        public bool canMoveLeft;
        public bool isAttacking1;
        public bool isAttacking2;
        public bool isAttacking3;

        public Transform isEmptyRight;
        public Transform isEmptyLeft;
        public Transform checkAttack1Rigth1;
        public Transform checkAttack1Left1;
        public Transform checkAttack1Rigth2;
        public Transform checkAttack1Left2;
        public Transform checkAttack2Rigth1;
        public Transform checkAttack2Left1;
        public Transform checkAttack2Rigth2;
        public Transform checkAttack2Left2;
        public Transform checkAttack3Rigth;
        public Transform checkAttack3Left;
        public Transform checkAttack4Rigth1;
        public Transform checkAttack4Left1;
        public Transform checkAttack4Rigth2;
        public Transform checkAttack4Left2;
        public Transform checkAttack5Rigth;
        public Transform checkAttack5Left;
        public Transform checkAttack6Rigth1;
        public Transform checkAttack6Left1;
        public Transform groudCheckMid;
        public SpriteRenderer spriteRenderer;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            currentWaypointIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if(immoblie)
            {
                return;
            }

            isjumping = rb.velocity.y > 0.3;
            isFalling = rb.velocity.y < -0.3;
            animator.SetBool("IsJumping",isjumping);
            animator.SetBool("IsFalling",isFalling);
            isGrounded = Physics2D.OverlapCircle(groudCheckMid.position, 0.2f, LayerMask.GetMask("Ground"));
            canMoveRight = Physics2D.OverlapCircle(isEmptyRight.position, 0.2f, LayerMask.GetMask("Ground"));
            canMoveLeft = Physics2D.OverlapCircle(isEmptyLeft.position, 0.2f, LayerMask.GetMask("Ground"));
            canAttack1 = Physics2D.OverlapCircle(checkAttack1Rigth1.position, 0.2f, LayerMask.GetMask("Player")) || Physics2D.OverlapCircle(checkAttack1Left1.position, 0.2f, LayerMask.GetMask("Player"));
            canAttack2 = Physics2D.OverlapCircle(checkAttack2Rigth1.position, 0.2f, LayerMask.GetMask("Player")) || Physics2D.OverlapCircle(checkAttack2Left1.position, 0.2f, LayerMask.GetMask("Player"));
            canAttack3 = Physics2D.OverlapCircle(checkAttack3Rigth.position, 0.2f, LayerMask.GetMask("Player")) || Physics2D.OverlapCircle(checkAttack3Left.position, 0.2f, LayerMask.GetMask("Player"));
            if(isJumping && isGrounded)
            {
                rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                isJumping = false;
            }
            Action();
            if(canAttack1)
            {
                isAttacking1 = canAttack1;
            }
            isAttacking2 = canAttack2;
            isAttacking3 = canAttack3;
            if(isAttacking1 && isGrounded)
            {
                StartCoroutine(DoAttack1());
                isAttacking1 = false;
            }
            else if (isAttacking2 && isGrounded)
            {
                StartCoroutine(DoAttack2());
            }
            else if (isAttacking3 && isGrounded)
            {
                StartCoroutine(DoAttack3());
            }
        }

        public void Action()
        {
            
            if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 0.5f) 
            {
                Debug.Log("ez");
                currentWaypointIndex++;
            }

            if (currentWaypointIndex >= waypoints.Count) 
            {
                currentWaypointIndex = 0;
            }

            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
            
            
        }
        public void Jump()
        {
            isJumping = true;
            
        }
        private void FixedUpdate()
        {
            isDead = body.health <= 0;
            velocity = rb.velocity;
            if (isDead)
            {
                StartCoroutine(Died());
            }
            float characterVelocity = Mathf.Abs(rb.velocity.x);
            Flip(rb.velocity.x);
            animator.SetFloat("Speed",characterVelocity);
        }

        public void ApplyDamage1()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack1Rigth2.position, checkAttack1Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<HealthIA>().TakeDamage(damage1);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack1Left2.position, checkAttack1Left1.position);
                foreach (var col in colls) 
                {
                    col.GetComponent<HealthIA>().TakeDamage(damage1);
                }
            }
            
        }

        public void ApplyDamage2()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack2Rigth2.position, checkAttack2Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<HealthIA>().TakeDamage(damage2);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack2Left2.position, checkAttack2Left1.position);
                foreach (var col in colls)
                {
                    col.GetComponent<HealthIA>().TakeDamage(damage2);
                }
            }
            
        }
        public void ApplyDamage3()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(checkAttack3Rigth.position, 1.5f, LayerMask.GetMask("IA"));
                foreach (var col in cols)
                {
                    col.GetComponent<HealthIA>().TakeDamage(damage3);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack3Left.position, 1.5f, LayerMask.GetMask("IA"));
                foreach (var col in colls)
                {
                    col.GetComponent<HealthIA>().TakeDamage(damage3);
                }
            }
        }

        public  void Flip(float velocity)
        {
            //if (inputManager.isAirAttacking)
            {
                //return;
            }
            if (velocity>0.1)
            {
                spriteRenderer.flipX = false;
            }
            else if (velocity<-0.1)
            {
                spriteRenderer.flipX = true;
            }
        }
        public IEnumerator Died()
        {
            animator.SetBool("IsDead",isDead);
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 0;
            //endScreen.SetActive(true);
        }
        public IEnumerator DoAttack1()
        {
            animator.SetBool("IsAttacking1",isAttacking1);
            yield return new WaitForSecondsRealtime(0.45f);
            animator.SetBool("IsAttacking1",false);
        }
        public IEnumerator DoAttack2()
        {
            animator.SetBool("IsAttacking2",isAttacking2);
            immoblie = true;
            yield return new WaitForSecondsRealtime(0.6f);
            immoblie = false;
            isAttacking2 = false;
            animator.SetBool("IsAttacking2",isAttacking2);
        }
        public IEnumerator DoAttack3()
        {
            animator.SetBool("IsAttacking3",isAttacking3);
            immoblie = true;
            yield return new WaitForSecondsRealtime(0.45f);
            immoblie = false;
            isAttacking3 = false;
            animator.SetBool("IsAttacking3",isAttacking3);
        }

        private IEnumerator DoDie()
        {
            body.invincible = true;
            yield return new WaitForSecondsRealtime(3f);
            body.invincible = false;
        }
    }
}

using System.Collections;
using Menu;
using Pathfinding;
using Script_HealthBar;
using UnityEngine;

namespace IA
{
    public class EnnemyIA : Health
    {
        public Transform groudCheckMiddle;
        public int jumpForce = 2;
        public AIPath aiPath;
        public Transform isEmptyLeft;
        public Transform isEmptyRight;
        public bool isGrounded = false;
        public MovementDirection dir;
        public Health body;
        public Animator animator;
        public bool isjumping;
        public bool isFalling;
        private bool canAttack1;
        private bool canAttack2;
        private bool canAttack3;
        private bool canAirAttack;
        private bool canAttackingSpe;
        public bool isAttacking1;
        public bool isAttacking2;
        public bool isAttacking3;
        public bool isAirAttacking;
        public bool isAttackingSpe;
        public bool isDefending;
        public bool isDead;
        public bool immobile;
        public bool attackSpeEnable;
        private int _nbAattack;
        public GameObject wp1;
        public GameObject wp2;
        public GameObject wp3;
        public GameObject wp4;
        public Rigidbody2D rb;
        public float distance;
        public Transform object1;
        public Transform target;
        public Transform checkwallleft;
        public Transform checkwallright;
        public Transform checkAttack1Rigth1;
        public Transform checkAttack1Rigth2;
        public Transform checkAttack1Left1;
        public Transform checkAttack1Left2;
        public Transform checkAttack2Rigth1;
        public Transform checkAttack2Left1;
        public Transform checkAttack3Rigth;
        public Transform checkAttack3Left;
        public Transform checkAttack4Rigth1;
        public Transform checkAttack4Left1;
        public Transform checkAttack4Rigth2;
        public Transform checkAttack4Left2;
        public Transform checkAttack2Rigth2;
        public Transform checkAttack2Left2;
        public Transform checkAttack5Rigth;
        public Transform checkAttack5Left;
        public Transform checkAttack6Rigth2;
        public Transform checkAttack6Left2;
        public Transform checkAttack6Rigth1;
        public Transform checkAttack6Left1;
        public HealthBar HealthBarClient;

        public float speed;

        AIPath myScript;
        AIDestinationSetter st;
        SpriteRenderer spriteRenderer;
        
        // Update is called once per frame

        void Start()
        {
            myScript = GetComponentInParent<AIPath>();
            //myScript = GetComponent<AIPath>();
            st = GetComponentInParent<AIDestinationSetter>();
            //st = GetComponent<AIDestinationSetter>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            HealthBarClient.gameObject.SetActive(true);
            HealthBarClient.SetMaxHealth(body.healthMax);
            wp1 = GameObject.Find("WayPoints1");
            wp2 = GameObject.Find("WayPoints2");
            wp3 = GameObject.Find("WayPoints3");
            wp4 = GameObject.Find("WayPoints4");
        }

        void Update()
        {
            if (SpawnPlayers.isgame)
            {
                animator.SetBool("IsGame",SpawnPlayers.isgame);
            }
            HealthBarClient.SetHealth(body.health);
            isDead = body.health <= 0;
            distance = Vector3.Distance(object1.position, target.position);
            if (isDead)
            {
                StartCoroutine(Died());
            }
            if (body.isHit)
            {
                StartCoroutine(DoHit());
                body.isHit = false;
                isHit = false;
            }
            if(immobile)
            {
                myScript.canMove = false;
                animator.SetBool("isImobile",true);
                dir = MovementDirection.None;
                return;
            }
            else
            {
                myScript.canMove = true;
                animator.SetBool("isImobile",false);
            }
            chooseTarget();
            if ((health <= 60 && _nbAattack == 0) || (health <= 20 && _nbAattack == 1))
            {
                attackSpeEnable = true;
            }
            if (IsPlayerAbove())
            {
                myScript.enabled = false;
            }
            else
            {
                myScript.enabled = true;
            }
            
            speed = rb.velocity.y;
            isGrounded = Physics2D.OverlapCircle(groudCheckMiddle.position, 0.2f, LayerMask.GetMask("Ground"));
            isjumping = rb.velocity.y > 3;
            isFalling = rb.velocity.y < -3;
            animator.SetBool("IsJumping",isjumping);
            animator.SetBool("IsFalling",isFalling);
            animator.SetFloat("Speed",Mathf.Abs(aiPath.desiredVelocity.x));
            canAirAttack = !isGrounded && distance <= 4.5f;
            canAttackingSpe = (Physics2D.OverlapCircle(checkAttack2Rigth2.position, 0.2f, LayerMask.GetMask("Player")) || Physics2D.OverlapCircle(checkAttack2Left2.position, 0.2f, LayerMask.GetMask("Player"))) && isGrounded && attackSpeEnable;
            canAttack1 = distance <= 6f;
            canAttack2 = Physics2D.OverlapCircle(checkAttack3Rigth.position, 0.2f, LayerMask.GetMask("Player")) || Physics2D.OverlapCircle(checkAttack3Left.position, 0.2f, LayerMask.GetMask("Player"));
            canAttack3 = Physics2D.OverlapCircle(checkAttack2Rigth2.position, 0.2f, LayerMask.GetMask("Player")) || Physics2D.OverlapCircle(checkAttack2Left2.position, 0.2f, LayerMask.GetMask("Player"));
            if (isGrounded)
            {
                jump();
            }
            
            if(aiPath.desiredVelocity.x >= 0.03f )
            {
                dir = MovementDirection.Right;
                //transform.localScale = new Vector3(-6.52f,6.52f,6.52f);
            } 
            else if(aiPath.desiredVelocity.x <= -0.03f)
            {
                dir = MovementDirection.Left;
                //transform.localScale = new Vector3(6.52f,6.52f,6.52f);
            }
        }

        void FixedUpdate()
        {
            if (SpawnPlayers.isgame)
            {
                animator.SetBool("IsGame",SpawnPlayers.isgame);
            }
            HealthBarClient.SetHealth(body.health);
            isDead = body.health <= 0;
            if (isDead)
            {
                StartCoroutine(Died());
            }
            if (body.isHit)
            {
                StartCoroutine(DoHit());
                body.isHit = false;
                isHit = false;
            }
            if (immobile || isDead || isHit || IsPlayerAbove())
            {
                rb.velocity = Vector2.zero;
                return;
            }
            if (dir == MovementDirection.Right)
            {
                spriteRenderer.flipX = false;
            }
            else if(dir == MovementDirection.Left)
            {
                spriteRenderer.flipX = true;
            }
            if ((body.health<=60 && _nbAattack==0) || (body.health<=20 && _nbAattack==1))
            {
                attackSpeEnable = true;
            }
            attack();
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
            else if (isAttackingSpe)
            {
                StartCoroutine(DoAttackSpe());
                _nbAattack += 1;
                attackSpeEnable = false;
            }
        }

        private void attack()
        {
            if (canAttack3)
            {
                isAttacking3 = canAttack3;
            }
            else if (canAttack2)
            {
                if(canAttackingSpe)
                {
                    isAttackingSpe = canAttackingSpe;
                }
                else
                {
                    isAttacking2 = canAttack2;
                }
                
            }
            else if(canAttack1)
            {
                isAttacking1 = canAttack1;
            }
            
            else if (canAirAttack)
            {
                isAirAttacking = canAirAttack;
            }
            
        }

        public bool IsPlayerAbove()
        {
                
            if (target.position.y > transform.position.y && Mathf.Abs(target.position.x - transform.position.x) < 1.5f)
            {
                return true;
            }

            return false;
        }
        
        public void chooseTarget()
        {
            if (target.transform.position.y <= -2.5)
            {
                immobile = true;
                //animator.SetBool("isImobile",true);
                target = wp1.transform;
                st.target = target;
            }
            else{
                
                //animator.SetBool("isImobile",true);
                target = GameObject.FindGameObjectWithTag("Player").transform;
                st.target = target;
                immobile = false;
            }
            if (body.health <= 20)
            {
                target = wp4.transform;
                st.target = target;
            }
        }

        public enum MovementDirection
        {
            None,
            Left,
            Right
        }
        public void jump()
        {
            // Check if the enemy is touching the ground on the left or right side
            bool isTouchingLeft = isEmptyLeft.GetComponent<Collider2D>().IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));
            bool isTouchingWallLeft = checkwallleft.GetComponent<Collider2D>().IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));
            bool isTouchingRight = isEmptyRight.GetComponent<Collider2D>().IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));
            bool isTouchingWallRight = checkwallright.GetComponent<Collider2D>().IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));

            if (dir == MovementDirection.Right)
            {
                if (!isTouchingRight)
                {
                    // Jump if not touching the ground on either side
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
                /*if(isTouchingWallRight)
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }*/
            }
            else
            {
                if (!isTouchingLeft)
                {
                    // Jump if not touching the ground on either side
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
                /*if(isTouchingWallLeft)
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }*/
            }
            
        }

        public IEnumerator DoAttack1()
        {
            ApplyDamage1();
            animator.SetBool("IsAttacking1",isAttacking1);
            immobile = true;
            yield return new WaitForSecondsRealtime(0.6f);
            immobile = false;
            isAttacking1 = false;
            animator.SetBool("IsAttacking1",false);
        }
        public IEnumerator DoAttack2()
        {
            ApplyDamage2();
            animator.SetBool("IsAttacking2",isAttacking2);
            immobile = true;
            yield return new WaitForSecondsRealtime(0.75f);
            immobile = false;
            isAttacking2 = false;
            animator.SetBool("IsAttacking2",false);
        }
        public IEnumerator DoAttack3()
        {
            ApplyDamage3();
            animator.SetBool("IsAttacking3",isAttacking3);
            immobile = true;
            yield return new WaitForSecondsRealtime(1.2f);
            immobile = false;
            isAttacking3 = false;
            animator.SetBool("IsAttacking3",isAttacking3);
        }
        public IEnumerator DoAirAttack()
        {
            animator.SetBool("IsAirAttacking",isAirAttacking);
            yield return new WaitForSecondsRealtime(0.65f);
            isAirAttacking = false;
            animator.SetBool("IsAirAttacking",isAirAttacking);
        }
        public IEnumerator DoHit()
        {
            animator.SetBool("IsHit",true);
            yield return new WaitForSecondsRealtime(0.45f);
            animator.SetBool("IsHit",false);
        }

        public void ApplyDamage1()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack1Rigth2.position, checkAttack1Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage1);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack1Left2.position, checkAttack1Left1.position);
                foreach (var col in colls) 
                {
                    col.GetComponent<Health>().TakeDamage(damage1);
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
                    col.GetComponent<Health>().TakeDamage(damage2);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack2Left2.position, checkAttack2Left1.position);
                foreach (var col in colls)
                {
                    col.GetComponent<Health>().TakeDamage(damage2);
                }
            }
            
        }
        public void ApplyDamage3()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(checkAttack3Rigth.position, 1.5f, LayerMask.GetMask("Player"));
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage3);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack3Left.position, 1.5f, LayerMask.GetMask("Player"));
                foreach (var col in colls)
                {
                    col.GetComponent<Health>().TakeDamage(damage3);
                }
            }
        }
        public void ApplyDamage4()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack4Rigth2.position, checkAttack4Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage4);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack4Left2.position, checkAttack4Left1.position);
                foreach (var col in colls)
                {
                    col.GetComponent<Health>().TakeDamage(damage4);
                }
            }
        }
        public void ApplyDamage5()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(checkAttack5Rigth.position, 1f, LayerMask.GetMask("Player"));
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage5);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack5Left.position, 1f, LayerMask.GetMask("Player"));
                foreach (var col in colls)
                {
                    col.GetComponent<Health>().TakeDamage(damage5);
                }
            }
        }
        public void ApplyDamage6()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack6Rigth2.position, checkAttack6Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage6);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack6Left2.position, checkAttack6Left1.position);
                foreach (var col in colls)
                {
                    col.GetComponent<Health>().TakeDamage(damage6);
                }
            }
        }
        public IEnumerator Died()
        {
            animator.SetBool("IsDead",isDead);
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 0;
        }
        internal void Die()
        {
            rb.velocity=Vector2.zero;
            StartCoroutine(DoDie());
        }
        
        private IEnumerator DoDie()
        {
            body.invincible = true;
            yield return new WaitForSecondsRealtime(3f);
            body.invincible = false;
        }

        public IEnumerator DoAttackSpe()
        {
            animator.SetBool("IsAttackingSpe",isAttackingSpe);
            yield return new WaitForSecondsRealtime(1.67f);
            isAttackingSpe = false;
            animator.SetBool("IsAttackingSpe",isAttackingSpe);
        }

        public IEnumerator DoDefend()
        {
            body.invincible = true;
            animator.SetBool("IsDefending",isDefending);
            yield return new WaitForSecondsRealtime(0.4f);
            isDefending = false;
            animator.SetBool("IsDefending",isDefending);
            body.invincible = false;
        }
    }
}

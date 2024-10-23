using System.Collections;
using Menu;
using Photon.Pun;
using Script_HealthBar;
using UnityEngine;

namespace ScriptPLayer.SandPlayer
{
    public class SandPlayerMovement : Player
    {
        public bool attackSpeEnable;
        private int _nbAattack;
        public int cdDef=3;
        public float cdCurrentDef=3;
        public bool isDef;
        private bool _flipX;
        PhotonView view;
        public Transform checkAttack1Rigth;
        public Transform checkAttack1Left;
        public Transform checkAttack2Rigth;
        public Transform checkAttack2Left;
        public Transform checkAttack3Rigth1;
        public Transform checkAttack3Left1;
        public Transform checkAttack3Rigth2;
        public Transform checkAttack3Left2;
        public Transform checkAttack4Rigth;
        public Transform checkAttack4Left;
        public Transform checkAttack5Rigth1;
        public Transform checkAttack5Left1;
        public Transform checkAttack5Rigth2;
        public Transform checkAttack5Left2;
        private void Start()
        {
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 15;
            view = GetComponent<PhotonView>();
        }
        void Update()
        {
            if (!photonView.IsMine && !solo)
            {
                return;
            }
            if (immoblie)
            {
                return;
            }
            if (isDef)
            {
                cdCurrentDef -= 1 * Time.deltaTime;
                inputManager.isDefending = false;
            }
            if (host)
            {
                HealthBarHost.SetHealth(body.health);
                photonView.RPC("SyncCanvasSetHealth", RpcTarget.Others);
            }
            else if (client)
            {
                HealthBarClient.SetHealth(body.health);
                photonView.RPC("SyncCanvasSetHealth", RpcTarget.Others);
            }
            else
            {
                HealthBarHost.SetHealth(body.health);
            }
            if (cdCurrentDef<=0)
            {
                isDef = false;
                cdCurrentDef = cdDef;
            }
            bool isWalledRigth1 = Physics2D.OverlapArea(wallCheckRigth1.position, wallCheckRigth2.position,LayerMask.GetMask("Wall"));
            bool isWallLeft1=Physics2D.OverlapArea(wallCheckLeft1.position, wallCheckLeft2.position,LayerMask.GetMask("Wall"));
            isGrounded = Physics2D.OverlapCircle(groudCheckMid.position, 0.2f, LayerMask.GetMask("Ground"));
            isWalledRigth = Physics2D.OverlapArea(wallCheckRigth1.position, wallCheckRigth2.position,LayerMask.GetMask("Player"));
            isWallLeft=Physics2D.OverlapArea(wallCheckLeft1.position, wallCheckLeft2.position,LayerMask.GetMask("Player"));
            if (isWalledRigth)
            {
                velocity = new Vector2(inputManager.InputVelocity.x * speed*0.5f, rb.velocity.y);
            }
            else if (isWallLeft)
            {
                velocity = new Vector2(inputManager.InputVelocity.x * speed*0.5f, rb.velocity.y);
            }
            else if (isGrounded)
            {
                velocity = new Vector2(inputManager.InputVelocity.x * speed,
                    inputManager.InputVelocity.y * jumpingPower);
            }
            else if (isWalledRigth1)
            {
                velocity = new Vector2(0, rb.velocity.y);
            }
            else if (isWallLeft1)
            {
                velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                velocity = new Vector2(inputManager.InputVelocity.x * speed, rb.velocity.y);
            }
            
        }
        private void FixedUpdate()
        {
            if (!photonView.IsMine && !solo)
            {
                return;
            }
            if (SpawnPlayers.isgame)
            {
                animator.SetBool("IsGame",SpawnPlayers.isgame);
            }
            if (body.isHit)
            {
                StartCoroutine(DoHit());
                body.isHit = false;
            }
            isDead = body.health <= 0;
            if (isDead)
            {
                StartCoroutine(Died());
            }
            if (immoblie)
            {
                return;
            }
            if (!attackSpeEnable)
            {
                inputManager.isAttackingSpe = false;
            }
            if ((body.health<=60 && _nbAattack==0) || (body.health<=20 && _nbAattack==1))
            {
                attackSpeEnable = true;
            }
            rb.velocity = velocity;
            Flip(rb.velocity.x);
            if (!isGrounded)
            {
                if (inputManager.isAirAttacking)
                {
                    StartCoroutine(DoAirAttack());
                }
                inputManager.ResetJump();
                inputManager.isAttacking1 = false;
                inputManager.isAttacking2 = false;
                inputManager.isAttacking3 = false;
            }
            else
            {
                inputManager.ResetAirAttack();
            }
            float characterVelocity = Mathf.Abs(rb.velocity.x);
            isJumping = rb.velocity.y > 0.3;
            isFalling = rb.velocity.y < -0.3;
            animator.SetFloat("Speed",characterVelocity);
            animator.SetBool("IsFalling",isFalling);
            animator.SetBool("IsJumping",isJumping);
            if (inputManager.isAttacking1)
            {
                StartCoroutine(DoAttack1());
            }
            else if (inputManager.isAttacking2)
            {
                StartCoroutine(DoAttack2());
            }
            else if (inputManager.isAttacking3)
            {
                StartCoroutine(DoAttack3());
            }
            if (inputManager.isRolling)
            {
                StartCoroutine(DoRoll());
            }
            if (inputManager.isAttackingSpe && isGrounded && attackSpeEnable)
            {
                StartCoroutine(DoAttackSpe());
                _nbAattack += 1;
                attackSpeEnable = false;
            }
            if (inputManager.isDefending && isGrounded && !isDef)
            {
                StartCoroutine(DoDefend());
                isDef = true;
            }
            photonView.RPC("SyncPosition", RpcTarget.Others, transform.position);
            photonView.RPC("SyncAnimationJump", RpcTarget.Others, isJumping);
            photonView.RPC("SyncAnimationFall", RpcTarget.Others, isFalling);
            if (isGrounded)
            {
                photonView.RPC("SyncAnimationRun", RpcTarget.Others, characterVelocity);
            }
            photonView.RPC("SyncFlipX", RpcTarget.Others, _flipX);
        }
        [PunRPC]
        void SyncCanvasSetHealth()
        {
            if (host)
            {
                HealthBarHost.SetHealth(body.health);
            }
            else
            {
                HealthBarClient.SetHealth(body.health);
            }
        }
        [PunRPC]
        void SyncFlipX(bool value)
        {
            _flipX = value;
            spriteRenderer.flipX = _flipX;
        }

        [PunRPC]
        void SyncPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }
        [PunRPC]
        void SyncAnimationJump(bool boolanim)
        {
            animator.SetBool("IsJumping", boolanim);
        }
        [PunRPC]
        void SyncAnimationFall(bool boolanim)
        {
            animator.SetBool("IsFalling", boolanim);
        }
        [PunRPC]
        void SyncAnimationRun(float boolanim)
        {
            animator.SetFloat("Speed", boolanim);
        }
        public new void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_flipX);
            }
            else
            {
                _flipX = (bool)stream.ReceiveNext();
                spriteRenderer.flipX = _flipX;
            }
        }
        public void ApplyDamage1()
        {
            if (solo)
            {
                if (!spriteRenderer.flipX)
                {
                    Collider2D[] cols =
                        Physics2D.OverlapCircleAll(checkAttack1Rigth.position, 0.5f, LayerMask.GetMask("IAH"));
                    foreach (var col in cols)
                    {
                        col.GetComponent<Health>().TakeDamage(damage1);
                    }
                }
                else
                {
                    Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack1Left.position, 0.5f, LayerMask.GetMask("IAH"));
                    foreach (var col in colls)
                    {
                        col.GetComponent<Health>().TakeDamage(damage1);
                    }
                }
            }
            else
            {
                if (!spriteRenderer.flipX)
                {
                    Collider2D[] cols =
                        Physics2D.OverlapCircleAll(checkAttack1Rigth.position, 0.5f, LayerMask.GetMask("Player"));
                    foreach (var col in cols)
                    {
                        col.GetComponent<Health>().TakeDamage(damage1);
                    }
                }
                else
                {
                    Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack1Left.position, 0.5f, LayerMask.GetMask("Player"));
                    foreach (var col in colls)
                    {
                        col.GetComponent<Health>().TakeDamage(damage1);
                    }
                }
            }
            
        }
        public void ApplyDamage2()
        {
            if (solo)
            {
                if (!spriteRenderer.flipX)
                {
                    Collider2D[] cols =
                        Physics2D.OverlapCircleAll(checkAttack2Rigth.position, 1.2f, LayerMask.GetMask("IAH"));
                    foreach (var col in cols)
                    {
                        col.GetComponent<Health>().TakeDamage(damage2);
                    }
                }
                else
                {
                    Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack2Left.position, 1.2f, LayerMask.GetMask("IAH"));
                    foreach (var col in colls)
                    {
                        col.GetComponent<Health>().TakeDamage(damage2);
                    }
                }
            }
            else
            {
                if (!spriteRenderer.flipX)
                {
                    Collider2D[] cols =
                        Physics2D.OverlapCircleAll(checkAttack2Rigth.position, 1.2f, LayerMask.GetMask("Player"));
                    foreach (var col in cols)
                    {
                        col.GetComponent<Health>().TakeDamage(damage2);
                    }
                }
                else
                {
                    Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack2Left.position, 1.2f, LayerMask.GetMask("Player"));
                    foreach (var col in colls)
                    {
                        col.GetComponent<Health>().TakeDamage(damage2);
                    }
                }
            }
            
        }
        public void ApplyDamage3()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack3Rigth2.position, checkAttack3Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage3);
                }
            }
            else
            {
                Collider2D[] colls = Physics2D.OverlapAreaAll(checkAttack3Left2.position, checkAttack3Left1.position);
                foreach (var col in colls)
                {
                    col.GetComponent<Health>().TakeDamage(damage3);
                }
            }
        }
        public void ApplyDamage4()
        {
            if (solo)
            {
                if (!spriteRenderer.flipX)
                {
                    Collider2D[] cols =
                        Physics2D.OverlapCircleAll(checkAttack4Rigth.position, 1.5f, LayerMask.GetMask("IAH"));
                    foreach (var col in cols)
                    {
                        col.GetComponent<Health>().TakeDamage(damage4);
                    }
                }
                else
                {
                    Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack4Left.position, 1.5f, LayerMask.GetMask("IAH"));
                    foreach (var col in colls)
                    {
                        col.GetComponent<Health>().TakeDamage(damage4);
                    }
                }
            }
            else
            {
                if (!spriteRenderer.flipX)
                {
                    Collider2D[] cols =
                        Physics2D.OverlapCircleAll(checkAttack4Rigth.position, 1.5f, LayerMask.GetMask("Player"));
                    foreach (var col in cols)
                    {
                        col.GetComponent<Health>().TakeDamage(damage4);
                    }
                }
                else
                {
                    Collider2D[] colls = Physics2D.OverlapCircleAll(checkAttack4Left.position, 1.5f, LayerMask.GetMask("Player"));
                    foreach (var col in colls)
                    {
                        col.GetComponent<Health>().TakeDamage(damage4);
                    }
                }
            }
            
        }
        public void ApplyDamage5()
        {
            if (!spriteRenderer.flipX)
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack5Rigth2.position, checkAttack5Rigth1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage5);
                }
            }
            else
            {
                Collider2D[] cols = Physics2D.OverlapAreaAll(checkAttack5Left2.position, checkAttack5Left1.position);
                foreach (var col in cols)
                {
                    col.GetComponent<Health>().TakeDamage(damage5);
                }
            }
        }
        public  void Flip(float velocity)
        {
            if (inputManager.isAirAttacking)
            {
                return;
            }
            if (velocity>0.1)
            {
                _flipX = false;
                spriteRenderer.flipX = false;
            }
            else if (velocity<-0.1)
            {
                _flipX = true;
                spriteRenderer.flipX = true;
            }
        }
        public IEnumerator DoAttack1()
        {
            animator.SetBool("IsAttacking1",inputManager.isAttacking1);
            yield return new WaitForSecondsRealtime(0.45f);
            inputManager.isAttacking1 = false;
            animator.SetBool("IsAttacking1",inputManager.isAttacking1);
        }
        public IEnumerator DoAttack2()
        {
            animator.SetBool("IsAttacking2",inputManager.isAttacking2);
            immoblie = true;
            yield return new WaitForSecondsRealtime(1f);
            immoblie = false;
            inputManager.isAttacking2 = false;
            animator.SetBool("IsAttacking2",inputManager.isAttacking2);
        }
        public IEnumerator DoAttack3()
        {
            animator.SetBool("IsAttacking3",inputManager.isAttacking3);
            immoblie = true;
            yield return new WaitForSecondsRealtime(0.6f);
            immoblie = false;
            inputManager.isAttacking3 = false;
            animator.SetBool("IsAttacking3",inputManager.isAttacking3);
        }

        public IEnumerator DoAirAttack()
        {
            animator.SetBool("IsAirAttacking",inputManager.isAirAttacking);
            yield return new WaitForSecondsRealtime(0.45f);
            inputManager.isAirAttacking = false;
            animator.SetBool("IsAirAttacking",inputManager.isAirAttacking);
        }
        public IEnumerator DoHit()
        {
            animator.SetBool("IsHit",true);
            yield return new WaitForSecondsRealtime(0.4f);
            animator.SetBool("IsHit",false);
        }
        
        public IEnumerator Died()
        {
            animator.SetBool("IsDead",isDead);
            yield return new WaitForSecondsRealtime(0.98f);
            animator.SetBool("IsDead",false);
        }

        public IEnumerator DoRoll()
        {
            animator.SetBool("IsRolling",inputManager.isRolling);
            yield return new WaitForSecondsRealtime(0.4f);
            inputManager.isRolling = false;
            animator.SetBool("IsRolling",inputManager.isRolling);
        }
        public IEnumerator DoAttackSpe()
        {
            animator.SetBool("IsAttackingSpe",inputManager.isAttackingSpe);
            yield return new WaitForSecondsRealtime(0.7f);
            inputManager.isAttackingSpe = false;
            animator.SetBool("IsAttackingSpe",inputManager.isAttackingSpe);
        }

        public IEnumerator DoDefend()
        {
            body.invincible = true;
            animator.SetBool("IsDefending",inputManager.isDefending);
            yield return new WaitForSecondsRealtime(0.4f);
            inputManager.isDefending = false;
            animator.SetBool("IsDefending",inputManager.isDefending);
            body.invincible = false;
        }
    }
}

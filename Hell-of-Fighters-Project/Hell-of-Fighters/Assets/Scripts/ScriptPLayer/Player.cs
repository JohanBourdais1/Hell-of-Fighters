using System;
using System.Collections;
using System.Collections.Generic;
using Menu;
using Photon.Pun;
using Script_HealthBar;
using UnityEngine;

namespace ScriptPLayer
{
    public class Player : Health,IPunObservable
    {
        public Vector2 velocity;
        public float speed;
        public float jumpingPower;
        public bool isGrounded;
        public bool isWalledRigth;
        public bool isWallLeft;
        public bool isJumping;
        public bool isFalling;
        public bool isDead;
        public bool immoblie;
        public Rigidbody2D rb;
        public Health body;
        private CapsuleCollider2D _capsuleCollider2D;
        public Animator animator;
        public PlayerInput inputManager;
        public SpriteRenderer spriteRenderer;
        public Transform groudCheckMid;
        public Transform wallCheckRigth1;
        public Transform wallCheckLeft1;
        public Transform wallCheckRigth2;
        public Transform wallCheckLeft2;
        public HealthBar HealthBarHost;
        public HealthBar HealthBarClient;
        public bool client;
        public bool host;
        public bool solo;

        public static List<Player> Players = new List<Player>();

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            inputManager = GetComponent<PlayerInput>();
            Players.Add(this);
            if (CreateAndJoinRooms.Client || CreateAndJoinRooms.Host)
            {
                if (transform.position == new Vector3(8, 6, 10))
                {
                    HealthBarClient.gameObject.SetActive(true);
                    client = true;
                    HealthBarClient.SetMaxHealth(body.healthMax);
                    photonView.RPC("SyncCanvasHealth", RpcTarget.Others);
                }
                else
                {
                    HealthBarHost.gameObject.SetActive(true);
                    host = true;
                    HealthBarHost.SetMaxHealth(body.healthMax);
                    photonView.RPC("SyncCanvasHealth", RpcTarget.Others);
                }
            }
            else
            {
                HealthBarHost.gameObject.SetActive(true);
                HealthBarHost.SetMaxHealth(body.healthMax);
                solo = true;
            }
        }
        
        [PunRPC]
        void SyncCanvasHealth()
        {
            if (host)
            {
                HealthBarHost.gameObject.SetActive(true);
                HealthBarHost.SetMaxHealth(body.healthMax);
            }
            else
            {
                HealthBarClient.gameObject.SetActive(true);
                HealthBarClient.SetMaxHealth(body.healthMax);
            }
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                
            }
            else
            {
                if (host)
                {
                    HealthBarHost.gameObject.SetActive(true);
                    HealthBarHost.SetMaxHealth(body.healthMax);
                    HealthBarHost.SetHealth(body.health);
                }
                else
                {
                    HealthBarClient.gameObject.SetActive(true);
                    HealthBarClient.SetMaxHealth(body.healthMax);
                    HealthBarClient.SetHealth(body.health);
                }
            }
        }
    }
}

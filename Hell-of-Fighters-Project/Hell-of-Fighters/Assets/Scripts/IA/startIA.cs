using System.Collections;
using System.Collections.Generic;
using IA;
using UnityEngine;
using Pathfinding;

namespace StartIA
{
    public class startIA : MonoBehaviour
    {
        public AIDestinationSetter st;
        public AIPath myScript;
        GameObject player;
        public BoxCollider2D bc;
        public EnnemyIA scriptia;
        
        // Start is called before the first frame update
        void Start()
        {
            myScript = GetComponent<AIPath>();
            bc = GetComponent<BoxCollider2D>();
            st = GetComponent<AIDestinationSetter>();
            player = GameObject.FindGameObjectWithTag("Player");
            scriptia = GetComponentInChildren<EnnemyIA>();
        }

        // Update is called once per frame
        void Update()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            bool playerExists = GameObject.FindGameObjectWithTag("Player") != null;
            if (playerExists)
            {
                myScript.enabled = true;
                st.target = player.transform;
                scriptia.enabled = true;
                scriptia.target = player.transform;
                if (transform.GetChild(0).position.y <= 2.54f)
                {
                    scriptia.enabled = false;
                    myScript.enabled = false;
                    bc.enabled = false;
                }
            }
            else
            {
                myScript.enabled = false;
                scriptia.enabled = false;
            }
            
        }
    }
}

